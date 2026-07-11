using System.Collections.Generic;

namespace Server.Engines.Rarity;

// Unique-clause archetypes for legendaries (framework §5 primitives, composed).
// The combat interpreter (RarityEffects) switches on these. P1/P2/P3 on the entry
// carry the numeric parameters (hit cadence N, splash %, target cap, seconds, etc.).
public enum ClauseType : byte
{
    None = 0,

    // Extra-swing family (Zephyr)
    ExtraSwingEveryN,   // P1 = N
    ExtraSwingOnParry,
    ExtraSwingFirstHit,
    DoubleStrikeEveryN, // P1 = N; the second strike always crits

    // Crit family (Phobos)
    CritFirstHit,       // P2 = splash %, P3 = target cap (0 = no splash)
    CritEveryN,         // P1 = N
    CritSplash,         // P1 = N, P2 = splash %, P3 = target cap
    CritArmorPen,       // P1 = N, P2 = armor-pen %
    CritExecuteUnder15, // P1 = N; crit x2 vs targets under 15% HP

    // Mark family (Agrotera)
    MarkFirstHit,
    MarkOnCrit,
    MarkAllSources25,   // P1 = extra % taken from all sources
    MarkSpreadOnDeath,  // P1 = spread target cap
    MarkNearbyAllies,   // P1 = ally cap (marked at half bonus)
    MarkHealBlock,      // mark on crit; P1 = heal-block seconds

    // Defense family (Pallas)
    BlockFirstHit,      // guaranteed block vs first hit; blocking a crit stuns (§9)
    BlockCritStun,
    ReflectFirstHit,    // P1 = reflect %; staggers attacker (1s stun, §9)

    // Drain family (Stygian)
    LifestealOnCrit,
    OnKillRestore,      // P1 = 1 stam, 2 stam+mana
    StamDrainOnCrit,
    PoisonTickDoubled,  // guaranteed mark on first hit; poison ticks on marked doubled

    // ---- P2 additions: weapon families 1-6 (swords/polearms/maces/staves/fencing/archery) ----

    // Extra-swing riders (Zephyr lines)
    ExtraSwingSplash,        // P1 = N, P2 = splash %, P3 = target cap; the extra swing sweeps
    ExtraSwingGuaranteedHit, // P1 = N; the extra swing always lands (P3 hit override)
    ExtraSwingManaLeech,     // P1 = N, P2 = mana %; the extra swing leeches the target's mana
    ExtraSwingElemental,     // P1 = N (0 = first hit), P2 = element (0 = lightning, 1 = fire)
    ExtraSwingHealBlock,     // P1 = N, P2 = heal-block seconds on the struck target
    ExtraSwingStackingHit,   // extra swing first hit; P1 = +hit % per later swing, P2 = cap %

    // Crit riders (Phobos / caster lines)
    CritPoisonTick,          // P1 = N; crits apply a poison tick (P10)
    CritStagger,             // P1 = N; crits stagger the target (1s stun, §9)
    CritElemental,           // P1 = N (0 = natural only), P2 = element, P3 = 1 -> also first hit
    CritManaLeech,           // P1 = N (0 = natural only), P2 = mana %, P3 = 1 -> also first hit
    CritHealBlock,           // P1 = N (0 = natural only), P2 = seconds, P3 = 1 -> also first hit
    CritFullHpDouble,        // P1 = N (0 = natural only), P3 = 1 -> first hit; crit x2 vs FULL-HP

    // Mark riders (Agrotera caster lines)
    MarkManaLeech,           // mark first hit; P1 = mana % leeched from the marked target each hit
    MarkElemental,           // mark first hit; P1 = element proc fired when the mark lands
    MarkHealBlockFirstHit,   // mark first hit; P1 = heal-block seconds on the marked target

    // Block / reflect riders (Pallas lines)
    BlockRestoreStam,        // block first hit; P1 = % max stamina restored on a block (P7)
    BlockDrainStam,          // block first hit; P1 = flat stamina drained from the attacker
    BlockManaLeech,          // block first hit; P1 = mana % leeched from the attacker on a block
    BlockElemental,          // block first hit; P1 = element proc fired back at the attacker
    BlockNextShotCrit,       // block first hit; a block guarantees the blocker's next hit crits
    ReflectHealBlock,        // reflect first hit; P1 = reflect %, P2 = heal-block seconds on attacker

    // ---- P3a additions: armor + shields (families 7-9) ----------------------------------

    // Polias/Aegis shrug riders (metal + light armor)
    ShrugStunAttacker,       // a fully shrugged blow briefly stuns the attacker (Kekrops/Kithairon)
    ShrugReflect,            // a fully shrugged blow reflects P1% of its damage back (Erechtheus/Erymanthos)
    ShrugFirstHitGuaranteed, // the first hit taken each fight is always shrugged (Kadmos/Nemea)

    // Cyclopean flame-proc riders
    FlameProcDoubleFirstHit,     // proc chance doubles vs the first hit of any fight (Perdix/Teumessos)
    FlameProcHealBlock,          // proc also heal-blocks the attacker; P1 = seconds (Erichthonios)
    FlameProcBoostLowHp,         // proc chance rises to P1% while wearer is under P2% HP (Talos)
    FlameProcPoison,             // proc also applies a poison tick to the attacker (Khimaira)
    FlameProcSplash,             // proc also splashes to P1 extra nearby attackers (Echidna)
    FlameProcDoubleLowDurability,// proc chance doubles while the shield is under P1% durability (Amphion)
    FlameProcEveryN,             // guaranteed proc every Pth hit taken; P1 = N (Tiryns)

    // Paean mending riders
    AutoCureRestoresStamMana, // auto-cure tick also restores P1% stam/mana (Machaon)
    OnKillRestoreMissingHpPct,// on-kill: restore P1% of missing HP (Podaleirios)
    AutoCureClearsDebuffsOnce,// auto-cure tick also clears mark/heal-block, once per fight (Iapyx)
    AutoCureRestoresHpPct,    // auto-cure tick also restores P1% missing HP (Kyrene)
    LowHpEmergencyCure,       // once per fight below P1% HP: free cure + restore P2% HP (Daphne)
    EmergencyRegenTick,       // once per fight, a hit dropping the wearer under P1% HP instead fires a full regen tick (Keryneia)

    // Tritonian ward riders
    ParaResistBoostsSpellDr,      // resisting a paralyze raises spell DR to P1% for P2s (Nereus)
    ParaResistStunsAttacker,      // a resisted paralyze/stun briefly stuns the attacker instead (Proteus)
    ResistSkillDoubleLowHp,       // Resisting Spells bonus becomes P1 while under P2% HP (Glaukos)
    ResistSkillBoostLowHp,        // +P1 additional Resisting Spells while under P2% HP (Ilion)
    FirstParaAutoFails,           // the first paralyze/stun attempt of any fight automatically fails (Arethousa)
    SpellDrVsPoisonDot,           // spell DR also applies to poison damage-over-time ticks (Skylla/Megareus)
    RerollFirstResist,            // the first failed para/stun resist roll each fight rerolls once (Krommyon)
    SpellDrBoostFirstHit,         // spell DR rises to P1% vs the first spell hit of any fight (Laomedon)
    SpellDrBurstOnCritTaken,      // spell DR doubles for P1s after taking a crit (Palaimon)
    ParaResistBoostsResistSkill,  // resisting a paralyze grants +P1 Resisting Spells for P2s (Alkathous)
    FirstHitNoSecondaryEffect,    // the first hit taken each fight applies no secondary effect (Hyperbios)

    // Talarian stride riders
    DodgeRefundStam,          // a successful dodge instantly refunds P1 stamina (Automedon/Melanippe)
    OnKillDodgeDoubleDuration,// on kill, dodge chance doubles for P1s (Patroklos)
    DodgeRestoreMana,         // a successful dodge also restores P1% mana (Damastor)
    DodgeDoubleFirstAttack,   // dodge chance doubles vs the first attack of any fight (Kyllene)
    DodgeRegenBurst,          // a successful dodge grants +P1% stam regen for P2s (Kalydon/Panoptes)
    WeightReductionSuiteBurstOnDodge, // weight reduction applies suit-wide for P1s after a dodge (Myrtilos)

    // Aegis shield-parry riders
    ParryFirstHitGuaranteed,      // guaranteed parry vs the first hit of any fight (Ankyle/Abderos/Kerberos)
    ParryCritStun,                // parrying a crit briefly stuns the attacker (Oiliades)
    ParryExtraReflect,            // every parry reflects an extra P1% on top of thorns (Salamis)
    ParryRepairsEveryN,           // every Pth parry repairs 1 durability point; P1 = N (Telamon)
    LowHpGuaranteedParry,         // while under P1% HP, the first hit taken is guaranteed-parried (Sakos)
    ParryFirstHitGuaranteedStun,  // guaranteed parry + always-stun vs the first hit of any fight (Aias)

    // Shield-only riders (Cyclopean/Paean/Tritonian/Talarian on a shield)
    ReflectBoostFirstHit,         // reflect % rises to P1% vs the first hit of any fight (Proitos)
    SelfRepairBurstOnCritBlock,   // self-repair rate doubles for P1s after blocking a crit (Zethos)
    SelfRepairRestoresHp,         // each self-repair tick also restores P1% max HP (Danaos)
    ReflectCritStun,              // reflect damage from a blocked crit briefly stuns the attacker (Akrisios)
    HpRegenBurstOnCritTaken,      // HP regen rate triples for P1s after taking a crit (Phylakos)
    OnKillRestoreExtraHp,         // on the wearer's first kill each fight, restore an extra P1% max HP (Autonoos)
    HealBlockOnFirstHitLanded,    // the first hit landed each fight heal-blocks its target; P1 = seconds (Aiakos)
    ManaRegenMirrorsHp,           // mana regen rate gains the same bonus as HP regen (Aristaios)
    StamRegenMirrorsHp,           // stamina regen rate gains the same bonus as HP regen (Boutes)
    OnKillRestoreHpPct,           // on-kill: restore P1% of max HP (Asklepios)
    StamRegenMirrorsManaHalf,     // stamina regen ticks also restore mana (Lasthenes)
    OnKillStamRestoreExtendImmunity, // on-kill: full stamina restore + extend stun immunity by P1s (Melanippos)

    // ---- P3b additions: jewelry (family 10) + clothing (family 11) ----------------------

    // Olympian riders (jewelry — lightning proc on the wearer's own melee hits)
    LightningProcRefundStam,        // lightning proc also refunds P1 stamina (Hyperion)
    LightningProcChanceRestoreMana, // lightning proc has P1% chance to also restore P2 mana (Ouranos)
    StatBonusSplashSecondStat,      // the rolled stat bonus also applies at half value to a second stat (Aither)
    LightningProcResistBurst,       // lightning proc grants +P1% spell DR for P2s (Astraios)

    // Hecatean riders (jewelry — mana leech on spell damage)
    ManaLeechRestoresStam,          // mana leech proc also restores P1 stamina (Selene)
    OnKillFullManaRestore,           // on-kill: full mana restore (Asteria)
    ManaRegenDoubleLowMana,          // mana regen bonus doubles while under P1% mana (Phoibe)
    ManaLeechResistBurst,            // mana leech proc grants +P1% spell DR for P2s (Theia)

    // Tychean riders (jewelry — incoming hit halved / attacker miss-reroll)
    HitHalvedRegenPulse,             // a halved hit grants a P1s HP/stam/mana regen pulse (Ananke)
    MissRerollGrazeRestoreStam,      // a re-rolled miss that fails again still restores P1 stamina (Metis)
    HitHalvedDurabilityImmunity,     // no durability loss for P1s after a halved hit (Nemesis)
    HitHalvedResistBurst,            // a halved hit grants +P1% spell DR for P2s (Themis)

    // Nyxian riders (jewelry — hide/stealth)
    StealthBreakRefundStam,          // opening a fight from stealth refunds the swing's stamina cost (Hypnos)
    RegenDoubleWhileHidden,          // HP/stam/mana regen doubles while hidden (Khaos)
    HideRestoresMana,                // successfully hiding restores P1 mana (Moros)
    PoisonResistDoubleWhileHidden,   // poison resist bonus doubles while hidden (Achlys)

    // Demetrian riders (jewelry — potions)
    PotionRestoresStam,              // potions also restore P1 stamina (Gaia)
    RegenDoubleAfterPotion,          // regen bonus doubles for P1s after drinking any potion (Rhea)
    PotionRestoresMana,              // potions also restore P1 mana (Tethys)
    OnKillTriggerHeldPotion,         // on-kill: instantly gain the effect of the strongest held heal potion (Okeanos)

    // Clothing relics (family 11 — one per theme god, each bound to one piece shape)
    OnKillStamRegenBurstStacking,    // on-kill: +P1% stam regen for P2s, stacks once more on a 2nd kill, max 2 (Klotho)
    AnimalTamingSkillBonus,          // +P1 Animal Taming while worn (Lachesis)
    FrenzyStaggerChance,             // while frenzied, hits carry a P1% chance to stagger the target 1s (Atropos)
    DurabilityLossImmunity,          // immune to durability loss while worn (Ariadne)
    DodgeReflectDamage,              // a successful dodge reflects P1% of the avoided damage (Penelope)

    // ---- P2 (re-theme 2026-07-07): per-family Epic signature clauses ---------------------
    // Populated only in the effect row's Signature slot (never a legendary's unique clause) by
    // later data phases; every current data row leaves Signature = None, so these are dormant.
    RampMaxStacksSplash,          // when a consecutive-hit ramp first hits max: splash P1% to P2 targets (Menis)
    OnKillFullStamNextHitCrit,    // on-kill: full stamina + the next hit crits within a P1s window (Aristeia)
    PoisonedTakeBonusDamage,      // the wielder's hits deal +P1% vs a poisoned target (Haima)
    BlockGrantsDrBurst,           // after a block, gain P1% DR for P2s (Eryma / Phalanx)
    NthHitSplash,                 // every P1th hit splashes P2% to P3 targets (Ennosigaios / Belos)
    NthHitFullArmorPen,           // every P1th hit ignores armor entirely (Rhaistes / Kentron)
    ExtraSwingChain,              // extra swing at cadence; the chained swing may proc ONE more (Aiolos / Horme)
    CritFirstHitStamRefund,       // first hit of a fight is a guaranteed crit and refunds its swing stamina (Ephodos)
    PoisonedTargetsMarked,        // targets the wielder poisons are marked (+P1% taken) (Toxikon)
    DodgeGrantsCounterWindow,     // for P1s after a dodge, the wielder's next swing crits (Ophis)
    ParryRestoresStam             // a successful parry restores P1% max stamina (Aegis line)
}

// One named legendary. Family 0 = axes; BaseIndex = ladder position within the family.
public readonly record struct LegendaryEntry(
    ushort Id,
    string Name,
    VariantRoot Root,
    byte Family,
    byte BaseIndex,
    ClauseType Clause,
    short P1,
    short P2,
    short P3,
    int Hue // 0 = derive from the root's Legendary shade
);

// Data-driven legendary table. Ids are globally unique and never reused (framework §10).
// P1 = axes (ids 1-40, 01-axes.md §3). Later families append here without reshaping.
public static class LegendaryRegistry
{
    public const byte FamilyAxes = 0;
    public const byte FamilySwords = 1;
    public const byte FamilyPolearms = 2;
    public const byte FamilyMaces = 3;
    public const byte FamilyStaves = 4;
    public const byte FamilyFencing = 5;
    public const byte FamilyArchery = 6;
    public const byte FamilyMetalArmor = 7;
    public const byte FamilyLightArmor = 8;
    public const byte FamilyShields = 9;
    public const byte FamilyJewelry = 10;
    public const byte FamilyClothing = 11;

    // Clothing relic BaseIndex — the piece each relic is permanently bound to (21-clothing.md §3).
    public const byte ClothingPieceBodySash = 0;
    public const byte ClothingPieceFancyShirt = 1;
    public const byte ClothingPieceKilt = 2;
    public const byte ClothingPieceRobe = 3;
    public const byte ClothingPieceCloak = 4;

    // Jewelry BaseIndex — the slot every jewelry legendary is bound to (20-jewelry.md §1).
    public const byte JewelrySlotRing = 0;
    public const byte JewelrySlotBracelet = 1;
    public const byte JewelrySlotNecklace = 2;
    public const byte JewelrySlotEarrings = 3;

    private static readonly LegendaryEntry[] _entries =
    {
        // Zephyr line (Hermes)
        new(1, "Aello", VariantRoot.Zephyr, FamilyAxes, 0, ClauseType.ExtraSwingOnParry, 0, 0, 0, 0),
        new(2, "Boreas", VariantRoot.Zephyr, FamilyAxes, 1, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
        new(3, "Podarge", VariantRoot.Zephyr, FamilyAxes, 2, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
        new(4, "Ocypete", VariantRoot.Zephyr, FamilyAxes, 3, ClauseType.DoubleStrikeEveryN, 5, 0, 0, 0),
        new(5, "Euros", VariantRoot.Zephyr, FamilyAxes, 4, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
        new(6, "Notos", VariantRoot.Zephyr, FamilyAxes, 5, ClauseType.ExtraSwingEveryN, 5, 1, 0, 0), // P2=1 stagger
        new(7, "Celaeno", VariantRoot.Zephyr, FamilyAxes, 6, ClauseType.ExtraSwingEveryN, 5, 10, 0, 0), // P2=armor pen %
        new(8, "Aellopos", VariantRoot.Zephyr, FamilyAxes, 7, ClauseType.ExtraSwingEveryN, 5, 0, 5, 0), // P3=stam leech %

        // Phobos line (Ares)
        new(9, "Ker", VariantRoot.Phobos, FamilyAxes, 0, ClauseType.CritFirstHit, 0, 10, 3, 0),
        new(10, "Enyo", VariantRoot.Phobos, FamilyAxes, 1, ClauseType.CritArmorPen, 5, 10, 0, 0),
        new(11, "Alala", VariantRoot.Phobos, FamilyAxes, 2, ClauseType.CritSplash, 6, 10, 3, 0),
        new(12, "Labrys", VariantRoot.Phobos, FamilyAxes, 3, ClauseType.CritSplash, 5, 15, 3, 0),
        new(13, "Polemos", VariantRoot.Phobos, FamilyAxes, 4, ClauseType.CritExecuteUnder15, 6, 0, 0, 0),
        new(14, "Deimos", VariantRoot.Phobos, FamilyAxes, 5, ClauseType.CritSplash, 6, 10, 3, 0),
        new(15, "Eris", VariantRoot.Phobos, FamilyAxes, 6, ClauseType.CritSplash, 5, 10, 3, 0),
        new(16, "Enyalios", VariantRoot.Phobos, FamilyAxes, 7, ClauseType.CritEveryN, 6, 0, 0, 0),

        // Agrotera line (Artemis)
        new(17, "Taygete", VariantRoot.Agrotera, FamilyAxes, 0, ClauseType.MarkNearbyAllies, 3, 0, 0, 0),
        new(18, "Kallisto", VariantRoot.Agrotera, FamilyAxes, 1, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(19, "Britomartis", VariantRoot.Agrotera, FamilyAxes, 2, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0),
        new(20, "Oupis", VariantRoot.Agrotera, FamilyAxes, 3, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(21, "Aktaion", VariantRoot.Agrotera, FamilyAxes, 4, ClauseType.MarkHealBlock, 3, 0, 0, 0),
        new(22, "Orion", VariantRoot.Agrotera, FamilyAxes, 5, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(23, "Sagaris", VariantRoot.Agrotera, FamilyAxes, 6, ClauseType.MarkFirstHit, 0, 0, 0, 0),
        new(24, "Atalanta", VariantRoot.Agrotera, FamilyAxes, 7, ClauseType.PoisonTickDoubled, 0, 0, 0, 0),

        // Pallas line (Athena)
        new(25, "Itonia", VariantRoot.Pallas, FamilyAxes, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(26, "Alalkomeneis", VariantRoot.Pallas, FamilyAxes, 1, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(27, "Promachos", VariantRoot.Pallas, FamilyAxes, 2, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(28, "Gorgoneion", VariantRoot.Pallas, FamilyAxes, 3, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(29, "Glaukopis", VariantRoot.Pallas, FamilyAxes, 4, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(30, "Tritogeneia", VariantRoot.Pallas, FamilyAxes, 5, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(31, "Hippia", VariantRoot.Pallas, FamilyAxes, 6, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(32, "Parthenos", VariantRoot.Pallas, FamilyAxes, 7, ClauseType.BlockFirstHit, 0, 0, 0, 0),

        // Stygian line (Hades)
        new(33, "Lethe", VariantRoot.Stygian, FamilyAxes, 0, ClauseType.StamDrainOnCrit, 0, 0, 0, 0),
        new(34, "Acheron", VariantRoot.Stygian, FamilyAxes, 1, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(35, "Kokytos", VariantRoot.Stygian, FamilyAxes, 2, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(36, "Phlegethon", VariantRoot.Stygian, FamilyAxes, 3, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(37, "Charon", VariantRoot.Stygian, FamilyAxes, 4, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(38, "Erebos", VariantRoot.Stygian, FamilyAxes, 5, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(39, "Tartaros", VariantRoot.Stygian, FamilyAxes, 6, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(40, "Thanatos", VariantRoot.Stygian, FamilyAxes, 7, ClauseType.OnKillRestore, 2, 0, 0, 0),

        // ============================ Swords — family 1 (ids 41-80) ==========================
        // Base ladder (BaseIndex): butcher knife 0, cleaver 1, cutlass 2, scimitar 3, katana 4,
        // broadsword 5, longsword 6, viking sword 7. Namespace: Trojan War / Perseid cycle.
        // Re-theme 2026-07-07: roots re-pointed per 02-swords.md §3 bijection (Zephyr→Menis,
        // Phobos→Phoibos, Agrotera→Haima, Pallas→Areia, Stygian→Aristeia); clauses audited below.

        // Menis line (was Zephyr — the relentless onslaught)
        new(41, "Podarkes", VariantRoot.Menis, FamilySwords, 0, ClauseType.DoubleStrikeEveryN, 5, 0, 0, 0), // SWAP ExtraSwingOnParry (parry belongs to Areia)
        new(42, "Antilochos", VariantRoot.Menis, FamilySwords, 1, ClauseType.ExtraSwingGuaranteedHit, 5, 0, 0, 0),
        new(43, "Xanthos", VariantRoot.Menis, FamilySwords, 2, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
        new(44, "Eumelos", VariantRoot.Menis, FamilySwords, 3, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
        new(45, "Idaios", VariantRoot.Menis, FamilySwords, 4, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
        new(46, "Thoas", VariantRoot.Menis, FamilySwords, 5, ClauseType.ExtraSwingEveryN, 5, 1, 0, 0), // P2=1 stagger
        new(47, "Rhesos", VariantRoot.Menis, FamilySwords, 6, ClauseType.ExtraSwingEveryN, 5, 10, 0, 0), // P2=armor pen %
        new(48, "Meriones", VariantRoot.Menis, FamilySwords, 7, ClauseType.ExtraSwingEveryN, 5, 0, 5, 0), // P3=stam leech %

        // Phoibos line (was Phobos — the unerring strike). Signature = CritFirstHit; uniques avoid it.
        new(49, "Diomedes", VariantRoot.Phoibos, FamilySwords, 0, ClauseType.CritFullHpDouble, 0, 0, 0, 0), // SWAP CritFirstHit (== signature)
        new(50, "Hektor", VariantRoot.Phoibos, FamilySwords, 1, ClauseType.CritSplash, 5, 10, 3, 0),
        new(51, "Sarpedon", VariantRoot.Phoibos, FamilySwords, 2, ClauseType.CritExecuteUnder15, 6, 0, 0, 0),
        new(52, "Aineias", VariantRoot.Phoibos, FamilySwords, 3, ClauseType.CritArmorPen, 5, 15, 0, 0), // SWAP CritPoisonTick (poison belongs to Haima)
        new(53, "Idomeneus", VariantRoot.Phoibos, FamilySwords, 4, ClauseType.CritSplash, 6, 15, 3, 0),
        new(54, "Neoptolemos", VariantRoot.Phoibos, FamilySwords, 5, ClauseType.CritSplash, 0, 10, 3, 0), // SWAP CritFirstHit(splash) (== signature); splash rider survives
        new(55, "Agenor", VariantRoot.Phoibos, FamilySwords, 6, ClauseType.CritEveryN, 5, 0, 0, 0),
        new(56, "Chrysaor", VariantRoot.Phoibos, FamilySwords, 7, ClauseType.CritArmorPen, 6, 15, 0, 0),

        // Haima line (was Agrotera — the opened vein; "mark" reads as a bleeding gash). All FIT.
        new(57, "Kephalos", VariantRoot.Haima, FamilySwords, 0, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(58, "Peirithoos", VariantRoot.Haima, FamilySwords, 1, ClauseType.PoisonTickDoubled, 0, 0, 0, 0),
        new(59, "Paris", VariantRoot.Haima, FamilySwords, 2, ClauseType.MarkHealBlock, 3, 0, 0, 0),
        new(60, "Melanion", VariantRoot.Haima, FamilySwords, 3, ClauseType.MarkFirstHit, 0, 0, 0, 0),
        new(61, "Dolon", VariantRoot.Haima, FamilySwords, 4, ClauseType.MarkNearbyAllies, 3, 0, 0, 0),
        new(62, "Odysseus", VariantRoot.Haima, FamilySwords, 5, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0),
        new(63, "Perseus", VariantRoot.Haima, FamilySwords, 6, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(64, "Harpe", VariantRoot.Haima, FamilySwords, 7, ClauseType.MarkOnCrit, 0, 0, 0, 0),

        // Areia line (was Pallas — the perfect riposte). Signature = BlockNextShotCrit; all FIT.
        new(65, "Nestor", VariantRoot.Areia, FamilySwords, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(66, "Polydamas", VariantRoot.Areia, FamilySwords, 1, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(67, "Antenor", VariantRoot.Areia, FamilySwords, 2, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(68, "Menestheus", VariantRoot.Areia, FamilySwords, 3, ClauseType.BlockRestoreStam, 10, 0, 0, 0),
        new(69, "Eurypylos", VariantRoot.Areia, FamilySwords, 4, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(70, "Sthenelos", VariantRoot.Areia, FamilySwords, 5, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(71, "Amphitryon", VariantRoot.Areia, FamilySwords, 6, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(72, "Deiphobos", VariantRoot.Areia, FamilySwords, 7, ClauseType.BlockRestoreStam, 10, 0, 0, 0),

        // Aristeia line (was Stygian — glory drinks deep). Signature = OnKillFullStamNextHitCrit.
        new(73, "Iphitos", VariantRoot.Aristeia, FamilySwords, 0, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(74, "Memnon", VariantRoot.Aristeia, FamilySwords, 1, ClauseType.OnKillRestore, 2, 0, 0, 0), // SWAP: doc's OnKillFullStamNextHitCrit == the Aristeia signature (invariant); on-kill restore instead
        new(75, "Euphorbos", VariantRoot.Aristeia, FamilySwords, 2, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(76, "Palamedes", VariantRoot.Aristeia, FamilySwords, 3, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(77, "Agamemnon", VariantRoot.Aristeia, FamilySwords, 4, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(78, "Aigisthos", VariantRoot.Aristeia, FamilySwords, 5, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(79, "Elektryon", VariantRoot.Aristeia, FamilySwords, 6, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(80, "Achilles", VariantRoot.Aristeia, FamilySwords, 7, ClauseType.StamDrainOnCrit, 0, 0, 0, 0),

        // ============================ Polearms — family 2 (ids 81-90) ========================
        // BaseIndex: bardiche 0, halberd 1. Namespace: Gigantes / Gigantomachy.
        // Re-theme 2026-07-07: roots per 03-polearms.md §3 (Zephyr→Theristes, Phobos→Sarisa,
        // Agrotera→Horme, Pallas→Phalanx, Stygian→Zophos).

        new(81, "Hippolytos", VariantRoot.Theristes, FamilyPolearms, 0, ClauseType.ExtraSwingSplash, 5, 10, 3, 0),
        new(82, "Thoon", VariantRoot.Theristes, FamilyPolearms, 1, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
        new(83, "Mimas", VariantRoot.Sarisa, FamilyPolearms, 0, ClauseType.CritArmorPen, 6, 10, 0, 0),
        new(84, "Porphyrion", VariantRoot.Sarisa, FamilyPolearms, 1, ClauseType.CritArmorPen, 5, 15, 0, 0), // SWAP CritSplash (splash is Theristes' now)
        new(85, "Gration", VariantRoot.Horme, FamilyPolearms, 0, ClauseType.ExtraSwingChain, 5, 0, 0, 0), // SWAP MarkNearbyAllies (mark foreign to momentum)
        new(86, "Polybotes", VariantRoot.Horme, FamilyPolearms, 1, ClauseType.NthHitFullArmorPen, 4, 0, 0, 0), // SWAP MarkAllSources25 (mark foreign to momentum)
        new(87, "Enkelados", VariantRoot.Phalanx, FamilyPolearms, 0, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(88, "Eurytos", VariantRoot.Phalanx, FamilyPolearms, 1, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(89, "Alkyoneus", VariantRoot.Zophos, FamilyPolearms, 0, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(90, "Klytios", VariantRoot.Zophos, FamilyPolearms, 1, ClauseType.OnKillRestore, 2, 0, 0, 0),

        // ============================ Maces — family 3 (ids 91-125) ==========================
        // BaseIndex: club 0, mace 1, maul 2, war axe 3, hammer pick 4, war mace 5, war hammer 6.
        // Namespace: Cyclopes / Hecatoncheires / storm-forge. (Crush identity is baseline, §1.)
        // Re-theme 2026-07-07: roots per 04-maces.md §3 (Zephyr→Ennosigaios, Phobos→Rhaistes,
        // Agrotera→Kataigis, Pallas→Eryma, Stygian→Kamatos); heavy clause re-audit below.

        // Ennosigaios line (was Zephyr — the earth-shaker's aftershocks)
        new(91, "Briareos", VariantRoot.Ennosigaios, FamilyMaces, 0, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
        new(92, "Kottos", VariantRoot.Ennosigaios, FamilyMaces, 1, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
        new(93, "Gyges", VariantRoot.Ennosigaios, FamilyMaces, 2, ClauseType.ExtraSwingEveryN, 5, 10, 0, 0), // P2=armor pen %
        new(94, "Polyphemos", VariantRoot.Ennosigaios, FamilyMaces, 3, ClauseType.ExtraSwingSplash, 6, 10, 3, 0), // signature is NthHitSplash, so no collision
        new(95, "Pyrphoros", VariantRoot.Ennosigaios, FamilyMaces, 4, ClauseType.ExtraSwingEveryN, 5, 0, 5, 0), // P3=stam leech %
        new(96, "Elektor", VariantRoot.Ennosigaios, FamilyMaces, 5, ClauseType.ExtraSwingOnParry, 0, 0, 0, 0),
        new(97, "Selaios", VariantRoot.Ennosigaios, FamilyMaces, 6, ClauseType.ExtraSwingEveryN, 7, 1, 0, 0), // SWAP cadence 5→7 (5+stagger echoed the Ennosigaios signature)

        // Rhaistes line (was Phobos — the smasher's sunder). Signature = NthHitFullArmorPen; uniques
        // use CritArmorPen (crit-gated partial pen), never NthHitFullArmorPen.
        new(98, "Brontes", VariantRoot.Rhaistes, FamilyMaces, 0, ClauseType.CritFirstHit, 0, 0, 0, 0), // SWAP drop splash (splash is Ennosigaios')
        new(99, "Steropes", VariantRoot.Rhaistes, FamilyMaces, 1, ClauseType.CritArmorPen, 5, 10, 0, 0),
        new(100, "Arges", VariantRoot.Rhaistes, FamilyMaces, 2, ClauseType.CritArmorPen, 6, 15, 0, 0), // SWAP CritSplash (splash is Ennosigaios')
        new(101, "Pyrakmon", VariantRoot.Rhaistes, FamilyMaces, 3, ClauseType.CritExecuteUnder15, 5, 0, 0, 0),
        new(102, "Thyella", VariantRoot.Rhaistes, FamilyMaces, 4, ClauseType.CritArmorPen, 7, 20, 0, 0), // SWAP CritStagger (== Kataigis signature type)
        new(103, "Sthenaros", VariantRoot.Rhaistes, FamilyMaces, 5, ClauseType.CritEveryN, 5, 0, 0, 0),
        new(104, "Keraunos", VariantRoot.Rhaistes, FamilyMaces, 6, ClauseType.CritArmorPen, 8, 25, 0, 0), // SWAP CritElemental (no owning mace lane)

        // Kataigis line (was Agrotera — the tempest's concussion). Signature = CritStagger, so no
        // legendary uses it; every mark clause swapped to a crit/extra-swing vehicle (7 of 7).
        new(105, "Kelmis", VariantRoot.Kataigis, FamilyMaces, 0, ClauseType.CritEveryN, 3, 0, 0, 0), // SWAP MarkNearbyAllies
        new(106, "Damnameneus", VariantRoot.Kataigis, FamilyMaces, 1, ClauseType.CritExecuteUnder15, 5, 0, 0, 0), // SWAP MarkAllSources25
        new(107, "Chalybos", VariantRoot.Kataigis, FamilyMaces, 2, ClauseType.CritFirstHit, 0, 0, 0, 0), // SWAP MarkSpreadOnDeath
        new(108, "Chalkeus", VariantRoot.Kataigis, FamilyMaces, 3, ClauseType.ExtraSwingEveryN, 6, 1, 0, 0), // SWAP MarkHealBlock; literal stagger via extra-swing trigger
        new(109, "Kabeiros", VariantRoot.Kataigis, FamilyMaces, 4, ClauseType.DoubleStrikeEveryN, 7, 0, 0, 0), // SWAP MarkAllSources25
        new(110, "Pyrigenes", VariantRoot.Kataigis, FamilyMaces, 5, ClauseType.CritFullHpDouble, 8, 0, 0, 0), // SWAP MarkFirstHit
        new(111, "Aitnaios", VariantRoot.Kataigis, FamilyMaces, 6, ClauseType.ExtraSwingEveryN, 9, 1, 0, 0), // SWAP PoisonTickDoubled; stagger via extra-swing trigger

        // Eryma line (was Pallas — the bulwark's answer). Signature = BlockGrantsDrBurst; all FIT.
        new(112, "Khalkaspis", VariantRoot.Eryma, FamilyMaces, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(113, "Chalkodamas", VariantRoot.Eryma, FamilyMaces, 1, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(114, "Akmon", VariantRoot.Eryma, FamilyMaces, 2, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(115, "Akmonides", VariantRoot.Eryma, FamilyMaces, 3, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(116, "Skeptron", VariantRoot.Eryma, FamilyMaces, 4, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(117, "Adamastos", VariantRoot.Eryma, FamilyMaces, 5, ClauseType.BlockDrainStam, 2, 0, 0, 0),
        new(118, "Ombrios", VariantRoot.Eryma, FamilyMaces, 6, ClauseType.ReflectFirstHit, 20, 0, 0, 0),

        // Kamatos line (was Stygian — toil unto collapse). Signature = None; all FIT.
        new(119, "Chalkoteuchos", VariantRoot.Kamatos, FamilyMaces, 0, ClauseType.StamDrainOnCrit, 0, 0, 0, 0),
        new(120, "Sphyreus", VariantRoot.Kamatos, FamilyMaces, 1, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(121, "Empyros", VariantRoot.Kamatos, FamilyMaces, 2, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(122, "Astrapios", VariantRoot.Kamatos, FamilyMaces, 3, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(123, "Brontaios", VariantRoot.Kamatos, FamilyMaces, 4, ClauseType.OnKillRestore, 2, 0, 0, 0),
        new(124, "Aitherios", VariantRoot.Kamatos, FamilyMaces, 5, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
        new(125, "Pyriphaes", VariantRoot.Kamatos, FamilyMaces, 6, ClauseType.OnKillRestore, 2, 0, 0, 0),

        // ============================ Staves — family 4 (ids 126-140) ========================
        // BaseIndex: quarter staff 0, gnarled staff 1, black staff 2. Namespace: seers/sorcerers.
        // Re-theme 2026-07-07: roots per 05-staves.md §3 (Zephyr→Empousa, Phobos→Prester,
        // Agrotera→Manteia, Pallas→Alexikakos, Stygian→Baskania); 10 of 15 clauses swapped.

        // Empousa line (was Zephyr — mana drawn from a shade's grasp)
        new(126, "Teiresias", VariantRoot.Empousa, FamilyStaves, 0, ClauseType.ExtraSwingManaLeech, 5, 5, 0, 0),
        new(127, "Kalchas", VariantRoot.Empousa, FamilyStaves, 1, ClauseType.ExtraSwingManaLeech, 5, 8, 0, 0), // SWAP ExtraSwingElemental (elemental is Prester's); P1 5-hit cadence (matches line-mates, was stale 0 from the swap)
        new(128, "Amphiaraos", VariantRoot.Empousa, FamilyStaves, 2, ClauseType.ExtraSwingManaLeech, 5, 10, 0, 0), // SWAP ExtraSwingHealBlock (heal-block is Baskania's)

        // Prester line (was Phobos — the storm's circle). Signature = None.
        new(129, "Kassandra", VariantRoot.Prester, FamilyStaves, 0, ClauseType.CritElemental, 0, 0, 1, 0), // SWAP CritManaLeech (mana leech is Empousa's); P3=1 first hit
        new(130, "Mopsos", VariantRoot.Prester, FamilyStaves, 1, ClauseType.CritElemental, 5, 1, 0, 0), // P2=1 fire
        new(131, "Melampos", VariantRoot.Prester, FamilyStaves, 2, ClauseType.CritElemental, 6, 0, 0, 0), // SWAP CritHealBlock (heal-block is Baskania's); lightning

        // Manteia line (was Agrotera — the oracle's mercy). All swap to the auto-cure family.
        new(132, "Medeia", VariantRoot.Manteia, FamilyStaves, 0, ClauseType.AutoCureRestoresStamMana, 10, 0, 0, 0), // SWAP MarkManaLeech
        new(133, "Kirke", VariantRoot.Manteia, FamilyStaves, 1, ClauseType.AutoCureRestoresHpPct, 8, 0, 0, 0), // SWAP MarkElemental
        new(134, "Phineus", VariantRoot.Manteia, FamilyStaves, 2, ClauseType.LowHpEmergencyCure, 30, 15, 0, 0), // SWAP MarkHealBlockFirstHit

        // Alexikakos line (was Pallas — the ward that turns evil aside). Signature = SpellDrBoostFirstHit.
        new(135, "Orpheus", VariantRoot.Alexikakos, FamilyStaves, 0, ClauseType.ParaResistBoostsSpellDr, 10, 5, 0, 0), // SWAP BlockManaLeech
        new(136, "Polyeidos", VariantRoot.Alexikakos, FamilyStaves, 1, ClauseType.SpellDrBurstOnCritTaken, 6, 0, 0, 0), // SWAP BlockElemental
        new(137, "Helenos", VariantRoot.Alexikakos, FamilyStaves, 2, ClauseType.FirstParaAutoFails, 0, 0, 0, 0), // SWAP ReflectHealBlock

        // Baskania line (was Stygian — the evil eye lingers). Signature = CritHealBlock; uniques avoid it.
        new(138, "Manto", VariantRoot.Baskania, FamilyStaves, 0, ClauseType.MarkHealBlock, 3, 0, 0, 0), // SWAP CritManaLeech
        new(139, "Idmon", VariantRoot.Baskania, FamilyStaves, 1, ClauseType.MarkHealBlockFirstHit, 3, 0, 0, 0), // SWAP CritElemental
        new(140, "Theoklymenos", VariantRoot.Baskania, FamilyStaves, 2, ClauseType.HealBlockOnFirstHitLanded, 3, 0, 0, 0), // SWAP CritHealBlock (== signature)

        // ============================ Fencing — family 5 (ids 141-170) =======================
        // BaseIndex: dagger 0, kryss 1, war fork 2, pitchfork 3, short spear 4, spear 5.
        // Namespace: spear-heroes and mythic serpents. Lower every-Nth (3rd/4th) reflects tempo.
        // Re-theme 2026-07-07: roots per 06-fencing.md §3 (Zephyr→Aiolos, Phobos→Ephodos,
        // Agrotera→Ios, Pallas→Ophis, Stygian→Kentron).

        // Aiolos line (was Zephyr — the wind-lord's flurry). All FIT.
        new(141, "Balios", VariantRoot.Aiolos, FamilyFencing, 0, ClauseType.ExtraSwingEveryN, 4, 0, 0, 0),
        new(142, "Kyknos", VariantRoot.Aiolos, FamilyFencing, 1, ClauseType.ExtraSwingStackingHit, 5, 20, 0, 0),
        new(143, "Asteropaios", VariantRoot.Aiolos, FamilyFencing, 2, ClauseType.ExtraSwingEveryN, 4, 10, 0, 0), // P2=armor pen %
        new(144, "Protesilaos", VariantRoot.Aiolos, FamilyFencing, 3, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
        new(145, "Akamas", VariantRoot.Aiolos, FamilyFencing, 4, ClauseType.ExtraSwingEveryN, 3, 0, 5, 0), // P3=stam leech %
        new(146, "Peleus", VariantRoot.Aiolos, FamilyFencing, 5, ClauseType.ExtraSwingGuaranteedHit, 4, 0, 0, 0),

        // Ephodos line (was Phobos — the opening lunge). Signature = CritFirstHitStamRefund.
        new(147, "Parthenopaios", VariantRoot.Ephodos, FamilyFencing, 0, ClauseType.CritFullHpDouble, 0, 0, 0, 0), // SWAP CritFirstHit (== signature restated); P3 stays 0
        new(148, "Kapaneus", VariantRoot.Ephodos, FamilyFencing, 1, ClauseType.CritSplash, 4, 10, 3, 0),
        new(149, "Tydeus", VariantRoot.Ephodos, FamilyFencing, 2, ClauseType.CritExecuteUnder15, 3, 25, 0, 0), // P2=25% threshold
        new(150, "Asios", VariantRoot.Ephodos, FamilyFencing, 3, ClauseType.CritSplash, 4, 15, 3, 0),
        new(151, "Meleagros", VariantRoot.Ephodos, FamilyFencing, 4, ClauseType.CritStagger, 4, 0, 0, 0), // SWAP CritFirstHit (== signature restated)
        new(152, "Pelion", VariantRoot.Ephodos, FamilyFencing, 5, ClauseType.CritEveryN, 3, 0, 0, 0), // SWAP CritArmorPen (pen is Kentron's)

        // Ios line (was Agrotera — venomous serpents, mark re-read as venom). All FIT.
        new(153, "Amphisbaena", VariantRoot.Ios, FamilyFencing, 0, ClauseType.MarkFirstHit, 0, 0, 0, 0),
        new(154, "Delphyne", VariantRoot.Ios, FamilyFencing, 1, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(155, "Python", VariantRoot.Ios, FamilyFencing, 2, ClauseType.MarkOnCrit, 0, 0, 0, 0),
        new(156, "Lamia", VariantRoot.Ios, FamilyFencing, 3, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0),
        new(157, "Typhon", VariantRoot.Ios, FamilyFencing, 4, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(158, "Hydra", VariantRoot.Ios, FamilyFencing, 5, ClauseType.MarkFirstHit, 0, 0, 0, 0),

        // Ophis line (was Pallas — guardian serpents, block re-read as sway). All FIT.
        new(159, "Aspis", VariantRoot.Ophis, FamilyFencing, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(160, "Ladon", VariantRoot.Ophis, FamilyFencing, 1, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
        new(161, "Ekhion", VariantRoot.Ophis, FamilyFencing, 2, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(162, "Kaineus", VariantRoot.Ophis, FamilyFencing, 3, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(163, "Kolchis", VariantRoot.Ophis, FamilyFencing, 4, ClauseType.ReflectFirstHit, 25, 0, 0, 0),
        new(164, "Bellerophon", VariantRoot.Ophis, FamilyFencing, 5, ClauseType.BlockFirstHit, 0, 0, 0, 0),

        // Kentron line (was Stygian — the sting bites through). Signature = NthHitFullArmorPen; the
        // doc's three "every Nth hit fully ignores armor" uniques restated that type (invariant), so
        // they use CritArmorPen (crit-gated partial pen) instead, like the Rhaistes line.
        new(165, "Ketos", VariantRoot.Kentron, FamilyFencing, 0, ClauseType.CritArmorPen, 4, 20, 0, 0), // SWAP StamDrainOnCrit
        new(166, "Sybaris", VariantRoot.Kentron, FamilyFencing, 1, ClauseType.CritArmorPen, 5, 20, 0, 0), // SWAP LifestealOnCrit (doc's NthHitFullArmorPen == signature)
        new(167, "Drakaina", VariantRoot.Kentron, FamilyFencing, 2, ClauseType.CritArmorPen, 6, 25, 0, 0), // SWAP OnKillRestore (doc's NthHitFullArmorPen == signature)
        new(168, "Ismenios", VariantRoot.Kentron, FamilyFencing, 3, ClauseType.CritArmorPen, 6, 25, 0, 0), // SWAP LifestealOnCrit
        new(169, "Ophion", VariantRoot.Kentron, FamilyFencing, 4, ClauseType.CritArmorPen, 4, 15, 0, 0), // SWAP OnKillRestore (doc's NthHitFullArmorPen == signature)
        new(170, "Kampe", VariantRoot.Kentron, FamilyFencing, 5, ClauseType.CritArmorPen, 6, 20, 0, 0), // SWAP CritManaLeech

        // ============================ Archery — family 6 (ids 171-185) =======================
        // BaseIndex: bow 0, crossbow 1, heavy crossbow 2. Namespace: archer-myths. Pallas legends
        // are pure block (no reflect); the Epic Pallas thorns rider is melee-only (gated in engine).
        // Re-theme 2026-07-07: roots per 07-archery.md §3 (Zephyr→Belos, Phobos→Hekatos,
        // Agrotera→Toxikon, Pallas→Skopos, Stygian→Pede).

        // Belos line (was Zephyr — swift arrows). Signature = NthHitSplash; all FIT.
        new(171, "Skythes", VariantRoot.Belos, FamilyArchery, 0, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
        new(172, "Molpadia", VariantRoot.Belos, FamilyArchery, 1, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
        new(173, "Stymphalia", VariantRoot.Belos, FamilyArchery, 2, ClauseType.ExtraSwingSplash, 5, 10, 3, 0),

        // Hekatos line (was Phobos — killing shots). Signature = CritFirstHit.
        new(174, "Teukros", VariantRoot.Hekatos, FamilyArchery, 0, ClauseType.CritFirstHitStamRefund, 0, 0, 0, 0), // SWAP CritFirstHit (== signature)
        new(175, "Pandaros", VariantRoot.Hekatos, FamilyArchery, 1, ClauseType.CritFullHpDouble, 0, 0, 0, 0), // SWAP drop P3=1 (would force a first-hit crit == signature)
        new(176, "Alkon", VariantRoot.Hekatos, FamilyArchery, 2, ClauseType.CritArmorPen, 5, 10, 0, 0),

        // Toxikon line (was Agrotera — the hunt made mark). Signature = PoisonedTargetsMarked; all FIT.
        new(177, "Skamandrios", VariantRoot.Toxikon, FamilyArchery, 0, ClauseType.MarkAllSources25, 25, 0, 0, 0),
        new(178, "Nessos", VariantRoot.Toxikon, FamilyArchery, 1, ClauseType.PoisonTickDoubled, 0, 0, 0, 0),
        new(179, "Penthesileia", VariantRoot.Toxikon, FamilyArchery, 2, ClauseType.MarkNearbyAllies, 3, 0, 0, 0),

        // Skopos line (was Pallas — deflection at range). Signature = BlockNextShotCrit.
        new(180, "Philoktetes", VariantRoot.Skopos, FamilyArchery, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
        new(181, "Kheiron", VariantRoot.Skopos, FamilyArchery, 1, ClauseType.BlockRestoreStam, 10, 0, 0, 0),
        new(182, "Kydon", VariantRoot.Skopos, FamilyArchery, 2, ClauseType.BlockDrainStam, 3, 0, 0, 0), // SWAP BlockNextShotCrit (== signature)

        // Pede line (was Stygian — the draining arrow). Signature = CritStagger; uniques force a crit
        // cadence (the lane signature then pins), never CritStagger itself (Pede precedent).
        new(183, "Toxeus", VariantRoot.Pede, FamilyArchery, 0, ClauseType.CritEveryN, 6, 0, 0, 0), // SWAP OnKillRestore
        new(184, "Lerna", VariantRoot.Pede, FamilyArchery, 1, ClauseType.CritEveryN, 5, 0, 0, 0), // SWAP LifestealOnCrit
        new(185, "Krotos", VariantRoot.Pede, FamilyArchery, 2, ClauseType.CritEveryN, 4, 0, 0, 0), // SWAP LifestealOnCrit

        // ==================== Metal armor — family 7 (ids 186-200) ===========================
        // BaseIndex: ring 0, chain 1, plate 2. Namespace: 10-armor-metal.md §3-4. Re-theme
        // 2026-07-07: per-material bijection (ring Polias→Hoplites/Cyclopean→Zoster/Paean→Alkimos/
        // Tritonian→Taxis/Talarian→Dromos; chain →Phylax/Halysis/Phrourion/Egregoros/Teichos;
        // plate →Adamas/Kaminos/Akamatos/Kolossos/Panoplia). SWAPs per 10-armor-metal.md §3 audit.

        // was Polias line (Athena)
        new(186, "Kekrops", VariantRoot.Hoplites, FamilyMetalArmor, 0, ClauseType.ShrugStunAttacker, 0, 0, 0, 0),
        new(187, "Erechtheus", VariantRoot.Phylax, FamilyMetalArmor, 1, ClauseType.ShrugReflect, 15, 0, 0, 0),
        new(188, "Kadmos", VariantRoot.Adamas, FamilyMetalArmor, 2, ClauseType.ShrugFirstHitGuaranteed, 0, 0, 0, 0),

        // was Cyclopean line (Hephaistos)
        new(189, "Perdix", VariantRoot.Zoster, FamilyMetalArmor, 0, ClauseType.SelfRepairBurstOnCritBlock, 5, 0, 0, 0), // SWAP FlameProcDoubleFirstHit
        new(190, "Erichthonios", VariantRoot.Halysis, FamilyMetalArmor, 1, ClauseType.SelfRepairRestoresHp, 1, 0, 0, 0), // SWAP FlameProcHealBlock
        new(191, "Talos", VariantRoot.Kaminos, FamilyMetalArmor, 2, ClauseType.FlameProcBoostLowHp, 12, 30, 0, 0),

        // was Paean line (Apollo)
        new(192, "Machaon", VariantRoot.Alkimos, FamilyMetalArmor, 0, ClauseType.ShrugReflect, 10, 0, 0, 0), // SWAP AutoCureRestoresStamMana
        new(193, "Podaleirios", VariantRoot.Phrourion, FamilyMetalArmor, 1, ClauseType.SpellDrBurstOnCritTaken, 3, 3, 0, 0), // SWAP OnKillRestoreMissingHpPct
        new(194, "Iapyx", VariantRoot.Akamatos, FamilyMetalArmor, 2, ClauseType.StamRegenMirrorsHp, 0, 0, 0, 0), // SWAP AutoCureClearsDebuffsOnce

        // was Tritonian line (Poseidon)
        new(195, "Nereus", VariantRoot.Taxis, FamilyMetalArmor, 0, ClauseType.ParaResistBoostsSpellDr, 10, 5, 0, 0),
        new(196, "Proteus", VariantRoot.Egregoros, FamilyMetalArmor, 1, ClauseType.ParaResistStunsAttacker, 0, 0, 0, 0),
        new(197, "Glaukos", VariantRoot.Kolossos, FamilyMetalArmor, 2, ClauseType.ResistSkillDoubleLowHp, 10, 50, 0, 0),

        // was Talarian line (Hermes)
        new(198, "Automedon", VariantRoot.Dromos, FamilyMetalArmor, 0, ClauseType.OnKillStamRestoreExtendImmunity, 5, 0, 0, 0), // SWAP DodgeRefundStam
        new(199, "Patroklos", VariantRoot.Teichos, FamilyMetalArmor, 1, ClauseType.FirstHitNoSecondaryEffect, 0, 0, 0, 0), // SWAP OnKillDodgeDoubleDuration
        new(200, "Damastor", VariantRoot.Panoplia, FamilyMetalArmor, 2, ClauseType.FirstHitNoSecondaryEffect, 0, 0, 0, 0), // SWAP DodgeRestoreMana

        // ==================== Light armor — family 8 (ids 201-215) ============================
        // BaseIndex: leather 0, studded 1, bone 2. Namespace: 11-armor-light.md §3-4.

        // Re-theme 2026-07-07: per-material bijection (leather Polias→Naias/Cyclopean→Dryas/
        // Paean→Melissa/Tritonian→Panika/Talarian→Oreias; studded →Arkas/Batos/Elaphis/Skia/
        // Kynegis; bone →Tymbos/Katachthon/Makaria/Nekyia/Melinoe). SWAPs per 11-armor-light §3.

        // was Polias line (Athena)
        new(201, "Nemea", VariantRoot.Naias, FamilyLightArmor, 0, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // SWAP ShrugFirstHitGuaranteed
        new(202, "Kithairon", VariantRoot.Arkas, FamilyLightArmor, 1, ClauseType.ShrugStunAttacker, 0, 0, 0, 0),
        new(203, "Erymanthos", VariantRoot.Tymbos, FamilyLightArmor, 2, ClauseType.FirstHitNoSecondaryEffect, 0, 0, 0, 0), // SWAP ShrugReflect

        // was Cyclopean line (Hephaistos)
        new(204, "Teumessos", VariantRoot.Dryas, FamilyLightArmor, 0, ClauseType.ReflectBoostFirstHit, 10, 0, 0, 0), // SWAP FlameProcDoubleFirstHit
        new(205, "Khimaira", VariantRoot.Batos, FamilyLightArmor, 1, ClauseType.ReflectCritStun, 0, 0, 0, 0), // SWAP FlameProcPoison
        new(206, "Echidna", VariantRoot.Katachthon, FamilyLightArmor, 2, ClauseType.ReflectCritStun, 0, 0, 0, 0), // SWAP FlameProcSplash

        // was Paean line (Apollo)
        new(207, "Kyrene", VariantRoot.Melissa, FamilyLightArmor, 0, ClauseType.AutoCureRestoresHpPct, 5, 0, 0, 0),
        new(208, "Daphne", VariantRoot.Elaphis, FamilyLightArmor, 1, ClauseType.WeightReductionSuiteBurstOnDodge, 5, 0, 0, 0), // SWAP LowHpEmergencyCure
        new(209, "Keryneia", VariantRoot.Makaria, FamilyLightArmor, 2, ClauseType.EmergencyRegenTick, 10, 0, 0, 0),

        // was Tritonian line (Poseidon)
        new(210, "Arethousa", VariantRoot.Panika, FamilyLightArmor, 0, ClauseType.DodgeGrantsCounterWindow, 0, 0, 0, 0), // SWAP FirstParaAutoFails
        new(211, "Skylla", VariantRoot.Skia, FamilyLightArmor, 1, ClauseType.SpellDrVsPoisonDot, 0, 0, 0, 0),
        new(212, "Krommyon", VariantRoot.Nekyia, FamilyLightArmor, 2, ClauseType.RerollFirstResist, 0, 0, 0, 0),

        // was Talarian line (Hermes)
        new(213, "Kyllene", VariantRoot.Oreias, FamilyLightArmor, 0, ClauseType.StamRegenMirrorsManaHalf, 0, 0, 0, 0), // SWAP DodgeDoubleFirstAttack
        new(214, "Melanippe", VariantRoot.Kynegis, FamilyLightArmor, 1, ClauseType.DodgeRefundStam, 15, 0, 0, 0),
        new(215, "Kalydon", VariantRoot.Melinoe, FamilyLightArmor, 2, ClauseType.DodgeRegenBurst, 10, 3, 0, 0),

        // ==================== Shields — family 9 (ids 216-245) ================================
        // BaseIndex (§1 ladder order): buckler 0, wooden shield 1, wooden kite 2, metal shield 3,
        // metal kite 4, heater 5. Namespace: 12-shields.md §3-4. Athena root here is Aegis.

        // Aegis line (Athena)
        new(216, "Ankyle", VariantRoot.Aegis, FamilyShields, 0, ClauseType.ParryFirstHitGuaranteed, 0, 0, 0, 0),
        new(217, "Oiliades", VariantRoot.Aegis, FamilyShields, 1, ClauseType.ParryCritStun, 0, 0, 0, 0),
        new(218, "Salamis", VariantRoot.Aegis, FamilyShields, 2, ClauseType.ParryExtraReflect, 6, 0, 0, 0),
        new(219, "Telamon", VariantRoot.Aegis, FamilyShields, 3, ClauseType.ParryRepairsEveryN, 10, 0, 0, 0),
        new(220, "Sakos", VariantRoot.Aegis, FamilyShields, 4, ClauseType.LowHpGuaranteedParry, 30, 0, 0, 0),
        new(221, "Aias", VariantRoot.Aegis, FamilyShields, 5, ClauseType.ParryFirstHitGuaranteedStun, 0, 0, 0, 0),

        // Re-theme 2026-07-07: Aegis kept; Cyclopean→Amyntor, Paean→Pnoe, Tritonian→Herkos,
        // Talarian→Probolos. SWAPs per 12-shields.md §3 audit. Shield-held Block* clauses
        // dispatch through the parry machinery (block == parry on a shield).

        // was Cyclopean line → Amyntor (the counter-blow)
        new(222, "Amphion", VariantRoot.Amyntor, FamilyShields, 0, ClauseType.ReflectFirstHit, 15, 0, 0, 0), // SWAP FlameProcDoubleLowDurability
        new(223, "Zethos", VariantRoot.Amyntor, FamilyShields, 1, ClauseType.SelfRepairBurstOnCritBlock, 5, 0, 0, 0),
        new(224, "Proitos", VariantRoot.Amyntor, FamilyShields, 2, ClauseType.ReflectBoostFirstHit, 15, 0, 0, 0),
        new(225, "Tiryns", VariantRoot.Amyntor, FamilyShields, 3, ClauseType.ReflectHealBlock, 15, 3, 0, 0), // SWAP FlameProcEveryN
        new(226, "Danaos", VariantRoot.Amyntor, FamilyShields, 4, ClauseType.SelfRepairRestoresHp, 1, 0, 0, 0),
        new(227, "Akrisios", VariantRoot.Amyntor, FamilyShields, 5, ClauseType.ReflectCritStun, 0, 0, 0, 0),

        // was Paean line → Pnoe (second wind)
        new(228, "Phylakos", VariantRoot.Pnoe, FamilyShields, 0, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0),
        new(229, "Autonoos", VariantRoot.Pnoe, FamilyShields, 1, ClauseType.OnKillRestoreExtraHp, 15, 0, 0, 0),
        new(230, "Aiakos", VariantRoot.Pnoe, FamilyShields, 2, ClauseType.BlockRestoreStam, 10, 0, 0, 0), // SWAP HealBlockOnFirstHitLanded
        new(231, "Aristaios", VariantRoot.Pnoe, FamilyShields, 3, ClauseType.ManaRegenMirrorsHp, 0, 0, 0, 0),
        new(232, "Boutes", VariantRoot.Pnoe, FamilyShields, 4, ClauseType.StamRegenMirrorsHp, 0, 0, 0, 0),
        new(233, "Asklepios", VariantRoot.Pnoe, FamilyShields, 5, ClauseType.OnKillRestoreHpPct, 25, 0, 0, 0),

        // was Tritonian line → Herkos (the fence of war)
        new(234, "Laomedon", VariantRoot.Herkos, FamilyShields, 0, ClauseType.SpellDrBoostFirstHit, 12, 0, 0, 0),
        new(235, "Ilion", VariantRoot.Herkos, FamilyShields, 1, ClauseType.ResistSkillBoostLowHp, 5, 50, 0, 0),
        new(236, "Palaimon", VariantRoot.Herkos, FamilyShields, 2, ClauseType.SpellDrBurstOnCritTaken, 3, 2, 0, 0),
        new(237, "Alkathous", VariantRoot.Herkos, FamilyShields, 3, ClauseType.ParaResistBoostsResistSkill, 10, 5, 0, 0),
        new(238, "Megareus", VariantRoot.Herkos, FamilyShields, 4, ClauseType.SpellDrVsPoisonDot, 0, 0, 0, 0),
        new(239, "Hyperbios", VariantRoot.Herkos, FamilyShields, 5, ClauseType.FirstHitNoSecondaryEffect, 0, 0, 0, 0),

        // was Talarian line → Probolos (the breakwater)
        new(240, "Panoptes", VariantRoot.Probolos, FamilyShields, 0, ClauseType.BlockRestoreStam, 10, 0, 0, 0), // SWAP DodgeRegenBurst
        new(241, "Abderos", VariantRoot.Probolos, FamilyShields, 1, ClauseType.ParryFirstHitGuaranteed, 0, 0, 0, 0),
        new(242, "Myrtilos", VariantRoot.Probolos, FamilyShields, 2, ClauseType.BlockDrainStam, 2, 0, 0, 0), // SWAP WeightReductionSuiteBurstOnDodge
        new(243, "Kerberos", VariantRoot.Probolos, FamilyShields, 3, ClauseType.ParryFirstHitGuaranteed, 0, 0, 0, 0),
        new(244, "Lasthenes", VariantRoot.Probolos, FamilyShields, 4, ClauseType.BlockManaLeech, 10, 0, 0, 0), // SWAP StamRegenMirrorsManaHalf
        new(245, "Melanippos", VariantRoot.Probolos, FamilyShields, 5, ClauseType.BlockNextShotCrit, 0, 0, 0, 0), // SWAP OnKillStamRestoreExtendImmunity

        // ==================== Jewelry — family 10 (ids 246-265) ===============================
        // BaseIndex (§1 slot order): ring 0, bracelet 1, necklace 2, earrings 3. Namespace:
        // 20-jewelry.md §3. Every base×theme combo gets one legendary per slot (not per base,
        // since jewelry has no damage/AR ladder — framework §7).

        // Olympian line (Zeus)
        new(246, "Hyperion", VariantRoot.Olympian, FamilyJewelry, JewelrySlotRing, ClauseType.LightningProcRefundStam, 10, 0, 0, 0),
        new(247, "Ouranos", VariantRoot.Olympian, FamilyJewelry, JewelrySlotBracelet, ClauseType.LightningProcChanceRestoreMana, 50, 10, 0, 0),
        new(248, "Aither", VariantRoot.Olympian, FamilyJewelry, JewelrySlotNecklace, ClauseType.StatBonusSplashSecondStat, 0, 0, 0, 0),
        new(249, "Astraios", VariantRoot.Olympian, FamilyJewelry, JewelrySlotEarrings, ClauseType.LightningProcResistBurst, 10, 3, 0, 0),

        // Hecatean line (Hecate)
        new(250, "Selene", VariantRoot.Hecatean, FamilyJewelry, JewelrySlotRing, ClauseType.ManaLeechRestoresStam, 5, 0, 0, 0),
        new(251, "Asteria", VariantRoot.Hecatean, FamilyJewelry, JewelrySlotBracelet, ClauseType.OnKillFullManaRestore, 0, 0, 0, 0),
        new(252, "Phoibe", VariantRoot.Hecatean, FamilyJewelry, JewelrySlotNecklace, ClauseType.ManaRegenDoubleLowMana, 25, 0, 0, 0),
        new(253, "Theia", VariantRoot.Hecatean, FamilyJewelry, JewelrySlotEarrings, ClauseType.ManaLeechResistBurst, 10, 3, 0, 0),

        // Tychean line (Tyche)
        new(254, "Ananke", VariantRoot.Tychean, FamilyJewelry, JewelrySlotRing, ClauseType.HitHalvedRegenPulse, 3, 0, 0, 0),
        new(255, "Metis", VariantRoot.Tychean, FamilyJewelry, JewelrySlotBracelet, ClauseType.MissRerollGrazeRestoreStam, 10, 0, 0, 0),
        new(256, "Nemesis", VariantRoot.Tychean, FamilyJewelry, JewelrySlotNecklace, ClauseType.HitHalvedDurabilityImmunity, 3, 0, 0, 0),
        new(257, "Themis", VariantRoot.Tychean, FamilyJewelry, JewelrySlotEarrings, ClauseType.HitHalvedResistBurst, 10, 3, 0, 0),

        // Nyxian line (Nyx)
        new(258, "Hypnos", VariantRoot.Nyxian, FamilyJewelry, JewelrySlotRing, ClauseType.StealthBreakRefundStam, 0, 0, 0, 0),
        new(259, "Khaos", VariantRoot.Nyxian, FamilyJewelry, JewelrySlotBracelet, ClauseType.RegenDoubleWhileHidden, 0, 0, 0, 0),
        new(260, "Moros", VariantRoot.Nyxian, FamilyJewelry, JewelrySlotNecklace, ClauseType.HideRestoresMana, 10, 0, 0, 0),
        new(261, "Achlys", VariantRoot.Nyxian, FamilyJewelry, JewelrySlotEarrings, ClauseType.PoisonResistDoubleWhileHidden, 0, 0, 0, 0),

        // Demetrian line (Demeter)
        new(262, "Gaia", VariantRoot.Demetrian, FamilyJewelry, JewelrySlotRing, ClauseType.PotionRestoresStam, 10, 0, 0, 0),
        new(263, "Rhea", VariantRoot.Demetrian, FamilyJewelry, JewelrySlotBracelet, ClauseType.RegenDoubleAfterPotion, 5, 0, 0, 0),
        new(264, "Tethys", VariantRoot.Demetrian, FamilyJewelry, JewelrySlotNecklace, ClauseType.PotionRestoresMana, 10, 0, 0, 0),
        new(265, "Okeanos", VariantRoot.Demetrian, FamilyJewelry, JewelrySlotEarrings, ClauseType.OnKillTriggerHeldPotion, 0, 0, 0, 0),

        // ==================== Clothing relics — family 11 (ids 266-270) =======================
        // BaseIndex = the one piece shape this relic is permanently bound to (21-clothing.md §3).
        // Every clothing legendary carries its theme's full Epic package (AccessoryEffectTable
        // duplicates Epic into the Legendary slot for clothing) plus this unique clause.

        new(266, "Klotho", VariantRoot.Laurel, FamilyClothing, ClothingPieceBodySash, ClauseType.OnKillStamRegenBurstStacking, 20, 5, 0, 0),
        new(267, "Lachesis", VariantRoot.Charis, FamilyClothing, ClothingPieceFancyShirt, ClauseType.AnimalTamingSkillBonus, 15, 0, 0, 0),
        new(268, "Atropos", VariantRoot.Maenad, FamilyClothing, ClothingPieceKilt, ClauseType.FrenzyStaggerChance, 4, 0, 0, 0),
        new(269, "Ariadne", VariantRoot.Hestian, FamilyClothing, ClothingPieceRobe, ClauseType.DurabilityLossImmunity, 0, 0, 0, 0),
        new(270, "Penelope", VariantRoot.Arachne, FamilyClothing, ClothingPieceCloak, ClauseType.DodgeReflectDamage, 10, 0, 0, 0)
    };

    private static readonly Dictionary<ushort, LegendaryEntry> _byId = BuildIndex();

    private static Dictionary<ushort, LegendaryEntry> BuildIndex()
    {
        var map = new Dictionary<ushort, LegendaryEntry>(_entries.Length);

        foreach (var entry in _entries)
        {
            map.Add(entry.Id, entry);
        }

        return map;
    }

    public static IReadOnlyList<LegendaryEntry> Entries => _entries;

    public static bool TryGet(ushort id, out LegendaryEntry entry) => _byId.TryGetValue(id, out entry);

    public static bool Exists(ushort id) => id != 0 && _byId.ContainsKey(id);

    // Loot-roller lookup: the roller knows (family, theme, baseIndex) from its decision, not the
    // id. Linear scan is fine — this only runs on a Legendary roll, a cold, rare path (mob death).
    public static bool TryGetByRootAndBase(byte family, VariantRoot root, byte baseIndex, out LegendaryEntry entry)
    {
        foreach (var candidate in _entries)
        {
            if (candidate.Family == family && candidate.Root == root && candidate.BaseIndex == baseIndex)
            {
                entry = candidate;
                return true;
            }
        }

        entry = default;
        return false;
    }
}
