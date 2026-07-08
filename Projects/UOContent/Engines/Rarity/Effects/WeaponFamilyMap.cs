using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Engines.Rarity;

// Maps a weapon's concrete Type to its LegendaryRegistry family byte, and each weapon VariantRoot
// to the family it belongs to (framework §3, 2026-07-07 per-family re-theme). Built from the same
// class lists as LootRoller._weaponFactories so the roller, the root validation, and the re-theme
// save migration all agree on one source of truth. Lookups are exact-type (drops construct exact
// types); an unknown/subclassed weapon type simply isn't found, and callers treat that leniently.
public static class WeaponFamilyMap
{
    private static readonly Dictionary<Type, byte> _typeToFamily = BuildTypeMap();
    private static readonly Dictionary<VariantRoot, byte> _rootToFamily = BuildRootMap();

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

    private static Dictionary<Type, byte> BuildTypeMap()
    {
        var map = new Dictionary<Type, byte>();

        Add(map, LegendaryRegistry.FamilyAxes,
            typeof(Hatchet), typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe),
            typeof(ExecutionersAxe), typeof(TwoHandedAxe), typeof(LargeBattleAxe), typeof(OrnateAxe));

        Add(map, LegendaryRegistry.FamilySwords,
            typeof(ButcherKnife), typeof(Cleaver), typeof(Cutlass), typeof(Scimitar),
            typeof(Katana), typeof(Broadsword), typeof(Longsword), typeof(VikingSword));

        Add(map, LegendaryRegistry.FamilyPolearms, typeof(Bardiche), typeof(Halberd));

        // "War axe" is mechanically a mace (DefSkill = Macing) despite the axe class name/model.
        Add(map, LegendaryRegistry.FamilyMaces,
            typeof(Club), typeof(Mace), typeof(Maul), typeof(WarAxe),
            typeof(HammerPick), typeof(WarMace), typeof(WarHammer));

        Add(map, LegendaryRegistry.FamilyStaves, typeof(QuarterStaff), typeof(GnarledStaff), typeof(BlackStaff));

        Add(map, LegendaryRegistry.FamilyFencing,
            typeof(Dagger), typeof(Kryss), typeof(WarFork), typeof(Pitchfork),
            typeof(ShortSpear), typeof(Spear));

        Add(map, LegendaryRegistry.FamilyArchery, typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow));

        return map;
    }

    private static Dictionary<VariantRoot, byte> BuildRootMap()
    {
        var map = new Dictionary<VariantRoot, byte>();

        AddRoots(map, LegendaryRegistry.FamilyAxes,
            VariantRoot.Zephyr, VariantRoot.Phobos, VariantRoot.Agrotera, VariantRoot.Pallas, VariantRoot.Stygian);
        AddRoots(map, LegendaryRegistry.FamilySwords,
            VariantRoot.Phoibos, VariantRoot.Areia, VariantRoot.Menis, VariantRoot.Aristeia, VariantRoot.Haima);
        AddRoots(map, LegendaryRegistry.FamilyMaces,
            VariantRoot.Ennosigaios, VariantRoot.Kataigis, VariantRoot.Rhaistes, VariantRoot.Eryma, VariantRoot.Kamatos);
        AddRoots(map, LegendaryRegistry.FamilyPolearms,
            VariantRoot.Theristes, VariantRoot.Sarisa, VariantRoot.Phalanx, VariantRoot.Horme, VariantRoot.Zophos);
        AddRoots(map, LegendaryRegistry.FamilyStaves,
            VariantRoot.Empousa, VariantRoot.Prester, VariantRoot.Alexikakos, VariantRoot.Manteia, VariantRoot.Baskania);
        AddRoots(map, LegendaryRegistry.FamilyFencing,
            VariantRoot.Ios, VariantRoot.Ephodos, VariantRoot.Aiolos, VariantRoot.Kentron, VariantRoot.Ophis);
        AddRoots(map, LegendaryRegistry.FamilyArchery,
            VariantRoot.Hekatos, VariantRoot.Belos, VariantRoot.Pede, VariantRoot.Toxikon, VariantRoot.Skopos);

        return map;
    }

    private static void Add(Dictionary<Type, byte> map, byte family, params Type[] types)
    {
        foreach (var type in types)
        {
            map[type] = family;
        }
    }

    private static void AddRoots(Dictionary<VariantRoot, byte> map, byte family, params VariantRoot[] roots)
    {
        foreach (var root in roots)
        {
            map[root] = family;
        }
    }
}
