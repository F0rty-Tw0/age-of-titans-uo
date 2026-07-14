using System;
using System.Collections.Generic;
using Server.Engines.Rarity;

namespace Server.Engines.LootBags;

// Fills a loot bag with exactly one item (framework §8 — "one item per bag" user directive).
// Roll order: rarity -> category -> family -> theme -> base -> [construction-only] slot.
// The pure decision (RollDecision) is kept separate from item construction (Roll) so the
// table/weight/ceiling logic is unit-testable without the world fixture.
//
// The family taxonomy — weapon/armor/shield/jewelry/clothing themes, the concrete item factories,
// and the ladder counts — is sourced from FamilyRegistry (the single source of truth, Families/*.cs)
// at static init. Only the DATA moved; the rarity/category weights, base-pick curve, and every
// rolling decision below are unchanged.
public static class LootRoller
{
    public enum LootCategory : byte
    {
        Weapon,
        Armor,
        Shield,
        Jewelry,
        Clothing
    }

    // One roll's outcome before any Item is constructed. BaseIndex meaning depends on Category:
    // weapon = ladder position in the chosen weapon family; armor = material index (0=ring/leather,
    // 1=chain/studded, 2=plate/bone); shield = shape index; jewelry = slot index; clothing = piece
    // index (0-11 for a drop-variant, or one of the 5 ClothingPieceXxx constants at Legendary).
    public readonly record struct LootRollDecision(
        ItemRarity Rarity, LootCategory Category, byte Family, VariantRoot Theme, int BaseIndex
    );

    // ---- Rarity weight table (framework §8, proposal — tune live) ------------------------
    // Index = bag level 0..10, columns = Common/Uncommon/Rare/Epic/Legendary. The ceiling is
    // re-applied via RarityConfig.MaxRarityForBagLevel after the roll as a defensive second gate.
    // User directive 2026-07-08: no Commons from bags; floors — L0-4 Uncommon, L5-8 Rare, L9
    // Epic, L10 Legendary. Ceilings (RarityConfig.MaxRarityForBagLevel) are ≥ the floor at every
    // level, so the defensive ceiling clamp can never push a roll below its floor.
    // User directive 2026-07-14: bag 10 = GUARANTEED Legendary (L10 mobs are brutal; dupes are
    // fine — Divine Resonance echoes them); bag 9 takes bag 10's old 90/10 Epic/Legendary split.
    private static readonly int[][] _rarityWeights =
    {
        new[] { 0, 100, 0, 0, 0 },
        new[] { 0, 100, 0, 0, 0 },
        new[] { 0, 80, 20, 0, 0 },
        new[] { 0, 65, 35, 0, 0 },
        new[] { 0, 50, 42, 8, 0 },
        new[] { 0, 0, 85, 15, 0 },
        new[] { 0, 0, 75, 25, 0 },
        new[] { 0, 0, 66, 30, 4 },
        new[] { 0, 0, 55, 39, 6 },
        new[] { 0, 0, 0, 90, 10 },
        new[] { 0, 0, 0, 0, 100 }
    };

    // ---- Category weights (tunable placeholder, not framework-fixed) ----------------------
    private const int CategoryWeightWeapon = 45;
    private const int CategoryWeightArmor = 25;
    private const int CategoryWeightShield = 10;
    private const int CategoryWeightJewelry = 12;
    private const int CategoryWeightClothing = 8;
    private const int CategoryWeightTotal =
        CategoryWeightWeapon + CategoryWeightArmor + CategoryWeightShield + CategoryWeightJewelry + CategoryWeightClothing;

    private const int MaterialsPerArmorFamily = 3; // ring/chain/plate or leather/studded/bone

    // ---- Family taxonomy, sourced from FamilyRegistry at static init ----------------------
    // Per-family weapon roots, aligned index-for-index with _weaponFamilies (framework §3). Per-
    // material armor roots are keyed by material ladder position; the theme rolls AFTER the material
    // so each material drops only its own five roots. Factories/ladder counts come from the same
    // definitions, so the roller, root validation, and re-theme migration agree on one source.
    private static readonly byte[] _weaponFamilies;
    private static readonly int[] _weaponFamilyBaseCount;
    private static readonly VariantRoot[][] _weaponThemesByFamily;
    private static readonly Func<Item>[][] _weaponFactories;

    private static readonly VariantRoot[][] _metalArmorThemesByMaterial;
    private static readonly VariantRoot[][] _lightArmorThemesByMaterial;
    private static readonly Func<Item>[][] _metalArmorSlotFactories;
    private static readonly Func<Item>[][] _lightArmorSlotFactories;

    private static readonly VariantRoot[] _shieldThemes;
    private static readonly Func<Item>[] _shieldFactories;
    private static readonly VariantRoot[] _jewelryThemes;
    private static readonly Func<Item>[] _jewelryFactories;
    private static readonly VariantRoot[] _clothingThemes;
    private static readonly Func<Item>[] _clothingFactories;

    // ---- Per-domain candidate tables (pantheon-bags §5) -----------------------------------
    // Filtered views of the tables above: for each of the 11 PantheonDomain values, only the
    // families/materials/themes whose PantheonFx.GetDomain matches. Compact (empty families or
    // materials are dropped, not kept as empty placeholders) so a uniform pick over "available"
    // never lands on a slot the domain doesn't own. Indexed by (int)domain.
    private static readonly int[][] _domainWeaponFamilyIndices;
    private static readonly VariantRoot[][][] _domainWeaponThemesByFamily;

    private static readonly int[][] _domainMetalMaterialIndices;
    private static readonly VariantRoot[][][] _domainMetalMaterialThemes;
    private static readonly int[][] _domainLightMaterialIndices;
    private static readonly VariantRoot[][][] _domainLightMaterialThemes;

    private static readonly VariantRoot[][] _domainShieldThemes;
    private static readonly VariantRoot[][] _domainJewelryThemes;
    private static readonly VariantRoot[][] _domainClothingThemes;

    static LootRoller()
    {
        var weaponFamilies = FamilyRegistry.WeaponFamilies; // family-id order (0=axes..6=archery)
        _weaponFamilies = new byte[weaponFamilies.Length];
        _weaponFamilyBaseCount = new int[weaponFamilies.Length];
        _weaponThemesByFamily = new VariantRoot[weaponFamilies.Length][];
        _weaponFactories = new Func<Item>[weaponFamilies.Length][];

        for (var i = 0; i < weaponFamilies.Length; i++)
        {
            var fam = weaponFamilies[i];
            _weaponFamilies[i] = fam.Family;
            _weaponFamilyBaseCount[i] = fam.LadderTypes.Length;
            _weaponThemesByFamily[i] = FamilyRegistry.LaneRoots(fam.Lanes);
            _weaponFactories[i] = fam.Factories;
        }

        var metal = FamilyRegistry.MetalArmorFamilies; // ladder order 0..2 (ring/chain/plate)
        var light = FamilyRegistry.LightArmorFamilies; // ladder order 0..2 (leather/studded/bone)
        _metalArmorThemesByMaterial = new VariantRoot[metal.Length][];
        _metalArmorSlotFactories = new Func<Item>[metal.Length][];
        _lightArmorThemesByMaterial = new VariantRoot[light.Length][];
        _lightArmorSlotFactories = new Func<Item>[light.Length][];

        for (var i = 0; i < metal.Length; i++)
        {
            _metalArmorThemesByMaterial[i] = FamilyRegistry.LaneRoots(metal[i].Lanes);
            _metalArmorSlotFactories[i] = metal[i].SlotFactories;
        }

        for (var i = 0; i < light.Length; i++)
        {
            _lightArmorThemesByMaterial[i] = FamilyRegistry.LaneRoots(light[i].Lanes);
            _lightArmorSlotFactories[i] = light[i].SlotFactories;
        }

        _shieldThemes = FamilyRegistry.LaneRoots(FamilyRegistry.ShieldFamilyDef.Lanes);
        _shieldFactories = FamilyRegistry.ShieldFamilyDef.ShieldFactories;
        _jewelryThemes = FamilyRegistry.LaneRoots(FamilyRegistry.JewelryFamilyDef.Lanes);
        _jewelryFactories = FamilyRegistry.JewelryFamilyDef.Factories;
        _clothingThemes = FamilyRegistry.LaneRoots(FamilyRegistry.ClothingFamilyDef.Lanes);
        _clothingFactories = FamilyRegistry.ClothingFamilyDef.Factories;

        var domainCount = Enum.GetValues<PantheonDomain>().Length;
        _domainWeaponFamilyIndices = new int[domainCount][];
        _domainWeaponThemesByFamily = new VariantRoot[domainCount][][];
        _domainMetalMaterialIndices = new int[domainCount][];
        _domainMetalMaterialThemes = new VariantRoot[domainCount][][];
        _domainLightMaterialIndices = new int[domainCount][];
        _domainLightMaterialThemes = new VariantRoot[domainCount][][];
        _domainShieldThemes = new VariantRoot[domainCount][];
        _domainJewelryThemes = new VariantRoot[domainCount][];
        _domainClothingThemes = new VariantRoot[domainCount][];

        for (var d = 0; d < domainCount; d++)
        {
            var domain = (PantheonDomain)d;

            BuildDomainSubsetTable(_weaponThemesByFamily, domain, out _domainWeaponFamilyIndices[d], out _domainWeaponThemesByFamily[d]);
            BuildDomainSubsetTable(_metalArmorThemesByMaterial, domain, out _domainMetalMaterialIndices[d], out _domainMetalMaterialThemes[d]);
            BuildDomainSubsetTable(_lightArmorThemesByMaterial, domain, out _domainLightMaterialIndices[d], out _domainLightMaterialThemes[d]);
            _domainShieldThemes[d] = FilterThemes(_shieldThemes, domain);
            _domainJewelryThemes[d] = FilterThemes(_jewelryThemes, domain);
            _domainClothingThemes[d] = FilterThemes(_clothingThemes, domain);
        }
    }

    // Filters a theme array down to the roots belonging to one domain. Cold path (static init
    // only), so the List<T> + linear PantheonFx.GetDomain scan is fine.
    private static VariantRoot[] FilterThemes(VariantRoot[] themes, PantheonDomain domain)
    {
        var filtered = new List<VariantRoot>(themes.Length);

        for (var i = 0; i < themes.Length; i++)
        {
            if (PantheonFx.GetDomain(themes[i]) == domain)
            {
                filtered.Add(themes[i]);
            }
        }

        return filtered.ToArray();
    }

    // Filters a per-family/per-material theme table down to one domain, keeping only the
    // families/materials with a non-empty subset (compact — no empty placeholders). Shared by
    // the weapon family table and both armor material tables since all three are "index -> theme
    // array" shaped. Cold path (static init only).
    private static void BuildDomainSubsetTable(
        VariantRoot[][] themesByIndex, PantheonDomain domain, out int[] availableIndices, out VariantRoot[][] filteredThemes
    )
    {
        var indices = new List<int>();
        var themes = new List<VariantRoot[]>();

        for (var i = 0; i < themesByIndex.Length; i++)
        {
            var filtered = FilterThemes(themesByIndex[i], domain);

            if (filtered.Length > 0)
            {
                indices.Add(i);
                themes.Add(filtered);
            }
        }

        availableIndices = indices.ToArray();
        filteredThemes = themes.ToArray();
    }

    public static LootRollDecision RollDecision(int bagLevel)
    {
        var rarity = RollRarity(bagLevel);

        return RollCategory() switch
        {
            LootCategory.Weapon => RollWeaponDecision(rarity, bagLevel),
            LootCategory.Armor => RollArmorDecision(rarity, bagLevel),
            LootCategory.Shield => RollShieldDecision(rarity, bagLevel),
            LootCategory.Jewelry => RollJewelryDecision(rarity),
            _ => RollClothingDecision(rarity)
        };
    }

    // Constructs exactly one item for the given bag level and applies its rolled rarity/variant.
    // No announce here — the world broadcast fires when the loot bag is opened (LootBag), not
    // when the roll happens at mob death, so an unlooted bag never spoils its contents.
    public static Item Roll(int bagLevel)
    {
        var decision = RollDecision(bagLevel);
        var item = Construct(decision);

        ApplyRarity(item, decision);

        return item;
    }

    // Domain-locked roll (pantheon-bags §5): every candidate is filtered to one god's roots at
    // every rarity tier — no leaks. Rarity weights are untouched; only category/family/material/
    // theme selection is restricted to the domain's pre-filtered tables.
    public static LootRollDecision RollDecision(int bagLevel, PantheonDomain domain)
    {
        if (!TryRollCategoryForDomain(domain, out var category))
        {
            // Defensive: the root audit (framework §3) guarantees every domain has candidates in
            // at least one category. If that ever regresses, fail safe to the generic path rather
            // than throw on a live server.
            return RollDecision(bagLevel);
        }

        var rarity = RollRarity(bagLevel);

        return category switch
        {
            LootCategory.Weapon => RollWeaponDecisionForDomain(rarity, bagLevel, domain),
            LootCategory.Armor => RollArmorDecisionForDomain(rarity, bagLevel, domain),
            LootCategory.Shield => RollShieldDecisionForDomain(rarity, bagLevel, domain),
            LootCategory.Jewelry => RollJewelryDecisionForDomain(rarity, domain),
            _ => RollClothingDecisionForDomain(rarity, domain)
        };
    }

    // Domain-locked counterpart to Roll(bagLevel) — same construct/apply-rarity path, only the
    // decision differs.
    public static Item Roll(int bagLevel, PantheonDomain domain)
    {
        var decision = RollDecision(bagLevel, domain);
        var item = Construct(decision);

        ApplyRarity(item, decision);

        return item;
    }

    // Test accessor (pantheon-bags §8 guard): whether a domain has any candidate for a category,
    // used to assert coverage without duplicating the per-domain table shapes in test code.
    internal static bool DomainHasCandidates(PantheonDomain domain, LootCategory category)
    {
        var d = (int)domain;

        return category switch
        {
            LootCategory.Weapon => _domainWeaponFamilyIndices[d].Length > 0,
            LootCategory.Armor => _domainMetalMaterialIndices[d].Length > 0 || _domainLightMaterialIndices[d].Length > 0,
            LootCategory.Shield => _domainShieldThemes[d].Length > 0,
            LootCategory.Jewelry => _domainJewelryThemes[d].Length > 0,
            _ => _domainClothingThemes[d].Length > 0
        };
    }

    private static ItemRarity RollRarity(int bagLevel)
    {
        var weights = _rarityWeights[Math.Clamp(bagLevel, 0, RarityConfig.MaxBagLevel)];

        var total = 0;

        for (var i = 0; i < weights.Length; i++)
        {
            total += weights[i];
        }

        var roll = Utility.Random(total);
        var cumulative = 0;

        for (var i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];

            if (roll < cumulative)
            {
                return RaritySystem.Clamp((ItemRarity)i, RarityConfig.MaxRarityForBagLevel(bagLevel));
            }
        }

        return ItemRarity.Uncommon; // unreachable (weights always sum > 0); Uncommon is the absolute floor.
    }

    private static LootCategory RollCategory()
    {
        var roll = Utility.Random(CategoryWeightTotal);

        if (roll < CategoryWeightWeapon)
        {
            return LootCategory.Weapon;
        }

        roll -= CategoryWeightWeapon;

        if (roll < CategoryWeightArmor)
        {
            return LootCategory.Armor;
        }

        roll -= CategoryWeightArmor;

        if (roll < CategoryWeightShield)
        {
            return LootCategory.Shield;
        }

        roll -= CategoryWeightShield;

        return roll < CategoryWeightJewelry ? LootCategory.Jewelry : LootCategory.Clothing;
    }

    // Same weighted pick as RollCategory, renormalized over only the categories this domain has
    // candidates for (pantheon-bags §5.2). Returns false if the domain has none anywhere — the
    // caller falls back to the generic roll.
    private static bool TryRollCategoryForDomain(PantheonDomain domain, out LootCategory category)
    {
        var weaponWeight = DomainHasCandidates(domain, LootCategory.Weapon) ? CategoryWeightWeapon : 0;
        var armorWeight = DomainHasCandidates(domain, LootCategory.Armor) ? CategoryWeightArmor : 0;
        var shieldWeight = DomainHasCandidates(domain, LootCategory.Shield) ? CategoryWeightShield : 0;
        var jewelryWeight = DomainHasCandidates(domain, LootCategory.Jewelry) ? CategoryWeightJewelry : 0;
        var clothingWeight = DomainHasCandidates(domain, LootCategory.Clothing) ? CategoryWeightClothing : 0;

        var total = weaponWeight + armorWeight + shieldWeight + jewelryWeight + clothingWeight;

        if (total == 0)
        {
            category = default;
            return false;
        }

        var roll = Utility.Random(total);

        if (roll < weaponWeight)
        {
            category = LootCategory.Weapon;
            return true;
        }

        roll -= weaponWeight;

        if (roll < armorWeight)
        {
            category = LootCategory.Armor;
            return true;
        }

        roll -= armorWeight;

        if (roll < shieldWeight)
        {
            category = LootCategory.Shield;
            return true;
        }

        roll -= shieldWeight;
        category = roll < jewelryWeight ? LootCategory.Jewelry : LootCategory.Clothing;
        return true;
    }

    // Closeness-weighted pick restricted to a subset of material indices (a domain armor table
    // only stocks materials that have a candidate root). Same weight curve as RollWeightedBaseIndex
    // evaluated against the full material range, so a subset still favors the index nearest the
    // bagLevel target; returns the POSITION within availableIndices, not the raw material value.
    private static int RollWeightedSubsetIndex(int bagLevel, int[] availableIndices)
    {
        if (availableIndices.Length <= 1)
        {
            return 0;
        }

        var t = (int)Math.Round(bagLevel / 10.0 * (MaterialsPerArmorFamily - 1));

        var total = 0;

        for (var i = 0; i < availableIndices.Length; i++)
        {
            total += Math.Max(1, 4 - Math.Abs(availableIndices[i] - t));
        }

        var roll = Utility.Random(total);
        var cumulative = 0;

        for (var i = 0; i < availableIndices.Length; i++)
        {
            cumulative += Math.Max(1, 4 - Math.Abs(availableIndices[i] - t));

            if (roll < cumulative)
            {
                return i;
            }
        }

        return availableIndices.Length - 1;
    }

    // Base pick within a ladder of n items: the index closest to a bagLevel-scaled target is
    // favored (weak bases common in low bags, top bases dominate high bags; all reachable at
    // every level). t = round(bagLevel/10 * (n-1)); weight(i) = max(1, 4 - |i - t|). Tunable.
    private static int RollWeightedBaseIndex(int bagLevel, int n)
    {
        if (n <= 1)
        {
            return 0;
        }

        var t = (int)Math.Round(bagLevel / 10.0 * (n - 1));

        var total = 0;

        for (var i = 0; i < n; i++)
        {
            total += Math.Max(1, 4 - Math.Abs(i - t));
        }

        var roll = Utility.Random(total);
        var cumulative = 0;

        for (var i = 0; i < n; i++)
        {
            cumulative += Math.Max(1, 4 - Math.Abs(i - t));

            if (roll < cumulative)
            {
                return i;
            }
        }

        return n - 1;
    }

    private static LootRollDecision RollWeaponDecision(ItemRarity rarity, int bagLevel)
    {
        var familyIndex = Utility.Random(_weaponFamilies.Length);
        var baseIndex = RollWeightedBaseIndex(bagLevel, _weaponFamilyBaseCount[familyIndex]);
        var themes = _weaponThemesByFamily[familyIndex];
        var theme = themes[Utility.Random(themes.Length)];

        return new LootRollDecision(rarity, LootCategory.Weapon, _weaponFamilies[familyIndex], theme, baseIndex);
    }

    private static LootRollDecision RollWeaponDecisionForDomain(ItemRarity rarity, int bagLevel, PantheonDomain domain)
    {
        var d = (int)domain;
        var availableFamilies = _domainWeaponFamilyIndices[d];
        var pick = Utility.Random(availableFamilies.Length);
        var familyIndex = availableFamilies[pick];

        var baseIndex = RollWeightedBaseIndex(bagLevel, _weaponFamilyBaseCount[familyIndex]);
        var themes = _domainWeaponThemesByFamily[d][pick];
        var theme = themes[Utility.Random(themes.Length)];

        return new LootRollDecision(rarity, LootCategory.Weapon, _weaponFamilies[familyIndex], theme, baseIndex);
    }

    private static LootRollDecision RollArmorDecision(ItemRarity rarity, int bagLevel)
    {
        var family = Utility.RandomBool() ? LegendaryRegistry.FamilyMetalArmor : LegendaryRegistry.FamilyLightArmor;
        var materialIndex = RollWeightedBaseIndex(bagLevel, MaterialsPerArmorFamily);

        // Theme rolls AFTER the material: each material owns its five roots (framework §3).
        var themes = (family == LegendaryRegistry.FamilyMetalArmor
            ? _metalArmorThemesByMaterial
            : _lightArmorThemesByMaterial)[materialIndex];
        var theme = themes[Utility.Random(themes.Length)];

        return new LootRollDecision(rarity, LootCategory.Armor, family, theme, materialIndex);
    }

    private static LootRollDecision RollArmorDecisionForDomain(ItemRarity rarity, int bagLevel, PantheonDomain domain)
    {
        var d = (int)domain;
        var metalMaterials = _domainMetalMaterialIndices[d];
        var lightMaterials = _domainLightMaterialIndices[d];

        bool useMetal;

        if (metalMaterials.Length == 0)
        {
            useMetal = false;
        }
        else if (lightMaterials.Length == 0)
        {
            useMetal = true;
        }
        else
        {
            useMetal = Utility.RandomBool();
        }

        var family = useMetal ? LegendaryRegistry.FamilyMetalArmor : LegendaryRegistry.FamilyLightArmor;
        var availableMaterials = useMetal ? metalMaterials : lightMaterials;
        var themesByMaterial = useMetal ? _domainMetalMaterialThemes[d] : _domainLightMaterialThemes[d];

        var pick = RollWeightedSubsetIndex(bagLevel, availableMaterials);
        var materialIndex = availableMaterials[pick];
        var themes = themesByMaterial[pick];
        var theme = themes[Utility.Random(themes.Length)];

        return new LootRollDecision(rarity, LootCategory.Armor, family, theme, materialIndex);
    }

    private static LootRollDecision RollShieldDecision(ItemRarity rarity, int bagLevel)
    {
        var baseIndex = RollWeightedBaseIndex(bagLevel, _shieldFactories.Length);
        var theme = _shieldThemes[Utility.Random(_shieldThemes.Length)];

        return new LootRollDecision(rarity, LootCategory.Shield, LegendaryRegistry.FamilyShields, theme, baseIndex);
    }

    private static LootRollDecision RollShieldDecisionForDomain(ItemRarity rarity, int bagLevel, PantheonDomain domain)
    {
        var baseIndex = RollWeightedBaseIndex(bagLevel, _shieldFactories.Length);
        var themes = _domainShieldThemes[(int)domain];
        var theme = themes[Utility.Random(themes.Length)];

        return new LootRollDecision(rarity, LootCategory.Shield, LegendaryRegistry.FamilyShields, theme, baseIndex);
    }

    // Jewelry has no ladder (framework §7) — slot is uniform, not bag-level weighted.
    private static LootRollDecision RollJewelryDecision(ItemRarity rarity)
    {
        var slot = Utility.Random(_jewelryFactories.Length);
        var theme = _jewelryThemes[Utility.Random(_jewelryThemes.Length)];

        return new LootRollDecision(rarity, LootCategory.Jewelry, LegendaryRegistry.FamilyJewelry, theme, slot);
    }

    private static LootRollDecision RollJewelryDecisionForDomain(ItemRarity rarity, PantheonDomain domain)
    {
        var slot = Utility.Random(_jewelryFactories.Length);
        var themes = _domainJewelryThemes[(int)domain];
        var theme = themes[Utility.Random(themes.Length)];

        return new LootRollDecision(rarity, LootCategory.Jewelry, LegendaryRegistry.FamilyJewelry, theme, slot);
    }

    // Clothing has no ladder either (21-clothing.md §1) — piece is uniform. Drop-variants cap at
    // Epic; Legendary clothing exists only as the bound relics, and each theme now has TWO (a body
    // piece + a hat, §3), so at Legendary the "piece" is a uniform pick between the theme's two
    // bound shapes instead of a free uniform pick over every piece.
    private static LootRollDecision RollClothingDecision(ItemRarity rarity)
    {
        var theme = _clothingThemes[Utility.Random(_clothingThemes.Length)];

        var piece = rarity == ItemRarity.Legendary
            ? ClothingRelicPieceIndex(theme)
            : Utility.Random(_clothingFactories.Length);

        return new LootRollDecision(rarity, LootCategory.Clothing, LegendaryRegistry.FamilyClothing, theme, piece);
    }

    private static LootRollDecision RollClothingDecisionForDomain(ItemRarity rarity, PantheonDomain domain)
    {
        var themes = _domainClothingThemes[(int)domain];
        var theme = themes[Utility.Random(themes.Length)];

        var piece = rarity == ItemRarity.Legendary
            ? ClothingRelicPieceIndex(theme)
            : Utility.Random(_clothingFactories.Length);

        return new LootRollDecision(rarity, LootCategory.Clothing, LegendaryRegistry.FamilyClothing, theme, piece);
    }

    // The theme's bound relic shapes, sourced from the registry so the mapping never drifts from the
    // legendary data. Each clothing theme has exactly two entries (a body piece + a hat); pick one
    // uniformly. Cold path (a Legendary roll on mob death), so the linear scan is fine.
    private static byte ClothingRelicPieceIndex(VariantRoot theme)
    {
        Span<byte> pieces = stackalloc byte[2];
        var count = 0;

        var entries = LegendaryRegistry.Entries;

        for (var i = 0; i < entries.Count && count < pieces.Length; i++)
        {
            var entry = entries[i];

            if (entry.Family == LegendaryRegistry.FamilyClothing && entry.Root == theme)
            {
                pieces[count++] = entry.BaseIndex;
            }
        }

        return count > 0 ? pieces[Utility.Random(count)] : LegendaryRegistry.ClothingPieceCloak;
    }

    private static Item Construct(in LootRollDecision d) => d.Category switch
    {
        LootCategory.Weapon => _weaponFactories[d.Family][d.BaseIndex](),
        LootCategory.Armor => ConstructArmor(d.Family, d.BaseIndex),
        LootCategory.Shield => _shieldFactories[d.BaseIndex](),
        LootCategory.Jewelry => _jewelryFactories[d.BaseIndex](),
        _ => _clothingFactories[d.BaseIndex]()
    };

    private static Item ConstructArmor(byte family, int materialIndex)
    {
        var slotFactories = (family == LegendaryRegistry.FamilyMetalArmor ? _metalArmorSlotFactories : _lightArmorSlotFactories)[materialIndex];
        return slotFactories[Utility.Random(slotFactories.Length)]();
    }

    // Constructs the concrete base item a legendary entry is bound to, from the same factories
    // the drop path uses: weapon/shield/jewelry/clothing entries name an exact shape (BaseIndex);
    // armor entries name a material (BaseIndex = material index) and pick a random piece of it.
    // Consumers: the Pantheon altar's domain reroll and the clause-reachability tests.
    public static Item ConstructForLegendary(in LegendaryEntry entry) => entry.Family switch
    {
        <= LegendaryRegistry.FamilyArchery => _weaponFactories[entry.Family][entry.BaseIndex](),
        LegendaryRegistry.FamilyMetalArmor or LegendaryRegistry.FamilyLightArmor =>
            ConstructArmor(entry.Family, entry.BaseIndex),
        LegendaryRegistry.FamilyShields => _shieldFactories[entry.BaseIndex](),
        LegendaryRegistry.FamilyJewelry => _jewelryFactories[entry.BaseIndex](),
        _ => _clothingFactories[entry.BaseIndex]()
    };

    private static void ApplyRarity(Item item, in LootRollDecision decision)
    {
        if (decision.Rarity == ItemRarity.Common)
        {
            return; // plain base item: stock name, no hue, no effects (framework §1/§6).
        }

        if (decision.Rarity == ItemRarity.Legendary)
        {
            // A registry miss here would mean a reachable (family, theme, baseIndex) combo has no
            // legendary entry — a data gap the reachable-combination test below guards against.
            // Fail safe rather than crash a live server: fall back to an Epic variant instead of
            // plain Common (user directive 2026-07-08: bags never drop Common). Belt-and-braces —
            // this path should be unreachable given the reachability tests.
            if (LegendaryRegistry.TryGetByRootAndBase(decision.Family, decision.Theme, (byte)decision.BaseIndex, out var entry))
            {
                RarityEffects.ApplyLegendary(item, entry.Id);
            }
            else
            {
                RarityEffects.ApplyVariant(item, decision.Theme, ItemRarity.Epic);
            }

            return;
        }

        RarityEffects.ApplyVariant(item, decision.Theme, decision.Rarity);
    }
}
