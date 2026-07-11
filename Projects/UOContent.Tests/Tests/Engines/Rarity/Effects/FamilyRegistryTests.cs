using System;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// Proves the additive FamilyRegistry (Families/*.cs) reproduces the live hand-written tables
// byte-for-byte. Per-root checks run for every CLAIMED root, so they pass while the registry is
// still being filled in; the whole-registry checks (stack groups, counts, slot signatures,
// capstones) are gated on IsComplete. Once every family is transcribed and IsComplete is true,
// this is the proof that the Stage-2 consumer flip preserves behavior.
public class FamilyRegistryTests
{
    private static readonly ItemRarity[] _rarities =
    {
        ItemRarity.Common, ItemRarity.Uncommon, ItemRarity.Rare, ItemRarity.Epic, ItemRarity.Legendary
    };

    [Fact]
    public void ClaimedRoots_DisplayNameAndHue_MatchLiveTable()
    {
        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            if (!FamilyRegistry.RootClaimed[(int)root])
            {
                continue;
            }

            Assert.Equal(VariantRootInfo.GetDisplayName(root), FamilyRegistry.RootDisplayNames[(int)root]);
            // BaseHue is the Uncommon (base+0) shade.
            Assert.Equal(VariantRootInfo.GetBodyHue(root, ItemRarity.Uncommon), FamilyRegistry.RootBaseHues[(int)root]);
        }
    }

    [Fact]
    public void AllClaimedRoots_HaveNonEmptyMythTag()
    {
        // The registry's boot Validate() throws on a missing tag; this pins the guarantee the
        // tooltip effects-line prefix relies on (every claimed root — including legacy roots — has
        // a non-empty myth tag reachable via VariantRootInfo.GetMythTag).
        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            if (root == VariantRoot.None || !FamilyRegistry.RootClaimed[(int)root])
            {
                continue;
            }

            Assert.False(string.IsNullOrEmpty(VariantRootInfo.GetMythTag(root)), $"{root} has no myth tag");
            Assert.Equal(FamilyRegistry.RootMythTags[(int)root], VariantRootInfo.GetMythTag(root));
        }
    }

    [Fact]
    public void ClaimedRoots_EffectRows_MatchLiveTables()
    {
        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            if (!FamilyRegistry.RootClaimed[(int)root])
            {
                continue;
            }

            foreach (var rarity in _rarities)
            {
                Assert.Equal(WeaponEffectTable.Get(root, rarity), FamilyRegistry.WeaponRows[(int)root, (int)rarity]);
                Assert.Equal(ArmorEffectTable.Get(root, rarity, false), FamilyRegistry.ArmorRows[(int)root, (int)rarity]);
                Assert.Equal(ArmorEffectTable.Get(root, rarity, true), FamilyRegistry.ShieldRows[(int)root, (int)rarity]);
                Assert.Equal(AccessoryEffectTable.Get(root, rarity, false), FamilyRegistry.JewelryRows[(int)root, (int)rarity]);
                Assert.Equal(AccessoryEffectTable.Get(root, rarity, true), FamilyRegistry.ClothingRows[(int)root, (int)rarity]);
            }
        }
    }

    [Fact]
    public void RegistryLegendaries_MatchLiveRegistryById()
    {
        foreach (var entry in FamilyRegistry.Legendaries)
        {
            Assert.True(LegendaryRegistry.TryGet(entry.Id, out var live), $"legendary {entry.Id} missing from live registry");
            Assert.Equal(live, entry);
        }
    }

    [Fact]
    public void WeaponFamilyMap_MatchesRegistry()
    {
        foreach (var (root, family) in FamilyRegistry.RootToWeaponFamily)
        {
            Assert.True(WeaponFamilyMap.TryGetRootFamily(root, out var live));
            Assert.Equal(live, family);
        }

        // Constructing real weapons needs the world fixture; compare the type map by reflection.
        var field = typeof(WeaponFamilyMap).GetField(
            "_typeToFamily", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
        );
        var liveMap = (System.Collections.Generic.Dictionary<Type, byte>)field!.GetValue(null)!;

        foreach (var (type, family) in FamilyRegistry.TypeToWeaponFamily)
        {
            Assert.True(liveMap.TryGetValue(type, out var live), $"{type.Name} missing from live map");
            Assert.Equal(live, family);
        }

        // Once every weapon family is registered, the two maps must be identical (no missing types).
        if (FamilyRegistry.IsComplete)
        {
            Assert.Equal(liveMap.Count, FamilyRegistry.TypeToWeaponFamily.Count);
        }
    }

    // ---- Whole-registry checks (only meaningful once every family is transcribed) -------------

    [Fact]
    public void Registry_IsComplete_WithExactCounts()
    {
        // Guards against the WhenComplete_* checks passing vacuously: every non-None root must be
        // claimed by exactly one definition, and the aggregate legendary count must be exact.
        Assert.True(FamilyRegistry.IsComplete, "not every VariantRoot is claimed by a family definition");
        Assert.Equal(275, FamilyRegistry.Legendaries.Length); // +5 hat-bound clothing relics (§3)
        Assert.Equal(VariantRootInfo.StackGroupCount, FamilyRegistry.StackGroupCount);
    }

    [Fact]
    public void WhenComplete_StackGroupsAndCounts_MatchLiveTables()
    {
        if (!FamilyRegistry.IsComplete)
        {
            return;
        }

        foreach (VariantRoot root in Enum.GetValues<VariantRoot>())
        {
            Assert.Equal(VariantRootInfo.GetStackGroup(root), FamilyRegistry.RootStackGroups[(int)root]);
        }

        Assert.Equal(VariantRootInfo.StackGroupCount, FamilyRegistry.StackGroupCount);
        Assert.Equal(LegendaryRegistry.Entries.Count, FamilyRegistry.Legendaries.Length);
    }

    [Fact]
    public void WhenComplete_SlotSignaturesAndCapstones_MatchLiveTables()
    {
        if (!FamilyRegistry.IsComplete)
        {
            return;
        }

        for (var m = 0; m < 13; m++)
        {
            for (var s = 0; s < 7; s++)
            {
                Assert.Equal(
                    ArmorSlotSignatureTable.Get((ArmorMaterialType)m, (ArmorBodyType)s),
                    FamilyRegistry.SlotSignatures[m, s]
                );
            }
        }

        foreach (var material in Enum.GetValues<ArmorMaterialType>())
        {
            Assert.Equal(WornEffectState.CapstoneName(material), FamilyRegistry.CapstoneName(material));
            Assert.Equal(WornEffectState.CapstoneIcon(material), FamilyRegistry.CapstoneIcon(material));
        }
    }
}
