using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targets;
using Xunit;

namespace UOContent.Tests;

[Collection("Sequential UOContent Tests")]
public class DoubleClickEquipTests
{
    // Felucca X offset avoids other sequential tests sharing the shared static World.
    private static PlayerMobile CreatePlayerMobile(Map map, Point3D location)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.MoveToWorld(location, map);
        m.AddItem(new Backpack());
        return m;
    }

    [Fact]
    public void TryEquip_WeaponInBackpack_EquipsOnMobile()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4400, 600, 0));
        var sword = new Longsword();

        try
        {
            player.Backpack.AddItem(sword);
            Assert.Equal(Layer.OneHanded, sword.Layer);

            var result = DoubleClickEquip.TryEquip(player, sword);

            Assert.True(result);
            Assert.Equal(player, sword.Parent);
        }
        finally
        {
            sword.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TryEquip_SecondWeapon_SwapsOldBackToBackpackAndEquipsNew()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4420, 600, 0));
        var first = new Longsword();
        var second = new Longsword();

        try
        {
            player.Backpack.AddItem(first);
            Assert.True(DoubleClickEquip.TryEquip(player, first));
            Assert.Equal(player, first.Parent);

            player.Backpack.AddItem(second);
            var result = DoubleClickEquip.TryEquip(player, second);

            Assert.True(result);
            Assert.Equal(player, second.Parent);
            Assert.Equal(player.Backpack, first.Parent);
        }
        finally
        {
            first.Delete();
            second.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TryEquip_TwoHandedWeapon_DisplacesOneHandedToBackpack()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4440, 600, 0));
        var sword = new Longsword();
        var halberd = new Halberd();

        try
        {
            player.Backpack.AddItem(sword);
            Assert.True(DoubleClickEquip.TryEquip(player, sword));
            Assert.Equal(player, sword.Parent);

            player.Backpack.AddItem(halberd);
            Assert.Equal(Layer.TwoHanded, halberd.Layer);

            var result = DoubleClickEquip.TryEquip(player, halberd);

            Assert.True(result);
            Assert.Equal(player, halberd.Parent);
            Assert.Equal(player.Backpack, sword.Parent);
        }
        finally
        {
            sword.Delete();
            halberd.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void EquipItem_SecondWeapon_PaperdollPathSwapsOldToBackpack()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4460, 600, 0));
        var first = new Longsword();
        var second = new Longsword();

        try
        {
            Assert.True(player.EquipItem(first));
            Assert.Equal(player, first.Parent);

            // Simulates the paperdoll drag path (IncomingItemPackets.EquipReq -> to.EquipItem),
            // which bounced before PlayerMobile.EquipItem started displacing conflicts.
            var result = player.EquipItem(second);

            Assert.True(result);
            Assert.Equal(player, second.Parent);
            Assert.Equal(player.Backpack, first.Parent);
        }
        finally
        {
            first.Delete();
            second.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void EquipItem_OneHandedWeapon_LeavesShieldEquipped()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4480, 600, 0));
        var shield = new WoodenShield();
        var sword = new Longsword();

        try
        {
            Assert.Equal(Layer.TwoHanded, shield.Layer);
            Assert.True(player.EquipItem(shield));

            var result = player.EquipItem(sword);

            Assert.True(result);
            Assert.Equal(player, sword.Parent);
            Assert.Equal(player, shield.Parent); // shield untouched, 1H + shield coexist
        }
        finally
        {
            shield.Delete();
            sword.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TryEquip_GMRobeInBackpack_StaffMobileEquipsOnOuterTorso()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4500, 600, 0));
        player.AccessLevel = AccessLevel.GameMaster; // BaseSuit.OnEquip rejects below the suit's AccessLevel
        var robe = new GMRobe();

        try
        {
            player.Backpack.AddItem(robe);
            Assert.Equal(Layer.OuterTorso, robe.Layer);

            var result = DoubleClickEquip.TryEquip(player, robe);

            Assert.True(result);
            Assert.Equal(player, robe.Parent);
        }
        finally
        {
            robe.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void OnDoubleClick_KnifeInBackpack_EquipsAndFallsThroughToUseTarget()
    {
        var map = Map.Felucca;
        var player = CreatePlayerMobile(map, new Point3D(4520, 600, 0));
        var dagger = new Dagger();

        try
        {
            player.Backpack.AddItem(dagger);

            dagger.OnDoubleClick(player);

            Assert.Equal(player, dagger.Parent); // equipped, not left in the pack
            Assert.IsType<BladedItemTarget>(player.Target); // same click opened the use target
        }
        finally
        {
            player.Target = null;
            dagger.Delete();
            player.Delete();
        }
    }
}
