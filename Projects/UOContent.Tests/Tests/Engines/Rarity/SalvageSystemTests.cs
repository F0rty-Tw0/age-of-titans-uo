using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Sequential collection: creates world items/mobiles on the shared static World.
[Collection("Sequential UOContent Tests")]
public class SalvageSystemTests
{
    static SalvageSystemTests()
    {
        Server.Tests.TestServerInitializer.Initialize();
    }

    // Felucca X offset avoids other sequential tests sharing the shared static World.
    private static PlayerMobile CreatePlayerMobile(Map map, Point3D location)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.MoveToWorld(location, map);
        m.AddItem(new Backpack());
        return m;
    }

    private static Katana CreateThemedKatana(PlayerMobile owner, ItemRarity rarity)
    {
        var katana = new Katana();
        owner.Backpack.AddItem(katana);
        RarityEffects.ApplyVariant(katana, VariantRoot.Zephyr, rarity);
        return katana;
    }

    [Theory]
    [InlineData(ItemRarity.Uncommon, 2)]
    [InlineData(ItemRarity.Rare, 4)]
    [InlineData(ItemRarity.Epic, 8)]
    public void CanSalvage_VariantTiers_YieldMultiplier(ItemRarity rarity, int expectedYield)
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4500, 600, 0));
        var katana = CreateThemedKatana(player, rarity);

        try
        {
            Assert.True(SalvageSystem.CanSalvage(player, katana, out var yield, out _));
            Assert.Equal(expectedYield, yield);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void CanSalvage_BelowHalfDurability_HalvesYield()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4520, 600, 0));
        var katana = CreateThemedKatana(player, ItemRarity.Epic); // full Epic yield = 8

        try
        {
            katana.HitPoints = katana.MaxHitPoints / 4; // below half durability

            Assert.True(SalvageSystem.CanSalvage(player, katana, out var yield, out _));
            Assert.Equal(4, yield); // halved from 8

            // At (or above) half durability the full yield is paid.
            katana.HitPoints = katana.MaxHitPoints;
            Assert.True(SalvageSystem.CanSalvage(player, katana, out var fullYield, out _));
            Assert.Equal(8, fullYield);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void CanSalvage_CommonItem_Rejected()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4510, 600, 0));
        var katana = new Katana();
        player.Backpack.AddItem(katana);

        try
        {
            Assert.False(SalvageSystem.CanSalvage(player, katana, out _, out var reason));
            Assert.NotNull(reason);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void CanSalvage_LegendaryItem_Rejected()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4520, 600, 0));
        var katana = new Katana();
        player.Backpack.AddItem(katana);

        try
        {
            // The legendary gate reads only LegendaryId — no registry lookup — so a direct
            // property set exercises exactly that gate.
            ((IVariantItem)katana).LegendaryId = 1;

            Assert.False(SalvageSystem.CanSalvage(player, katana, out _, out var reason));
            Assert.NotNull(reason);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void CanSalvage_ItemNotInBackpack_Rejected()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4530, 600, 0));
        var katana = new Katana();

        try
        {
            RarityEffects.ApplyVariant(katana, VariantRoot.Zephyr, ItemRarity.Rare);
            katana.MoveToWorld(player.Location, player.Map);

            Assert.False(SalvageSystem.CanSalvage(player, katana, out _, out var reason));
            Assert.NotNull(reason);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TrySalvage_DeletesItemAndStacksIchor()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4540, 600, 0));
        var katana = CreateThemedKatana(player, ItemRarity.Rare);
        var seed = new PantheonIchor(5);
        player.Backpack.AddItem(seed);

        try
        {
            Assert.True(SalvageSystem.TrySalvage(player, katana));

            Assert.True(katana.Deleted);
            Assert.Equal(9, player.Backpack.GetAmount(typeof(PantheonIchor))); // 5 seed + 4 yield
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Theory]
    [InlineData(ItemRarity.Uncommon, ItemRarity.Rare, 20)]
    [InlineData(ItemRarity.Rare, ItemRarity.Epic, 80)]
    public void CanUpgrade_UncommonAndRare_ReportNextTierAndCost(
        ItemRarity current, ItemRarity expectedNext, int expectedCost
    )
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4550, 600, 0));
        var katana = CreateThemedKatana(player, current);
        player.Backpack.AddItem(new PantheonIchor(100));

        try
        {
            Assert.True(SalvageSystem.CanUpgrade(player, katana, out var next, out var cost, out _));
            Assert.Equal(expectedNext, next);
            Assert.Equal(expectedCost, cost);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void CanUpgrade_EpicCommonOrLegendary_Rejected()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4560, 600, 0));
        var epic = CreateThemedKatana(player, ItemRarity.Epic);
        var common = new Katana();
        player.Backpack.AddItem(common);
        var legendary = CreateThemedKatana(player, ItemRarity.Uncommon);
        ((IVariantItem)legendary).LegendaryId = 1;
        player.Backpack.AddItem(new PantheonIchor(1000));

        try
        {
            Assert.False(SalvageSystem.CanUpgrade(player, epic, out _, out _, out _));
            Assert.False(SalvageSystem.CanUpgrade(player, common, out _, out _, out _));
            Assert.False(SalvageSystem.CanUpgrade(player, legendary, out _, out _, out _));
        }
        finally
        {
            epic.Delete();
            common.Delete();
            legendary.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void CanUpgrade_MaxRarityCap_Rejected()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4570, 600, 0));
        var katana = new RareCappedKatana();
        player.Backpack.AddItem(katana);
        player.Backpack.AddItem(new PantheonIchor(1000));

        try
        {
            RarityEffects.ApplyVariant(katana, VariantRoot.Zephyr, ItemRarity.Rare);

            // next would be Epic > MaxRarity (Rare) — must reject, never eat the material.
            Assert.False(SalvageSystem.CanUpgrade(player, katana, out _, out _, out _));
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TryUpgrade_ConsumesIchorAndRaisesRarity()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4580, 600, 0));
        var katana = CreateThemedKatana(player, ItemRarity.Uncommon);
        player.Backpack.AddItem(new PantheonIchor(25));

        try
        {
            Assert.True(SalvageSystem.TryUpgrade(player, katana));

            Assert.Equal(ItemRarity.Rare, katana.Rarity);
            Assert.Equal(VariantRoot.Zephyr, ((IVariantItem)katana).VariantRoot);
            Assert.Equal(5, player.Backpack.GetAmount(typeof(PantheonIchor)));
            Assert.Equal(VariantRootInfo.GetBodyHue(VariantRoot.Zephyr, ItemRarity.Rare), katana.Hue);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TryUpgrade_InsufficientIchor_NoChangeNoConsume()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4590, 600, 0));
        var katana = CreateThemedKatana(player, ItemRarity.Uncommon);
        player.Backpack.AddItem(new PantheonIchor(19));

        try
        {
            Assert.False(SalvageSystem.TryUpgrade(player, katana));

            Assert.Equal(ItemRarity.Uncommon, katana.Rarity);
            Assert.Equal(19, player.Backpack.GetAmount(typeof(PantheonIchor)));
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TryUpgrade_DoesNotDuplicateRootPrefixInName()
    {
        var player = CreatePlayerMobile(Map.Felucca, new Point3D(4600, 600, 0));
        var katana = CreateThemedKatana(player, ItemRarity.Uncommon);
        player.Backpack.AddItem(new PantheonIchor(25));

        try
        {
            var themedName = katana.Name; // e.g. "Zephyr Katana"

            Assert.True(SalvageSystem.TryUpgrade(player, katana));

            // Same root + same base shape => the themed name must survive the re-apply
            // unchanged (guards the BuildRootName item.Name-seeding duplication bug).
            Assert.Equal(themedName, katana.Name);
        }
        finally
        {
            katana.Delete();
            player.Delete();
        }
    }

    private class RareCappedKatana : Katana
    {
        public override ItemRarity MaxRarity => ItemRarity.Rare;
    }
}
