using System.Collections.Generic;
using Server.Items;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// Pure lookups against the static (material x slot) signature table — no world boot needed.
public class ArmorSlotSignatureTableTests
{
    [Theory]
    // One representative per material, including all four Phase 3 cells.
    [InlineData(ArmorMaterialType.Leather, ArmorBodyType.Helmet, ClauseType.SpellDrBoostFirstHit, 10, 0)]
    [InlineData(ArmorMaterialType.Studded, ArmorBodyType.Chest, ClauseType.ShrugFirstHitPoisonAttacker, 0, 0)]
    [InlineData(ArmorMaterialType.Bone, ArmorBodyType.Chest, ClauseType.ShrugFirstHitDrainStam, 5, 0)]
    [InlineData(ArmorMaterialType.Ringmail, ArmorBodyType.Chest, ClauseType.ShrugFirstHitDrBurst, 8, 3)]
    [InlineData(ArmorMaterialType.Plate, ArmorBodyType.Arms, ClauseType.ShrugReflectStun, 10, 0)]
    [InlineData(ArmorMaterialType.Chainmail, ArmorBodyType.Legs, ClauseType.HpRegenBurstOnCritTaken, 5, 0)]
    public void Get_ReturnsPopulatedCell(
        ArmorMaterialType material, ArmorBodyType slot, ClauseType expected, short s1, short s2
    )
    {
        var (signature, p1, p2, _) = ArmorSlotSignatureTable.Get(material, slot);

        Assert.Equal(expected, signature);
        Assert.Equal(s1, p1);
        Assert.Equal(s2, p2);
    }

    [Theory]
    [InlineData(ArmorMaterialType.Cloth, ArmorBodyType.Helmet)]    // out-of-scope material
    [InlineData(ArmorMaterialType.Leather, ArmorBodyType.Shield)]  // shields stay root-keyed
    [InlineData(ArmorMaterialType.Ringmail, ArmorBodyType.Helmet)] // no ringmail helm exists
    [InlineData((ArmorMaterialType)200, (ArmorBodyType)200)]       // out-of-range indexes
    [InlineData((ArmorMaterialType)(-1), (ArmorBodyType)(-1))]
    public void Get_UnpopulatedOrOutOfRange_ReturnsNone(ArmorMaterialType material, ArmorBodyType slot)
    {
        Assert.Equal(ClauseType.None, ArmorSlotSignatureTable.Get(material, slot).Signature);
    }

    [Fact]
    public void AllLiveCells_AreGloballyUnique()
    {
        // Option A locked decision: full global uniqueness across every (material x slot) cell.
        var seen = new HashSet<ClauseType>();

        for (var material = 0; material < ArmorSlotSignatureTable.MaterialCount; material++)
        {
            for (var slot = 0; slot <= (int)ArmorBodyType.Shield; slot++)
            {
                var signature = ArmorSlotSignatureTable.Get((ArmorMaterialType)material, (ArmorBodyType)slot).Signature;

                if (signature != ClauseType.None)
                {
                    Assert.True(seen.Add(signature), $"duplicate signature {signature} at {(ArmorMaterialType)material}/{(ArmorBodyType)slot}");
                }
            }
        }

        Assert.Equal(30, seen.Count); // the full matrix: 6+6+5+4+3+6 populated cells
    }
}
