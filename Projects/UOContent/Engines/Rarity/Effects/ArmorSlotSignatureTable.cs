using Server.Items;

namespace Server.Engines.Rarity;

// Option A milestone (armor-slot-set-design.md "FINAL MATRIX"): Epic/Legendary armor Signature
// clauses keyed per-(material x slot). Shields are excluded — they stay on the root-keyed
// ArmorEffectTable.Signature lookup. Array-backed facade over the family registry: the live cells
// now live in each ArmorFamilyDefinition.SlotSignatures (Families/*.cs). Cells with no entry
// resolve to ClauseType.None via the registry's default-tuple fallback.
public static class ArmorSlotSignatureTable
{
    internal const int MaterialCount = 13; // ArmorMaterialType.Cloth..Stone (also sizes the P4 set counter)
    private const int SlotCount = 7;       // ArmorBodyType.Gorget..Shield

    private static readonly (ClauseType Signature, short S1, short S2, short S3)[,] _cells = FamilyRegistry.SlotSignatures;

    // Zero-allocation lookup. Any material/slot pair with no entry (out-of-scope material, shield
    // slot, or an un-populated cell) returns the default None-tuple.
    public static (ClauseType Signature, short S1, short S2, short S3) Get(ArmorMaterialType material, ArmorBodyType slot)
    {
        if ((int)material < 0 || (int)material >= MaterialCount || (int)slot < 0 || (int)slot >= SlotCount)
        {
            return default;
        }

        return _cells[(int)material, (int)slot];
    }
}
