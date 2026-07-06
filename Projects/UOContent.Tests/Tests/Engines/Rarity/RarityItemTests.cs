using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

[Collection("Sequential UOContent Tests")]
public class RarityItemTests
{
    [Fact]
    public void Katana_DefaultsToCommonWithLegendaryMaxRarity()
    {
        var item = new Katana();

        try
        {
            Assert.Equal(ItemRarity.Common, item.Rarity);
            Assert.Equal(ItemRarity.Legendary, item.MaxRarity);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void PlateChest_DefaultsToCommonWithLegendaryMaxRarity()
    {
        var item = new PlateChest();

        try
        {
            Assert.Equal(ItemRarity.Common, item.Rarity);
            Assert.Equal(ItemRarity.Legendary, item.MaxRarity);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void FancyShirt_DefaultsToCommonWithLegendaryMaxRarity()
    {
        var item = new FancyShirt();

        try
        {
            Assert.Equal(ItemRarity.Common, item.Rarity);
            Assert.Equal(ItemRarity.Legendary, item.MaxRarity);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void GoldRing_DefaultsToCommonWithLegendaryMaxRarity()
    {
        var item = new GoldRing();

        try
        {
            Assert.Equal(ItemRarity.Common, item.Rarity);
            Assert.Equal(ItemRarity.Legendary, item.MaxRarity);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void Katana_SetRarityViaIRarity_ReadsBackThroughInterface()
    {
        var item = new Katana();

        try
        {
            var rarity = (IRarity)item;
            rarity.Rarity = ItemRarity.Rare;

            Assert.Equal(ItemRarity.Rare, rarity.Rarity);
            Assert.Equal(ItemRarity.Rare, item.Rarity);
        }
        finally
        {
            item.Delete();
        }
    }
}
