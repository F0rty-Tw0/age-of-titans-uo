using Server.Engines.LootBags;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

public class LootBagConfigTests
{
    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(1, 0.10)]
    [InlineData(-1, 0.0)] // clamped to level 0
    [InlineData(11, 0.50)] // clamped to level 10
    [InlineData(99, 0.50)] // clamped to level 10
    public void ChanceForMobLevel_MapsLevelToTable(int level, double expected)
    {
        Assert.Equal(expected, LootBagConfig.ChanceForMobLevel(level));
    }

    [Fact]
    public void ChanceForMobLevel_IsMonotonicNonDecreasingAndInRange()
    {
        for (var level = 0; level < 10; level++)
        {
            var current = LootBagConfig.ChanceForMobLevel(level);
            var next = LootBagConfig.ChanceForMobLevel(level + 1);

            Assert.True(current <= next);
            Assert.True(current >= 0 && current <= 1);
        }

        var last = LootBagConfig.ChanceForMobLevel(10);
        Assert.True(last > 0 && last <= 1);
        Assert.True(LootBagConfig.ChanceForMobLevel(1) > 0);
    }
}

[Collection("Sequential UOContent Tests")]
public class LootBagItemTests
{
    [Fact]
    public void Constructor_LevelAboveMax_ClampsToMax()
    {
        var bag = new LootBag(15);

        try
        {
            Assert.Equal(10, bag.Level);
        }
        finally
        {
            bag.Delete();
        }
    }

    [Fact]
    public void Constructor_LevelBelowMin_ClampsToMin()
    {
        var bag = new LootBag(-3);

        try
        {
            Assert.Equal(0, bag.Level);
        }
        finally
        {
            bag.Delete();
        }
    }

    [Fact]
    public void Constructor_SetsDefaultName()
    {
        var bag = new LootBag(5);

        try
        {
            Assert.Equal("a loot bag", bag.Name);
        }
        finally
        {
            bag.Delete();
        }
    }
}
