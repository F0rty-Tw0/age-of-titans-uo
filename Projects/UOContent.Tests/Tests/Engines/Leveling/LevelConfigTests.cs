using Server.Engines.Leveling;
using Xunit;

namespace UOContent.Tests;

public class LevelConfigTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)] // TESTING: L1 at FirstLevelXP = 1
    [InlineData(2999, 1)]
    [InlineData(3000, 2)]
    [InlineData(5999, 2)]
    [InlineData(6000, 3)]
    [InlineData(54999, 9)]
    [InlineData(55000, 10)]
    [InlineData(999999, 10)]
    public void LevelForXP_MapsCumulativeXpToLevel(long xp, int expected)
    {
        Assert.Equal(expected, LevelConfig.LevelForXP(xp));
    }

    [Theory]
    [InlineData(1, 1)] // TESTING: FirstLevelXP knob, production 1000
    [InlineData(2, 3000)]
    [InlineData(3, 6000)]
    [InlineData(5, 15000)]
    [InlineData(10, 55000)]
    [InlineData(0, 0)]
    [InlineData(11, 55000)] // clamped to MaxLevel
    public void XPToReach_IsCumulativeTriangular(int level, long expected)
    {
        Assert.Equal(expected, LevelConfig.XPToReach(level));
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(1, 150)]
    [InlineData(2, 200)]
    [InlineData(3, 250)]
    [InlineData(4, 300)]
    [InlineData(5, 300)]
    [InlineData(10, 300)]
    [InlineData(-1, 100)] // clamped to level 0
    public void StatCapFor_UsesMinLevelFourTable(int level, int expected)
    {
        Assert.Equal(expected, LevelConfig.StatCapFor(level));
    }

    [Theory]
    [InlineData(0, 50.0)]
    [InlineData(1, 60.0)]
    [InlineData(2, 70.0)]
    [InlineData(3, 80.0)]
    [InlineData(4, 90.0)]
    [InlineData(5, 100.0)]
    [InlineData(6, 100.0)]
    [InlineData(10, 100.0)]
    public void SkillCapFor_TableThenHundred(int level, double expected)
    {
        Assert.Equal(expected, LevelConfig.SkillCapFor(level));
    }

    [Theory]
    // gap = mobLevel - playerLevel
    [InlineData(2, 5, 0.0)]  // gap -3
    [InlineData(1, 5, 0.0)]  // gap -4
    [InlineData(3, 5, 0.25)] // gap -2
    [InlineData(4, 5, 0.5)]  // gap -1
    [InlineData(5, 5, 1.0)]  // gap 0
    [InlineData(6, 5, 1.25)] // gap +1
    [InlineData(7, 5, 1.5)]  // gap +2
    [InlineData(10, 5, 1.5)] // gap +5
    public void GapMultiplier_MatchesTable(int mobLevel, int playerLevel, double expected)
    {
        Assert.Equal(expected, LevelConfig.GapMultiplier(mobLevel, playerLevel));
    }

    [Theory]
    [InlineData(30, 1)]
    [InlineData(31, 2)]
    [InlineData(60, 2)]
    [InlineData(61, 3)]
    [InlineData(115, 3)]
    [InlineData(116, 4)]
    [InlineData(200, 4)]
    [InlineData(325, 5)]
    [InlineData(500, 6)]
    [InlineData(750, 7)]
    [InlineData(1100, 8)]
    [InlineData(1600, 9)]
    [InlineData(1601, 10)]
    [InlineData(99999, 10)]
    public void MobLevelFromHits_Buckets(int hitsMax, int expected)
    {
        Assert.Equal(expected, LevelConfig.MobLevelFromHits(hitsMax));
    }

    [Theory]
    // str,dex,int, strUp,dexUp,intUp, delta, expStr,expDex,expInt
    // All three locked Up: even round-robin.
    [InlineData(50, 50, 50, true, true, true, 9, 3, 3, 3)]
    // All three Up with a remainder: extra points land on Str first, then Dex.
    [InlineData(50, 50, 50, true, true, true, 10, 4, 3, 3)]
    // Single Up (Str only): everything goes to Str.
    [InlineData(50, 50, 50, true, false, false, 5, 5, 0, 0)]
    // No stat marked Up: all three are candidates.
    [InlineData(50, 50, 50, false, false, false, 6, 2, 2, 2)]
    // Single Up hits the 200 per-stat cap, then spills round-robin into the rest.
    [InlineData(198, 50, 50, true, false, false, 10, 2, 4, 4)]
    // Candidate near cap is skipped once maxed while others keep filling.
    [InlineData(199, 50, 50, true, true, true, 6, 1, 3, 2)]
    // All candidates already at 200: everything spills to the remaining stat.
    [InlineData(200, 200, 50, true, true, false, 4, 0, 0, 4)]
    // delta <= 0 hands out nothing.
    [InlineData(50, 50, 50, true, true, true, 0, 0, 0, 0)]
    [InlineData(50, 50, 50, true, true, true, -5, 0, 0, 0)]
    public void DistributeTopUp_DistributesPoints(
        int str, int dex, int intel,
        bool strUp, bool dexUp, bool intUp,
        int delta,
        int expStr, int expDex, int expInt
    )
    {
        var (strInc, dexInc, intInc) = LevelConfig.DistributeTopUp(str, dex, intel, strUp, dexUp, intUp, delta);

        Assert.Equal(expStr, strInc);
        Assert.Equal(expDex, dexInc);
        Assert.Equal(expInt, intInc);
    }
}
