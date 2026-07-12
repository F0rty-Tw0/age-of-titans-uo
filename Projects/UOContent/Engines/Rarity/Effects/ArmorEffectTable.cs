namespace Server.Engines.Rarity;

// One row of armor/shield theme magnitudes for a given [root, rarity, isShield]. Percent
// fields are whole percents (8 = +8%). Zero-valued = effect absent. Unlike weapons, armor's
// Legendary column is a step ABOVE Epic by design (framework §4) — not a duplicate of Epic.
public readonly struct ArmorEffectRow
{
    // Polias / Aegis (bulwark)
    public int BonusAr { get; init; }
    public int DrPct { get; init; }
    public int ShrugPct { get; init; }

    // Cyclopean (forge)
    public int ReflectPct { get; init; }
    public int FlameProcPct { get; init; }

    // Paean (mending)
    public int HpRegenPct { get; init; }
    public int HealsReceivedPct { get; init; }
    public bool AutoCure { get; init; }

    // Tritonian (ward)
    public int SpellDrPct { get; init; }
    public int ParaResistPct { get; init; }
    public int ResistSkillBonus { get; init; }

    // Talarian (stride)
    public int WeightReductionPct { get; init; }
    public int StamRegenPct { get; init; }
    public int DodgePct { get; init; }

    // Aegis-only (shield bulwark — parry replaces Polias' block/AR identity on shields)
    public int ParryPct { get; init; }
    public int ParryDrPct { get; init; }
    public bool ParryThorns { get; init; }

    // ---- P2 re-theme: new per-material lane fields (populated by later data phases) ------
    public int PoisonResistPct { get; init; }  // folds into the aggregate poison-resist pool
    public int HidingBonus { get; init; }      // folds into the aggregate Hiding skill bonus
    public int OnKillStamPct { get; init; }    // folds into on-kill stamina restore
    public int OnKillHpPct { get; init; }      // folds into on-kill HP restore

    // ---- P2 re-theme: per-root Epic signature (event-gated proc, engine-dispatched) -----
    public ClauseType Signature { get; init; }
    public short S1 { get; init; }
    public short S2 { get; init; }
    public short S3 { get; init; }

    public bool IsEmpty => this is
    {
        BonusAr: 0, DrPct: 0, ShrugPct: 0,
        ReflectPct: 0, FlameProcPct: 0,
        HpRegenPct: 0, HealsReceivedPct: 0, AutoCure: false,
        SpellDrPct: 0, ParaResistPct: 0, ResistSkillBonus: 0,
        WeightReductionPct: 0, StamRegenPct: 0, DodgePct: 0,
        ParryPct: 0, ParryDrPct: 0, ParryThorns: false,
        PoisonResistPct: 0, HidingBonus: 0, OnKillStamPct: 0, OnKillHpPct: 0,
        Signature: ClauseType.None, S1: 0, S2: 0, S3: 0
    };
}

// Array-backed facade over the family registry (the single source of truth — armor magnitudes
// live in Families/{Leather,Studded,Bone,Ringmail,Chainmail,Plate}Family.cs and LegacyRoots.cs;
// shields in ShieldsFamily.cs). Populated once at type init; zero allocation per lookup.
public static class ArmorEffectTable
{
    private static readonly ArmorEffectRow[,] _armorRows = FamilyRegistry.ArmorRows;
    private static readonly ArmorEffectRow[,] _shieldRows = FamilyRegistry.ShieldRows;

    // Zero-allocation lookup. Returns an all-zero row for roots/rarities with no package.
    public static ArmorEffectRow Get(VariantRoot root, ItemRarity rarity, bool isShield)
    {
        if (root == VariantRoot.None)
        {
            return default;
        }

        return (isShield ? _shieldRows : _armorRows)[(int)root, (int)rarity];
    }
}
