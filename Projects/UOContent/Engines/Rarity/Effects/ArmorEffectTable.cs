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
    public bool SelfRepair { get; init; }
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
        ReflectPct: 0, FlameProcPct: 0, SelfRepair: false,
        HpRegenPct: 0, HealsReceivedPct: 0, AutoCure: false,
        SpellDrPct: 0, ParaResistPct: 0, ResistSkillBonus: 0,
        WeightReductionPct: 0, StamRegenPct: 0, DodgePct: 0,
        ParryPct: 0, ParryDrPct: 0, ParryThorns: false,
        PoisonResistPct: 0, HidingBonus: 0, OnKillStamPct: 0, OnKillHpPct: 0,
        Signature: ClauseType.None
    };
}

// Exact magnitudes from 10-armor-metal.md §2 / 11-armor-light.md §2 (identical tables — armor
// magnitudes do not vary by material, only the AR number on the concrete item class does) and
// 12-shields.md §2 (shield table runs ~2x an armor piece, own Legendary-above-Epic step).
public static class ArmorEffectTable
{
    private const int VariantRootCount = VariantRootInfo.RootCount; // VariantRoot.None..Pnoe
    private const int RarityCount = 5;        // ItemRarity.Common..Legendary

    private static readonly ArmorEffectRow[,] _armorRows = new ArmorEffectRow[VariantRootCount, RarityCount];
    private static readonly ArmorEffectRow[,] _shieldRows = new ArmorEffectRow[VariantRootCount, RarityCount];

    static ArmorEffectTable()
    {
        // ---- Armor (metal + light — identical §2 tables) ----

        // Polias — Athena (bulwark)
        SetArmor(VariantRoot.Polias, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1 });
        SetArmor(VariantRoot.Polias, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, DrPct = 1 });
        SetArmor(VariantRoot.Polias, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, DrPct = 2, ShrugPct = 5 });
        SetArmor(VariantRoot.Polias, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, DrPct = 3, ShrugPct = 8 });

        // Cyclopean — Hephaistos (forge)
        SetArmor(VariantRoot.Cyclopean, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 2 });
        SetArmor(VariantRoot.Cyclopean, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 3, SelfRepair = true });
        SetArmor(VariantRoot.Cyclopean, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 5, SelfRepair = true, FlameProcPct = 4 });
        SetArmor(VariantRoot.Cyclopean, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 6, SelfRepair = true, FlameProcPct = 8 });

        // Paean — Apollo (mending)
        SetArmor(VariantRoot.Paean, ItemRarity.Uncommon, new ArmorEffectRow { HpRegenPct = 8 });
        SetArmor(VariantRoot.Paean, ItemRarity.Rare, new ArmorEffectRow { HpRegenPct = 12, HealsReceivedPct = 5 });
        SetArmor(VariantRoot.Paean, ItemRarity.Epic, new ArmorEffectRow { HpRegenPct = 18, HealsReceivedPct = 8, AutoCure = true });
        SetArmor(VariantRoot.Paean, ItemRarity.Legendary, new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, AutoCure = true });

        // Tritonian — Poseidon (ward)
        SetArmor(VariantRoot.Tritonian, ItemRarity.Uncommon, new ArmorEffectRow { SpellDrPct = 2 });
        SetArmor(VariantRoot.Tritonian, ItemRarity.Rare, new ArmorEffectRow { SpellDrPct = 3, ParaResistPct = 10 });
        SetArmor(VariantRoot.Tritonian, ItemRarity.Epic, new ArmorEffectRow { SpellDrPct = 5, ParaResistPct = 20, ResistSkillBonus = 5 });
        SetArmor(VariantRoot.Tritonian, ItemRarity.Legendary, new ArmorEffectRow { SpellDrPct = 6, ParaResistPct = 30, ResistSkillBonus = 5 });

        // Talarian — Hermes (stride)
        SetArmor(VariantRoot.Talarian, ItemRarity.Uncommon, new ArmorEffectRow { WeightReductionPct = 10, StamRegenPct = 4 });
        SetArmor(VariantRoot.Talarian, ItemRarity.Rare, new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 6 });
        SetArmor(VariantRoot.Talarian, ItemRarity.Epic, new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8, DodgePct = 3 });
        SetArmor(VariantRoot.Talarian, ItemRarity.Legendary, new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10, DodgePct = 5 });

        // ---- Shields (12-shields.md §2 — own scaled-up table) ----

        // Aegis — Athena (shield bulwark)
        SetShield(VariantRoot.Aegis, ItemRarity.Uncommon, new ArmorEffectRow { ParryPct = 3 });
        SetShield(VariantRoot.Aegis, ItemRarity.Rare, new ArmorEffectRow { ParryPct = 5, ParryDrPct = 5 });
        SetShield(VariantRoot.Aegis, ItemRarity.Epic, new ArmorEffectRow { ParryPct = 7, ParryDrPct = 10, ParryThorns = true });
        SetShield(VariantRoot.Aegis, ItemRarity.Legendary, new ArmorEffectRow { ParryPct = 8, ParryDrPct = 12, ParryThorns = true });

        // Cyclopean (shield)
        SetShield(VariantRoot.Cyclopean, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 4 });
        SetShield(VariantRoot.Cyclopean, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 6, SelfRepair = true });
        SetShield(VariantRoot.Cyclopean, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 8, SelfRepair = true, FlameProcPct = 6 });
        SetShield(VariantRoot.Cyclopean, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 10, SelfRepair = true, FlameProcPct = 10 });

        // Paean (shield)
        SetShield(VariantRoot.Paean, ItemRarity.Uncommon, new ArmorEffectRow { HpRegenPct = 10 });
        SetShield(VariantRoot.Paean, ItemRarity.Rare, new ArmorEffectRow { HpRegenPct = 15, HealsReceivedPct = 5 });
        SetShield(VariantRoot.Paean, ItemRarity.Epic, new ArmorEffectRow { HpRegenPct = 20, HealsReceivedPct = 8, AutoCure = true });
        SetShield(VariantRoot.Paean, ItemRarity.Legendary, new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, AutoCure = true });

        // Tritonian (shield)
        SetShield(VariantRoot.Tritonian, ItemRarity.Uncommon, new ArmorEffectRow { SpellDrPct = 3 });
        SetShield(VariantRoot.Tritonian, ItemRarity.Rare, new ArmorEffectRow { SpellDrPct = 5, ParaResistPct = 15 });
        SetShield(VariantRoot.Tritonian, ItemRarity.Epic, new ArmorEffectRow { SpellDrPct = 7, ParaResistPct = 25, ResistSkillBonus = 5 });
        SetShield(VariantRoot.Tritonian, ItemRarity.Legendary, new ArmorEffectRow { SpellDrPct = 8, ParaResistPct = 30, ResistSkillBonus = 5 });

        // Talarian (shield)
        SetShield(VariantRoot.Talarian, ItemRarity.Uncommon, new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 5 });
        SetShield(VariantRoot.Talarian, ItemRarity.Rare, new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8 });
        SetShield(VariantRoot.Talarian, ItemRarity.Epic, new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10, DodgePct = 4 });
        SetShield(VariantRoot.Talarian, ItemRarity.Legendary, new ArmorEffectRow { WeightReductionPct = 50, StamRegenPct = 12, DodgePct = 6 });

        // ================= Re-theme 2026-07-07: per-material armor roots ==================
        // Magnitude scales carry over from the retired shared tables per primitive (docs 10/11
        // §2). Signature = the lane's Epic rider, present on Epic AND Legendary rows (armor
        // Legendary stays a numeric step above Epic per framework §4).

        // ---- Ring — "the hoplite's kit" (10-armor-metal.md §2) ----

        // Hoplites — line-shield (AR + block-as-shrug)
        SetArmor(VariantRoot.Hoplites, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1 });
        SetArmor(VariantRoot.Hoplites, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, ShrugPct = 3 });
        SetArmor(VariantRoot.Hoplites, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, ShrugPct = 5, Signature = ClauseType.ShrugFirstHitGuaranteed });
        SetArmor(VariantRoot.Hoplites, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, ShrugPct = 8, Signature = ClauseType.ShrugFirstHitGuaranteed });

        // Taxis — formation (DR + para resist)
        SetArmor(VariantRoot.Taxis, ItemRarity.Uncommon, new ArmorEffectRow { DrPct = 1, ParaResistPct = 10 });
        SetArmor(VariantRoot.Taxis, ItemRarity.Rare, new ArmorEffectRow { DrPct = 2, ParaResistPct = 15 });
        SetArmor(VariantRoot.Taxis, ItemRarity.Epic, new ArmorEffectRow { DrPct = 2, ParaResistPct = 20, Signature = ClauseType.ParaResistStunsAttacker });
        SetArmor(VariantRoot.Taxis, ItemRarity.Legendary, new ArmorEffectRow { DrPct = 3, ParaResistPct = 30, Signature = ClauseType.ParaResistStunsAttacker });

        // Dromos — march (stam regen + weight)
        SetArmor(VariantRoot.Dromos, ItemRarity.Uncommon, new ArmorEffectRow { WeightReductionPct = 10, StamRegenPct = 4 });
        SetArmor(VariantRoot.Dromos, ItemRarity.Rare, new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 6 });
        SetArmor(VariantRoot.Dromos, ItemRarity.Epic, new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8, Signature = ClauseType.StamRegenMirrorsManaHalf });
        SetArmor(VariantRoot.Dromos, ItemRarity.Legendary, new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10, Signature = ClauseType.StamRegenMirrorsManaHalf });

        // Zoster — war-belt (durability/self-repair; bool ladder is flat, tune-live)
        SetArmor(VariantRoot.Zoster, ItemRarity.Uncommon, new ArmorEffectRow { SelfRepair = true });
        SetArmor(VariantRoot.Zoster, ItemRarity.Rare, new ArmorEffectRow { SelfRepair = true, BonusAr = 1 });
        SetArmor(VariantRoot.Zoster, ItemRarity.Epic, new ArmorEffectRow { SelfRepair = true, BonusAr = 2, Signature = ClauseType.SelfRepairRestoresHp, S1 = 1 });
        SetArmor(VariantRoot.Zoster, ItemRarity.Legendary, new ArmorEffectRow { SelfRepair = true, BonusAr = 3, Signature = ClauseType.SelfRepairRestoresHp, S1 = 1 });

        // Alkimos — valiant (shrug)
        SetArmor(VariantRoot.Alkimos, ItemRarity.Uncommon, new ArmorEffectRow { ShrugPct = 3 });
        SetArmor(VariantRoot.Alkimos, ItemRarity.Rare, new ArmorEffectRow { ShrugPct = 4 });
        SetArmor(VariantRoot.Alkimos, ItemRarity.Epic, new ArmorEffectRow { ShrugPct = 5, Signature = ClauseType.ShrugStunAttacker });
        SetArmor(VariantRoot.Alkimos, ItemRarity.Legendary, new ArmorEffectRow { ShrugPct = 8, Signature = ClauseType.ShrugStunAttacker });

        // ---- Chain — "the watchful wall" ----

        // Phylax — guard (AR + shrug)
        SetArmor(VariantRoot.Phylax, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1 });
        SetArmor(VariantRoot.Phylax, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, ShrugPct = 3 });
        SetArmor(VariantRoot.Phylax, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, ShrugPct = 5, Signature = ClauseType.ShrugStunAttacker });
        SetArmor(VariantRoot.Phylax, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, ShrugPct = 8, Signature = ClauseType.ShrugStunAttacker });

        // Egregoros — unsleeping (para resist + shrug)
        SetArmor(VariantRoot.Egregoros, ItemRarity.Uncommon, new ArmorEffectRow { ParaResistPct = 10 });
        SetArmor(VariantRoot.Egregoros, ItemRarity.Rare, new ArmorEffectRow { ParaResistPct = 15, ShrugPct = 3 });
        SetArmor(VariantRoot.Egregoros, ItemRarity.Epic, new ArmorEffectRow { ParaResistPct = 20, ShrugPct = 5, Signature = ClauseType.ShrugFirstHitGuaranteed });
        SetArmor(VariantRoot.Egregoros, ItemRarity.Legendary, new ArmorEffectRow { ParaResistPct = 30, ShrugPct = 8, Signature = ClauseType.ShrugFirstHitGuaranteed });

        // Teichos — wall (DR)
        SetArmor(VariantRoot.Teichos, ItemRarity.Uncommon, new ArmorEffectRow { DrPct = 1 });
        SetArmor(VariantRoot.Teichos, ItemRarity.Rare, new ArmorEffectRow { DrPct = 2 });
        SetArmor(VariantRoot.Teichos, ItemRarity.Epic, new ArmorEffectRow { DrPct = 3, Signature = ClauseType.SpellDrBurstOnCritTaken, S1 = 3 });
        SetArmor(VariantRoot.Teichos, ItemRarity.Legendary, new ArmorEffectRow { DrPct = 3, BonusAr = 1, Signature = ClauseType.SpellDrBurstOnCritTaken, S1 = 3 });

        // Halysis — the chain (durability/self-repair)
        SetArmor(VariantRoot.Halysis, ItemRarity.Uncommon, new ArmorEffectRow { SelfRepair = true });
        SetArmor(VariantRoot.Halysis, ItemRarity.Rare, new ArmorEffectRow { SelfRepair = true, BonusAr = 1 });
        SetArmor(VariantRoot.Halysis, ItemRarity.Epic, new ArmorEffectRow { SelfRepair = true, BonusAr = 2, Signature = ClauseType.SelfRepairBurstOnCritBlock, S1 = 5 });
        SetArmor(VariantRoot.Halysis, ItemRarity.Legendary, new ArmorEffectRow { SelfRepair = true, BonusAr = 3, Signature = ClauseType.SelfRepairBurstOnCritBlock, S1 = 5 });

        // Phrourion — fortress (spell DR)
        SetArmor(VariantRoot.Phrourion, ItemRarity.Uncommon, new ArmorEffectRow { SpellDrPct = 2 });
        SetArmor(VariantRoot.Phrourion, ItemRarity.Rare, new ArmorEffectRow { SpellDrPct = 3 });
        SetArmor(VariantRoot.Phrourion, ItemRarity.Epic, new ArmorEffectRow { SpellDrPct = 5, Signature = ClauseType.SpellDrBoostFirstHit, S1 = 10 });
        SetArmor(VariantRoot.Phrourion, ItemRarity.Legendary, new ArmorEffectRow { SpellDrPct = 6, Signature = ClauseType.SpellDrBoostFirstHit, S1 = 10 });

        // ---- Plate — "the forged colossus" ----

        // Adamas — adamant (top AR + DR)
        SetArmor(VariantRoot.Adamas, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1, DrPct = 1 });
        SetArmor(VariantRoot.Adamas, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, DrPct = 2 });
        SetArmor(VariantRoot.Adamas, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, DrPct = 2, Signature = ClauseType.EmergencyRegenTick, S1 = 10 });
        SetArmor(VariantRoot.Adamas, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, DrPct = 3, Signature = ClauseType.EmergencyRegenTick, S1 = 10 });

        // Kaminos — kiln (flame proc + reflect)
        SetArmor(VariantRoot.Kaminos, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 2 });
        SetArmor(VariantRoot.Kaminos, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 3, FlameProcPct = 4 });
        SetArmor(VariantRoot.Kaminos, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 5, FlameProcPct = 8, Signature = ClauseType.FlameProcDoubleFirstHit });
        SetArmor(VariantRoot.Kaminos, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 6, FlameProcPct = 10, Signature = ClauseType.FlameProcDoubleFirstHit });

        // Kolossos — colossus (shrug + para resist)
        SetArmor(VariantRoot.Kolossos, ItemRarity.Uncommon, new ArmorEffectRow { ShrugPct = 3, ParaResistPct = 10 });
        SetArmor(VariantRoot.Kolossos, ItemRarity.Rare, new ArmorEffectRow { ShrugPct = 4, ParaResistPct = 15 });
        SetArmor(VariantRoot.Kolossos, ItemRarity.Epic, new ArmorEffectRow { ShrugPct = 5, ParaResistPct = 20, Signature = ClauseType.ParaResistStunsAttacker });
        SetArmor(VariantRoot.Kolossos, ItemRarity.Legendary, new ArmorEffectRow { ShrugPct = 8, ParaResistPct = 30, Signature = ClauseType.ParaResistStunsAttacker });

        // Panoplia — panoply (AR + durability)
        SetArmor(VariantRoot.Panoplia, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1, SelfRepair = true });
        SetArmor(VariantRoot.Panoplia, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, SelfRepair = true });
        SetArmor(VariantRoot.Panoplia, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, SelfRepair = true, Signature = ClauseType.SelfRepairRestoresHp, S1 = 1 });
        SetArmor(VariantRoot.Panoplia, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, SelfRepair = true, Signature = ClauseType.SelfRepairRestoresHp, S1 = 1 });

        // Akamatos — unwearying (self-repair + HP regen)
        SetArmor(VariantRoot.Akamatos, ItemRarity.Uncommon, new ArmorEffectRow { HpRegenPct = 8, SelfRepair = true });
        SetArmor(VariantRoot.Akamatos, ItemRarity.Rare, new ArmorEffectRow { HpRegenPct = 12, SelfRepair = true });
        SetArmor(VariantRoot.Akamatos, ItemRarity.Epic, new ArmorEffectRow { HpRegenPct = 18, SelfRepair = true, Signature = ClauseType.HpRegenBurstOnCritTaken, S1 = 5 });
        SetArmor(VariantRoot.Akamatos, ItemRarity.Legendary, new ArmorEffectRow { HpRegenPct = 25, SelfRepair = true, Signature = ClauseType.HpRegenBurstOnCritTaken, S1 = 5 });

        // ---- Leather — "the nymph's hide" (11-armor-light.md §2) ----

        // Naias — mending (HP regen + heals received)
        SetArmor(VariantRoot.Naias, ItemRarity.Uncommon, new ArmorEffectRow { HpRegenPct = 8 });
        SetArmor(VariantRoot.Naias, ItemRarity.Rare, new ArmorEffectRow { HpRegenPct = 12, HealsReceivedPct = 5 });
        SetArmor(VariantRoot.Naias, ItemRarity.Epic, new ArmorEffectRow { HpRegenPct = 18, HealsReceivedPct = 8, Signature = ClauseType.EmergencyRegenTick, S1 = 10 });
        SetArmor(VariantRoot.Naias, ItemRarity.Legendary, new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, Signature = ClauseType.EmergencyRegenTick, S1 = 10 });

        // Dryas — briar (thorns + poison resist)
        SetArmor(VariantRoot.Dryas, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 2, PoisonResistPct = 5 });
        SetArmor(VariantRoot.Dryas, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 3, PoisonResistPct = 8 });
        SetArmor(VariantRoot.Dryas, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 5, PoisonResistPct = 10, Signature = ClauseType.SpellDrVsPoisonDot });
        SetArmor(VariantRoot.Dryas, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 6, PoisonResistPct = 12, Signature = ClauseType.SpellDrVsPoisonDot });

        // Oreias — stride (weight + stam regen)
        SetArmor(VariantRoot.Oreias, ItemRarity.Uncommon, new ArmorEffectRow { WeightReductionPct = 10, StamRegenPct = 4 });
        SetArmor(VariantRoot.Oreias, ItemRarity.Rare, new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 6 });
        SetArmor(VariantRoot.Oreias, ItemRarity.Epic, new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8, Signature = ClauseType.OnKillStamRestoreExtendImmunity, S1 = 5 });
        SetArmor(VariantRoot.Oreias, ItemRarity.Legendary, new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10, Signature = ClauseType.OnKillStamRestoreExtendImmunity, S1 = 5 });

        // Melissa — balm (auto-cure + heals received)
        SetArmor(VariantRoot.Melissa, ItemRarity.Uncommon, new ArmorEffectRow { HealsReceivedPct = 5 });
        SetArmor(VariantRoot.Melissa, ItemRarity.Rare, new ArmorEffectRow { HealsReceivedPct = 5, AutoCure = true });
        SetArmor(VariantRoot.Melissa, ItemRarity.Epic, new ArmorEffectRow { HealsReceivedPct = 8, AutoCure = true, Signature = ClauseType.AutoCureClearsDebuffsOnce });
        SetArmor(VariantRoot.Melissa, ItemRarity.Legendary, new ArmorEffectRow { HealsReceivedPct = 10, AutoCure = true, Signature = ClauseType.AutoCureClearsDebuffsOnce });

        // Panika — startle (dodge)
        SetArmor(VariantRoot.Panika, ItemRarity.Uncommon, new ArmorEffectRow { DodgePct = 3 });
        SetArmor(VariantRoot.Panika, ItemRarity.Rare, new ArmorEffectRow { DodgePct = 4 });
        SetArmor(VariantRoot.Panika, ItemRarity.Epic, new ArmorEffectRow { DodgePct = 5, Signature = ClauseType.DodgeDoubleFirstAttack });
        SetArmor(VariantRoot.Panika, ItemRarity.Legendary, new ArmorEffectRow { DodgePct = 8, Signature = ClauseType.DodgeDoubleFirstAttack });

        // ---- Studded — "the hunter's brand" ----

        // Kynegis — chase (dodge + stam regen)
        SetArmor(VariantRoot.Kynegis, ItemRarity.Uncommon, new ArmorEffectRow { DodgePct = 3, StamRegenPct = 4 });
        SetArmor(VariantRoot.Kynegis, ItemRarity.Rare, new ArmorEffectRow { DodgePct = 4, StamRegenPct = 6 });
        SetArmor(VariantRoot.Kynegis, ItemRarity.Epic, new ArmorEffectRow { DodgePct = 5, StamRegenPct = 8, Signature = ClauseType.DodgeRegenBurst, S1 = 20, S2 = 5 });
        SetArmor(VariantRoot.Kynegis, ItemRarity.Legendary, new ArmorEffectRow { DodgePct = 8, StamRegenPct = 10, Signature = ClauseType.DodgeRegenBurst, S1 = 20, S2 = 5 });

        // Batos — bramble (thorns)
        SetArmor(VariantRoot.Batos, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 2 });
        SetArmor(VariantRoot.Batos, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 3 });
        SetArmor(VariantRoot.Batos, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 5, Signature = ClauseType.ReflectBoostFirstHit, S1 = 10 });
        SetArmor(VariantRoot.Batos, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 6, Signature = ClauseType.ReflectBoostFirstHit, S1 = 10 });

        // Arkas — bear (AR + shrug)
        SetArmor(VariantRoot.Arkas, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1 });
        SetArmor(VariantRoot.Arkas, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, ShrugPct = 3 });
        SetArmor(VariantRoot.Arkas, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, ShrugPct = 5, Signature = ClauseType.ShrugFirstHitGuaranteed });
        SetArmor(VariantRoot.Arkas, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, ShrugPct = 8, Signature = ClauseType.ShrugFirstHitGuaranteed });

        // Elaphis — deer (weight + dodge)
        SetArmor(VariantRoot.Elaphis, ItemRarity.Uncommon, new ArmorEffectRow { WeightReductionPct = 10, DodgePct = 3 });
        SetArmor(VariantRoot.Elaphis, ItemRarity.Rare, new ArmorEffectRow { WeightReductionPct = 20, DodgePct = 4 });
        SetArmor(VariantRoot.Elaphis, ItemRarity.Epic, new ArmorEffectRow { WeightReductionPct = 30, DodgePct = 5, Signature = ClauseType.DodgeRestoreMana, S1 = 5 });
        SetArmor(VariantRoot.Elaphis, ItemRarity.Legendary, new ArmorEffectRow { WeightReductionPct = 40, DodgePct = 8, Signature = ClauseType.DodgeRestoreMana, S1 = 5 });

        // Skia — shadow (poison resist + Hiding)
        SetArmor(VariantRoot.Skia, ItemRarity.Uncommon, new ArmorEffectRow { PoisonResistPct = 5, HidingBonus = 2 });
        SetArmor(VariantRoot.Skia, ItemRarity.Rare, new ArmorEffectRow { PoisonResistPct = 8, HidingBonus = 3 });
        SetArmor(VariantRoot.Skia, ItemRarity.Epic, new ArmorEffectRow { PoisonResistPct = 10, HidingBonus = 5, Signature = ClauseType.PoisonResistDoubleWhileHidden });
        SetArmor(VariantRoot.Skia, ItemRarity.Legendary, new ArmorEffectRow { PoisonResistPct = 12, HidingBonus = 5, Signature = ClauseType.PoisonResistDoubleWhileHidden });

        // ---- Bone — "the grave-warden" ----

        // Melinoe — phantom (dodge + para resist)
        SetArmor(VariantRoot.Melinoe, ItemRarity.Uncommon, new ArmorEffectRow { DodgePct = 3, ParaResistPct = 10 });
        SetArmor(VariantRoot.Melinoe, ItemRarity.Rare, new ArmorEffectRow { DodgePct = 4, ParaResistPct = 15 });
        SetArmor(VariantRoot.Melinoe, ItemRarity.Epic, new ArmorEffectRow { DodgePct = 5, ParaResistPct = 20, Signature = ClauseType.DodgeGrantsCounterWindow });
        SetArmor(VariantRoot.Melinoe, ItemRarity.Legendary, new ArmorEffectRow { DodgePct = 8, ParaResistPct = 30, Signature = ClauseType.DodgeGrantsCounterWindow });

        // Makaria — blessed death (on-kill restores)
        SetArmor(VariantRoot.Makaria, ItemRarity.Uncommon, new ArmorEffectRow { OnKillStamPct = 5 });
        SetArmor(VariantRoot.Makaria, ItemRarity.Rare, new ArmorEffectRow { OnKillStamPct = 8 });
        SetArmor(VariantRoot.Makaria, ItemRarity.Epic, new ArmorEffectRow { OnKillStamPct = 10, Signature = ClauseType.OnKillRestoreHpPct, S1 = 10 });
        SetArmor(VariantRoot.Makaria, ItemRarity.Legendary, new ArmorEffectRow { OnKillStamPct = 15, Signature = ClauseType.OnKillRestoreHpPct, S1 = 10 });

        // Tymbos — tomb (AR + DR)
        SetArmor(VariantRoot.Tymbos, ItemRarity.Uncommon, new ArmorEffectRow { BonusAr = 1, DrPct = 1 });
        SetArmor(VariantRoot.Tymbos, ItemRarity.Rare, new ArmorEffectRow { BonusAr = 2, DrPct = 2 });
        SetArmor(VariantRoot.Tymbos, ItemRarity.Epic, new ArmorEffectRow { BonusAr = 3, DrPct = 2, Signature = ClauseType.SpellDrBurstOnCritTaken, S1 = 3 });
        SetArmor(VariantRoot.Tymbos, ItemRarity.Legendary, new ArmorEffectRow { BonusAr = 4, DrPct = 3, Signature = ClauseType.SpellDrBurstOnCritTaken, S1 = 3 });

        // Nekyia — death-ward (spell DR)
        SetArmor(VariantRoot.Nekyia, ItemRarity.Uncommon, new ArmorEffectRow { SpellDrPct = 2 });
        SetArmor(VariantRoot.Nekyia, ItemRarity.Rare, new ArmorEffectRow { SpellDrPct = 3 });
        SetArmor(VariantRoot.Nekyia, ItemRarity.Epic, new ArmorEffectRow { SpellDrPct = 5, Signature = ClauseType.SpellDrBoostFirstHit, S1 = 10 });
        SetArmor(VariantRoot.Nekyia, ItemRarity.Legendary, new ArmorEffectRow { SpellDrPct = 6, Signature = ClauseType.SpellDrBoostFirstHit, S1 = 10 });

        // Katachthon — grave-thorns (reflect)
        SetArmor(VariantRoot.Katachthon, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 2 });
        SetArmor(VariantRoot.Katachthon, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 3 });
        SetArmor(VariantRoot.Katachthon, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 5, Signature = ClauseType.ReflectBoostFirstHit, S1 = 10 });
        SetArmor(VariantRoot.Katachthon, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 6, Signature = ClauseType.ReflectBoostFirstHit, S1 = 10 });

        // ---- Shields — "the aegis line" (12-shields.md §2; Aegis itself unchanged above) ----

        // Amyntor — counter (reflect + self-repair)
        SetShield(VariantRoot.Amyntor, ItemRarity.Uncommon, new ArmorEffectRow { ReflectPct = 4 });
        SetShield(VariantRoot.Amyntor, ItemRarity.Rare, new ArmorEffectRow { ReflectPct = 6, SelfRepair = true });
        SetShield(VariantRoot.Amyntor, ItemRarity.Epic, new ArmorEffectRow { ReflectPct = 8, SelfRepair = true, Signature = ClauseType.ParryExtraReflect, S1 = 6 });
        SetShield(VariantRoot.Amyntor, ItemRarity.Legendary, new ArmorEffectRow { ReflectPct = 10, SelfRepair = true, Signature = ClauseType.ParryExtraReflect, S1 = 6 });

        // Pnoe — second wind (HP regen + heals received, parry-stam signature)
        SetShield(VariantRoot.Pnoe, ItemRarity.Uncommon, new ArmorEffectRow { HpRegenPct = 10 });
        SetShield(VariantRoot.Pnoe, ItemRarity.Rare, new ArmorEffectRow { HpRegenPct = 15, HealsReceivedPct = 5 });
        SetShield(VariantRoot.Pnoe, ItemRarity.Epic, new ArmorEffectRow { HpRegenPct = 20, HealsReceivedPct = 8, Signature = ClauseType.ParryRestoresStam, S1 = 10 });
        SetShield(VariantRoot.Pnoe, ItemRarity.Legendary, new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, Signature = ClauseType.ParryRestoresStam, S1 = 10 });

        // Herkos — barrier (spell DR + para resist)
        SetShield(VariantRoot.Herkos, ItemRarity.Uncommon, new ArmorEffectRow { SpellDrPct = 3 });
        SetShield(VariantRoot.Herkos, ItemRarity.Rare, new ArmorEffectRow { SpellDrPct = 5, ParaResistPct = 15 });
        SetShield(VariantRoot.Herkos, ItemRarity.Epic, new ArmorEffectRow { SpellDrPct = 7, ParaResistPct = 25, ResistSkillBonus = 5, Signature = ClauseType.ParaResistBoostsSpellDr, S1 = 15, S2 = 5 });
        SetShield(VariantRoot.Herkos, ItemRarity.Legendary, new ArmorEffectRow { SpellDrPct = 8, ParaResistPct = 30, ResistSkillBonus = 5, Signature = ClauseType.ParaResistBoostsSpellDr, S1 = 15, S2 = 5 });

        // Probolos — breakwater (DR on block + stam regen)
        SetShield(VariantRoot.Probolos, ItemRarity.Uncommon, new ArmorEffectRow { ParryDrPct = 5 });
        SetShield(VariantRoot.Probolos, ItemRarity.Rare, new ArmorEffectRow { ParryDrPct = 8, StamRegenPct = 8 });
        SetShield(VariantRoot.Probolos, ItemRarity.Epic, new ArmorEffectRow { ParryDrPct = 10, StamRegenPct = 10, Signature = ClauseType.BlockElemental, S1 = 0 });
        SetShield(VariantRoot.Probolos, ItemRarity.Legendary, new ArmorEffectRow { ParryDrPct = 12, StamRegenPct = 12, Signature = ClauseType.BlockElemental, S1 = 0 });
    }

    private static void SetArmor(VariantRoot root, ItemRarity rarity, ArmorEffectRow row) =>
        _armorRows[(int)root, (int)rarity] = row;

    private static void SetShield(VariantRoot root, ItemRarity rarity, ArmorEffectRow row) =>
        _shieldRows[(int)root, (int)rarity] = row;

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
