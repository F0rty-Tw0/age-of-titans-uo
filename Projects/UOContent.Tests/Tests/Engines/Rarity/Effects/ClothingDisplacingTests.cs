using System;
using System.Linq;
using System.Reflection;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// The armor-displacing clothing rows (hats + cloth pants, 21-clothing.md §1) must be exactly 2x the
// regular clothing rows, field for field — booleans carry over unchanged. No clothing field trips a
// framework §9.8 shared-pool ceiling even doubled, so no cap-clamps are expected; if that ever
// changes, this test is where the documented exception goes.
public class ClothingDisplacingTests
{
    private static readonly VariantRoot[] ClothingRoots =
    {
        VariantRoot.Laurel, VariantRoot.Charis, VariantRoot.Maenad, VariantRoot.Hestian, VariantRoot.Arachne
    };

    [Fact]
    public void DisplacingRows_AreExactlyDoubleTheRegularRows()
    {
        var intProps = typeof(AccessoryEffectRow).GetProperties()
            .Where(p => p.PropertyType == typeof(int)).ToArray();
        var boolProps = typeof(AccessoryEffectRow).GetProperties()
            .Where(p => p.PropertyType == typeof(bool) && p.Name != nameof(AccessoryEffectRow.IsEmpty)).ToArray();

        foreach (var root in ClothingRoots)
        {
            for (var rarity = ItemRarity.Uncommon; rarity <= ItemRarity.Legendary; rarity++)
            {
                var regular = AccessoryEffectTable.Get(root, rarity, isClothing: true, displacing: false);
                var displacing = AccessoryEffectTable.Get(root, rarity, isClothing: true, displacing: true);

                Assert.False(displacing.IsEmpty, $"{root} {rarity} has no displacing row");

                foreach (var p in intProps)
                {
                    var r = (int)p.GetValue(regular)!;
                    var d = (int)p.GetValue(displacing)!;

                    Assert.True(d == r * 2, $"{root} {rarity} {p.Name}: displacing {d} != 2 * regular {r}");
                }

                foreach (var p in boolProps)
                {
                    Assert.Equal((bool)p.GetValue(regular)!, (bool)p.GetValue(displacing)!);
                }
            }
        }
    }

    [Fact]
    public void DisplacingRows_StayInsideSharedPoolCeilings()
    {
        // §9.8 dodge pool = 12%; the doubled Arachne dodge (max 8) stays under it. The stationary
        // regen bonus folds into the HP-regen consume cap (60%) at use time, not here. Assert the
        // one pooled clothing field that could collide.
        foreach (var root in ClothingRoots)
        {
            for (var rarity = ItemRarity.Uncommon; rarity <= ItemRarity.Legendary; rarity++)
            {
                var row = AccessoryEffectTable.Get(root, rarity, isClothing: true, displacing: true);
                Assert.True(row.DodgePct <= 12, $"{root} {rarity}: displacing dodge {row.DodgePct}% > 12% pool");
            }
        }
    }
}
