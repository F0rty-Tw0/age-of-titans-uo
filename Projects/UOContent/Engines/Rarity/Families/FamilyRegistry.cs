using System;
using System.Collections.Generic;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Single source of truth for the rarity engine. Aggregates the per-family declarative definitions
// (Families/*.cs) into the flat, array-backed lookups the hot paths need. Consumers keep their
// public facades; those facades read the arrays built here at type init (zero allocation per
// lookup). One registration line per family below — explicit, not reflection-scanned.
public static class FamilyRegistry
{
    // ---- Registration (one line per family) --------------------------------------------------
    private static readonly WeaponFamilyDefinition[] _weaponFamilies =
    {
        AxesFamily.Definition,
        SwordsFamily.Definition,
        PolearmsFamily.Definition,
        MacesFamily.Definition,
        StavesFamily.Definition,
        FencingFamily.Definition,
        ArcheryFamily.Definition
    };

    private static readonly ArmorFamilyDefinition[] _armorFamilies =
    {
        RingmailFamily.Definition,
        ChainmailFamily.Definition,
        PlateFamily.Definition,
        LeatherFamily.Definition,
        StuddedFamily.Definition,
        BoneFamily.Definition
    };

    private static readonly ShieldFamilyDefinition[] _shieldFamilies =
    {
        ShieldsFamily.Definition
    };

    private static readonly AccessoryFamilyDefinition[] _accessoryFamilies =
    {
        JewelryFamily.Definition,
        ClothingFamily.Definition
    };

    private static readonly LegacyRootsDefinition[] _legacyDefinitions =
    {
        LegacyRoots.Definition
    };

    // ---- Sizes (enum-derived; NOT data) ------------------------------------------------------
    private const int RootCount = VariantRootInfo.RootCount;   // VariantRoot.None..Pnoe
    private const int RarityCount = 5;                          // ItemRarity.Common..Legendary
    private const int MaterialCount = 13;                       // ArmorMaterialType.Cloth..Stone
    private const int SlotCount = 7;                            // ArmorBodyType.Gorget..Shield

    // Fixed named stack-group ids (mirror the legacy VariantRootInfo constants exactly).
    private const byte FirstSingletonGroup = 9;

    // ---- Aggregated lookups ------------------------------------------------------------------
    public static readonly string[] RootDisplayNames = new string[RootCount];
    public static readonly string[] RootMythTags = new string[RootCount];
    public static readonly int[] RootBaseHues = new int[RootCount];
    public static readonly byte[] RootStackGroups = new byte[RootCount];
    public static readonly int StackGroupCount;

    public static readonly WeaponEffectRow[,] WeaponRows = new WeaponEffectRow[RootCount, RarityCount];
    public static readonly ArmorEffectRow[,] ArmorRows = new ArmorEffectRow[RootCount, RarityCount];
    public static readonly ArmorEffectRow[,] ShieldRows = new ArmorEffectRow[RootCount, RarityCount];
    public static readonly AccessoryEffectRow[,] JewelryRows = new AccessoryEffectRow[RootCount, RarityCount];
    public static readonly AccessoryEffectRow[,] ClothingRows = new AccessoryEffectRow[RootCount, RarityCount];
    public static readonly AccessoryEffectRow[,] ClothingDisplacingRows = new AccessoryEffectRow[RootCount, RarityCount];

    public static readonly (ClauseType Signature, short S1, short S2, short S3)[,] SlotSignatures =
        new (ClauseType, short, short, short)[MaterialCount, SlotCount];

    public static readonly Dictionary<Type, byte> TypeToWeaponFamily = new();

    // Weapon type -> its framework §7 base ladder stats (ratio, swing seconds). Feeds the runtime
    // rarity damage/speed anchors (RarityDamageAnchors) — one hashed lookup per swing, no allocation.
    public static readonly Dictionary<Type, (double Ratio, double SwingSeconds)> WeaponBaseStats = new();

    public static readonly Dictionary<VariantRoot, byte> RootToWeaponFamily = new();
    public static readonly Dictionary<VariantRoot, ArmorMaterialType> RootToArmorMaterial = new();
    public static readonly Dictionary<ArmorMaterialType, ArmorFamilyDefinition> ArmorFamilyByMaterial = new();

    public static readonly LegendaryEntry[] Legendaries;

    private static readonly Dictionary<ArmorMaterialType, (int Threshold, string Name, BuffIcon Icon)> _capstones = new();

    // Which single definition claims each non-None root (for validation + Stage 2 root resolution).
    public static readonly bool[] RootClaimed = new bool[RootCount];
    private static readonly bool[] _rootIsLegacy = new bool[RootCount];

    static FamilyRegistry()
    {
        var legendaries = new List<LegendaryEntry>();
        var namedGroup = new StackGroup[RootCount];

        RootDisplayNames[0] = ""; // VariantRoot.None — unhued, ungrouped, empty prefix.

        foreach (var fam in _weaponFamilies)
        {
            foreach (var lane in fam.Lanes)
            {
                ApplyLane(lane, namedGroup);
                CopyRows(lane.Weapon, WeaponRows, lane.Root);
                RootToWeaponFamily[lane.Root] = fam.Family;
            }

            if (fam.Ratios?.Length != fam.LadderTypes.Length || fam.SwingSeconds?.Length != fam.LadderTypes.Length)
            {
                throw new InvalidOperationException(
                    $"Weapon family {fam.Family} must have one ratio and one swing-seconds entry per ladder type."
                );
            }

            for (var i = 0; i < fam.LadderTypes.Length; i++)
            {
                TypeToWeaponFamily[fam.LadderTypes[i]] = fam.Family;
                WeaponBaseStats[fam.LadderTypes[i]] = (fam.Ratios[i], fam.SwingSeconds[i]);
            }

            legendaries.AddRange(fam.Legendaries);
        }

        foreach (var fam in _armorFamilies)
        {
            foreach (var lane in fam.Lanes)
            {
                ApplyLane(lane, namedGroup);
                CopyRows(lane.Armor, ArmorRows, lane.Root);
                RootToArmorMaterial[lane.Root] = fam.Material;
            }

            foreach (var sig in fam.SlotSignatures)
            {
                SlotSignatures[(int)fam.Material, (int)sig.Slot] = (sig.Signature, sig.S1, sig.S2, sig.S3);
            }

            _capstones[fam.Material] = (fam.CapstoneThreshold, fam.CapstoneName, fam.CapstoneIcon);
            ArmorFamilyByMaterial[fam.Material] = fam;
            legendaries.AddRange(fam.Legendaries);
        }

        foreach (var fam in _shieldFamilies)
        {
            foreach (var lane in fam.Lanes)
            {
                ApplyLane(lane, namedGroup);
                CopyRows(lane.Shield, ShieldRows, lane.Root);
            }

            legendaries.AddRange(fam.Legendaries);
        }

        foreach (var fam in _accessoryFamilies)
        {
            var target = fam.IsClothing ? ClothingRows : JewelryRows;

            foreach (var lane in fam.Lanes)
            {
                ApplyLane(lane, namedGroup);
                CopyRows(fam.IsClothing ? lane.Clothing : lane.Jewelry, target, lane.Root);

                if (fam.IsClothing)
                {
                    if (lane.ClothingDisplacing == null)
                    {
                        throw new InvalidOperationException(
                            $"Clothing lane {lane.Root} is missing its displacing (armor-displacing) row set."
                        );
                    }

                    CopyRows(lane.ClothingDisplacing, ClothingDisplacingRows, lane.Root);
                }
            }

            legendaries.AddRange(fam.Legendaries);
        }

        foreach (var def in _legacyDefinitions)
        {
            foreach (var lane in def.Lanes)
            {
                ApplyLane(lane, namedGroup);
                _rootIsLegacy[(int)lane.Root] = true;

                if (lane.Armor != null)
                {
                    CopyRows(lane.Armor, ArmorRows, lane.Root);
                }

                if (lane.Shield != null)
                {
                    CopyRows(lane.Shield, ShieldRows, lane.Root);
                }
            }
        }

        // Order armor families by their material ladder index (0..2) for the roller; split the
        // accessory families into the jewelry / clothing singletons.
        var metal = new ArmorFamilyDefinition[3];
        var light = new ArmorFamilyDefinition[3];

        foreach (var fam in _armorFamilies)
        {
            if (fam.Family == LegendaryRegistry.FamilyMetalArmor)
            {
                metal[fam.LadderIndex] = fam;
            }
            else
            {
                light[fam.LadderIndex] = fam;
            }
        }

        MetalArmorFamilies = metal;
        LightArmorFamilies = light;

        foreach (var fam in _accessoryFamilies)
        {
            if (fam.IsClothing)
            {
                ClothingFamilyDef = fam;
            }
            else
            {
                JewelryFamilyDef = fam;
            }
        }

        // Resolve stack-group ids: named groups take their fixed id; every remaining claimed root
        // (Singleton) gets a unique id assigned in ENUM order, exactly as the legacy pass did.
        var next = FirstSingletonGroup;

        for (var i = 1; i < RootCount; i++)
        {
            if (!RootClaimed[i])
            {
                continue;
            }

            RootStackGroups[i] = namedGroup[i] == StackGroup.Singleton ? next++ : (byte)namedGroup[i];
        }

        var max = 0;

        foreach (var g in RootStackGroups)
        {
            if (g > max)
            {
                max = g;
            }
        }

        StackGroupCount = max + 1;

        legendaries.Sort((a, b) => a.Id.CompareTo(b.Id));
        Legendaries = legendaries.ToArray();

        Validate();
    }

    private static void ApplyLane(LaneDefinition lane, StackGroup[] namedGroup)
    {
        var i = (int)lane.Root;

        if (RootClaimed[i])
        {
            throw new InvalidOperationException($"VariantRoot {lane.Root} is claimed by more than one family definition.");
        }

        RootClaimed[i] = true;
        RootDisplayNames[i] = lane.DisplayName;
        RootMythTags[i] = lane.MythTag;
        RootBaseHues[i] = lane.BaseHue;
        namedGroup[i] = lane.StackGroup;
    }

    // Boot validation — throws at type init on any data gap so a mis-authored family fails fast
    // instead of silently corrupting drops/saves.
    private static void Validate()
    {
        for (var i = 1; i < RootCount; i++)
        {
            var root = (VariantRoot)i;

            if (!RootClaimed[i])
            {
                throw new InvalidOperationException($"VariantRoot {root} is not claimed by any family definition.");
            }

            if (string.IsNullOrEmpty(RootDisplayNames[i]))
            {
                throw new InvalidOperationException($"VariantRoot {root} has an empty display name.");
            }

            if (string.IsNullOrEmpty(RootMythTags[i]))
            {
                throw new InvalidOperationException($"VariantRoot {root} has an empty myth tag.");
            }

            if (!_rootIsLegacy[i] && RootBaseHues[i] == 0)
            {
                throw new InvalidOperationException($"VariantRoot {root} has a zero base hue.");
            }
        }

        foreach (var (type, stats) in WeaponBaseStats)
        {
            if (stats.Ratio <= 0 || stats.SwingSeconds <= 0)
            {
                throw new InvalidOperationException(
                    $"Weapon type {type.Name} has a non-positive anchor (ratio {stats.Ratio}, seconds {stats.SwingSeconds})."
                );
            }
        }

        var seenIds = new HashSet<ushort>(Legendaries.Length);
        var seenTriples = new HashSet<(byte, VariantRoot, byte)>(Legendaries.Length);

        foreach (var entry in Legendaries)
        {
            if (!seenIds.Add(entry.Id))
            {
                throw new InvalidOperationException($"Duplicate legendary id {entry.Id} ({entry.Name}).");
            }

            if (!seenTriples.Add((entry.Family, entry.Root, entry.BaseIndex)))
            {
                throw new InvalidOperationException(
                    $"Duplicate legendary (family {entry.Family}, root {entry.Root}, base {entry.BaseIndex})."
                );
            }
        }
    }

    private static void CopyRows<T>(T[] rows, T[,] target, VariantRoot root)
    {
        if (rows == null)
        {
            return;
        }

        if (rows.Length != 4)
        {
            throw new InvalidOperationException(
                $"VariantRoot {root} lane must have exactly 4 rows (Uncommon..Legendary), found {rows.Length}."
            );
        }

        // rows[0..3] = Uncommon..Legendary; the table's Common slot stays default.
        for (var r = 0; r < rows.Length; r++)
        {
            target[(int)root, (int)ItemRarity.Uncommon + r] = rows[r];
        }
    }

    // ---- Capstone facade (reproduces WornEffectState's switch defaults exactly) ---------------
    public static int CapstoneThreshold(ArmorMaterialType material) =>
        _capstones.TryGetValue(material, out var c) ? c.Threshold : 0;

    public static string CapstoneName(ArmorMaterialType material) =>
        _capstones.TryGetValue(material, out var c) ? c.Name : "Siege-Shock";

    public static BuffIcon CapstoneIcon(ArmorMaterialType material) =>
        _capstones.TryGetValue(material, out var c) ? c.Icon : BuffIcon.Knockout;

    // A per-material body-armor root (Naias, Adamas, …) -> its material. False for legacy shared
    // armor roots, shield roots, and non-armor roots.
    public static bool TryGetArmorMaterial(VariantRoot root, out ArmorMaterialType material) =>
        RootToArmorMaterial.TryGetValue(root, out material);

    // A concrete weapon type -> its framework §7 ladder ratio + base swing seconds. False for
    // non-ladder (e.g. subclassed) weapon types; callers then leave the weapon's stock stats alone.
    public static bool TryGetWeaponBaseStats(Type type, out double ratio, out double swingSeconds)
    {
        if (WeaponBaseStats.TryGetValue(type, out var stats))
        {
            ratio = stats.Ratio;
            swingSeconds = stats.SwingSeconds;
            return true;
        }

        ratio = 0;
        swingSeconds = 0;
        return false;
    }

    // ---- Definition accessors (for the loot roller + GM commands to shape their own arrays) ----
    // Weapon families in family-id order (0=axes..6=archery) — index-aligned with the roller's
    // familyIndex. Armor families filtered/ordered by material ladder index (0..2).
    public static WeaponFamilyDefinition[] WeaponFamilies => _weaponFamilies;

    public static ArmorFamilyDefinition[] MetalArmorFamilies { get; private set; }
    public static ArmorFamilyDefinition[] LightArmorFamilies { get; private set; }
    public static ShieldFamilyDefinition ShieldFamilyDef => _shieldFamilies[0];
    public static AccessoryFamilyDefinition JewelryFamilyDef { get; private set; }
    public static AccessoryFamilyDefinition ClothingFamilyDef { get; private set; }

    // The theme roots of a family/set, in lane order (uniform-random pick order is irrelevant, but
    // this preserves the legacy arrays' order for readability + GenArmorSet round-robin).
    public static VariantRoot[] LaneRoots(LaneDefinition[] lanes)
    {
        var roots = new VariantRoot[lanes.Length];

        for (var i = 0; i < lanes.Length; i++)
        {
            roots[i] = lanes[i].Root;
        }

        return roots;
    }

    // True once every non-None VariantRoot is claimed by exactly one definition (full-registry
    // checks — stack-group ids, table completeness — are only valid in this state).
    public static bool IsComplete
    {
        get
        {
            for (var i = 1; i < RootCount; i++)
            {
                if (!RootClaimed[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
