namespace Server.Engines.Rarity;

// One row of jewelry/clothing theme magnitudes for a given [root, rarity, isClothing]. Percent
// fields are whole percents (8 = +8%). Zero-valued = effect absent. Jewelry's Legendary column is
// a step ABOVE Epic (framework §4, same rule as armor/shields) — clothing has no drop Legendary
// row at all (21-clothing.md §1 caps drops at Epic); the 5 relics instead duplicate their Epic
// row at Legendary (mirrors how weapons duplicate Epic into Legendary, framework §4).
public readonly struct AccessoryEffectRow
{
    // Olympian (might) — StatBonus is the magnitude of the stat rolled at item creation
    // (BaseJewel.OlympianStat); LightningProcPct procs on the wearer's own melee hits.
    public int StatBonus { get; init; }
    public int LightningProcPct { get; init; }

    // Hecatean (sorcery)
    public int ManaRegenPct { get; init; }
    public int SpellDamagePct { get; init; }
    public int ManaLeechPct { get; init; }

    // Tychean (fortune) — HitHalvedPct feeds the SAME ShrugPct pool/cap as armor's Polias/Aegis
    // shrug (identical mechanic: a % chance an incoming hit is halved). MissRerollPct is its own
    // attacker-side field (BaseWeapon.CheckHit).
    public int HitHalvedPct { get; init; }
    public int MissRerollPct { get; init; }

    // Nyxian (night)
    public int HidingBonus { get; init; }
    public int StealthBonus { get; init; }
    public bool NightSight { get; init; }
    public int PoisonResistPct { get; init; }

    // Demetrian (harvest) — AllRegenPct feeds HpRegenPct/StamRegenPct/ManaRegenPct alike.
    public int AllRegenPct { get; init; }
    public int PotionEffectPct { get; init; }

    // Laurel (victory) — clothing, flat on-kill restores.
    public int OnKillStamina { get; init; }
    public int OnKillHp { get; init; }

    // Charis (charm) — clothing.
    public int KarmaGainPct { get; init; }
    public int VendorPricePct { get; init; }

    // Maenad (frenzy) — clothing. Chance rolled on being struck; magnitudes ride a timed
    // CombatFxState buff (FrenzySwingPct is an Epic-only rider on top of FrenzyDamagePct).
    public int FrenzyChancePct { get; init; }
    public int FrenzyDamagePct { get; init; }
    public int FrenzySwingPct { get; init; }

    // Hestian (hearth) — clothing. Conditional on the wearer being stationary >=10s.
    public int StationaryRegenPct { get; init; }
    public bool StationaryAppliesMana { get; init; }

    // Arachne (web) — clothing. DodgePct feeds the SAME shared dodge pool/cap as armor's Talarian
    // dodge; PoisonResistPct feeds the same pooled field as Nyxian above.
    public int DodgePct { get; init; }

    public bool IsEmpty => this is
    {
        StatBonus: 0, LightningProcPct: 0,
        ManaRegenPct: 0, SpellDamagePct: 0, ManaLeechPct: 0,
        HitHalvedPct: 0, MissRerollPct: 0,
        HidingBonus: 0, StealthBonus: 0, NightSight: false, PoisonResistPct: 0,
        AllRegenPct: 0, PotionEffectPct: 0,
        OnKillStamina: 0, OnKillHp: 0,
        KarmaGainPct: 0, VendorPricePct: 0,
        FrenzyChancePct: 0, FrenzyDamagePct: 0, FrenzySwingPct: 0,
        StationaryRegenPct: 0, StationaryAppliesMana: false,
        DodgePct: 0
    };
}

// Exact magnitudes from 20-jewelry.md §2 (jewelry, 4 rarities) and 21-clothing.md §2 (clothing,
// 3 rarities — Epic duplicated into the Legendary slot for the 5 bound relics, §3).
public static class AccessoryEffectTable
{
    private const int VariantRootCount = VariantRootInfo.RootCount; // VariantRoot.None..Pnoe
    private const int RarityCount = 5;        // ItemRarity.Common..Legendary

    private static readonly AccessoryEffectRow[,] _jewelryRows = new AccessoryEffectRow[VariantRootCount, RarityCount];
    private static readonly AccessoryEffectRow[,] _clothingRows = new AccessoryEffectRow[VariantRootCount, RarityCount];

    static AccessoryEffectTable()
    {
        // ---- Jewelry (20-jewelry.md §2 — own Legendary column, a step above Epic) ----

        // Olympian — Zeus (might)
        SetJewelry(VariantRoot.Olympian, ItemRarity.Uncommon, new AccessoryEffectRow { StatBonus = 2 });
        SetJewelry(VariantRoot.Olympian, ItemRarity.Rare, new AccessoryEffectRow { StatBonus = 4 });
        SetJewelry(VariantRoot.Olympian, ItemRarity.Epic, new AccessoryEffectRow { StatBonus = 6, LightningProcPct = 3 });
        SetJewelry(VariantRoot.Olympian, ItemRarity.Legendary, new AccessoryEffectRow { StatBonus = 9, LightningProcPct = 5 });

        // Hecatean — Hecate (sorcery)
        SetJewelry(VariantRoot.Hecatean, ItemRarity.Uncommon, new AccessoryEffectRow { ManaRegenPct = 8 });
        SetJewelry(VariantRoot.Hecatean, ItemRarity.Rare, new AccessoryEffectRow { ManaRegenPct = 12, SpellDamagePct = 4 });
        SetJewelry(
            VariantRoot.Hecatean, ItemRarity.Epic,
            new AccessoryEffectRow { ManaRegenPct = 18, SpellDamagePct = 7, ManaLeechPct = 3 }
        );
        SetJewelry(
            VariantRoot.Hecatean, ItemRarity.Legendary,
            new AccessoryEffectRow { ManaRegenPct = 25, SpellDamagePct = 10, ManaLeechPct = 5 }
        );

        // Tychean — Tyche (fortune)
        SetJewelry(VariantRoot.Tychean, ItemRarity.Uncommon, new AccessoryEffectRow { HitHalvedPct = 2 });
        SetJewelry(VariantRoot.Tychean, ItemRarity.Rare, new AccessoryEffectRow { HitHalvedPct = 3 });
        SetJewelry(VariantRoot.Tychean, ItemRarity.Epic, new AccessoryEffectRow { HitHalvedPct = 4, MissRerollPct = 4 });
        SetJewelry(VariantRoot.Tychean, ItemRarity.Legendary, new AccessoryEffectRow { HitHalvedPct = 5, MissRerollPct = 6 });

        // Nyxian — Nyx (night)
        SetJewelry(VariantRoot.Nyxian, ItemRarity.Uncommon, new AccessoryEffectRow { HidingBonus = 5 });
        SetJewelry(VariantRoot.Nyxian, ItemRarity.Rare, new AccessoryEffectRow { HidingBonus = 5, StealthBonus = 5 });
        SetJewelry(
            VariantRoot.Nyxian, ItemRarity.Epic,
            new AccessoryEffectRow { HidingBonus = 10, StealthBonus = 10, NightSight = true }
        );
        SetJewelry(
            VariantRoot.Nyxian, ItemRarity.Legendary,
            new AccessoryEffectRow { HidingBonus = 15, StealthBonus = 15, NightSight = true, PoisonResistPct = 10 }
        );

        // Demetrian — Demeter (harvest)
        SetJewelry(VariantRoot.Demetrian, ItemRarity.Uncommon, new AccessoryEffectRow { AllRegenPct = 4 });
        SetJewelry(VariantRoot.Demetrian, ItemRarity.Rare, new AccessoryEffectRow { AllRegenPct = 6 });
        SetJewelry(VariantRoot.Demetrian, ItemRarity.Epic, new AccessoryEffectRow { AllRegenPct = 8, PotionEffectPct = 10 });
        SetJewelry(VariantRoot.Demetrian, ItemRarity.Legendary, new AccessoryEffectRow { AllRegenPct = 12, PotionEffectPct = 15 });

        // ---- Clothing (21-clothing.md §2 — Uncommon..Epic; Epic duplicates into Legendary for
        // the 5 bound relics, since drops themselves never reach Legendary, §1/§3) ----

        // Laurel — Nike (victory)
        SetClothing(VariantRoot.Laurel, ItemRarity.Uncommon, new AccessoryEffectRow { OnKillStamina = 5 });
        SetClothing(VariantRoot.Laurel, ItemRarity.Rare, new AccessoryEffectRow { OnKillStamina = 10 });
        var laurelEpic = new AccessoryEffectRow { OnKillStamina = 10, OnKillHp = 5 };
        SetClothing(VariantRoot.Laurel, ItemRarity.Epic, laurelEpic);
        SetClothing(VariantRoot.Laurel, ItemRarity.Legendary, laurelEpic);

        // Charis — Aphrodite (charm)
        SetClothing(VariantRoot.Charis, ItemRarity.Uncommon, new AccessoryEffectRow { KarmaGainPct = 5 });
        SetClothing(VariantRoot.Charis, ItemRarity.Rare, new AccessoryEffectRow { KarmaGainPct = 10, VendorPricePct = 3 });
        var charisEpic = new AccessoryEffectRow { KarmaGainPct = 15, VendorPricePct = 5 };
        SetClothing(VariantRoot.Charis, ItemRarity.Epic, charisEpic);
        SetClothing(VariantRoot.Charis, ItemRarity.Legendary, charisEpic);

        // Maenad — Dionysos (frenzy)
        SetClothing(VariantRoot.Maenad, ItemRarity.Uncommon, new AccessoryEffectRow { FrenzyChancePct = 2, FrenzyDamagePct = 10 });
        SetClothing(VariantRoot.Maenad, ItemRarity.Rare, new AccessoryEffectRow { FrenzyChancePct = 3, FrenzyDamagePct = 10 });
        var maenadEpic = new AccessoryEffectRow { FrenzyChancePct = 4, FrenzyDamagePct = 10, FrenzySwingPct = 10 };
        SetClothing(VariantRoot.Maenad, ItemRarity.Epic, maenadEpic);
        SetClothing(VariantRoot.Maenad, ItemRarity.Legendary, maenadEpic);

        // Hestian — Hestia (hearth)
        SetClothing(VariantRoot.Hestian, ItemRarity.Uncommon, new AccessoryEffectRow { StationaryRegenPct = 10 });
        SetClothing(VariantRoot.Hestian, ItemRarity.Rare, new AccessoryEffectRow { StationaryRegenPct = 15 });
        var hestianEpic = new AccessoryEffectRow { StationaryRegenPct = 20, StationaryAppliesMana = true };
        SetClothing(VariantRoot.Hestian, ItemRarity.Epic, hestianEpic);
        SetClothing(VariantRoot.Hestian, ItemRarity.Legendary, hestianEpic);

        // Arachne — Arachne (web)
        SetClothing(VariantRoot.Arachne, ItemRarity.Uncommon, new AccessoryEffectRow { DodgePct = 2 });
        SetClothing(VariantRoot.Arachne, ItemRarity.Rare, new AccessoryEffectRow { DodgePct = 3, PoisonResistPct = 5 });
        var arachneEpic = new AccessoryEffectRow { DodgePct = 4, PoisonResistPct = 10 };
        SetClothing(VariantRoot.Arachne, ItemRarity.Epic, arachneEpic);
        SetClothing(VariantRoot.Arachne, ItemRarity.Legendary, arachneEpic);
    }

    private static void SetJewelry(VariantRoot root, ItemRarity rarity, AccessoryEffectRow row) =>
        _jewelryRows[(int)root, (int)rarity] = row;

    private static void SetClothing(VariantRoot root, ItemRarity rarity, AccessoryEffectRow row) =>
        _clothingRows[(int)root, (int)rarity] = row;

    // Zero-allocation lookup. Returns an all-zero row for roots/rarities with no package.
    public static AccessoryEffectRow Get(VariantRoot root, ItemRarity rarity, bool isClothing)
    {
        if (root == VariantRoot.None)
        {
            return default;
        }

        return (isClothing ? _clothingRows : _jewelryRows)[(int)root, (int)rarity];
    }
}
