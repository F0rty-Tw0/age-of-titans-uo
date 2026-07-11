using System;

namespace Server.Engines.LootBags;

// Drop tuning for level-tagged loot bags. Pure table so it is unit testable.
public static class LootBagConfig
{
    public const int MaxLevel = 10;

    // Index = mob level 0..10. L0 = trivial critters, never drop in the open world
    // (newbie dungeon will use its own region-scoped rule later). Convex ramp
    // (user directive 2026-07-09): L1 10% -> L10 50%, back-loaded so L8-10 feel
    // like the payoff. Tune live.
    private static readonly double[] _chanceByLevel =
    {
        0.0, 0.10, 0.13, 0.16, 0.20, 0.24, 0.29, 0.34, 0.39, 0.44, 0.50
    };

    public static double ChanceForMobLevel(int level) => _chanceByLevel[Math.Clamp(level, 0, MaxLevel)];
}
