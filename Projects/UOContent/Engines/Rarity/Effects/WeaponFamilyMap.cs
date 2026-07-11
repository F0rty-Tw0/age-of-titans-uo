using System;
using System.Collections.Generic;

namespace Server.Engines.Rarity;

// Maps a weapon's concrete Type to its LegendaryRegistry family byte, and each weapon VariantRoot
// to the family it belongs to (framework §3). Array-backed facade over the family registry: the
// type lists and root sets come from each WeaponFamilyDefinition (Families/*.cs), so the roller,
// the root validation, and the re-theme save migration all agree on one source of truth. Lookups
// are exact-type (drops construct exact types); an unknown/subclassed weapon type simply isn't
// found, and callers treat that leniently.
public static class WeaponFamilyMap
{
    private static readonly Dictionary<Type, byte> _typeToFamily = FamilyRegistry.TypeToWeaponFamily;
    private static readonly Dictionary<VariantRoot, byte> _rootToFamily = FamilyRegistry.RootToWeaponFamily;

    // Weapon type -> weapon family. False for non-weapon or unmapped (e.g. subclassed) types.
    public static bool TryGetFamily(Item item, out byte family)
    {
        family = 0;
        return item != null && _typeToFamily.TryGetValue(item.GetType(), out family);
    }

    // Weapon root -> its owning weapon family. False for non-weapon roots (armor/jewelry/clothing).
    public static bool TryGetRootFamily(VariantRoot root, out byte family) =>
        _rootToFamily.TryGetValue(root, out family);

    public static bool IsWeaponRoot(VariantRoot root) => _rootToFamily.ContainsKey(root);
}
