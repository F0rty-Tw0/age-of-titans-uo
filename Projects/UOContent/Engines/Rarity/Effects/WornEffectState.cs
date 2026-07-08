using System;
using System.Collections.Generic;
using Server.Collections;
using Server.Items;

namespace Server.Engines.Rarity;

// Suit-wide totals after framework §9.4 stacking (strongest same-root instance counts full
// weight, every other same-root instance counts at half weight) and the §9.8 shared-pool
// clamps. Bonus AR and per-piece weight reduction are NOT included here — both are applied
// directly on the item itself (see RarityEffects.GetBonusArmorRating / ApplyVariant), per the
// P3a architecture call. Flagged: framework §9.8 also lists "bonus AR ≤ 15" as a suit-wide cap,
// which per-piece application does not enforce; a full Polias suit can exceed it. Not
// reconciled here — surfaced for the balance pass.
//
// P3b extends this aggregate with jewelry (Olympian/Hecatean/Tychean/Nyxian/Demetrian) and
// clothing (Laurel/Charis/Maenad/Hestian/Arachne) fields. Two P3a mechanics are reused rather
// than duplicated: Tychean's "incoming hit halved" feeds the SAME ShrugPct field/cap as
// Polias/Aegis shrug (mechanically identical roll), and Arachne's dodge feeds the SAME DodgePct
// field/cap as Talarian dodge (framework §9.8's shared pools).
public readonly struct WornAggregate
{
    public int DrPct { get; init; }
    public int ShrugPct { get; init; }
    public int ReflectPct { get; init; }
    public int FlameProcPct { get; init; }
    public bool SelfRepair { get; init; }
    public int HpRegenPct { get; init; }
    public int HealsReceivedPct { get; init; }
    public bool AutoCure { get; init; }
    public int SpellDrPct { get; init; }
    public int ParaResistPct { get; init; }
    public int ResistSkillBonus { get; init; }
    public int StamRegenPct { get; init; }
    public int DodgePct { get; init; }
    public int ParryPct { get; init; }
    public int ParryDrPct { get; init; }
    public bool ParryThorns { get; init; }

    // ---- P3b: jewelry ----
    public int LightningProcPct { get; init; }
    public int ManaRegenPct { get; init; }
    public int SpellDamagePct { get; init; }
    public int ManaLeechPct { get; init; }
    public int MissRerollPct { get; init; }
    public int HidingBonus { get; init; }
    public int StealthBonus { get; init; }
    public bool NightSight { get; init; }
    public int PoisonResistPct { get; init; }
    public int PotionEffectPct { get; init; }

    // ---- P3b: clothing ----
    public int OnKillStamina { get; init; }
    public int OnKillHp { get; init; }
    public int KarmaGainPct { get; init; }
    public int VendorPricePct { get; init; }
    public int FrenzyChancePct { get; init; }
    public int FrenzyDamagePct { get; init; }
    public int FrenzySwingPct { get; init; }
    public int StationaryRegenPct { get; init; }
    public bool StationaryAppliesMana { get; init; }
}

// Per-Mobile aggregate of every worn variant armor/shield/jewelry/clothing item, rebuilt on
// OnWornAdded/OnWornRemoved. Single-threaded plain dictionaries (CLAUDE.md rule 3) — evicted on
// death/delete via RarityEffects.OnMobileGone, same as CombatFxState.
public static class WornEffectState
{
    // §9.4 stacking is keyed by mechanic StackGroup, not raw root: the strongest piece in a
    // group counts full, every other piece in that group at half, so cross-material same-mechanic
    // pieces (e.g. a Talarian suit + an Ophis weapon) pool instead of each counting in full.
    private const int GroupCount = VariantRootInfo.StackGroupCount;

    // Suit-wide hard ceilings (framework §9.8).
    private const int DrCap = 12;
    private const int ShrugCap = 20;
    private const int ReflectCap = 25;
    private const int HpRegenCap = 60;
    private const int SpellDrCap = 18;
    private const int DodgeCap = 12;

    private static readonly Dictionary<Mobile, WornAggregate> _aggregates = new();
    private static readonly Dictionary<Mobile, List<LegendaryEntry>> _legendaries = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _resistSkillMods = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _hidingSkillMods = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _stealthSkillMods = new();
    private static readonly Dictionary<Mobile, DefaultSkillMod> _animalTamingSkillMods = new();

    // Klotho (body sash relic): a stand-alone stacking burst (max 2 stacks). Narrow enough — one
    // relic, one behavior — that generalizing the ArmClauseBurst API below for it would be
    // over-engineering for a single caller.
    private static readonly Dictionary<Mobile, (int Stacks, long ExpiryTick)> _klothoStacks = new();

    // Timed "burst" windows for clauses that grant a temporary bonus after a trigger (crit
    // taken, dodge, etc). Keyed by clause so unrelated bursts on the same mobile (e.g. a
    // Talarian dodge burst and a Tritonian resist burst) never collide.
    private static readonly Dictionary<(Mobile, ClauseType), long> _clauseBursts = new();

    // Once-per-fight flags for clauses gated to a single trigger per combat window (framework
    // §9 rule 7 — resets only after 30s out of combat). Reuses CombatFxState's fight-freshness
    // read so "once per fight" always lines up with the same window as "first hit of fight".
    private static readonly HashSet<(Mobile, ClauseType)> _usedOncePerFight = new();

    // Hyperbios: armed by AdjustShieldParryChance when the wearer's first hit of the fight is
    // about to resolve, consumed by ApplyMark so that hit applies no mark/poison. A single-hit
    // pending flag (not a burst window) — always cleared by the consuming read.
    private static readonly HashSet<Mobile> _suppressSecondaryEffect = new();

    public static WornAggregate GetAggregate(Mobile wearer) =>
        wearer != null && _aggregates.TryGetValue(wearer, out var agg) ? agg : default;

    public static IReadOnlyList<LegendaryEntry> GetLegendaries(Mobile wearer) =>
        wearer != null && _legendaries.TryGetValue(wearer, out var list) ? list : Array.Empty<LegendaryEntry>();

    // Rebuilds the wearer's aggregate + legendary list from scratch by walking their worn
    // items (armor/shields, then jewelry/clothing). Called after any piece is added or removed.
    public static void Rebuild(Mobile wearer)
    {
        if (wearer?.Items == null)
        {
            return;
        }

        Span<int> strongestRarity = stackalloc int[GroupCount];
        strongestRarity.Clear();

        using var armorItems = PooledRefList<(VariantRoot Root, ItemRarity Rarity, ArmorEffectRow Row)>.Create();
        using var weaponItems = PooledRefList<(VariantRoot Root, ItemRarity Rarity, WeaponEffectRow Row)>.Create();
        using var accessoryItems =
            PooledRefList<(VariantRoot Root, ItemRarity Rarity, AccessoryEffectRow Row, Item Source)>.Create();
        List<LegendaryEntry> legendaries = null;

        var wornItems = wearer.Items;

        for (var i = 0; i < wornItems.Count; i++)
        {
            var worn = wornItems[i];

            if (worn is BaseArmor armor && armor is IVariantItem armorVariant &&
                (armorVariant.VariantRoot != VariantRoot.None || armorVariant.LegendaryId != 0))
            {
                var (root, rarity) = RarityEffects.ResolveRootRarity(armorVariant, armor.Rarity);
                var row = ArmorEffectTable.Get(root, rarity, armor is BaseShield);

                armorItems.Add((root, rarity, row));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (armorVariant.LegendaryId != 0 && LegendaryRegistry.TryGet(armorVariant.LegendaryId, out var armorEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(armorEntry);
                }
            }
            else if (worn is BaseJewel jewel && jewel is IVariantItem jewelVariant &&
                     (jewelVariant.VariantRoot != VariantRoot.None || jewelVariant.LegendaryId != 0))
            {
                var (root, rarity) = RarityEffects.ResolveRootRarity(jewelVariant, jewel.Rarity);
                var row = AccessoryEffectTable.Get(root, rarity, isClothing: false);

                accessoryItems.Add((root, rarity, row, jewel));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (jewelVariant.LegendaryId != 0 && LegendaryRegistry.TryGet(jewelVariant.LegendaryId, out var jewelEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(jewelEntry);
                }
            }
            else if (worn is BaseClothing clothing && clothing is IVariantItem clothingVariant &&
                     (clothingVariant.VariantRoot != VariantRoot.None || clothingVariant.LegendaryId != 0))
            {
                var (root, rarity) = RarityEffects.ResolveRootRarity(clothingVariant, clothing.Rarity);
                var row = AccessoryEffectTable.Get(root, rarity, isClothing: true);

                accessoryItems.Add((root, rarity, row, clothing));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (clothingVariant.LegendaryId != 0 &&
                    LegendaryRegistry.TryGet(clothingVariant.LegendaryId, out var clothingEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(clothingEntry);
                }
            }
            else if (worn is BaseWeapon weapon && weapon is IVariantItem weaponVariant &&
                     (weaponVariant.VariantRoot != VariantRoot.None || weaponVariant.LegendaryId != 0))
            {
                // A held weapon folds only its worn-side utility (spell DR / mana regen / dodge /
                // resist skill) into the aggregate, plus its legendary + lane signature. BaseShield
                // is a BaseArmor and is already handled above, so this is real weapons only.
                var (root, rarity) = RarityEffects.ResolveRootRarity(weaponVariant, weapon.Rarity);
                var row = WeaponEffectTable.Get(root, rarity);

                weaponItems.Add((root, rarity, row));

                var group = VariantRootInfo.GetStackGroup(root);

                if ((int)rarity > strongestRarity[group])
                {
                    strongestRarity[group] = (int)rarity;
                }

                if (weaponVariant.LegendaryId != 0 && LegendaryRegistry.TryGet(weaponVariant.LegendaryId, out var weaponEntry))
                {
                    (legendaries ??= new List<LegendaryEntry>()).Add(weaponEntry);
                }
            }
        }

        var dr = 0;
        var shrug = 0;
        var reflect = 0;
        var flameProc = 0;
        var selfRepair = false;
        var hpRegen = 0;
        var healsReceived = 0;
        var autoCure = false;
        var spellDr = 0;
        var paraResist = 0;
        var resistSkill = 0;
        var stamRegen = 0;
        var dodge = 0;
        var parry = 0;
        var parryDr = 0;
        var parryThorns = false;

        var lightningProc = 0;
        var manaRegen = 0;
        var spellDamage = 0;
        var manaLeech = 0;
        var missReroll = 0;
        var hidingBonus = 0;
        var stealthBonus = 0;
        var nightSight = false;
        var poisonResist = 0;
        var potionEffect = 0;
        var onKillStamina = 0;
        var onKillHp = 0;
        var karmaGain = 0;
        var vendorPrice = 0;
        var frenzyChance = 0;
        var frenzyDamage = 0;
        var frenzySwing = 0;
        var stationaryRegen = 0;
        var stationaryAppliesMana = false;

        Span<int> statTotals = stackalloc int[3]; // 0 = Str, 1 = Dex, 2 = Int (Olympian)
        statTotals.Clear();

        // Only ONE instance per stack-group counts at full weight — ties at the same rarity (e.g.
        // two Legendary Talarian pieces, or a Talarian piece + an Ophis weapon) still pick just one
        // "strongest", not award both 100%. Shared across armor, weapons, and accessories.
        Span<bool> fullWeightConsumed = stackalloc bool[GroupCount];
        fullWeightConsumed.Clear();

        for (var i = 0; i < armorItems.Count; i++)
        {
            var (root, rarity, row) = armorItems[i];
            var group = VariantRootInfo.GetStackGroup(root);
            var isStrongest = !fullWeightConsumed[group] && (int)rarity == strongestRarity[group];

            if (isStrongest)
            {
                fullWeightConsumed[group] = true;
                AppendSignature(ref legendaries, root, row.Signature, row.S1, row.S2, row.S3);
            }

            var weight = isStrongest ? 100 : 50;

            dr += row.DrPct * weight / 100;
            shrug += row.ShrugPct * weight / 100;
            reflect += row.ReflectPct * weight / 100;
            flameProc += row.FlameProcPct * weight / 100;
            selfRepair |= row.SelfRepair;
            hpRegen += row.HpRegenPct * weight / 100;
            healsReceived += row.HealsReceivedPct * weight / 100;
            autoCure |= row.AutoCure;
            spellDr += row.SpellDrPct * weight / 100;
            paraResist += row.ParaResistPct * weight / 100;
            resistSkill += row.ResistSkillBonus * weight / 100;
            stamRegen += row.StamRegenPct * weight / 100;
            dodge += row.DodgePct * weight / 100;
            parry += row.ParryPct * weight / 100; // single shield slot — weight is always 100
            parryDr += row.ParryDrPct * weight / 100;
            parryThorns |= row.ParryThorns;

            // P2 re-theme per-material fields fold into the existing aggregate accumulators.
            poisonResist += row.PoisonResistPct * weight / 100;
            hidingBonus += row.HidingBonus * weight / 100;
            onKillStamina += row.OnKillStamPct * weight / 100;
            onKillHp += row.OnKillHpPct * weight / 100;
        }

        for (var i = 0; i < accessoryItems.Count; i++)
        {
            var (root, rarity, row, source) = accessoryItems[i];
            var group = VariantRootInfo.GetStackGroup(root);
            var isStrongest = !fullWeightConsumed[group] && (int)rarity == strongestRarity[group];

            if (isStrongest)
            {
                fullWeightConsumed[group] = true;
            }

            var weight = isStrongest ? 100 : 50;

            // Olympian: the rolled stat (persisted on the item, BaseJewel.OlympianStat) receives
            // the weighted magnitude.
            if (root == VariantRoot.Olympian && source is BaseJewel jewel)
            {
                var statContribution = row.StatBonus * weight / 100;
                statTotals[StatIndex(jewel.OlympianStat)] += statContribution;

                // Aither: half of THIS item's own contribution also splashes to a second stat.
                // The doc says "your choice" — there's no player-choice primitive at this layer,
                // so a deterministic Str->Dex->Int->Str rotation stands in for it (flagged in the
                // P3b report).
                if (jewel is IVariantItem aitherVariant && aitherVariant.LegendaryId != 0 &&
                    LegendaryRegistry.TryGet(aitherVariant.LegendaryId, out var aitherEntry) &&
                    aitherEntry.Clause == ClauseType.StatBonusSplashSecondStat)
                {
                    statTotals[StatIndex(NextStat(jewel.OlympianStat))] += statContribution / 2;
                }
            }

            lightningProc += row.LightningProcPct * weight / 100;
            shrug += row.HitHalvedPct * weight / 100; // Tychean shares the shrug pool/cap
            dodge += row.DodgePct * weight / 100;       // Arachne shares the dodge pool/cap
            hpRegen += row.AllRegenPct * weight / 100;  // Demetrian all-regen
            stamRegen += row.AllRegenPct * weight / 100;
            manaRegen += (row.ManaRegenPct + row.AllRegenPct) * weight / 100;
            spellDamage += row.SpellDamagePct * weight / 100;
            manaLeech += row.ManaLeechPct * weight / 100;
            missReroll += row.MissRerollPct * weight / 100;
            hidingBonus += row.HidingBonus * weight / 100;
            stealthBonus += row.StealthBonus * weight / 100;
            nightSight |= row.NightSight;
            poisonResist += row.PoisonResistPct * weight / 100;
            potionEffect += row.PotionEffectPct * weight / 100;
            onKillStamina += row.OnKillStamina * weight / 100;
            onKillHp += row.OnKillHp * weight / 100;
            karmaGain += row.KarmaGainPct * weight / 100;
            vendorPrice += row.VendorPricePct * weight / 100;
            frenzyChance += row.FrenzyChancePct * weight / 100;
            frenzyDamage += row.FrenzyDamagePct * weight / 100;
            frenzySwing += row.FrenzySwingPct * weight / 100;
            stationaryRegen += row.StationaryRegenPct * weight / 100;
            stationaryAppliesMana |= row.StationaryAppliesMana;
        }

        for (var i = 0; i < weaponItems.Count; i++)
        {
            var (root, rarity, row) = weaponItems[i];
            var group = VariantRootInfo.GetStackGroup(root);
            var isStrongest = !fullWeightConsumed[group] && (int)rarity == strongestRarity[group];

            if (isStrongest)
            {
                fullWeightConsumed[group] = true;
                AppendSignature(ref legendaries, root, row.Signature, row.S1, row.S2, row.S3);
            }

            var weight = isStrongest ? 100 : 50;

            // A held weapon contributes ONLY its worn-side utility lanes (staves/fencing/archery
            // defensive lanes) to the aggregate; every offensive field stays on the hit pipeline.
            spellDr += row.SpellDrPct * weight / 100;
            manaRegen += row.ManaRegenPct * weight / 100;
            dodge += row.DodgePct * weight / 100;
            resistSkill += row.ResistSkillBonus * weight / 100;
            healsReceived += row.HealsReceivedPct * weight / 100; // Manteia staff lane
            autoCure |= row.AutoCure;
        }

        _aggregates[wearer] = new WornAggregate
        {
            DrPct = Math.Min(dr, DrCap),
            ShrugPct = Math.Min(shrug, ShrugCap),
            ReflectPct = Math.Min(reflect, ReflectCap),
            FlameProcPct = flameProc,
            SelfRepair = selfRepair,
            HpRegenPct = Math.Min(hpRegen, HpRegenCap),
            HealsReceivedPct = healsReceived,
            AutoCure = autoCure,
            SpellDrPct = Math.Min(spellDr, SpellDrCap),
            ParaResistPct = paraResist,
            ResistSkillBonus = resistSkill,
            StamRegenPct = stamRegen,
            DodgePct = Math.Min(dodge, DodgeCap),
            ParryPct = parry,
            ParryDrPct = Math.Min(parryDr, DrCap),
            ParryThorns = parryThorns,

            LightningProcPct = lightningProc,
            ManaRegenPct = manaRegen,
            SpellDamagePct = spellDamage,
            ManaLeechPct = manaLeech,
            MissRerollPct = missReroll,
            HidingBonus = hidingBonus,
            StealthBonus = stealthBonus,
            NightSight = nightSight,
            PoisonResistPct = poisonResist,
            PotionEffectPct = potionEffect,

            OnKillStamina = onKillStamina,
            OnKillHp = onKillHp,
            KarmaGainPct = karmaGain,
            VendorPricePct = vendorPrice,
            FrenzyChancePct = frenzyChance,
            FrenzyDamagePct = frenzyDamage,
            FrenzySwingPct = frenzySwing,
            StationaryRegenPct = stationaryRegen,
            StationaryAppliesMana = stationaryAppliesMana
        };

        if (legendaries != null)
        {
            _legendaries[wearer] = legendaries;
        }
        else
        {
            _legendaries.Remove(wearer);
        }

        SyncResistSkillMod(wearer, resistSkill);
        SyncSkillMod(wearer, _hidingSkillMods, SkillName.Hiding, "RarityHiding", hidingBonus);
        SyncSkillMod(wearer, _stealthSkillMods, SkillName.Stealth, "RarityStealth", stealthBonus);
        SyncAnimalTamingSkillMod(wearer, legendaries);
        SyncOlympianStatMods(wearer, statTotals[0], statTotals[1], statTotals[2]);
        SyncNightSight(wearer, nightSight);
    }

    // Appends a synthetic legendary entry carrying a worn piece's lane signature (Id 0, no name,
    // Family 0xFF) so the ~15 worn-side clause-dispatch sites pick it up unchanged. Called only for
    // the strongest piece per stack-group, so each group contributes its signature at most once.
    // Internal so the synthesis shape can be unit-tested while every production row still leaves
    // Signature = None this phase.
    internal static void AppendSignature(
        ref List<LegendaryEntry> legendaries, VariantRoot root, ClauseType signature, short s1, short s2, short s3
    )
    {
        if (signature == ClauseType.None)
        {
            return;
        }

        (legendaries ??= new List<LegendaryEntry>()).Add(
            new LegendaryEntry(0, null, root, 0xFF, 0, signature, s1, s2, s3, 0)
        );
    }

    private static int StatIndex(StatType type) => type switch
    {
        StatType.Dex => 1,
        StatType.Int => 2,
        _            => 0
    };

    private static StatType NextStat(StatType type) => type switch
    {
        StatType.Str => StatType.Dex,
        StatType.Dex => StatType.Int,
        _            => StatType.Str
    };

    private static void SyncOlympianStatMods(Mobile wearer, int strBonus, int dexBonus, int intBonus)
    {
        // StatMod offsets are immutable once created (no public setter) — remove-and-recreate is
        // the existing engine pattern (mirrors BaseJewel's own AOS stat-bonus wiring) and only
        // runs on equip/unequip (Rebuild), not per-tick, so the churn is cheap.
        wearer.RemoveStatMod("RarityOlympianStr");
        wearer.RemoveStatMod("RarityOlympianDex");
        wearer.RemoveStatMod("RarityOlympianInt");

        if (strBonus != 0)
        {
            wearer.AddStatMod(new StatMod(StatType.Str, "RarityOlympianStr", strBonus, TimeSpan.Zero));
        }

        if (dexBonus != 0)
        {
            wearer.AddStatMod(new StatMod(StatType.Dex, "RarityOlympianDex", dexBonus, TimeSpan.Zero));
        }

        if (intBonus != 0)
        {
            wearer.AddStatMod(new StatMod(StatType.Int, "RarityOlympianInt", intBonus, TimeSpan.Zero));
        }

        wearer.CheckStatTimers();
    }

    private static void SyncNightSight(Mobile wearer, bool nightSight)
    {
        if (nightSight)
        {
            wearer.LightLevel = LightCycle.DungeonLevel;
        }
        else if (wearer.LightLevel == LightCycle.DungeonLevel)
        {
            // ponytail: best-effort — only clears OUR contribution, and only when nothing else
            // has since changed it. A real Night Sight spell/potion active at the same instant
            // would be cut short by doffing Nyxian jewelry; flagged, not fixed (reconciling with
            // every personal-light source is a much larger change than this family needs).
            wearer.LightLevel = 0;
        }
    }

    private static void SyncResistSkillMod(Mobile wearer, int bonus) =>
        SyncSkillMod(wearer, _resistSkillMods, SkillName.MagicResist, "RarityResistingSpells", bonus);

    private static void SyncAnimalTamingSkillMod(Mobile wearer, List<LegendaryEntry> legendaries)
    {
        var bonus = 0;

        if (legendaries != null)
        {
            for (var i = 0; i < legendaries.Count; i++)
            {
                if (legendaries[i].Clause == ClauseType.AnimalTamingSkillBonus)
                {
                    bonus = legendaries[i].P1 > 0 ? legendaries[i].P1 : 15; // Lachesis
                }
            }
        }

        SyncSkillMod(wearer, _animalTamingSkillMods, SkillName.AnimalTaming, "RarityAnimalTaming", bonus);
    }

    private static void SyncSkillMod(Mobile wearer, Dictionary<Mobile, DefaultSkillMod> table, SkillName skill, string name, int bonus)
    {
        table.TryGetValue(wearer, out var mod);

        if (bonus <= 0)
        {
            if (mod != null)
            {
                mod.Remove();
                table.Remove(wearer);
            }

            return;
        }

        if (mod == null)
        {
            mod = new DefaultSkillMod(skill, name, true, bonus);
            wearer.AddSkillMod(mod);
            table[wearer] = mod;
        }
        else
        {
            mod.Value = bonus;
        }
    }

    // Glaukos: overrides the synced Resisting Spells bonus with a conditional value (e.g. while
    // below 50% HP), checked each HP-regen tick by RarityEffects.AdjustHitsRegenRate since there
    // is no per-mobile "HP changed" event to react to instantly.
    public static void BoostResistSkill(Mobile wearer, int value)
    {
        if (wearer != null && _resistSkillMods.TryGetValue(wearer, out var mod))
        {
            mod.Value = value;
        }
    }

    public static void ArmClauseBurst(Mobile wearer, ClauseType clause, TimeSpan duration)
    {
        if (wearer != null)
        {
            _clauseBursts[(wearer, clause)] = Core.TickCount + (long)duration.TotalMilliseconds;
        }
    }

    public static bool IsClauseBurstActive(Mobile wearer, ClauseType clause) =>
        wearer != null && _clauseBursts.TryGetValue((wearer, clause), out var until) && Core.TickCount < until;

    // Klotho: arms a fresh burst on the first kill, or stacks (max `maxStacks`) if a prior burst
    // is still active. Returns the resulting stack count.
    public static int ArmOrStackKlothoBurst(Mobile wearer, TimeSpan duration, int maxStacks)
    {
        if (wearer == null)
        {
            return 0;
        }

        var now = Core.TickCount;
        var stacks = 1;

        if (_klothoStacks.TryGetValue(wearer, out var cur) && now < cur.ExpiryTick)
        {
            stacks = Math.Min(cur.Stacks + 1, maxStacks);
        }

        _klothoStacks[wearer] = (stacks, now + (long)duration.TotalMilliseconds);
        return stacks;
    }

    public static int GetKlothoStacks(Mobile wearer) =>
        wearer != null && _klothoStacks.TryGetValue(wearer, out var cur) && Core.TickCount < cur.ExpiryTick
            ? cur.Stacks
            : 0;

    // Marks a once-per-fight clause as spent; returns false (and does nothing) if already used
    // this fight. Reset naturally once CombatFxState considers the fight stale (30s idle).
    public static bool TryUseOncePerFight(Mobile wearer, ClauseType clause, bool fightIsFresh)
    {
        if (wearer == null)
        {
            return false;
        }

        var key = (wearer, clause);

        if (fightIsFresh)
        {
            _usedOncePerFight.Remove(key);
        }

        if (!_usedOncePerFight.Add(key))
        {
            return false;
        }

        return true;
    }

    public static void ArmSecondaryEffectSuppression(Mobile wearer)
    {
        if (wearer != null)
        {
            _suppressSecondaryEffect.Add(wearer);
        }
    }

    public static bool ConsumeSecondaryEffectSuppression(Mobile wearer) =>
        wearer != null && _suppressSecondaryEffect.Remove(wearer);

    public static void Evict(Mobile wearer)
    {
        if (wearer == null)
        {
            return;
        }

        _aggregates.Remove(wearer);
        _legendaries.Remove(wearer);
        _suppressSecondaryEffect.Remove(wearer);
        _klothoStacks.Remove(wearer);

        RemoveSkillMod(wearer, _resistSkillMods);
        RemoveSkillMod(wearer, _hidingSkillMods);
        RemoveSkillMod(wearer, _stealthSkillMods);
        RemoveSkillMod(wearer, _animalTamingSkillMods);

        List<ClauseType> staleBursts = null;

        foreach (var key in _clauseBursts.Keys)
        {
            if (key.Item1 == wearer)
            {
                (staleBursts ??= new List<ClauseType>()).Add(key.Item2);
            }
        }

        if (staleBursts != null)
        {
            foreach (var clause in staleBursts)
            {
                _clauseBursts.Remove((wearer, clause));
            }
        }

        List<ClauseType> staleOnce = null;

        foreach (var key in _usedOncePerFight)
        {
            if (key.Item1 == wearer)
            {
                (staleOnce ??= new List<ClauseType>()).Add(key.Item2);
            }
        }

        if (staleOnce != null)
        {
            foreach (var clause in staleOnce)
            {
                _usedOncePerFight.Remove((wearer, clause));
            }
        }
    }

    private static void RemoveSkillMod(Mobile wearer, Dictionary<Mobile, DefaultSkillMod> table)
    {
        if (table.TryGetValue(wearer, out var mod))
        {
            mod.Remove();
            table.Remove(wearer);
        }
    }
}
