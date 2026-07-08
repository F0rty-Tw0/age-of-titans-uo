namespace Server.Engines.Rarity;

// Implemented by the four equipment bases (BaseWeapon/BaseArmor/BaseClothing/BaseJewel)
// through their generated VariantRoot/LegendaryId serializable properties. Lets the
// RarityEffects API set and read variant state uniformly across item types.
public interface IVariantItem
{
    VariantRoot VariantRoot { get; set; }
    ushort LegendaryId { get; set; }
}
