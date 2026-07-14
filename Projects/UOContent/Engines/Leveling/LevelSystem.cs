using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ModernUO.CodeGeneratedEvents;
using Server.Engines.BuffIcons;
using Server.Engines.MLQuests.Definitions;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Leveling;

// Per-player level/XP context. Trivial payload, so it is serialized inline by
// LevelSystem rather than through the codegen serializer.
public class LevelContext
{
    public int Level { get; set; }
    public long XP { get; set; }
    public bool PrimerShown { get; set; }

    // Used for load-time pruning: an empty context is dropped instead of re-added.
    // PrimerShown is deliberately excluded — it is only ever set alongside Level >= 1.
    public bool IsUsed() => Level > 0 || XP > 0;
}

// GenericPersistence sidecar for the player leveling system, keyed per PlayerMobile.
// Structure mirrors VirtueSystem: Configure registers the singleton, a dictionary
// holds the contexts, save/load is versioned, and deleted players are pruned.
public class LevelSystem : GenericPersistence
{
    private static readonly Dictionary<PlayerMobile, LevelContext> _playerLevels = new();

    private static LevelSystem _levelSystemPersistence;

    public static void Configure()
    {
        _levelSystemPersistence = new LevelSystem();
    }

    public LevelSystem() : base("Levels", 10)
    {
    }

    [OnEvent(nameof(PlayerMobile.PlayerDeletedEvent))]
    public static void OnPlayerDeleted(PlayerMobile pm) => _playerLevels.Remove(pm);

    // Caps are re-asserted on every login rather than hooked into character creation:
    // it fires after creation completes (no handler-ordering dependency), keeps this
    // system out of CharacterCreation.cs, and migrates pre-existing characters to
    // their level's caps the first time they log in.
    [OnEvent(nameof(PlayerMobile.PlayerLoginEvent))]
    public static void OnPlayerLogin(PlayerMobile pm)
    {
        if (pm?.Deleted == false && pm.AccessLevel == AccessLevel.Player)
        {
            ApplyCaps(pm, GetLevel(pm));
        }
    }

    public override void Serialize(IGenericWriter writer)
    {
        writer.WriteEncodedInt(1); // version

        writer.WriteEncodedInt(_playerLevels.Count);
        foreach (var (pm, context) in _playerLevels)
        {
            writer.Write(pm);
            writer.WriteEncodedInt(context.Level);
            writer.Write(context.XP);
            writer.Write(context.PrimerShown); // v1
        }
    }

    public override void Deserialize(IGenericReader reader)
    {
        var version = reader.ReadEncodedInt();

        var contextCount = reader.ReadEncodedInt();
        for (var i = 0; i < contextCount; i++)
        {
            var player = reader.ReadEntity<PlayerMobile>();
            var context = new LevelContext
            {
                Level = reader.ReadEncodedInt(),
                XP = reader.ReadLong()
            };
            // v1 added PrimerShown; short-circuit keeps v0 saves from reading a stray bool.
            context.PrimerShown = version >= 1 && reader.ReadBool();

            if (player != null && context.IsUsed())
            {
                _playerLevels.Add(player, context);
            }
        }
    }

    public static int GetLevel(PlayerMobile pm) =>
        pm != null && _playerLevels.TryGetValue(pm, out var context) ? context.Level : 0;

    public static long GetXP(PlayerMobile pm) =>
        pm != null && _playerLevels.TryGetValue(pm, out var context) ? context.XP : 0;

    private static LevelContext GetOrCreate(PlayerMobile pm)
    {
        ref var context = ref CollectionsMarshal.GetValueRefOrAddDefault(_playerLevels, pm, out var exists);
        if (!exists)
        {
            context = new LevelContext();
        }

        return context;
    }

    // Adds raw XP (the caller is responsible for any gap scaling) and processes any level-ups
    // it triggers. Staff are fully exempt: no context is created and no XP is tracked.
    public static void AwardXP(PlayerMobile pm, int amount)
    {
        if (pm == null || amount <= 0 || pm.AccessLevel > AccessLevel.Player)
        {
            return;
        }

        var maxXP = LevelConfig.XPToReach(LevelConfig.MaxLevel);
        var context = GetOrCreate(pm);

        if (context.Level >= LevelConfig.MaxLevel)
        {
            context.XP = maxXP; // Already max; clamp and stop.
            return;
        }

        var oldXP = context.XP;
        context.XP += amount;
        if (context.XP > maxXP)
        {
            context.XP = maxXP;
        }

        // Report what was actually applied, not what was requested — near the level-10
        // clamp the two can differ.
        var gained = context.XP - oldXP;
        if (gained > 0)
        {
            pm.SendMessage($"You have gained {gained} experience.");
        }

        var newLevel = LevelConfig.LevelForXP(context.XP);
        if (newLevel <= context.Level)
        {
            return;
        }

        // Apply every level crossed in this award (XP may span several thresholds).
        for (var level = context.Level + 1; level <= newLevel; level++)
        {
            ApplyLevelUp(pm, level);
        }

        context.Level = newLevel;

        // One celebratory effect regardless of how many levels were crossed.
        pm.FixedParticles(0x373A, 1, 15, 5012, 3, 2, EffectLayer.Waist);
        pm.PlaySound(0x1E0);

        // First ding ever: explain the system once. Reopenable later via [levelguide.
        if (!context.PrimerShown)
        {
            context.PrimerShown = true;
            pm.SendGump(new LevelingPrimerGump());
        }
    }

    // GM testing hook (Commands/LevelTestCommands.cs): sets a player's level directly, bypassing
    // the XP curve. Raising replays ApplyLevelUp for every level crossed (stat top-up, caps, and
    // the level-4 coin/bolt path all fire exactly as they would from real XP); lowering just resets
    // the level and caps without re-triggering those one-time effects.
    public static void SetLevel(PlayerMobile pm, int level)
    {
        if (pm == null)
        {
            return;
        }

        level = Math.Clamp(level, 0, LevelConfig.MaxLevel);
        var context = GetOrCreate(pm);
        var oldLevel = context.Level;

        if (level > oldLevel)
        {
            for (var lvl = oldLevel + 1; lvl <= level; lvl++)
            {
                ApplyLevelUp(pm, lvl);
            }
        }
        else
        {
            ApplyCaps(pm, level);
        }

        context.Level = level;
        context.XP = LevelConfig.XPToReach(level);
    }

    // Applies the stat top-up (levels 1..5 only) and the level's caps, then notifies the player.
    private static void ApplyLevelUp(PlayerMobile pm, int level)
    {
        if (level <= 5)
        {
            var delta = LevelConfig.StatThreshold(level) - pm.RawStatTotal;
            if (delta > 0)
            {
                var (strInc, dexInc, intInc) = LevelConfig.DistributeTopUp(
                    pm.RawStr,
                    pm.RawDex,
                    pm.RawInt,
                    pm.StrLock == StatLockType.Up,
                    pm.DexLock == StatLockType.Up,
                    pm.IntLock == StatLockType.Up,
                    delta
                );

                if (strInc > 0)
                {
                    pm.RawStr += strInc;
                }

                if (dexInc > 0)
                {
                    pm.RawDex += dexInc;
                }

                if (intInc > 0)
                {
                    pm.RawInt += intInc;
                }
            }
        }

        ApplyCaps(pm, level);

        pm.SendMessage($"You have reached level {level}!");
        pm.LocalOverheadMessage(MessageType.Regular, 0x35, false, $"LEVEL {level}");
        BuffHelper.AddCustomBuff(pm, BuffIcon.ArcaneEmpowerment, $"Level {level}", TimeSpan.FromSeconds(5));

        FerrymansTollHooks.OnLevelUp(pm, level);

        // Newbie Dungeon graduation (dev-docs/newbie-dungeon.md §5): fires once per character,
        // the level the entry gate's XP curve first grays out L1 mobs entirely.
        if (level == 4)
        {
            Effects.SendBoltEffect(pm);

            var hasCoin = pm.Backpack?.FindItemByType<FerrymansCoin>() != null ||
                          pm.BankBox?.FindItemByType<FerrymansCoin>() != null;
            if (!hasCoin)
            {
                pm.AddToBackpack(new FerrymansCoin(pm.RawName));
                pm.SendMessage(
                    "The Ferryman presses a cold coin into your hand. 'Not yet your time. Go up — and remember the road down.'"
                );
            }
        }
    }

    // Sets the StatCap and every per-skill Skill.Cap for the given level. Public so character
    // creation can seed a new non-staff player at level 0.
    public static void ApplyCaps(PlayerMobile pm, int level)
    {
        if (pm == null)
        {
            return;
        }

        pm.StatCap = LevelConfig.StatCapFor(level);

        var skillCap = LevelConfig.SkillCapFor(level);
        var skills = pm.Skills;
        for (var i = 0; i < skills.Length; i++)
        {
            skills[i].Cap = skillCap;
        }
    }

    // Splits a killed creature's XP among the players holding looting rights: proportional to
    // damage dealt, then scaled per player by the level gap. Reuses the fame code's rights list;
    // pet damage is already credited to the master and minimum-damage thresholds already applied.
    public static void DistributeXP(BaseCreature killed, List<DamageStore> rights)
    {
        if (killed == null || rights == null || rights.Count == 0)
        {
            return;
        }

        var mobLevel = LevelConfig.GetMobLevel(killed);
        var baseXP = LevelConfig.BaseMobXP(mobLevel);
        if (baseXP <= 0)
        {
            return;
        }

        var totalDamage = 0;
        for (var i = 0; i < rights.Count; i++)
        {
            var ds = rights[i];
            if (ds.m_HasRight && ds.m_Mobile is PlayerMobile)
            {
                totalDamage += ds.m_Damage;
            }
        }

        if (totalDamage <= 0)
        {
            return;
        }

        for (var i = 0; i < rights.Count; i++)
        {
            var ds = rights[i];
            if (!ds.m_HasRight || ds.m_Mobile is not PlayerMobile pm)
            {
                continue;
            }

            var share = (double)ds.m_Damage / totalDamage * baseXP;
            var multiplier = LevelConfig.GapMultiplier(mobLevel, GetLevel(pm));
            var award = (int)(share * multiplier);

            if (award > 0)
            {
                AwardXP(pm, award);
            }
        }
    }
}
