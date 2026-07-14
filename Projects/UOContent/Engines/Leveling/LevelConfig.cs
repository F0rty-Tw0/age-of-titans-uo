using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Engines.Leveling;

// Pure configuration + math for the player leveling system. No mobile state is
// mutated here; everything is a static table lookup or a pure function so it can
// be unit tested in isolation. All numbers are design-final.
public static class LevelConfig
{
    public const int MaxLevel = 10;

    // Never push a single stat past this during a level-up top-up.
    public const int PerStatCap = 200;

    // Total (RawStr + RawDex + RawInt) top-up target when reaching level 1..5.
    // Index i corresponds to level i + 1, so threshold(1) = 100 .. threshold(5) = 300.
    private static readonly int[] _statThresholds = { 100, 150, 200, 250, 300 };

    // Per-skill Skill.Cap by level 0..5 (100-scale). Level > 5 stays at 100.0.
    private static readonly double[] _skillCaps = { 50.0, 60.0, 70.0, 80.0, 90.0, 100.0 };

    // Hand-tuning hook. When a type is present it wins over the HP heuristic in GetMobLevel.
    public static readonly Dictionary<Type, int> MobLevelOverrides = new()
    {
        // Ambient / farm / pack animals: no XP, gray tag. Predators (wolves, bears,
        // panthers, snakes, scorpions) intentionally stay on the HP curve.
        [typeof(Bird)] = 0, [typeof(Chicken)] = 0, [typeof(Rabbit)] = 0, [typeof(JackRabbit)] = 0,
        [typeof(Cat)] = 0, [typeof(Dog)] = 0, [typeof(Rat)] = 0, [typeof(SewerRat)] = 0,
        [typeof(Goat)] = 0, [typeof(MountainGoat)] = 0, [typeof(Pig)] = 0, [typeof(Sheep)] = 0,
        [typeof(Cow)] = 0, [typeof(Bull)] = 0, [typeof(Boar)] = 0,
        [typeof(Horse)] = 0, [typeof(PackHorse)] = 0, [typeof(PackLlama)] = 0,
        [typeof(Llama)] = 0, [typeof(RidableLlama)] = 0,
        [typeof(Hind)] = 0, [typeof(GreatHart)] = 0,
        [typeof(Dolphin)] = 0, [typeof(Walrus)] = 0, [typeof(Squirrel)] = 0, [typeof(Ferret)] = 0,
        [typeof(Eagle)] = 0, [typeof(DesertOstard)] = 0, [typeof(ForestOstard)] = 0,

        // Casters punch above their HP: +1..2 over the HP curve.
        [typeof(EvilMage)] = 3, [typeof(EvilMageLord)] = 4,
        [typeof(SkeletalMage)] = 3, [typeof(BoneMagi)] = 3,
        [typeof(OrcishMage)] = 4, [typeof(RatmanMage)] = 4,
        [typeof(Gazer)] = 3, [typeof(ElderGazer)] = 6,
        [typeof(OphidianMage)] = 5, [typeof(OphidianArchmage)] = 6,
        [typeof(Lich)] = 5, [typeof(LichLord)] = 7, [typeof(AncientLich)] = 9,

        // Newbie dungeon (Barrow of the Unremembered): pinned regardless of HP tuning so
        // the [lvl N] tag, XP gap, and bag level stay stable as stats get balanced.
        [typeof(NewbieBoneShade)] = 1, [typeof(NewbieGraveRat)] = 1, [typeof(NewbieCorpseCrawler)] = 1,
        [typeof(NewbieGraveMiasma)] = 2, [typeof(NewbieRestlessArcher)] = 2,
        [typeof(NewbieCharon)] = 3, [typeof(NewbieFallenChampion)] = 3, [typeof(NewbieHollowWarden)] = 3
    };

    // Cumulative XP needed to reach each level (index 0 = level 1 .. index 9 = level 10).
    // Hand-tuned, not a formula: per-level cost = kill target x mob XP one level above the
    // player, where the +1 level gap applies the 1.25x GapMultiplier bonus and mob XP is
    // BaseMobXP(mobLevel). Kill targets by level: L1=30, L2=40, L3=60, L4=80, L5=100, L6=120,
    // L7=140, L8=160, L9=180, L10=200. E.g. L1 = 30 * 100 * 1.25 = 3750; the L10 leg alone is
    // 200 * 1000 * 1.25 = 250000, on top of the L1..L9 total for a 963750 cumulative.
    private static readonly long[] _cumulativeXP =
    {
        3_750, 13_750, 36_250, 76_250, 138_750, 228_750, 351_250, 511_250, 713_750, 963_750
    };

    // Cumulative XP needed to reach the given level.
    public static long XPToReach(int level)
    {
        if (level <= 0)
        {
            return 0;
        }

        if (level > MaxLevel)
        {
            level = MaxLevel;
        }

        return _cumulativeXP[level - 1];
    }

    // Level for a cumulative XP total, clamped to 0..MaxLevel.
    public static int LevelForXP(long xp)
    {
        for (var level = 1; level <= MaxLevel; level++)
        {
            if (xp < XPToReach(level))
            {
                return level - 1;
            }
        }

        return MaxLevel;
    }

    // StatCap at a given level: [100,150,200,250,300][Min(level, 4)].
    // L0 -> 100 (fresh char), L1 -> 150 ... L4+ -> 300. This lets passive stat
    // training run ahead of the level-up top-up.
    public static int StatCapFor(int level)
    {
        if (level < 0)
        {
            level = 0;
        }

        return _statThresholds[Math.Min(level, 4)];
    }

    // Top-up target total when reaching level 1..5. threshold(1) = 100 .. threshold(5) = 300.
    public static int StatThreshold(int level) => _statThresholds[Math.Clamp(level, 1, 5) - 1];

    // Per-skill cap for a level. Levels 0..5 use the table; level > 5 stays at 100.0.
    public static double SkillCapFor(int level)
    {
        if (level < 0)
        {
            level = 0;
        }

        return level <= 5 ? _skillCaps[level] : 100.0;
    }

    // Multiplier applied to a receiving player's XP share, keyed on gap = mobLevel - playerLevel.
    public static double GapMultiplier(int mobLevel, int playerLevel)
    {
        var gap = mobLevel - playerLevel;

        return gap switch
        {
            <= -3 => 0.0,
            -2    => 0.25,
            -1    => 0.5,
            0     => 1.0,
            1     => 1.25,
            _     => 1.5 // gap >= +2
        };
    }

    // Overhead-label hue for a mob's [lvl N] tag as seen by a player, keyed on the same
    // gap brackets as GapMultiplier. Level 0 mobs award no XP, so they always read gray.
    // Hues are placeholders to tune in-game (same spirit as the loot bag hue table).
    public static int GapHue(int mobLevel, int playerLevel)
    {
        if (mobLevel <= 0)
        {
            return 0x3B2; // gray — always 0 XP
        }

        return (mobLevel - playerLevel) switch
        {
            <= -3 => 0x3B2, // gray: trivial, 0x XP
            -2 or -1 => 0x3F,  // green: easy, reduced XP
            0 => 0x481, // white: even
            1 => 0x35,  // yellow: tough, 1.25x
            _ => 0x22   // red: danger, 1.5x (gap >= +2)
        };
    }

    // v2 mob level from real (post-T2A-scaling) max hit points. Minimum HP level is 1;
    // level 0 exists only via MobLevelOverrides pins (ambient/farm creatures).
    public static int MobLevelFromHits(int hitsMax) =>
        hitsMax switch
        {
            <= 65   => 1,  // mongbat, giant rat, slime, headless
            <= 100  => 2,  // zombie, skeleton, wolves
            <= 160  => 3,  // orc, ratman, low elementals
            <= 240  => 4,  // ogre, troll, lich
            <= 380  => 5,  // ore elementals, elder gazer, efreet
            <= 550  => 6,  // drake, daemon
            <= 720  => 7,  // titan, blood elemental, phoenix
            <= 950  => 8,  // dragon, white wyrm, ogre lord
            <= 2400 => 9,  // balron, ancient wyrm, hydra
            _       => 10  // future custom bosses, champion-tier
        };

    // Override table wins first; otherwise fall back to the HP heuristic.
    public static int GetMobLevel(BaseCreature bc)
    {
        if (bc == null)
        {
            return 1;
        }

        if (MobLevelOverrides.TryGetValue(bc.GetType(), out var level))
        {
            return level;
        }

        return MobLevelFromHits(bc.HitsMax);
    }

    // Base XP a mob is worth on death before per-player gap scaling.
    public static int BaseMobXP(int mobLevel) => mobLevel * 100;

    // Pure top-up distribution. Given the three current stat values, their "is locked Up"
    // flags, and the number of points to hand out, returns how many points each stat gains.
    //
    // Rules: distribute one point at a time, round-robin Str -> Dex -> Int, into stats whose
    // lock is Up and are below PerStatCap. If no stat is Up, all three are candidates. If every
    // candidate reaches PerStatCap with points left, spill into the remaining stats still below
    // the cap. delta <= 0 hands out nothing.
    public static (int strInc, int dexInc, int intInc) DistributeTopUp(
        int str, int dex, int intel,
        bool strUp, bool dexUp, bool intUp,
        int delta
    )
    {
        if (delta <= 0)
        {
            return (0, 0, 0);
        }

        // Explicit initializers matter: this assembly builds with SkipLocalsInit, so a bare
        // stackalloc is not zeroed. increments accumulates, so it must start at zero.
        Span<int> values = stackalloc int[3] { str, dex, intel };
        Span<int> increments = stackalloc int[3] { 0, 0, 0 };
        Span<bool> candidates = stackalloc bool[3] { strUp, dexUp, intUp };

        // No stat marked Up -> all three are eligible.
        if (!strUp && !dexUp && !intUp)
        {
            candidates[0] = true;
            candidates[1] = true;
            candidates[2] = true;
        }

        // Fill the candidate stats first, then spill the remainder into the rest.
        delta = FillRoundRobin(values, increments, candidates, delta);

        if (delta > 0)
        {
            Span<bool> spill = stackalloc bool[3];
            spill[0] = !candidates[0];
            spill[1] = !candidates[1];
            spill[2] = !candidates[2];
            FillRoundRobin(values, increments, spill, delta);
        }

        return (increments[0], increments[1], increments[2]);
    }

    // Round-robin one point at a time over eligible stats until delta runs out or every
    // eligible stat sits at PerStatCap. Returns the leftover delta.
    private static int FillRoundRobin(Span<int> values, Span<int> increments, ReadOnlySpan<bool> eligible, int delta)
    {
        while (delta > 0)
        {
            var placed = false;

            for (var i = 0; i < 3; i++)
            {
                if (delta <= 0)
                {
                    break;
                }

                if (eligible[i] && values[i] < PerStatCap)
                {
                    values[i]++;
                    increments[i]++;
                    delta--;
                    placed = true;
                }
            }

            if (!placed)
            {
                break;
            }
        }

        return delta;
    }
}
