using System;
using System.Collections.Generic;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// Cross-cutting re-theme guards (Phase 5): the root/legendary shared namespace (framework §10)
// and the §9.8 shared defensive pools as per-row ceilings (no single row may exceed a pool that
// is meant to cap an entire worn suit).
public class RethemeGuardTests
{
    [Fact]
    public void RootDisplayNames_AreUnique_CaseInsensitive()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var root = (VariantRoot)1; (int)root < VariantRootInfo.RootCount; root++)
        {
            var name = VariantRootInfo.GetDisplayName(root);

            Assert.False(string.IsNullOrEmpty(name), $"{root} has no display name");
            Assert.True(seen.Add(name), $"duplicate root display name: {name}");
        }
    }

    [Fact]
    public void RootDisplayNames_NeverCollideWithLegendaryNames()
    {
        var legendaryNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var entry in LegendaryRegistry.Entries)
        {
            legendaryNames.Add(entry.Name);
        }

        for (var root = (VariantRoot)1; (int)root < VariantRootInfo.RootCount; root++)
        {
            var name = VariantRootInfo.GetDisplayName(root);

            Assert.False(
                legendaryNames.Contains(name),
                $"root '{name}' collides with a legendary proper noun (framework §10 shared namespace)"
            );
        }
    }

    [Fact]
    public void ArmorAndShieldRows_StayInsideSharedPoolCeilings()
    {
        // §9.8 hard pools cap the WHOLE worn suit; a single row above a pool is a data error.
        for (var root = (VariantRoot)1; (int)root < VariantRootInfo.RootCount; root++)
        {
            for (var rarity = ItemRarity.Common; rarity <= ItemRarity.Legendary; rarity++)
            {
                AssertPools(ArmorEffectTable.Get(root, rarity, isShield: false), root, rarity, "armor");
                AssertPools(ArmorEffectTable.Get(root, rarity, isShield: true), root, rarity, "shield");
            }
        }
    }

    private static void AssertPools(in ArmorEffectRow row, VariantRoot root, ItemRarity rarity, string table)
    {
        Assert.True(row.BonusAr <= 15, $"{table} {root} {rarity}: bonus AR {row.BonusAr} > 15 pool");
        Assert.True(row.DrPct <= 12, $"{table} {root} {rarity}: DR {row.DrPct}% > 12% pool");
        Assert.True(row.ShrugPct <= 20, $"{table} {root} {rarity}: shrug {row.ShrugPct}% > 20% pool");
        Assert.True(row.ReflectPct <= 25, $"{table} {root} {rarity}: reflect {row.ReflectPct}% > 25% pool");
        Assert.True(row.HpRegenPct <= 60, $"{table} {root} {rarity}: HP regen {row.HpRegenPct}% > 60% pool");
        Assert.True(row.SpellDrPct <= 18, $"{table} {root} {rarity}: spell DR {row.SpellDrPct}% > 18% pool");
        Assert.True(row.DodgePct <= 12, $"{table} {root} {rarity}: dodge {row.DodgePct}% > 12% pool");
    }
}
