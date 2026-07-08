using System;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.LootBags;

// Fills a loot bag with exactly one item (framework §8 — "one item per bag" user directive).
// Roll order: rarity -> category -> family -> theme -> base -> [construction-only] slot.
// The pure decision (RollDecision) is kept separate from item construction (Roll) so the
// table/weight/ceiling logic is unit-testable without the world fixture.
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
    // User directive 2026-07-08: no Commons from bags; floors — L0-4 Uncommon, L5-9 Rare, L10
    // Epic. Ceilings (RarityConfig.MaxRarityForBagLevel) are ≥ the floor at every level, so the
    // defensive ceiling clamp can never push a roll below its floor.
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
        new[] { 0, 0, 40, 52, 8 },
        new[] { 0, 0, 0, 90, 10 }
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
    private const int ShieldShapeCount = 6;
    private const int JewelrySlotCount = 4;
    private const int ClothingPieceCount = 12;

    // Per-family weapon roots (framework §3, 2026-07-07 re-theme). Aligned index-for-index with
    // _weaponFamilies below: each family drops only its own five bespoke roots. Axes keep the
    // original five; every other family carries its lane-unique set.
    private static readonly VariantRoot[][] _weaponThemesByFamily =
    {
        new[] { VariantRoot.Zephyr, VariantRoot.Phobos, VariantRoot.Agrotera, VariantRoot.Pallas, VariantRoot.Stygian },
        new[] { VariantRoot.Phoibos, VariantRoot.Areia, VariantRoot.Menis, VariantRoot.Aristeia, VariantRoot.Haima },
        new[] { VariantRoot.Theristes, VariantRoot.Sarisa, VariantRoot.Phalanx, VariantRoot.Horme, VariantRoot.Zophos },
        new[] { VariantRoot.Ennosigaios, VariantRoot.Kataigis, VariantRoot.Rhaistes, VariantRoot.Eryma, VariantRoot.Kamatos },
        new[] { VariantRoot.Empousa, VariantRoot.Prester, VariantRoot.Alexikakos, VariantRoot.Manteia, VariantRoot.Baskania },
        new[] { VariantRoot.Ios, VariantRoot.Ephodos, VariantRoot.Aiolos, VariantRoot.Kentron, VariantRoot.Ophis },
        new[] { VariantRoot.Hekatos, VariantRoot.Belos, VariantRoot.Pede, VariantRoot.Toxikon, VariantRoot.Skopos }
    };

    // Per-material armor roots (framework §3, 2026-07-07 re-theme). Outer index = material
    // ladder position (0 ring/leather, 1 chain/studded, 2 plate/bone) — the theme is rolled
    // AFTER the material so each material drops only its own five roots.
    private static readonly VariantRoot[][] _metalArmorThemesByMaterial =
    {
        new[] { VariantRoot.Hoplites, VariantRoot.Taxis, VariantRoot.Dromos, VariantRoot.Zoster, VariantRoot.Alkimos },
        new[] { VariantRoot.Phylax, VariantRoot.Egregoros, VariantRoot.Teichos, VariantRoot.Halysis, VariantRoot.Phrourion },
        new[] { VariantRoot.Adamas, VariantRoot.Kaminos, VariantRoot.Kolossos, VariantRoot.Panoplia, VariantRoot.Akamatos }
    };

    private static readonly VariantRoot[][] _lightArmorThemesByMaterial =
    {
        new[] { VariantRoot.Naias, VariantRoot.Dryas, VariantRoot.Oreias, VariantRoot.Melissa, VariantRoot.Panika },
        new[] { VariantRoot.Kynegis, VariantRoot.Batos, VariantRoot.Arkas, VariantRoot.Elaphis, VariantRoot.Skia },
        new[] { VariantRoot.Melinoe, VariantRoot.Makaria, VariantRoot.Tymbos, VariantRoot.Nekyia, VariantRoot.Katachthon }
    };

    // Shields keep Aegis; the other four are shield-only (framework §3).
    private static readonly VariantRoot[] _shieldThemes =
    {
        VariantRoot.Aegis, VariantRoot.Amyntor, VariantRoot.Probolos, VariantRoot.Herkos, VariantRoot.Pnoe
    };

    private static readonly VariantRoot[] _jewelryThemes =
    {
        VariantRoot.Olympian, VariantRoot.Hecatean, VariantRoot.Tychean, VariantRoot.Nyxian, VariantRoot.Demetrian
    };

    private static readonly VariantRoot[] _clothingThemes =
    {
        VariantRoot.Laurel, VariantRoot.Charis, VariantRoot.Maenad, VariantRoot.Hestian, VariantRoot.Arachne
    };

    private static readonly byte[] _weaponFamilies =
    {
        LegendaryRegistry.FamilyAxes, LegendaryRegistry.FamilySwords, LegendaryRegistry.FamilyPolearms,
        LegendaryRegistry.FamilyMaces, LegendaryRegistry.FamilyStaves, LegendaryRegistry.FamilyFencing,
        LegendaryRegistry.FamilyArchery
    };

    // Ladder length per weapon family (framework §7), indexed the same as _weaponFamilies.
    // Axes 8, swords 8, polearms 2, maces 7, staves 3, fencing 6, archery 3.
    private static readonly int[] _weaponFamilyBaseCount = { 8, 8, 2, 7, 3, 6, 3 };

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
    // killer is the finder for the Announce hook — pass null (or a non-player) to skip it.
    public static Item Roll(int bagLevel, Mobile killer = null)
    {
        var decision = RollDecision(bagLevel);
        var item = Construct(decision);

        ApplyRarity(item, decision);

        if (killer is PlayerMobile)
        {
            RaritySystem.Announce(killer, item); // Announce itself gates on the Epic+ min tier.
        }

        return item;
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

    private static LootRollDecision RollShieldDecision(ItemRarity rarity, int bagLevel)
    {
        var baseIndex = RollWeightedBaseIndex(bagLevel, ShieldShapeCount);
        var theme = _shieldThemes[Utility.Random(_shieldThemes.Length)];

        return new LootRollDecision(rarity, LootCategory.Shield, LegendaryRegistry.FamilyShields, theme, baseIndex);
    }

    // Jewelry has no ladder (framework §7) — slot is uniform, not bag-level weighted.
    private static LootRollDecision RollJewelryDecision(ItemRarity rarity)
    {
        var slot = Utility.Random(JewelrySlotCount);
        var theme = _jewelryThemes[Utility.Random(_jewelryThemes.Length)];

        return new LootRollDecision(rarity, LootCategory.Jewelry, LegendaryRegistry.FamilyJewelry, theme, slot);
    }

    // Clothing has no ladder either (21-clothing.md §1) — piece is uniform. Drop-variants cap at
    // Epic; Legendary clothing exists only as the 5 bound relics, one per theme, so at Legendary
    // the "piece" is forced to that relic's bound shape instead of a free uniform pick.
    private static LootRollDecision RollClothingDecision(ItemRarity rarity)
    {
        var theme = _clothingThemes[Utility.Random(_clothingThemes.Length)];

        var piece = rarity == ItemRarity.Legendary
            ? ClothingRelicPieceIndex(theme)
            : Utility.Random(ClothingPieceCount);

        return new LootRollDecision(rarity, LootCategory.Clothing, LegendaryRegistry.FamilyClothing, theme, piece);
    }

    private static byte ClothingRelicPieceIndex(VariantRoot theme) => theme switch
    {
        VariantRoot.Laurel => LegendaryRegistry.ClothingPieceBodySash,
        VariantRoot.Charis => LegendaryRegistry.ClothingPieceFancyShirt,
        VariantRoot.Maenad => LegendaryRegistry.ClothingPieceKilt,
        VariantRoot.Hestian => LegendaryRegistry.ClothingPieceRobe,
        _ => LegendaryRegistry.ClothingPieceCloak // Arachne
    };

    // ---- Item construction (type maps) ----------------------------------------------------

    private static readonly Func<Item>[][] _weaponFactories =
    {
        // Axes — hatchet, axe, battle axe, double axe, executioner's axe, two-handed axe,
        // large battle axe, ornate axe (framework §7 ladder order).
        new Func<Item>[]
        {
            () => new Hatchet(), () => new Axe(), () => new BattleAxe(), () => new DoubleAxe(),
            () => new ExecutionersAxe(), () => new TwoHandedAxe(), () => new LargeBattleAxe(), () => new OrnateAxe()
        },
        // Swords — butcher knife, cleaver, cutlass, scimitar, katana, broadsword, longsword, viking sword.
        new Func<Item>[]
        {
            () => new ButcherKnife(), () => new Cleaver(), () => new Cutlass(), () => new Scimitar(),
            () => new Katana(), () => new Broadsword(), () => new Longsword(), () => new VikingSword()
        },
        // Polearms — bardiche, halberd.
        new Func<Item>[] { () => new Bardiche(), () => new Halberd() },
        // Maces — club, mace, maul, war axe, hammer pick, war mace, war hammer. "War axe" is
        // mechanically a mace (DefSkill = Macing) despite the axe-family class name/model.
        new Func<Item>[]
        {
            () => new Club(), () => new Mace(), () => new Maul(), () => new WarAxe(),
            () => new HammerPick(), () => new WarMace(), () => new WarHammer()
        },
        // Staves — quarter staff, gnarled staff, black staff.
        new Func<Item>[] { () => new QuarterStaff(), () => new GnarledStaff(), () => new BlackStaff() },
        // Fencing — dagger, kryss, war fork, pitchfork, short spear, spear.
        new Func<Item>[]
        {
            () => new Dagger(), () => new Kryss(), () => new WarFork(), () => new Pitchfork(),
            () => new ShortSpear(), () => new Spear()
        },
        // Archery — bow, crossbow, heavy crossbow.
        new Func<Item>[] { () => new Bow(), () => new Crossbow(), () => new HeavyCrossbow() }
    };

    // Metal materials, ladder-ordered to match LegendaryRegistry's BaseIndex (ring 0, chain 1,
    // plate 2). Each material's factory then picks uniformly among its real T2A slot pieces —
    // the slot itself carries no rarity/legendary meaning, only the material does (framework §7).
    private static readonly Func<Item>[][] _metalArmorSlotFactories =
    {
        // Ring — chest, legs, arms, gloves. No helm/gorget piece exists in T2A.
        new Func<Item>[]
        {
            () => new RingmailChest(), () => new RingmailLegs(), () => new RingmailArms(), () => new RingmailGloves()
        },
        // Chain — chest, legs, coif (helm slot). No arms/gloves/gorget piece exists in T2A.
        new Func<Item>[] { () => new ChainChest(), () => new ChainLegs(), () => new ChainCoif() },
        // Plate — chest, legs, arms, gorget, gloves, helm (5 interchangeable shape variants).
        new Func<Item>[]
        {
            () => new PlateChest(), () => new PlateLegs(), () => new PlateArms(),
            () => new PlateGorget(), () => new PlateGloves(), RollPlateHelm
        }
    };

    // Light materials, ladder-ordered (leather 0, studded 1, bone 2).
    private static readonly Func<Item>[][] _lightArmorSlotFactories =
    {
        // Leather — chest, legs, cap (helm slot), arms, gorget, gloves.
        new Func<Item>[]
        {
            () => new LeatherChest(), () => new LeatherLegs(), RollLeatherHelm,
            () => new LeatherArms(), () => new LeatherGorget(), () => new LeatherGloves()
        },
        // Studded — chest, legs, arms, gorget, gloves. No studded helm exists in T2A.
        new Func<Item>[]
        {
            () => new StuddedChest(), () => new StuddedLegs(), () => new StuddedArms(),
            () => new StuddedGorget(), () => new StuddedGloves()
        },
        // Bone — chest, legs, helmet, arms, gloves. No bone gorget exists in T2A.
        new Func<Item>[]
        {
            () => new BoneChest(), () => new BoneLegs(), () => new BoneHelm(), () => new BoneArms(), () => new BoneGloves()
        }
    };

    // Plate helm slot: uniform among the five interchangeable metal-helm shapes (same AR row,
    // model only — 10-armor-metal.md §1).
    private static Item RollPlateHelm() => Utility.Random(5) switch
    {
        0 => new Helmet(),
        1 => new Bascinet(),
        2 => new NorseHelm(),
        3 => new CloseHelm(),
        _ => new PlateHelm()
    };

    // Substitution flag: 11-armor-light.md §1 claims "orc helm is a bone-helm shape variant,"
    // but in this codebase OrcHelm.MaterialType is Leather, not Bone (Bone has no true orc-helm
    // reskin here). Treated as a Leather-material helm alternate instead — code truth wins over
    // the doc's claim, and it stays a real Leather-material item either way.
    private static Item RollLeatherHelm() => Utility.RandomBool() ? new LeatherCap() : new OrcHelm();

    private static readonly Func<Item>[] _shieldFactories =
    {
        () => new Buckler(), () => new WoodenShield(), () => new WoodenKiteShield(),
        () => new MetalShield(), () => new MetalKiteShield(), () => new HeaterShield()
    };

    // Gold variants only (cosmetic silver alternates skipped — ponytail: not requested).
    private static readonly Func<Item>[] _jewelryFactories =
    {
        () => new GoldRing(), () => new GoldBracelet(), () => new GoldNecklace(), () => new GoldEarrings()
    };

    // Order matches LegendaryRegistry.ClothingPieceXxx for indices 0-4 (BodySash, FancyShirt,
    // Kilt, Robe, Cloak) so the same array serves both the Legendary relic path and the general
    // Uncommon-Epic uniform pick (21-clothing.md §1/§3).
    private static readonly Func<Item>[] _clothingFactories =
    {
        () => new BodySash(), () => new FancyShirt(), () => new Kilt(), () => new Robe(), () => new Cloak(),
        () => new Doublet(), () => new Tunic(), () => new Skirt(),
        () => new StrawHat(), () => new WideBrimHat(), () => new FeatheredHat(), () => new Cap()
    };

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
