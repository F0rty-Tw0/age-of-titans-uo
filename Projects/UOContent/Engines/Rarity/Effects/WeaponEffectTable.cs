namespace Server.Engines.Rarity;

// One row of weapon theme magnitudes for a given [root, rarity].
// Percent fields are whole percents (8 = +8%). Zero-valued = effect absent.
public readonly struct WeaponEffectRow
{
    // Zephyr (speed)
    public int SwingSpeedPct { get; init; }
    public int HitChancePct { get; init; }
    public int ExtraSwingPct { get; init; }

    // Phobos (damage)
    public int DamagePct { get; init; }
    public int CritChancePct { get; init; }
    public int CritDamagePct { get; init; }

    // Agrotera (mark)
    public int MarkChancePct { get; init; }
    public int MarkBonusPct { get; init; }
    public bool MarkPoisonTick { get; init; }

    // Pallas (defense)
    public int BlockPct { get; init; }
    public int BlockDrPct { get; init; }
    public bool BlockThorns { get; init; }

    // Stygian (drain)
    public int LifestealPct { get; init; }
    public int StamRegenPct { get; init; }
    public bool LifestealExecute { get; init; } // x2 lifesteal vs targets under 30% HP

    // ---- P2 re-theme: always-on numeric lane fields (populated by later data phases) -----
    public int SplashPct { get; init; }              // EndWeaponHit -> Splash()
    public int ArmorPenPct { get; init; }            // P27: scales absorbed AR by (100-pen)/100
    public int StaggerProcPct { get; init; }         // % chance to stun the target 1s (§9.2 caps)
    public int PoisonApplyPct { get; init; }         // % chance to poison on hit
    public int PoisonTier { get; init; }             // 0 = Lesser, 1 = Regular
    public int ManaLeechPct { get; init; }           // % of the target's mana leeched on hit
    public int ElementalProcPct { get; init; }       // % chance for an elemental proc
    public int ElementalKind { get; init; }          // 0 = lightning, 1 = fire
    public int HealBlockProcPct { get; init; }       // % chance to heal-block the target (3s cap)
    public int NthHitBonusPct { get; init; }         // +dmg% on every NthHitN hit
    public int NthHitN { get; init; }
    public int RampPerStackPct { get; init; }        // +dmg% per consecutive same-target hit (P28)
    public int RampMaxStacks { get; init; }
    public int FirstHitBonusPct { get; init; }       // +dmg% on the first hit of a fight
    public int OnKillStamPct { get; init; }          // on-kill: restore this % of max stamina
    public int DefenderStamDrainFlat { get; init; }  // flat stamina drained from the target on hit

    // ---- P2 re-theme: worn-side utility (folded into WornEffectState when the weapon is held) --
    public int SpellDrPct { get; init; }
    public int ManaRegenPct { get; init; }
    public int DodgePct { get; init; }
    public int ResistSkillBonus { get; init; }
    public int HealsReceivedPct { get; init; } // Manteia staff lane: heals-received while held
    public bool AutoCure { get; init; }         // Manteia staff lane: periodic auto-cure tick while held

    // ---- P2 re-theme: per-root Epic signature (event-gated proc, engine-dispatched) -----
    public ClauseType Signature { get; init; }
    public short S1 { get; init; }
    public short S2 { get; init; }
    public short S3 { get; init; }

    public bool IsEmpty => this is
    {
        SwingSpeedPct: 0, HitChancePct: 0, ExtraSwingPct: 0,
        DamagePct: 0, CritChancePct: 0, CritDamagePct: 0,
        MarkChancePct: 0, MarkBonusPct: 0,
        BlockPct: 0, BlockDrPct: 0,
        LifestealPct: 0, StamRegenPct: 0,
        SplashPct: 0, ArmorPenPct: 0, StaggerProcPct: 0, PoisonApplyPct: 0,
        ManaLeechPct: 0, ElementalProcPct: 0, HealBlockProcPct: 0,
        NthHitBonusPct: 0, RampPerStackPct: 0, FirstHitBonusPct: 0,
        OnKillStamPct: 0, DefenderStamDrainFlat: 0,
        SpellDrPct: 0, ManaRegenPct: 0, DodgePct: 0, ResistSkillBonus: 0,
        HealsReceivedPct: 0,
        Signature: ClauseType.None
    };
}

// Exact magnitudes from 01-axes.md §2 / framework §4 — apply to EVERY weapon family.
// Legendary base package == Epic (framework §4); the Legendary column duplicates Epic.
public static class WeaponEffectTable
{
    private static readonly WeaponEffectRow[,] _rows =
        new WeaponEffectRow[VariantRootCount, RarityCount];

    private const int VariantRootCount = VariantRootInfo.RootCount; // VariantRoot.None..Pnoe
    private const int RarityCount = 5;        // ItemRarity.Common..Legendary

    static WeaponEffectTable()
    {
        // Zephyr — Hermes (speed)
        Set(VariantRoot.Zephyr, ItemRarity.Uncommon, new WeaponEffectRow { SwingSpeedPct = 8 });
        Set(VariantRoot.Zephyr, ItemRarity.Rare, new WeaponEffectRow { SwingSpeedPct = 8, HitChancePct = 6 });
        var zephyrEpic = new WeaponEffectRow { SwingSpeedPct = 10, HitChancePct = 8, ExtraSwingPct = 10 };
        Set(VariantRoot.Zephyr, ItemRarity.Epic, zephyrEpic);
        Set(VariantRoot.Zephyr, ItemRarity.Legendary, zephyrEpic);

        // Phobos — Ares (damage)
        Set(VariantRoot.Phobos, ItemRarity.Uncommon, new WeaponEffectRow { DamagePct = 8 });
        Set(VariantRoot.Phobos, ItemRarity.Rare, new WeaponEffectRow { DamagePct = 8, CritChancePct = 8 });
        var phobosEpic = new WeaponEffectRow { DamagePct = 10, CritChancePct = 10, CritDamagePct = 20 };
        Set(VariantRoot.Phobos, ItemRarity.Epic, phobosEpic);
        Set(VariantRoot.Phobos, ItemRarity.Legendary, phobosEpic);

        // Agrotera — Artemis (mark)
        Set(VariantRoot.Agrotera, ItemRarity.Uncommon, new WeaponEffectRow { MarkChancePct = 6, MarkBonusPct = 8 });
        Set(VariantRoot.Agrotera, ItemRarity.Rare, new WeaponEffectRow { MarkChancePct = 8, MarkBonusPct = 10 });
        var agroteraEpic = new WeaponEffectRow { MarkChancePct = 10, MarkBonusPct = 14, MarkPoisonTick = true };
        Set(VariantRoot.Agrotera, ItemRarity.Epic, agroteraEpic);
        Set(VariantRoot.Agrotera, ItemRarity.Legendary, agroteraEpic);

        // Pallas — Athena (defense)
        Set(VariantRoot.Pallas, ItemRarity.Uncommon, new WeaponEffectRow { BlockPct = 6 });
        Set(VariantRoot.Pallas, ItemRarity.Rare, new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 });
        var pallasEpic = new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true };
        Set(VariantRoot.Pallas, ItemRarity.Epic, pallasEpic);
        Set(VariantRoot.Pallas, ItemRarity.Legendary, pallasEpic);

        // Stygian — Hades (drain)
        Set(VariantRoot.Stygian, ItemRarity.Uncommon, new WeaponEffectRow { LifestealPct = 6 });
        Set(VariantRoot.Stygian, ItemRarity.Rare, new WeaponEffectRow { LifestealPct = 6, StamRegenPct = 6 });
        var stygianEpic = new WeaponEffectRow { LifestealPct = 8, StamRegenPct = 8, LifestealExecute = true };
        Set(VariantRoot.Stygian, ItemRarity.Epic, stygianEpic);
        Set(VariantRoot.Stygian, ItemRarity.Legendary, stygianEpic);

        InitSwords();
        InitMaces();
        InitPolearms();
        InitStaves();
        InitFencing();
        InitArchery();
    }

    // Magnitudes below are the family-doc §2 lane tables (02-07-*.md), which restate the framework
    // §4 magnitude menu. Epic carries the lane's signature (Signature/S1-3); the Legendary column
    // duplicates Epic verbatim (framework §4 weapon rule). Signatures never appear on Uncommon/Rare.

    // 02-swords.md §2 — "the hero's duel".
    private static void InitSwords()
    {
        // Phoibos (precision): hit% + crit%; signature = first hit of each fight always crits.
        Set(VariantRoot.Phoibos, ItemRarity.Uncommon, new WeaponEffectRow { HitChancePct = 6 });
        Set(VariantRoot.Phoibos, ItemRarity.Rare, new WeaponEffectRow { HitChancePct = 6, CritChancePct = 8 });
        var phoibosEpic = new WeaponEffectRow
        {
            HitChancePct = 8, CritChancePct = 10, Signature = ClauseType.CritFirstHit
        };
        Set(VariantRoot.Phoibos, ItemRarity.Epic, phoibosEpic);
        Set(VariantRoot.Phoibos, ItemRarity.Legendary, phoibosEpic);

        // Areia (riposte): block + DR-on-block; signature = a block guarantees the next hit crits.
        Set(VariantRoot.Areia, ItemRarity.Uncommon, new WeaponEffectRow { BlockPct = 6 });
        Set(VariantRoot.Areia, ItemRarity.Rare, new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 });
        var areiaEpic = new WeaponEffectRow
        {
            BlockPct = 8, BlockDrPct = 12, Signature = ClauseType.BlockNextShotCrit
        };
        Set(VariantRoot.Areia, ItemRarity.Epic, areiaEpic);
        Set(VariantRoot.Areia, ItemRarity.Legendary, areiaEpic);

        // Menis (wrath-ramp, P28): stacking dmg per consecutive same-target hit; signature = burst
        // splash on reaching max stacks (RampMaxStacksSplash S1 = splash %, S2 = target cap).
        Set(VariantRoot.Menis, ItemRarity.Uncommon, new WeaponEffectRow { RampPerStackPct = 2, RampMaxStacks = 5 });
        Set(VariantRoot.Menis, ItemRarity.Rare, new WeaponEffectRow { RampPerStackPct = 3, RampMaxStacks = 5, DamagePct = 8 });
        var menisEpic = new WeaponEffectRow
        {
            RampPerStackPct = 3, RampMaxStacks = 6, DamagePct = 10,
            Signature = ClauseType.RampMaxStacksSplash, S1 = 12, S2 = 3
        };
        Set(VariantRoot.Menis, ItemRarity.Epic, menisEpic);
        Set(VariantRoot.Menis, ItemRarity.Legendary, menisEpic);

        // Aristeia (glory, P23): on-kill stamina restore; signature = on-kill full stam + the next
        // swing within S1 seconds crits.
        Set(VariantRoot.Aristeia, ItemRarity.Uncommon, new WeaponEffectRow { OnKillStamPct = 10 });
        Set(VariantRoot.Aristeia, ItemRarity.Rare, new WeaponEffectRow { OnKillStamPct = 15, DamagePct = 8 });
        var aristeiaEpic = new WeaponEffectRow
        {
            OnKillStamPct = 20, DamagePct = 10, Signature = ClauseType.OnKillFullStamNextHitCrit, S1 = 5
        };
        Set(VariantRoot.Aristeia, ItemRarity.Epic, aristeiaEpic);
        Set(VariantRoot.Aristeia, ItemRarity.Legendary, aristeiaEpic);

        // Haima (bleed): "gash" poison tick + lifesteal; signature = poisoned targets take +S1% dmg.
        Set(VariantRoot.Haima, ItemRarity.Uncommon, new WeaponEffectRow { PoisonApplyPct = 8 });
        Set(VariantRoot.Haima, ItemRarity.Rare, new WeaponEffectRow { PoisonApplyPct = 10, LifestealPct = 6 });
        var haimaEpic = new WeaponEffectRow
        {
            PoisonApplyPct = 12, LifestealPct = 8, Signature = ClauseType.PoisonedTakeBonusDamage, S1 = 10
        };
        Set(VariantRoot.Haima, ItemRarity.Epic, haimaEpic);
        Set(VariantRoot.Haima, ItemRarity.Legendary, haimaEpic);
    }

    // 04-maces.md §2 — "storm & anvil". (The family-wide crush baseline lives outside the effect
    // row; these rows carry only the theme package.)
    private static void InitMaces()
    {
        // Ennosigaios (quake): splash; signature = every 5th hit fires a full-power splash burst
        // (NthHitSplash S1 = cadence, S2 = splash %, S3 = target cap). The doc's "+ stagger" half
        // was trimmed at wiring — stagger is Kataigis' identity (04-maces §2 / punch list).
        Set(VariantRoot.Ennosigaios, ItemRarity.Uncommon, new WeaponEffectRow { SplashPct = 8 });
        Set(VariantRoot.Ennosigaios, ItemRarity.Rare, new WeaponEffectRow { SplashPct = 10, DamagePct = 8 });
        var ennosigaiosEpic = new WeaponEffectRow
        {
            SplashPct = 12, DamagePct = 10, Signature = ClauseType.NthHitSplash, S1 = 5, S2 = 12, S3 = 3
        };
        Set(VariantRoot.Ennosigaios, ItemRarity.Epic, ennosigaiosEpic);
        Set(VariantRoot.Ennosigaios, ItemRarity.Legendary, ennosigaiosEpic);

        // Kataigis (concussion): stagger procs; signature = crits stagger the target (CritStagger
        // S1 = 0 -> natural crit only).
        Set(VariantRoot.Kataigis, ItemRarity.Uncommon, new WeaponEffectRow { StaggerProcPct = 4 });
        Set(VariantRoot.Kataigis, ItemRarity.Rare, new WeaponEffectRow { StaggerProcPct = 6, DamagePct = 8 });
        var kataigisEpic = new WeaponEffectRow
        {
            StaggerProcPct = 8, DamagePct = 10, Signature = ClauseType.CritStagger
        };
        Set(VariantRoot.Kataigis, ItemRarity.Epic, kataigisEpic);
        Set(VariantRoot.Kataigis, ItemRarity.Legendary, kataigisEpic);

        // Rhaistes (sunder): armor pen; signature = every 4th hit ignores armor entirely.
        Set(VariantRoot.Rhaistes, ItemRarity.Uncommon, new WeaponEffectRow { ArmorPenPct = 10 });
        Set(VariantRoot.Rhaistes, ItemRarity.Rare, new WeaponEffectRow { ArmorPenPct = 15, CritChancePct = 8 });
        var rhaistesEpic = new WeaponEffectRow
        {
            ArmorPenPct = 20, CritChancePct = 10, Signature = ClauseType.NthHitFullArmorPen, S1 = 4
        };
        Set(VariantRoot.Rhaistes, ItemRarity.Epic, rhaistesEpic);
        Set(VariantRoot.Rhaistes, ItemRarity.Legendary, rhaistesEpic);

        // Eryma (anvil): block + DR + thorns (thorns per framework §3 identity); signature =
        // BlockGrantsDrBurst (after a block, gain S1% DR for S2s). Doc §2's "next swing bonus
        // damage" wording was reconciled to the engine's DR-burst clause at wiring.
        Set(VariantRoot.Eryma, ItemRarity.Uncommon, new WeaponEffectRow { BlockPct = 6 });
        Set(VariantRoot.Eryma, ItemRarity.Rare, new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 });
        var erymaEpic = new WeaponEffectRow
        {
            BlockPct = 8, BlockDrPct = 12, BlockThorns = true,
            Signature = ClauseType.BlockGrantsDrBurst, S1 = 8, S2 = 5
        };
        Set(VariantRoot.Eryma, ItemRarity.Epic, erymaEpic);
        Set(VariantRoot.Eryma, ItemRarity.Legendary, erymaEpic);

        // Kamatos (exhaust): stamina lane. The engine has no %-based leech-the-target field, so
        // "stam leech" folds into StamRegenPct (attacker stam on hit) + DefenderStamDrainFlat
        // (target exhaust). Signature = None: the doc's "bonus dmg vs low-stam" has no clause.
        Set(VariantRoot.Kamatos, ItemRarity.Uncommon, new WeaponEffectRow { StamRegenPct = 6 });
        Set(VariantRoot.Kamatos, ItemRarity.Rare, new WeaponEffectRow { StamRegenPct = 8, DefenderStamDrainFlat = 2 });
        var kamatosEpic = new WeaponEffectRow { StamRegenPct = 10, DefenderStamDrainFlat = 3 };
        Set(VariantRoot.Kamatos, ItemRarity.Epic, kamatosEpic);
        Set(VariantRoot.Kamatos, ItemRarity.Legendary, kamatosEpic);
    }

    // 03-polearms.md §2 — "the reaping line".
    private static void InitPolearms()
    {
        // Theristes (reap): splash; signature = every crit also splashes (CritSplash S1 = 0 natural
        // crit only, S2 = splash %, S3 = target cap).
        Set(VariantRoot.Theristes, ItemRarity.Uncommon, new WeaponEffectRow { SplashPct = 8 });
        Set(VariantRoot.Theristes, ItemRarity.Rare, new WeaponEffectRow { SplashPct = 10, DamagePct = 8 });
        var theristesEpic = new WeaponEffectRow
        {
            SplashPct = 12, DamagePct = 10, Signature = ClauseType.CritSplash, S2 = 12, S3 = 3
        };
        Set(VariantRoot.Theristes, ItemRarity.Epic, theristesEpic);
        Set(VariantRoot.Theristes, ItemRarity.Legendary, theristesEpic);

        // Sarisa (impale): armor pen + crit dmg; signature = guaranteed first-hit crit. The always-on
        // ArmorPenPct = 20 carries through the first-hit crit, so the "crit w/ full pen" reads
        // automatically (CritFirstHit, no splash params).
        Set(VariantRoot.Sarisa, ItemRarity.Uncommon, new WeaponEffectRow { ArmorPenPct = 10 });
        Set(VariantRoot.Sarisa, ItemRarity.Rare, new WeaponEffectRow { ArmorPenPct = 15, CritChancePct = 8 });
        var sarisaEpic = new WeaponEffectRow
        {
            ArmorPenPct = 20, CritChancePct = 10, CritDamagePct = 20, Signature = ClauseType.CritFirstHit
        };
        Set(VariantRoot.Sarisa, ItemRarity.Epic, sarisaEpic);
        Set(VariantRoot.Sarisa, ItemRarity.Legendary, sarisaEpic);

        // Phalanx (hold): block + thorns (framework §3); signature = BlockGrantsDrBurst.
        Set(VariantRoot.Phalanx, ItemRarity.Uncommon, new WeaponEffectRow { BlockPct = 6 });
        Set(VariantRoot.Phalanx, ItemRarity.Rare, new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 });
        var phalanxEpic = new WeaponEffectRow
        {
            BlockPct = 8, BlockDrPct = 12, BlockThorns = true,
            Signature = ClauseType.BlockGrantsDrBurst, S1 = 8, S2 = 5
        };
        Set(VariantRoot.Phalanx, ItemRarity.Epic, phalanxEpic);
        Set(VariantRoot.Phalanx, ItemRarity.Legendary, phalanxEpic);

        // Horme (momentum, P15): every-Nth escalating dmg; signature = the same cadence hit also
        // lands a guaranteed extra swing (ExtraSwingEveryN S1 = cadence).
        Set(VariantRoot.Horme, ItemRarity.Uncommon, new WeaponEffectRow { NthHitBonusPct = 15, NthHitN = 5 });
        Set(VariantRoot.Horme, ItemRarity.Rare, new WeaponEffectRow { NthHitBonusPct = 20, NthHitN = 5, HitChancePct = 6 });
        var hormeEpic = new WeaponEffectRow
        {
            NthHitBonusPct = 25, NthHitN = 4, HitChancePct = 8, Signature = ClauseType.ExtraSwingEveryN, S1 = 4
        };
        Set(VariantRoot.Horme, ItemRarity.Epic, hormeEpic);
        Set(VariantRoot.Horme, ItemRarity.Legendary, hormeEpic);

        // Zophos (toll): lifesteal + on-kill restore; signature = None (the "lifesteal x2 vs low HP"
        // half is the LifestealExecute row bool, not an event clause).
        Set(VariantRoot.Zophos, ItemRarity.Uncommon, new WeaponEffectRow { LifestealPct = 6 });
        Set(VariantRoot.Zophos, ItemRarity.Rare, new WeaponEffectRow { LifestealPct = 6, OnKillStamPct = 15 });
        var zophosEpic = new WeaponEffectRow { LifestealPct = 8, OnKillStamPct = 20, LifestealExecute = true };
        Set(VariantRoot.Zophos, ItemRarity.Epic, zophosEpic);
        Set(VariantRoot.Zophos, ItemRarity.Legendary, zophosEpic);
    }

    // 05-staves.md §2 — "the seer's rod". Every staff variant also carries the universal mana-regen
    // rider (§1: +6/+8/+12 by tier), stacked underneath each lane row via ManaRegenPct.
    private static void InitStaves()
    {
        // Empousa (siphon): mana leech; signature = crits burst-leech double the normal %
        // (CritManaLeech S1 = 0 natural crit, S2 = burst leech %).
        Set(VariantRoot.Empousa, ItemRarity.Uncommon, new WeaponEffectRow { ManaLeechPct = 6, ManaRegenPct = 6 });
        Set(VariantRoot.Empousa, ItemRarity.Rare, new WeaponEffectRow { ManaLeechPct = 8, CritChancePct = 8, ManaRegenPct = 8 });
        var empousaEpic = new WeaponEffectRow
        {
            ManaLeechPct = 10, CritChancePct = 10, ManaRegenPct = 12,
            Signature = ClauseType.CritManaLeech, S2 = 20
        };
        Set(VariantRoot.Empousa, ItemRarity.Epic, empousaEpic);
        Set(VariantRoot.Empousa, ItemRarity.Legendary, empousaEpic);

        // Prester (storm): elemental proc; signature = None. The engine's ElementalKind is a fixed
        // element with no "alternate" convention, so the doc's "alternates fire/lightning" is served
        // as fixed lightning; the "proc chance up" is the numeric ElementalProcPct ramp (6/8/10).
        Set(VariantRoot.Prester, ItemRarity.Uncommon, new WeaponEffectRow { ElementalProcPct = 6, ManaRegenPct = 6 });
        Set(VariantRoot.Prester, ItemRarity.Rare, new WeaponEffectRow { ElementalProcPct = 8, CritChancePct = 8, ManaRegenPct = 8 });
        var presterEpic = new WeaponEffectRow { ElementalProcPct = 10, CritChancePct = 10, ManaRegenPct = 12 };
        Set(VariantRoot.Prester, ItemRarity.Epic, presterEpic);
        Set(VariantRoot.Prester, ItemRarity.Legendary, presterEpic);

        // Alexikakos (ward): spell DR + Resisting Spells (worn-side); signature = the first spell
        // each fight is heavily DR'd (SpellDrBoostFirstHit S1 = boosted DR %).
        Set(VariantRoot.Alexikakos, ItemRarity.Uncommon, new WeaponEffectRow { SpellDrPct = 2, ManaRegenPct = 6 });
        Set(VariantRoot.Alexikakos, ItemRarity.Rare, new WeaponEffectRow { SpellDrPct = 3, HitChancePct = 6, ManaRegenPct = 8 });
        var alexikakosEpic = new WeaponEffectRow
        {
            SpellDrPct = 5, HitChancePct = 8, ResistSkillBonus = 5, ManaRegenPct = 12,
            Signature = ClauseType.SpellDrBoostFirstHit, S1 = 15
        };
        Set(VariantRoot.Alexikakos, ItemRarity.Epic, alexikakosEpic);
        Set(VariantRoot.Alexikakos, ItemRarity.Legendary, alexikakosEpic);

        // Manteia (oracle): mana regen (lane + universal) + heals-received + auto-cure; signature =
        // None (its Epic adds are the numeric HealsReceivedPct + AutoCure fields). The doc's "+5
        // Meditation" is not representable — the only wired skill-mod field is Resisting Spells
        // (ResistSkillBonus), which belongs to Alexikakos — so it is dropped here.
        Set(VariantRoot.Manteia, ItemRarity.Uncommon, new WeaponEffectRow { ManaRegenPct = 14 });
        Set(VariantRoot.Manteia, ItemRarity.Rare, new WeaponEffectRow { ManaRegenPct = 20, HitChancePct = 6 });
        var manteiaEpic = new WeaponEffectRow
        {
            ManaRegenPct = 30, HitChancePct = 8, HealsReceivedPct = 15, AutoCure = true
        };
        Set(VariantRoot.Manteia, ItemRarity.Epic, manteiaEpic);
        Set(VariantRoot.Manteia, ItemRarity.Legendary, manteiaEpic);

        // Baskania (curse): lifesteal + heal-block procs (Rare-unlock per §9.3); signature = crits
        // heal-block (CritHealBlock S1 = 0 natural crit, S2 = seconds).
        Set(VariantRoot.Baskania, ItemRarity.Uncommon, new WeaponEffectRow { LifestealPct = 6, ManaRegenPct = 6 });
        Set(VariantRoot.Baskania, ItemRarity.Rare, new WeaponEffectRow { LifestealPct = 6, HealBlockProcPct = 6, ManaRegenPct = 8 });
        var baskaniaEpic = new WeaponEffectRow
        {
            LifestealPct = 8, HealBlockProcPct = 8, ManaRegenPct = 12,
            Signature = ClauseType.CritHealBlock, S2 = 3
        };
        Set(VariantRoot.Baskania, ItemRarity.Epic, baskaniaEpic);
        Set(VariantRoot.Baskania, ItemRarity.Legendary, baskaniaEpic);
    }

    // 06-fencing.md §2 — "serpent's tempo".
    private static void InitFencing()
    {
        // Ios (venom): poison apply chance; signature = None (poison tier-up is the numeric
        // PoisonTier field: 1 -> Regular poison).
        Set(VariantRoot.Ios, ItemRarity.Uncommon, new WeaponEffectRow { PoisonApplyPct = 8 });
        Set(VariantRoot.Ios, ItemRarity.Rare, new WeaponEffectRow { PoisonApplyPct = 10, HitChancePct = 6 });
        var iosEpic = new WeaponEffectRow { PoisonApplyPct = 12, HitChancePct = 8, PoisonTier = 1 };
        Set(VariantRoot.Ios, ItemRarity.Epic, iosEpic);
        Set(VariantRoot.Ios, ItemRarity.Legendary, iosEpic);

        // Ephodos (lunge, P14): first-hit-of-fight bonus dmg + crit; signature = guaranteed first-hit
        // crit that refunds its swing stamina.
        Set(VariantRoot.Ephodos, ItemRarity.Uncommon, new WeaponEffectRow { FirstHitBonusPct = 15 });
        Set(VariantRoot.Ephodos, ItemRarity.Rare, new WeaponEffectRow { FirstHitBonusPct = 20, CritChancePct = 8 });
        var ephodosEpic = new WeaponEffectRow
        {
            FirstHitBonusPct = 25, CritChancePct = 10, Signature = ClauseType.CritFirstHitStamRefund
        };
        Set(VariantRoot.Ephodos, ItemRarity.Epic, ephodosEpic);
        Set(VariantRoot.Ephodos, ItemRarity.Legendary, ephodosEpic);

        // Aiolos (flurry): swing speed + extra-swing proc; signature = an extra swing may chain into
        // another (ExtraSwingChain S1 = cadence for the chained proc).
        Set(VariantRoot.Aiolos, ItemRarity.Uncommon, new WeaponEffectRow { SwingSpeedPct = 8 });
        Set(VariantRoot.Aiolos, ItemRarity.Rare, new WeaponEffectRow { SwingSpeedPct = 8, HitChancePct = 6 });
        var aiolosEpic = new WeaponEffectRow
        {
            SwingSpeedPct = 10, HitChancePct = 8, ExtraSwingPct = 10, Signature = ClauseType.ExtraSwingChain, S1 = 5
        };
        Set(VariantRoot.Aiolos, ItemRarity.Epic, aiolosEpic);
        Set(VariantRoot.Aiolos, ItemRarity.Legendary, aiolosEpic);

        // Kentron (puncture): armor pen + hit%; signature = every 3rd hit fully ignores armor.
        Set(VariantRoot.Kentron, ItemRarity.Uncommon, new WeaponEffectRow { ArmorPenPct = 10 });
        Set(VariantRoot.Kentron, ItemRarity.Rare, new WeaponEffectRow { ArmorPenPct = 15, HitChancePct = 6 });
        var kentronEpic = new WeaponEffectRow
        {
            ArmorPenPct = 20, HitChancePct = 8, Signature = ClauseType.NthHitFullArmorPen, S1 = 3
        };
        Set(VariantRoot.Kentron, ItemRarity.Epic, kentronEpic);
        Set(VariantRoot.Kentron, ItemRarity.Legendary, kentronEpic);

        // Ophis (evasion): worn-side dodge/parry; signature = a successful dodge opens a counter
        // window (DodgeGrantsCounterWindow). The doc's "DR on a successful dodge/parry" is not a
        // separate mechanic — a dodge fully avoids the hit — so the rows carry DodgePct only.
        Set(VariantRoot.Ophis, ItemRarity.Uncommon, new WeaponEffectRow { DodgePct = 6 });
        Set(VariantRoot.Ophis, ItemRarity.Rare, new WeaponEffectRow { DodgePct = 6 });
        var ophisEpic = new WeaponEffectRow { DodgePct = 8, Signature = ClauseType.DodgeGrantsCounterWindow };
        Set(VariantRoot.Ophis, ItemRarity.Epic, ophisEpic);
        Set(VariantRoot.Ophis, ItemRarity.Legendary, ophisEpic);
    }

    // 07-archery.md §2 — "the far mark".
    private static void InitArchery()
    {
        // Hekatos (deadeye): hit% + crit% + crit dmg; signature = first shot of each fight crits.
        Set(VariantRoot.Hekatos, ItemRarity.Uncommon, new WeaponEffectRow { HitChancePct = 6 });
        Set(VariantRoot.Hekatos, ItemRarity.Rare, new WeaponEffectRow { HitChancePct = 6, CritChancePct = 8 });
        var hekatosEpic = new WeaponEffectRow
        {
            HitChancePct = 8, CritChancePct = 10, CritDamagePct = 20, Signature = ClauseType.CritFirstHit
        };
        Set(VariantRoot.Hekatos, ItemRarity.Epic, hekatosEpic);
        Set(VariantRoot.Hekatos, ItemRarity.Legendary, hekatosEpic);

        // Belos (volley): splash; signature = every 4th shot fires a full-power splash burst.
        Set(VariantRoot.Belos, ItemRarity.Uncommon, new WeaponEffectRow { SplashPct = 8 });
        Set(VariantRoot.Belos, ItemRarity.Rare, new WeaponEffectRow { SplashPct = 10, DamagePct = 8 });
        var belosEpic = new WeaponEffectRow
        {
            SplashPct = 12, DamagePct = 10, Signature = ClauseType.NthHitSplash, S1 = 4, S2 = 12, S3 = 3
        };
        Set(VariantRoot.Belos, ItemRarity.Epic, belosEpic);
        Set(VariantRoot.Belos, ItemRarity.Legendary, belosEpic);

        // Pede (pin): stagger procs; signature = crits stagger (same shape as Kataigis).
        Set(VariantRoot.Pede, ItemRarity.Uncommon, new WeaponEffectRow { StaggerProcPct = 4 });
        Set(VariantRoot.Pede, ItemRarity.Rare, new WeaponEffectRow { StaggerProcPct = 6, DamagePct = 8 });
        var pedeEpic = new WeaponEffectRow { StaggerProcPct = 8, DamagePct = 10, Signature = ClauseType.CritStagger };
        Set(VariantRoot.Pede, ItemRarity.Epic, pedeEpic);
        Set(VariantRoot.Pede, ItemRarity.Legendary, pedeEpic);

        // Toxikon (toxin): poison apply chance + hit%; signature = poisoned targets are marked,
        // taking +S1% dmg while poisoned (PoisonedTargetsMarked).
        Set(VariantRoot.Toxikon, ItemRarity.Uncommon, new WeaponEffectRow { PoisonApplyPct = 8 });
        Set(VariantRoot.Toxikon, ItemRarity.Rare, new WeaponEffectRow { PoisonApplyPct = 10, HitChancePct = 6 });
        var toxikonEpic = new WeaponEffectRow
        {
            PoisonApplyPct = 12, HitChancePct = 8, Signature = ClauseType.PoisonedTargetsMarked, S1 = 14
        };
        Set(VariantRoot.Toxikon, ItemRarity.Epic, toxikonEpic);
        Set(VariantRoot.Toxikon, ItemRarity.Legendary, toxikonEpic);

        // Skopos (warden): hit% + defensive block/DR (holds at range); signature = a block guarantees
        // the next shot crits.
        Set(VariantRoot.Skopos, ItemRarity.Uncommon, new WeaponEffectRow { HitChancePct = 6, BlockPct = 6 });
        Set(VariantRoot.Skopos, ItemRarity.Rare, new WeaponEffectRow { HitChancePct = 8, BlockPct = 6, BlockDrPct = 8 });
        var skoposEpic = new WeaponEffectRow
        {
            HitChancePct = 8, BlockPct = 8, BlockDrPct = 12, Signature = ClauseType.BlockNextShotCrit
        };
        Set(VariantRoot.Skopos, ItemRarity.Epic, skoposEpic);
        Set(VariantRoot.Skopos, ItemRarity.Legendary, skoposEpic);
    }

    private static void Set(VariantRoot root, ItemRarity rarity, WeaponEffectRow row) =>
        _rows[(int)root, (int)rarity] = row;

    // Zero-allocation lookup. Returns an all-zero row for roots/rarities with no package.
    public static WeaponEffectRow Get(VariantRoot root, ItemRarity rarity)
    {
        if (root == VariantRoot.None)
        {
            return default;
        }

        return _rows[(int)root, (int)rarity];
    }
}
