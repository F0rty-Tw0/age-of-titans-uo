using Server.Items;

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

// Array-backed facade over the family registry (the single source of truth — jewelry magnitudes
// live in JewelryFamily.cs, clothing in ClothingFamily.cs). Populated once at type init; zero
// allocation per lookup.
public static class AccessoryEffectTable
{
    private static readonly AccessoryEffectRow[,] _jewelryRows = FamilyRegistry.JewelryRows;
    private static readonly AccessoryEffectRow[,] _clothingRows = FamilyRegistry.ClothingRows;

    // Armor-displacing cloth (21-clothing.md §1) — the two piece shapes that REPLACE an armor slot
    // (hats replace helms, cloth pants replace leg armor) run at 2x the regular clothing magnitudes,
    // sacrificing an armor slot for jewelry-level bonuses. Same [root, rarity] shape, doubled values.
    private static readonly AccessoryEffectRow[,] _clothingDisplacingRows = FamilyRegistry.ClothingDisplacingRows;

    // A worn clothing piece is "displacing" iff it occupies an armor layer (Helm or Pants) — the
    // one classification helper both WornEffectState.Rebuild and RarityEffects.Tooltips key off.
    public static bool IsDisplacingClothing(BaseClothing clothing) =>
        clothing.Layer is Layer.Helm or Layer.Pants;

    // Zero-allocation lookup. Returns an all-zero row for roots/rarities with no package. `displacing`
    // only applies to clothing (jewelry ignores it), selecting the 2x armor-displacing row set.
    public static AccessoryEffectRow Get(VariantRoot root, ItemRarity rarity, bool isClothing, bool displacing = false)
    {
        if (root == VariantRoot.None)
        {
            return default;
        }

        if (isClothing)
        {
            return (displacing ? _clothingDisplacingRows : _clothingRows)[(int)root, (int)rarity];
        }

        return _jewelryRows[(int)root, (int)rarity];
    }
}
