using System;

namespace Server.Engines.LootBags;

// Drop tuning for level-tagged loot bags. Pure table so it is unit testable.
public static class LootBagConfig
{
    public const int MaxLevel = 10;

    // Index = mob level 0..10. L0 = trivial critters, never drop in the open world
    // (newbie dungeon will use its own region-scoped rule later). L1 = 0.05 from
    // design doc; the rest are placeholders to tune.
    private static readonly double[] _chanceByLevel =
    {
        0.0, 0.05, 0.07, 0.09, 0.11, 0.13, 0.16, 0.19, 0.22, 0.26, 0.30
    };

    public static double ChanceForMobLevel(int level) => _chanceByLevel[Math.Clamp(level, 0, MaxLevel)];
}
