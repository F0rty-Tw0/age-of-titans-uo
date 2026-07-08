using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Server.Tests.Network;
using Xunit;

namespace UOContent.Tests;

// Uses the shared UOContentFixture world boot (via the collection) rather than a self-boot
// static ctor, which keeps these item-touching tests off the parallel double-boot race.
[Collection("Sequential UOContent Tests")]
public class RarityEffectsTests
{
    [Fact]
    public void ApplyVariant_SetsRootRarityHueAndName()
    {
        var item = new Katana();

        try
        {
            RarityEffects.ApplyVariant(item, VariantRoot.Zephyr, ItemRarity.Rare);

            Assert.Equal(VariantRoot.Zephyr, ((IVariantItem)item).VariantRoot);
            Assert.Equal((ushort)0, ((IVariantItem)item).LegendaryId);
            Assert.Equal(ItemRarity.Rare, item.Rarity);
            Assert.Equal(VariantRootInfo.GetBodyHue(VariantRoot.Zephyr, ItemRarity.Rare), item.Hue);
            Assert.NotNull(item.Name);
            Assert.StartsWith("zephyr", item.Name);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void ApplyVariant_ClampsRarityToMaxRarity()
    {
        var item = new RareCappedKatana();

        try
        {
            RarityEffects.ApplyVariant(item, VariantRoot.Phobos, ItemRarity.Legendary);

            Assert.Equal(ItemRarity.Rare, item.Rarity);
            Assert.Equal(VariantRootInfo.GetBodyHue(VariantRoot.Phobos, ItemRarity.Rare), item.Hue);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void ApplyLegendary_SetsLegendaryIdRootAndProperNoun()
    {
        var item = new DoubleAxe();

        try
        {
            RarityEffects.ApplyLegendary(item, 12); // Labrys

            Assert.Equal((ushort)12, ((IVariantItem)item).LegendaryId);
            Assert.Equal(VariantRoot.Phobos, ((IVariantItem)item).VariantRoot);
            Assert.Equal(ItemRarity.Legendary, item.Rarity);
            Assert.Equal("Labrys", item.Name);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void ClearVariant_RevertsToPlainItem()
    {
        var item = new Katana();

        try
        {
            RarityEffects.ApplyVariant(item, VariantRoot.Stygian, ItemRarity.Epic);
            RarityEffects.ClearVariant(item);

            Assert.Equal(VariantRoot.None, ((IVariantItem)item).VariantRoot);
            Assert.Equal((ushort)0, ((IVariantItem)item).LegendaryId);
            Assert.Equal(ItemRarity.Common, item.Rarity);
            Assert.Equal(0, item.Hue);
            Assert.Null(item.Name);
        }
        finally
        {
            item.Delete();
        }
    }

    [Fact]
    public void ApplyVariant_WeaponRootOnArmor_Throws()
    {
        var armor = new PlateChest();

        try
        {
            Assert.Throws<ArgumentException>(() => RarityEffects.ApplyVariant(armor, VariantRoot.Phobos, ItemRarity.Rare));
        }
        finally
        {
            armor.Delete();
        }
    }

    [Fact]
    public void ApplyVariant_ArmorRootOnWeapon_Throws()
    {
        var weapon = new Katana();

        try
        {
            Assert.Throws<ArgumentException>(() => RarityEffects.ApplyVariant(weapon, VariantRoot.Polias, ItemRarity.Rare));
        }
        finally
        {
            weapon.Delete();
        }
    }

    [Fact]
    public void ApplyLegendary_AxeLegendaryOnArmor_Throws()
    {
        var armor = new PlateChest();

        try
        {
            Assert.Throws<ArgumentException>(() => RarityEffects.ApplyLegendary(armor, 1));
        }
        finally
        {
            armor.Delete();
        }
    }

    [Fact]
    public void ApplyLegendary_UnknownId_Throws()
    {
        var item = new DoubleAxe();

        try
        {
            Assert.Throws<ArgumentException>(() => RarityEffects.ApplyLegendary(item, 999));
        }
        finally
        {
            item.Delete();
        }
    }

    // Pre-UOTD (T2A) clients only see single-click LabelTo lines, never the OPL tooltip, and the
    // classic client caps single-click overhead text at ~5 lines per item (oldest dropped first).
    // This verifies RarityEffects.LabelVariantDetails compresses its mirror of the OPL content
    // (base shape + numeric summary on one line, clause text on another) to at most 2 lines, via
    // the wire, not just by reading the built strings back.
    [Fact]
    public void LabelVariantDetails_LegendaryWeapon_EmitsBaseShapeSummaryAndClauseLines()
    {
        var item = new Halberd();
        var player = new PlayerMobile(World.NewMobile);
        player.DefaultMobileInit();

        try
        {
            RarityEffects.ApplyLegendary(item, 90); // Klytios (Zophos, on-kill restore stam+mana)

            using var ns = PacketTestUtilities.CreateTestNetState();
            player.NetState = ns;
            ns.Mobile = player;

            RarityEffects.LabelVariantDetails(player, item);

            var messages = DecodeUnicodeMessages(ns.SendBuffer.GetReadSpan());

            Assert.True(
                messages.Count <= 2,
                $"Expected at most 2 label lines, got {messages.Count}: {string.Join(" | ", messages)}"
            );

            var halberdName = Localization.GetText(item.LabelNumber);
            Assert.Contains(
                messages,
                m => m.StartsWith(halberdName, StringComparison.OrdinalIgnoreCase) &&
                     m.Contains("lifesteal", StringComparison.OrdinalIgnoreCase)
            );
            Assert.Contains(messages, m => m.Contains("on-kill: restores stamina and mana", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            item.Delete();
            player.Delete();
        }
    }

    // Walks the concatenated 0xAE (Unicode Message) packets written by LabelTo(Mobile, string)
    // using each packet's own big-endian length prefix, decoding the BigUniNull text payload
    // (header is id(1)+len(2)+serial(4)+graphic(2)+type(1)+hue(2)+font(2)+lang(4)+name(30) = 48
    // bytes, per OutgoingMessagePackets.CreateMessage) — matches OnSingleClickPreUOTD's own LabelTo
    // calls exactly, so this reads the real wire output rather than the built strings.
    private static List<string> DecodeUnicodeMessages(ReadOnlySpan<byte> buffer)
    {
        var messages = new List<string>();
        var offset = 0;

        while (offset + 3 <= buffer.Length)
        {
            var packetId = buffer[offset];
            var length = BinaryPrimitives.ReadUInt16BigEndian(buffer.Slice(offset + 1, 2));

            if (length < 3 || offset + length > buffer.Length)
            {
                break;
            }

            if (packetId == 0xAE)
            {
                var textBytes = buffer.Slice(offset + 48, length - 48 - 2);
                messages.Add(Encoding.BigEndianUnicode.GetString(textBytes));
            }

            offset += length;
        }

        return messages;
    }

    private class RareCappedKatana : Katana
    {
        public override ItemRarity MaxRarity => ItemRarity.Rare;
    }
}
