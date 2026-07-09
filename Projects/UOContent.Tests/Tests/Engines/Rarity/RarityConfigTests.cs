using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

public class RarityConfigTests
{
    [Theory]
    [InlineData(0, ItemRarity.Uncommon)]
    [InlineData(1, ItemRarity.Uncommon)]
    [InlineData(2, ItemRarity.Rare)]
    [InlineData(10, ItemRarity.Legendary)]
    [InlineData(-1, ItemRarity.Uncommon)] // clamped to level 0
    [InlineData(11, ItemRarity.Legendary)] // clamped to level 10
    public void MaxRarityForBagLevel_MapsLevelToTable(int bagLevel, ItemRarity expected)
    {
        Assert.Equal(expected, RarityConfig.MaxRarityForBagLevel(bagLevel));
    }

    [Fact]
    public void SalvageMultiplier_IsStrictlyIncreasingAcrossAllTiers()
    {
        var previous = RarityConfig.SalvageMultiplier(ItemRarity.Common);

        for (var tier = ItemRarity.Uncommon; tier <= ItemRarity.Legendary; tier++)
        {
            var current = RarityConfig.SalvageMultiplier(tier);

            Assert.True(current > previous);
            previous = current;
        }
    }

    [Theory]
    [InlineData(ItemRarity.Common, "Common")]
    [InlineData(ItemRarity.Uncommon, "Uncommon")]
    [InlineData(ItemRarity.Rare, "Rare")]
    [InlineData(ItemRarity.Epic, "Epic")]
    [InlineData(ItemRarity.Legendary, "Legendary")]
    public void GetName_ReturnsExpectedStringForAllTiers(ItemRarity rarity, string expected)
    {
        Assert.Equal(expected, RarityConfig.GetName(rarity));
    }

    [Theory]
    [InlineData("Klytios", ItemRarity.Legendary, "Klytios [Legendary]")]
    [InlineData("Menis Katana", ItemRarity.Rare, "Menis Katana [Rare]")]
    [InlineData("Klytios", ItemRarity.Common, "Klytios")] // Common passes through unchanged
    public void WithSuffix_AppendsTierForNonCommonNames(string name, ItemRarity rarity, string expected)
    {
        Assert.Equal(expected, RarityConfig.WithSuffix(name, rarity));
    }

    [Fact]
    public void WithSuffix_NullNamePassesThrough()
    {
        Assert.Null(RarityConfig.WithSuffix(null, ItemRarity.Legendary));
    }

    [Fact]
    public void GetHue_ReturnsValueForAllTiersWithoutThrowing()
    {
        for (var tier = ItemRarity.Common; tier <= ItemRarity.Legendary; tier++)
        {
            var hue = RarityConfig.GetHue(tier);
            Assert.True(hue >= 0);
        }
    }
}
