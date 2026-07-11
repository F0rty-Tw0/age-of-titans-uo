using System;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

public class RarityInvariantTests
{
    // 4a: every real ClauseType must render tooltip text. If a future clause is added with no
    // ClauseText.Describe arm, it silently ships as a blank tooltip line — this test fails loudly
    // instead. No clause is currently excluded; if one legitimately has no text, add it to
    // ExcludedFromTextCoverage with a comment explaining why, rather than skipping silently.
    private static readonly ClauseType[] ExcludedFromTextCoverage = Array.Empty<ClauseType>();

    [Fact]
    public void EveryClauseType_ExceptNoneAndExcluded_ProducesText()
    {
        foreach (var clause in Enum.GetValues<ClauseType>())
        {
            if (clause == ClauseType.None || Array.IndexOf(ExcludedFromTextCoverage, clause) >= 0)
            {
                continue;
            }

            var text = ClauseText.Describe(clause, 1, 1, 1);

            Assert.False(string.IsNullOrEmpty(text), $"{clause} produced no clause text");
        }
    }

    // 4b: TryGetByRootAndBase is first-match — a duplicate (Family, Root, BaseIndex) triple would
    // silently shadow one legendary behind another on every loot roll.
    [Fact]
    public void LegendaryEntries_HaveUniqueFamilyRootBaseIndexTriples()
    {
        var seen = new System.Collections.Generic.HashSet<(byte Family, VariantRoot Root, byte BaseIndex)>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            var triple = (entry.Family, entry.Root, entry.BaseIndex);

            Assert.True(
                seen.Add(triple),
                $"{entry.Name} duplicates the (Family={entry.Family}, Root={entry.Root}, BaseIndex={entry.BaseIndex}) triple of another entry"
            );
        }
    }

    // 4c: table completeness per root. Every non-None root's owning table must carry non-empty
    // rows for Uncommon/Rare/Epic/Legendary — a root with a gap silently drops to a plain item at
    // that rarity. Classification mirrors RarityEffects.ValidateRootForItem / WeaponFamilyMap.
    private static readonly VariantRoot[] ShieldRoots =
    {
        VariantRoot.Aegis, VariantRoot.Amyntor, VariantRoot.Probolos, VariantRoot.Herkos, VariantRoot.Pnoe
    };

    private static readonly VariantRoot[] JewelryRoots =
    {
        VariantRoot.Olympian, VariantRoot.Hecatean, VariantRoot.Tychean, VariantRoot.Nyxian, VariantRoot.Demetrian
    };

    private static readonly VariantRoot[] ClothingRoots =
    {
        VariantRoot.Laurel, VariantRoot.Charis, VariantRoot.Maenad, VariantRoot.Hestian, VariantRoot.Arachne
    };

    [Fact]
    public void EveryRoot_HasNonEmptyRowsAtEveryDropTier()
    {
        for (var root = VariantRoot.Zephyr; root <= VariantRoot.Pnoe; root++)
        {
            for (var rarity = ItemRarity.Uncommon; rarity <= ItemRarity.Legendary; rarity++)
            {
                var isEmpty = GetRowIsEmpty(root, rarity);

                Assert.False(isEmpty, $"{root} has no {rarity} rows in its owning table");
            }
        }
    }

    private static bool GetRowIsEmpty(VariantRoot root, ItemRarity rarity)
    {
        if (WeaponFamilyMap.IsWeaponRoot(root))
        {
            return WeaponEffectTable.Get(root, rarity).IsEmpty;
        }

        if (Array.IndexOf(ShieldRoots, root) >= 0)
        {
            return ArmorEffectTable.Get(root, rarity, isShield: true).IsEmpty;
        }

        if (Array.IndexOf(JewelryRoots, root) >= 0)
        {
            return AccessoryEffectTable.Get(root, rarity, isClothing: false).IsEmpty;
        }

        if (Array.IndexOf(ClothingRoots, root) >= 0)
        {
            return AccessoryEffectTable.Get(root, rarity, isClothing: true).IsEmpty;
        }

        // Every remaining root (material-locked armor roots + the 5 retired legacy shared armor
        // roots, decode-only but still fully rowed) lives on the armor side.
        return ArmorEffectTable.Get(root, rarity, isShield: false).IsEmpty;
    }
}
