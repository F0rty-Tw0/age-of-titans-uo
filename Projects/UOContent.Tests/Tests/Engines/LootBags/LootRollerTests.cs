using System.Collections.Generic;
using Server.Engines.LootBags;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// Pure decision-logic tests — no item construction, no world fixture (LootRoller.RollDecision
// never touches an Item). Statistical tolerances below use samples large enough that the actual
// std error is a small fraction of the tolerance (documented per-test), to avoid flaky failures.
public class LootRollerDecisionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void RollDecision_NeverExceedsBagLevelRarityCeiling(int bagLevel)
    {
        var ceiling = RarityConfig.MaxRarityForBagLevel(bagLevel);

        for (var i = 0; i < 3000; i++)
        {
            var decision = LootRoller.RollDecision(bagLevel);
            Assert.True(decision.Rarity <= ceiling, $"bag {bagLevel} rolled {decision.Rarity}, above ceiling {ceiling}");
        }
    }

    [Fact]
    public void RollDecision_BagLevelOne_NeverExceedsUncommon()
    {
        for (var i = 0; i < 3000; i++)
        {
            Assert.True(LootRoller.RollDecision(1).Rarity <= ItemRarity.Uncommon);
        }
    }

    [Fact]
    public void RollDecision_BagLevelThree_NeverExceedsRare()
    {
        for (var i = 0; i < 3000; i++)
        {
            Assert.True(LootRoller.RollDecision(3).Rarity <= ItemRarity.Rare);
        }
    }

    [Fact]
    public void RollDecision_CategoryDistribution_MatchesWeightsWithinTolerance()
    {
        // Weights 45/25/10/12/8 out of 100. n=50000 → smallest bucket (8%) expects 4000 with a
        // std dev of ~61; a tolerance of 1500 is ~25x that, effectively flake-proof.
        const int samples = 50000;
        var counts = new int[5];

        for (var i = 0; i < samples; i++)
        {
            counts[(int)LootRoller.RollDecision(5).Category]++;
        }

        AssertWithinTolerance(counts[(int)LootRoller.LootCategory.Weapon], samples, 45);
        AssertWithinTolerance(counts[(int)LootRoller.LootCategory.Armor], samples, 25);
        AssertWithinTolerance(counts[(int)LootRoller.LootCategory.Shield], samples, 10);
        AssertWithinTolerance(counts[(int)LootRoller.LootCategory.Jewelry], samples, 12);
        AssertWithinTolerance(counts[(int)LootRoller.LootCategory.Clothing], samples, 8);
    }

    [Fact]
    public void RollDecision_RarityDistributionAtBagLevelTen_MatchesWeightsWithinTolerance()
    {
        // Weights 0/0/0/90/10 out of 100 (framework §8 bag-level-10 row, 2026-07-08 no-Commons
        // directive). Common/Uncommon/Rare are 0-weight buckets — zero must be exact, not "within
        // tolerance", since a single stray sample would mean the floor is broken.
        const int samples = 50000;
        var counts = new int[5];

        for (var i = 0; i < samples; i++)
        {
            counts[(int)LootRoller.RollDecision(10).Rarity]++;
        }

        Assert.Equal(0, counts[(int)ItemRarity.Common]);
        Assert.Equal(0, counts[(int)ItemRarity.Uncommon]);
        Assert.Equal(0, counts[(int)ItemRarity.Rare]);
        AssertWithinTolerance(counts[(int)ItemRarity.Epic], samples, 90);
        AssertWithinTolerance(counts[(int)ItemRarity.Legendary], samples, 10);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void RollDecision_NeverRollsBelowBagLevelRarityFloor(int bagLevel)
    {
        // User directive 2026-07-08: floors rise with bag level — L0-4 Uncommon, L5-9 Rare, L10 Epic.
        var floor = bagLevel switch
        {
            <= 4 => ItemRarity.Uncommon,
            <= 9 => ItemRarity.Rare,
            _ => ItemRarity.Epic
        };

        for (var i = 0; i < 3000; i++)
        {
            var decision = LootRoller.RollDecision(bagLevel);
            Assert.True(decision.Rarity >= floor, $"bag {bagLevel} rolled {decision.Rarity}, below floor {floor}");
        }
    }

    [Fact]
    public void RollDecision_NeverRollsCommon_AtAnyBagLevel()
    {
        for (var bagLevel = 0; bagLevel <= 10; bagLevel++)
        {
            for (var i = 0; i < 2000; i++)
            {
                Assert.NotEqual(ItemRarity.Common, LootRoller.RollDecision(bagLevel).Rarity);
            }
        }
    }

    [Fact]
    public void RollDecision_BaseIndexWeighting_FavorsLowIndexAtBagZeroAndHighIndexAtBagTen()
    {
        // Isolate the axe family (N=8, weapon category ~45% * 1/7 ~6.4% of all rolls) and check
        // the modal (most-frequent) base index sits at the ladder extreme the bag level favors.
        var low = new int[8];
        var high = new int[8];

        for (var i = 0; i < 100000; i++)
        {
            var lowRoll = LootRoller.RollDecision(0);

            if (lowRoll.Category == LootRoller.LootCategory.Weapon && lowRoll.Family == LegendaryRegistry.FamilyAxes)
            {
                low[lowRoll.BaseIndex]++;
            }

            var highRoll = LootRoller.RollDecision(10);

            if (highRoll.Category == LootRoller.LootCategory.Weapon && highRoll.Family == LegendaryRegistry.FamilyAxes)
            {
                high[highRoll.BaseIndex]++;
            }
        }

        Assert.Equal(0, IndexOfMax(low));  // bag 0 favors the weakest base (hatchet)
        Assert.Equal(7, IndexOfMax(high)); // bag 10 favors the strongest base (ornate axe)
    }

    [Fact]
    public void RollClothingDecision_NonRelicPieces_NeverRollLegendary()
    {
        // Only the 5 relic-bound pieces (BodySash/FancyShirt/Kilt/Robe/Cloak, indices 0-4) may
        // carry Legendary rarity — the other 7 curated pieces cap at Epic (21-clothing.md §1).
        var relicPieces = new HashSet<int> { 0, 1, 2, 3, 4 };

        for (var i = 0; i < 20000; i++)
        {
            var decision = LootRoller.RollDecision(10);

            if (decision.Category != LootRoller.LootCategory.Clothing || relicPieces.Contains(decision.BaseIndex))
            {
                continue;
            }

            Assert.NotEqual(ItemRarity.Legendary, decision.Rarity);
        }
    }

    private static void AssertWithinTolerance(int actualCount, int samples, int expectedWeightOutOf100)
    {
        var expected = samples * expectedWeightOutOf100 / 100.0;
        var tolerance = samples * 0.03;

        Assert.InRange(actualCount, expected - tolerance, expected + tolerance);
    }

    private static int IndexOfMax(int[] values)
    {
        var maxIndex = 0;

        for (var i = 1; i < values.Length; i++)
        {
            if (values[i] > values[maxIndex])
            {
                maxIndex = i;
            }
        }

        return maxIndex;
    }
}

// Legendary lookup reachability — every (family, theme, baseIndex) tuple LootRoller can produce
// must resolve through LegendaryRegistry.TryGetByRootAndBase. Catches registry holes directly,
// independent of LootRoller's internals (the enumerated universe mirrors the framework docs).
public class LootRollerLegendaryReachabilityTests
{
    // Per-family weapon roots (framework §3, 2026-07-07 re-theme). Mirrors LootRoller's own
    // _weaponThemesByFamily so a registry hole for any reachable (family, root, base) is caught.
    private static VariantRoot[] WeaponThemesFor(byte family) => family switch
    {
        LegendaryRegistry.FamilyAxes => new[]
        {
            VariantRoot.Zephyr, VariantRoot.Phobos, VariantRoot.Agrotera, VariantRoot.Pallas, VariantRoot.Stygian
        },
        LegendaryRegistry.FamilySwords => new[]
        {
            VariantRoot.Phoibos, VariantRoot.Areia, VariantRoot.Menis, VariantRoot.Aristeia, VariantRoot.Haima
        },
        LegendaryRegistry.FamilyPolearms => new[]
        {
            VariantRoot.Theristes, VariantRoot.Sarisa, VariantRoot.Phalanx, VariantRoot.Horme, VariantRoot.Zophos
        },
        LegendaryRegistry.FamilyMaces => new[]
        {
            VariantRoot.Ennosigaios, VariantRoot.Kataigis, VariantRoot.Rhaistes, VariantRoot.Eryma, VariantRoot.Kamatos
        },
        LegendaryRegistry.FamilyStaves => new[]
        {
            VariantRoot.Empousa, VariantRoot.Prester, VariantRoot.Alexikakos, VariantRoot.Manteia, VariantRoot.Baskania
        },
        LegendaryRegistry.FamilyFencing => new[]
        {
            VariantRoot.Ios, VariantRoot.Ephodos, VariantRoot.Aiolos, VariantRoot.Kentron, VariantRoot.Ophis
        },
        _ => new[]
        {
            VariantRoot.Hekatos, VariantRoot.Belos, VariantRoot.Pede, VariantRoot.Toxikon, VariantRoot.Skopos
        }
    };

    // Per-material armor roots (framework §3, 2026-07-07 re-theme). Mirrors LootRoller's
    // _metalArmorThemesByMaterial/_lightArmorThemesByMaterial: outer index = material ladder
    // position (0 ring/leather, 1 chain/studded, 2 plate/bone).
    private static VariantRoot[][] ArmorThemesByMaterialFor(byte family) =>
        family == LegendaryRegistry.FamilyMetalArmor
            ? new[]
            {
                new[] { VariantRoot.Hoplites, VariantRoot.Taxis, VariantRoot.Dromos, VariantRoot.Zoster, VariantRoot.Alkimos },
                new[] { VariantRoot.Phylax, VariantRoot.Egregoros, VariantRoot.Teichos, VariantRoot.Halysis, VariantRoot.Phrourion },
                new[] { VariantRoot.Adamas, VariantRoot.Kaminos, VariantRoot.Kolossos, VariantRoot.Panoplia, VariantRoot.Akamatos }
            }
            : new[]
            {
                new[] { VariantRoot.Naias, VariantRoot.Dryas, VariantRoot.Oreias, VariantRoot.Melissa, VariantRoot.Panika },
                new[] { VariantRoot.Kynegis, VariantRoot.Batos, VariantRoot.Arkas, VariantRoot.Elaphis, VariantRoot.Skia },
                new[] { VariantRoot.Melinoe, VariantRoot.Makaria, VariantRoot.Tymbos, VariantRoot.Nekyia, VariantRoot.Katachthon }
            };

    // Shields keep Aegis; the other four are shield-only (framework §3, 2026-07-07 re-theme).
    private static readonly VariantRoot[] ShieldThemes =
    {
        VariantRoot.Aegis, VariantRoot.Amyntor, VariantRoot.Probolos, VariantRoot.Herkos, VariantRoot.Pnoe
    };

    private static readonly VariantRoot[] JewelryThemes =
    {
        VariantRoot.Olympian, VariantRoot.Hecatean, VariantRoot.Tychean, VariantRoot.Nyxian, VariantRoot.Demetrian
    };

    [Theory]
    [InlineData(LegendaryRegistry.FamilyAxes, 8)]
    [InlineData(LegendaryRegistry.FamilySwords, 8)]
    [InlineData(LegendaryRegistry.FamilyPolearms, 2)]
    [InlineData(LegendaryRegistry.FamilyMaces, 7)]
    [InlineData(LegendaryRegistry.FamilyStaves, 3)]
    [InlineData(LegendaryRegistry.FamilyFencing, 6)]
    [InlineData(LegendaryRegistry.FamilyArchery, 3)]
    public void WeaponFamily_EveryThemeAndBaseIndex_ResolvesToALegendary(byte family, int baseCount)
    {
        foreach (var theme in WeaponThemesFor(family))
        {
            for (byte baseIndex = 0; baseIndex < baseCount; baseIndex++)
            {
                Assert.True(
                    LegendaryRegistry.TryGetByRootAndBase(family, theme, baseIndex, out _),
                    $"Missing legendary: family {family}, theme {theme}, base {baseIndex}"
                );
            }
        }
    }

    [Theory]
    [InlineData(LegendaryRegistry.FamilyMetalArmor)]
    [InlineData(LegendaryRegistry.FamilyLightArmor)]
    public void ArmorFamily_EveryThemeAndMaterial_ResolvesToALegendary(byte family)
    {
        var themesByMaterial = ArmorThemesByMaterialFor(family);

        for (byte material = 0; material < 3; material++)
        {
            foreach (var theme in themesByMaterial[material])
            {
                Assert.True(
                    LegendaryRegistry.TryGetByRootAndBase(family, theme, material, out _),
                    $"Missing legendary: family {family}, theme {theme}, material {material}"
                );
            }
        }
    }

    [Fact]
    public void Shields_EveryThemeAndShape_ResolvesToALegendary()
    {
        foreach (var theme in ShieldThemes)
        {
            for (byte shape = 0; shape < 6; shape++)
            {
                Assert.True(
                    LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyShields, theme, shape, out _),
                    $"Missing legendary: shield theme {theme}, shape {shape}"
                );
            }
        }
    }

    [Fact]
    public void Jewelry_EveryThemeAndSlot_ResolvesToALegendary()
    {
        foreach (var theme in JewelryThemes)
        {
            for (byte slot = 0; slot < 4; slot++)
            {
                Assert.True(
                    LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyJewelry, theme, slot, out _),
                    $"Missing legendary: jewelry theme {theme}, slot {slot}"
                );
            }
        }
    }

    [Fact]
    public void Clothing_EveryRelicThemeAndBoundPiece_ResolvesToALegendary()
    {
        Assert.True(LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyClothing, VariantRoot.Laurel, LegendaryRegistry.ClothingPieceBodySash, out _));
        Assert.True(LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyClothing, VariantRoot.Charis, LegendaryRegistry.ClothingPieceFancyShirt, out _));
        Assert.True(LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyClothing, VariantRoot.Maenad, LegendaryRegistry.ClothingPieceKilt, out _));
        Assert.True(LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyClothing, VariantRoot.Hestian, LegendaryRegistry.ClothingPieceRobe, out _));
        Assert.True(LegendaryRegistry.TryGetByRootAndBase(LegendaryRegistry.FamilyClothing, VariantRoot.Arachne, LegendaryRegistry.ClothingPieceCloak, out _));
    }
}

// Integration: exercises real item construction (Construct + ApplyRarity) under the shared world
// fixture, matching the pattern in RarityItemTests / LootBagItemTests.
[Collection("Sequential UOContent Tests")]
public class LootRollerIntegrationTests
{
    [Fact]
    public void Roll_AtBagLevelTen_AlwaysReturnsNonNullItemWithValidRarity()
    {
        for (var i = 0; i < 100; i++)
        {
            var item = LootRoller.Roll(10);

            try
            {
                Assert.NotNull(item);
                var rarity = Assert.IsAssignableFrom<IRarity>(item).Rarity;
                Assert.True(rarity <= ItemRarity.Legendary);
            }
            finally
            {
                item?.Delete();
            }
        }
    }
}
