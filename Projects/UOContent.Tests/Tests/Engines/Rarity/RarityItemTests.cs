using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Tests.Maps;
using Xunit;

namespace UOContent.Tests;

public class RarityItemTests
{
    static RarityItemTests()
    {
        Core.ApplicationAssembly = typeof(RarityItemTests).Assembly;
        ServerConfiguration.Load(true);
        Core.LoopContext = new EventLoopContext();

        if (Map.Internal == null)
        {
            TestMapDefinitions.ConfigureTestMapDefinitions();
        }

        World.Configure();
        World.Load();
        DecayScheduler.Configure();
        Timer.Init(0);
    }

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

    [Fact]
    public void RareCappedKatana_SetRarityAboveMaxRarity_ClampsToMaxRarity()
    {
        var item = new RareCappedKatana();

        try
        {
            item.Rarity = ItemRarity.Legendary;

            Assert.Equal(ItemRarity.Rare, item.Rarity);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void Katana_SetRarityToNegativeValue_ClampsToCommon()
    {
        var item = new Katana();

        try
        {
            item.Rarity = (ItemRarity)(-1);

            Assert.Equal(ItemRarity.Common, item.Rarity);
        }
        finally
        {
            item.Delete();
        }
    }

    private class RareCappedKatana : Katana
    {
        public override ItemRarity MaxRarity => ItemRarity.Rare;
    }
}
