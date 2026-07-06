using System.Text;
using Server.Items;
using Server.Network;
using Xunit;

namespace Server.Tests.Network;

[Collection("Sequential Server Tests")]
public class ItemPacketTests
{
    [Fact]
    public void TestWorldItemPacket()
    {
        var serial = (Serial)0x1024;
        var itemId = 1;

        // Move to fixture
        TileData.ItemTable[itemId] = new ItemData(
            "Test Item Data",
            TileFlag.Generic,
            1,
            1,
            1,
            1,
            1,
            1
        );

        var item = new Item(serial)
        {
            ItemID = itemId,
            Hue = 0x1024,
            Amount = 10,
            Location = new Point3D(1000, 100, -10),
            Direction = Direction.Left
        };

        var expected = new WorldItem(item).Compile();

        using var ns = PacketTestUtilities.CreateTestNetState();
        ns.SendWorldItem(item);

        var result = ns.SendBuffer.GetReadSpan();
        AssertThat.Equal(result, expected);
    }

    [Fact]
    public void TestWorldItemSAPacket()
    {
        var serial = (Serial)0x1024;
        ushort itemId = 1;

        // Move to fixture
        TileData.ItemTable[itemId] = new ItemData(
            "Test Item Data",
            TileFlag.Generic,
            1,
            1,
            1,
            1,
            1,
            1
        );

        var item = new Item(serial)
        {
            ItemID = itemId,
            Hue = 0x1024,
            Amount = 10,
            Location = new Point3D(1000, 100, -10)
        };

        var expected = new WorldItemSA(item).Compile();

        using var ns = PacketTestUtilities.CreateTestNetState();
        ns.ProtocolChanges = ProtocolChanges.StygianAbyss;
        ns.SendWorldItem(item);

        var result = ns.SendBuffer.GetReadSpan();
        AssertThat.Equal(result, expected);
    }

    [Fact]
    public void TestWorldItemHSPacket()
    {
        var serial = (Serial)0x1024;
        var itemId = 1;

        // Move to fixture
        TileData.ItemTable[itemId] = new ItemData(
            "Test Item Data",
            TileFlag.Generic,
            1,
            1,
            1,
            1,
            1,
            1
        );

        var item = new Item(serial)
        {
            ItemID = itemId,
            Hue = 0x1024,
            Amount = 10,
            Location = new Point3D(1000, 100, -10)
        };

        var expected = new WorldItemHS(item).Compile();

        using var ns = PacketTestUtilities.CreateTestNetState();
        ns.ProtocolChanges = ProtocolChanges.StygianAbyss | ProtocolChanges.HighSeas;
        ns.SendWorldItem(item);

        var result = ns.SendBuffer.GetReadSpan();
        AssertThat.Equal(result, expected);
    }

    [Fact]
    public void TestWeaponSingleClickShowsT2ADetailsWhenEnabled()
    {
        var expansion = Core.Expansion;

        try
        {
            Core.Expansion = Expansion.T2A;
            ServerConfiguration.Load(true);
            ServerConfiguration.SetSetting("itemInfo.singleClickWeaponDetails", true);
            TileData.ItemTable[0xF52] = new ItemData(
                "dagger",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1,
                1
            );

            using var ns = PacketTestUtilities.CreateTestNetState();
            var from = new Mobile
            {
                NetState = ns
            };
            ns.Mobile = from;

            var dagger = new Dagger { Name = "dagger" };
            dagger.OnSingleClick(from);

            var text = Encoding.BigEndianUnicode.GetString(ns.SendBuffer.GetReadSpan());
            Assert.Contains("Damage:", text);
            Assert.Contains("Speed:", text);
            Assert.Contains("Durability:", text);
            Assert.Contains("Skill: Fencing", text);
        }
        finally
        {
            Core.Expansion = expansion;
            ServerConfiguration.SetSetting("itemInfo.singleClickWeaponDetails", false);
        }
    }

    [Fact]
    public void TestArmorSingleClickShowsT2ADetailsWhenEnabled()
    {
        var expansion = Core.Expansion;

        try
        {
            Core.Expansion = Expansion.T2A;
            ServerConfiguration.Load(true);
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", true);

            TileData.ItemTable[0x1415] = new ItemData(
                "plate chest",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1,
                1
            );

            using var ns = PacketTestUtilities.CreateTestNetState();
            var from = new Mobile
            {
                NetState = ns
            };
            ns.Mobile = from;

            var armor = new PlateChest { Name = "plate chest" };
            armor.OnSingleClick(from);

            var text = Encoding.BigEndianUnicode.GetString(ns.SendBuffer.GetReadSpan());
            Assert.Contains("Armor Rating:", text);
            Assert.Contains("Durability:", text);
        }
        finally
        {
            Core.Expansion = expansion;
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", false);
        }
    }

    [Fact]
    public void TestClothingSingleClickShowsT2ADetailsWhenEnabled()
    {
        var expansion = Core.Expansion;

        try
        {
            Core.Expansion = Expansion.T2A;
            ServerConfiguration.Load(true);
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", true);

            TileData.ItemTable[0x1517] = new ItemData(
                "shirt",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1,
                1
            );

            using var ns = PacketTestUtilities.CreateTestNetState();
            var from = new Mobile
            {
                NetState = ns
            };
            ns.Mobile = from;

            var shirt = new Shirt { Name = "shirt" };
            shirt.MaxHitPoints = 50;
            shirt.HitPoints = 50;
            shirt.OnSingleClick(from);

            var text = Encoding.BigEndianUnicode.GetString(ns.SendBuffer.GetReadSpan());
            Assert.Contains("Durability:", text);
        }
        finally
        {
            Core.Expansion = expansion;
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", false);
        }
    }

    [Fact]
    public void TestJewelSingleClickShowsT2ADetailsWhenEnabled()
    {
        var expansion = Core.Expansion;

        try
        {
            Core.Expansion = Expansion.T2A;
            ServerConfiguration.Load(true);
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", true);

            TileData.ItemTable[0x108a] = new ItemData(
                "gold ring",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1,
                1
            );

            using var ns = PacketTestUtilities.CreateTestNetState();
            var from = new Mobile
            {
                NetState = ns
            };
            ns.Mobile = from;

            var ring = new GoldRing { Name = "gold ring" };
            ring.MaxHitPoints = 50;
            ring.HitPoints = 50;
            ring.OnSingleClick(from);

            var text = Encoding.BigEndianUnicode.GetString(ns.SendBuffer.GetReadSpan());
            Assert.Contains("Durability:", text);
        }
        finally
        {
            Core.Expansion = expansion;
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", false);
        }
    }

    [Fact]
    public void TestArmorSingleClickShowsDetailsWhenOnlyWeaponFlagEnabled()
    {
        // Either flag enables the detail labels on all item types — pins the intentional
        // cross-flag behavior of ItemInfoConfiguration.SingleClickDetails.
        var expansion = Core.Expansion;

        try
        {
            Core.Expansion = Expansion.T2A;
            ServerConfiguration.Load(true);
            ServerConfiguration.SetSetting("itemInfo.singleClickWeaponDetails", true);
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", false);
            TileData.ItemTable[0x1452] = new ItemData(
                "bone legs",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1,
                1
            );

            using var ns = PacketTestUtilities.CreateTestNetState();
            var from = new Mobile
            {
                NetState = ns
            };
            ns.Mobile = from;

            var armor = new BoneLegs { Name = "bone legs" };
            armor.OnSingleClick(from);

            var text = Encoding.BigEndianUnicode.GetString(ns.SendBuffer.GetReadSpan());
            Assert.Contains("Armor Rating:", text);
        }
        finally
        {
            Core.Expansion = expansion;
            ServerConfiguration.SetSetting("itemInfo.singleClickWeaponDetails", false);
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", false);
        }
    }

    [Fact]
    public void TestWeaponSingleClickShowsDetailsWhenOnlyItemDetailsFlagEnabled()
    {
        // Either flag enables the detail labels on all item types — pins the intentional
        // cross-flag behavior of ItemInfoConfiguration.SingleClickDetails.
        var expansion = Core.Expansion;

        try
        {
            Core.Expansion = Expansion.T2A;
            ServerConfiguration.Load(true);
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", true);
            ServerConfiguration.SetSetting("itemInfo.singleClickWeaponDetails", false);
            TileData.ItemTable[0xF52] = new ItemData(
                "dagger",
                TileFlag.Generic,
                1,
                1,
                1,
                1,
                1,
                1
            );

            using var ns = PacketTestUtilities.CreateTestNetState();
            var from = new Mobile
            {
                NetState = ns
            };
            ns.Mobile = from;

            var dagger = new Dagger { Name = "dagger" };
            dagger.OnSingleClick(from);

            var text = Encoding.BigEndianUnicode.GetString(ns.SendBuffer.GetReadSpan());
            Assert.Contains("Damage:", text);
        }
        finally
        {
            Core.Expansion = expansion;
            ServerConfiguration.SetSetting("itemInfo.singleClickItemDetails", false);
            ServerConfiguration.SetSetting("itemInfo.singleClickWeaponDetails", false);
        }
    }
}
