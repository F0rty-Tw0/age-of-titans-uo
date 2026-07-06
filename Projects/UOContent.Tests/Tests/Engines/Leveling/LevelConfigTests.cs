using Server;
using Server.Engines.Leveling;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// GetMobLevel_OverrideWinsOverHpHeuristic constructs real BaseCreature instances, which need
// Map.Internal set up by UOContentFixture (see BaseCreatureSingleClickTests for the same pattern).
[Collection("Sequential UOContent Tests")]
public class LevelConfigTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)] // TESTING: L1 at FirstLevelXP = 1
    [InlineData(13749, 1)]
    [InlineData(13750, 2)]
    [InlineData(36249, 2)]
    [InlineData(36250, 3)]
    [InlineData(76249, 3)]
    [InlineData(76250, 4)]
    [InlineData(963749, 9)]
    [InlineData(963750, 10)]
    [InlineData(999999999, 10)]
    public void LevelForXP_MapsCumulativeXpToLevel(long xp, int expected)
    {
        Assert.Equal(expected, LevelConfig.LevelForXP(xp));
    }

    [Theory]
    [InlineData(1, 1)] // TESTING: FirstLevelXP knob, production 3750
    [InlineData(2, 13750)]
    [InlineData(3, 36250)]
    [InlineData(5, 138750)]
    [InlineData(10, 963750)]
    [InlineData(0, 0)]
    [InlineData(11, 963750)] // clamped to MaxLevel
    public void XPToReach_MatchesHandTunedCumulativeTable(int level, long expected)
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
    [InlineData(6, 1)]  // updated: HP-only floor is now level 1, not 0 (0 is override-only)
    [InlineData(20, 1)] // updated: was the foreign <= 20 => 0 case
    [InlineData(21, 1)] // updated: was the foreign <= 20 => 0 boundary case
    [InlineData(65, 1)]
    [InlineData(66, 2)]
    [InlineData(100, 2)]
    [InlineData(101, 3)]
    [InlineData(160, 3)]
    [InlineData(161, 4)]
    [InlineData(240, 4)]
    [InlineData(241, 5)]
    [InlineData(380, 5)]
    [InlineData(381, 6)]
    [InlineData(550, 6)]
    [InlineData(551, 7)]
    [InlineData(720, 7)]
    [InlineData(721, 8)]
    [InlineData(950, 8)]
    [InlineData(951, 9)]
    [InlineData(2400, 9)]
    [InlineData(2401, 10)]
    [InlineData(99999, 10)]
    public void MobLevelFromHits_Buckets(int hitsMax, int expected)
    {
        Assert.Equal(expected, LevelConfig.MobLevelFromHits(hitsMax));
    }

    [Theory]
    [InlineData(typeof(Dog), 0)]
    [InlineData(typeof(Lich), 5)]
    public void MobLevelOverrides_ContainsHandTunedPins(System.Type mobType, int expected)
    {
        Assert.True(LevelConfig.MobLevelOverrides.TryGetValue(mobType, out var level));
        Assert.Equal(expected, level);
    }

    [Fact]
    public void GetMobLevel_OverrideWinsOverHpHeuristic()
    {
        var dog = new Dog((Serial)0x1);
        var lich = new Lich((Serial)0x2);

        Assert.Equal(0, LevelConfig.GetMobLevel(dog));
        Assert.Equal(5, LevelConfig.GetMobLevel(lich));
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

    [Theory]
    // gap = mobLevel - playerLevel
    [InlineData(2, 5, 0x3B2)]  // gap -3 -> gray
    [InlineData(3, 5, 0x3F)]   // gap -2 -> green
    [InlineData(4, 5, 0x3F)]   // gap -1 -> green
    [InlineData(5, 5, 0x481)]  // gap  0 -> white
    [InlineData(6, 5, 0x35)]   // gap +1 -> yellow
    [InlineData(7, 5, 0x22)]   // gap +2 -> red
    [InlineData(15, 5, 0x22)]  // gap +10 -> red
    [InlineData(0, 0, 0x3B2)]  // mobLevel 0 -> gray regardless of gap
    [InlineData(0, 10, 0x3B2)] // mobLevel 0 -> gray even when playerLevel is high
    public void GapHue_MatchesGapBracketsAndLvlZeroOverride(int mobLevel, int playerLevel, int expected)
    {
        Assert.Equal(expected, LevelConfig.GapHue(mobLevel, playerLevel));
    }
}
