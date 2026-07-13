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
            Assert.StartsWith("Zephyr", item.Name); // root display name is capitalized in BuildRootName
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
    public void ApplyLegendary_EveryClothingRelic_AppliesToItsBoundFactoryPiece()
    {
        // Regression: hat relics (271-275) threw in ValidateFamilyForItem because the clothing
        // piece switch only knew indices 0-4. Generic guard: every clothing legendary must apply
        // cleanly to the exact piece the loot roller constructs for its BaseIndex.
        var factories = FamilyRegistry.ClothingFamilyDef.Factories;

        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (entry.Family != LegendaryRegistry.FamilyClothing)
            {
                continue;
            }

            var item = factories[entry.BaseIndex]();

            try
            {
                RarityEffects.ApplyLegendary(item, entry.Id);

                Assert.Equal(entry.Id, ((IVariantItem)item).LegendaryId);
                Assert.StartsWith(entry.Name, item.Name);
            }
            finally
            {
                item.Delete();
            }
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
            Assert.StartsWith("Labrys", item.Name); // legendary name prefix; base type may not resolve in test cliloc
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
    // classic client caps single-click overhead text at 5 lines per item (oldest dropped first).
    // The approved layout (2026-07-11) spends those lines on name (labelled by OnSingleClick) +
    // stats + myth-tagged effects (with the base shape merged in) + signature clause + legendary
    // clause, so LabelVariantDetails itself emits at most 4. This checks the wire output, not just
    // the built strings.
    [Fact]
    public void LabelVariantDetails_LegendaryWeapon_EmitsBaseShapeSummaryAndClauseLines()
    {
        var item = new Halberd();
        var labelNumber = item.LabelNumber;
        var shape = Localization.GetText(labelNumber);
        var addedSyntheticShape = shape == null;

        if (addedSyntheticShape)
        {
            shape = "a halberd";
            Localization.Add("enu", labelNumber, shape);
        }
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
                messages.Count <= 4,
                $"Expected at most 4 label lines (+ name == 5-line cap), got {messages.Count}: {string.Join(" | ", messages)}"
            );

            // Effects line: bare effect summary — no myth tag (user directive 2026-07-12, pantheon
            // name removed from the click tooltip) and no base shape (2026-07-11).
            Assert.Contains(messages, m => m.Contains("lifesteal", StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(messages, m => m.Contains(" — ", StringComparison.Ordinal));
            Assert.DoesNotContain(messages, m => m.StartsWith(shape, StringComparison.OrdinalIgnoreCase));
            // Stats line is present and first.
            Assert.StartsWith("Damage:", messages[0]);
            Assert.Contains(messages, m => m.Contains("on kill: restores full stamina and mana", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            if (addedSyntheticShape)
            {
                Localization.Remove("enu", labelNumber);
            }
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
