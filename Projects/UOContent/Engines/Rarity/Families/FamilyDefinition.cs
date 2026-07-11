using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Declarative family/set definitions — the single source of truth for the rarity engine
// (framework §3). One family = one file under Families/; FamilyRegistry aggregates them and
// the hot-path lookup tables/maps are populated from that aggregate at type init. Plain data
// classes only — no builders, no interfaces-with-one-impl.

// Named stacking pool (framework §9.4). Singleton = the root gets its own unique id, assigned in
// enum order at registry build (reproduces the legacy VariantRootInfo.BuildStackGroups pass).
public enum StackGroup : byte
{
    Singleton = 0,
    Bulwark,
    Forge,
    Mending,
    Ward,
    Stride,
    Sorcery,
    Night,
    Durability
}

// One root's identity + its per-rarity effect rows. Row arrays are indexed [0]=Uncommon, [1]=Rare,
// [2]=Epic, [3]=Legendary (Common is always empty). Exactly one category array is populated per
// lane; legacy armor roots populate BOTH Armor and Shield.
public sealed class LaneDefinition
{
    public VariantRoot Root { get; init; }
    public string DisplayName { get; init; }   // lowercase single-click prefix (framework §6)
    public string MythTag { get; init; }       // short myth owner/concept (framework §3 "Myth"); tooltip effects-line prefix
    public int BaseHue { get; init; }          // the Uncommon (2nd) shade; ramp adds +0..+3 by rarity
    public StackGroup StackGroup { get; init; } = StackGroup.Singleton;

    public WeaponEffectRow[] Weapon { get; init; }
    public ArmorEffectRow[] Armor { get; init; }
    public ArmorEffectRow[] Shield { get; init; }
    public AccessoryEffectRow[] Jewelry { get; init; }
    public AccessoryEffectRow[] Clothing { get; init; }

    // The armor-displacing clothing row set (hats/pants — 21-clothing.md §1): same shape as
    // Clothing, values doubled. Populated only on clothing lanes, alongside Clothing.
    public AccessoryEffectRow[] ClothingDisplacing { get; init; }
}

// One (material x slot) Epic/Legendary signature cell (Option A slot-set milestone).
public readonly record struct SlotSignature(ArmorBodyType Slot, ClauseType Signature, short S1, short S2, short S3);

// A weapon family (framework §3/§7): five lanes, its ladder-ordered concrete weapon types (feeds
// WeaponFamilyMap, the LootRoller factories, and the ladder base count), and its legendaries.
// Ratios/SwingSeconds are the framework §7 base ladder, index-aligned with LadderTypes: they drive
// the runtime rarity damage/speed anchors (FamilyRegistry.WeaponBaseStats + RarityDamageAnchors).
public sealed class WeaponFamilyDefinition
{
    public byte Family { get; init; }
    public LaneDefinition[] Lanes { get; init; }
    public Type[] LadderTypes { get; init; }
    public double[] Ratios { get; init; }        // framework §7 damage ratio per base (LadderTypes order)
    public double[] SwingSeconds { get; init; }  // framework §7 base swing seconds per base (LadderTypes order)
    public Func<Item>[] Factories { get; init; }
    public LegendaryEntry[] Legendaries { get; init; }
}

// An armor family: one material, five lanes (body-armor rows), its per-slot signatures, capstone
// metadata (framework §P4), the ladder-ordered slot factories for the roller, and its legendaries.
public sealed class ArmorFamilyDefinition
{
    public byte Family { get; init; }                 // FamilyMetalArmor or FamilyLightArmor
    public ArmorMaterialType Material { get; init; }
    public int LadderIndex { get; init; }             // material ladder position within the family (0..2)
    public LaneDefinition[] Lanes { get; init; }
    public SlotSignature[] SlotSignatures { get; init; }
    public int CapstoneThreshold { get; init; }
    public string CapstoneName { get; init; }
    public BuffIcon CapstoneIcon { get; init; }
    public Func<Item>[] SlotFactories { get; init; }  // roller: uniform pick among real slot pieces
    public Func<Item>[] SetPieces { get; init; }      // GenArmorSet: one full body suit, slot order
    public LegendaryEntry[] Legendaries { get; init; }
}

// The shield family (12-shields.md): root-keyed lanes (shields are NOT in ArmorSlotSignatureTable),
// shape factories, and legendaries.
public sealed class ShieldFamilyDefinition
{
    public byte Family { get; init; }                 // FamilyShields
    public LaneDefinition[] Lanes { get; init; }
    public Func<Item>[] ShieldFactories { get; init; }
    public LegendaryEntry[] Legendaries { get; init; }
}

// A jewelry or clothing family (framework §7 — no damage/AR ladder).
public sealed class AccessoryFamilyDefinition
{
    public byte Family { get; init; }                 // FamilyJewelry or FamilyClothing
    public bool IsClothing { get; init; }
    public LaneDefinition[] Lanes { get; init; }
    public Func<Item>[] Factories { get; init; }
    public LegendaryEntry[] Legendaries { get; init; }
}

// The five retired shared armor roots (Polias/Cyclopean/Paean/Tritonian/Talarian) — decode-only for
// old saves (re-theme 2026-07-07), remapped to lane-specific roots on load. They keep their live
// table rows (armor, and shields for four of them) and their names/hues/groups. Not a drop source.
public sealed class LegacyRootsDefinition
{
    public LaneDefinition[] Lanes { get; init; }
}
