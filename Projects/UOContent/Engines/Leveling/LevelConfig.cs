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

    // Hand-tuning hook: shipped empty. When a type is present it wins over the HP
    // heuristic in GetMobLevel.
    public static readonly Dictionary<Type, int> MobLevelOverrides = new();

    // TESTING knob: first ding after a single kill. Production: delete this constant
    // and the level == 1 branch below so L1 costs 1000 like the formula says.
    public const long FirstLevelXP = 1;

    // Cumulative XP needed to reach the given level. Cost from N-1 to N is N * 1000,
    // so the cumulative total is 1000 * N(N+1)/2. L1 = 1000 .. L10 = 55000.
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

        if (level == 1)
        {
            return FirstLevelXP;
        }

        return 1000L * level * (level + 1) / 2;
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

    // v1 mob level from max hit points.
    public static int MobLevelFromHits(int hitsMax) =>
        hitsMax switch
        {
            <= 30   => 1,
            <= 60   => 2,
            <= 115  => 3,
            <= 200  => 4,
            <= 325  => 5,
            <= 500  => 6,
            <= 750  => 7,
            <= 1100 => 8,
            <= 1600 => 9,
            _       => 10
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
