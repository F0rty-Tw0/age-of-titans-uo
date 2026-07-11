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
    // BlockCritStun removed 2026-07-11: orphaned — no registry entry ever carried it, nothing in
    // game promised it. ClauseType is not serialized, so removal is save-safe.
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

    // Option A slot-set signatures (armor-slotsets plan, Phase 3) — chest/arms shrug riders
    ShrugFirstHitPoisonAttacker, // first hit always shrugged + poisons the attacker (Studded Chest)
    ShrugFirstHitDrainStam,      // first hit always shrugged + drains P1 stamina from the attacker (Bone Chest)
    ShrugFirstHitDrBurst,        // first hit always shrugged + grants +P1% DR for P2s (Ringmail Chest)
    ShrugReflectStun,            // a fully shrugged blow reflects P1% back and briefly stuns (Plate Arms)

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
    DodgeRefundStamSuitWeight, // a dodge converts armor burden into vigor: refunds (worn armor weight / P1) stamina (Daphne). Re-spec 2026-07-11 of the never-consumed WeightReductionSuiteBurstOnDodge burst — ClauseType is not serialized, so the rename is save-safe.

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
    DodgeSnare,                      // a successful dodge webs the attacker: -P1% swing speed for P2s (Penelope)

    // Hat-bound clothing relics (family 11 — displacing-cloth cycle, 21-clothing.md §3)
    HealsReceivedBonusPct,           // +P1% to all healing received while worn (Diadema)
    StationaryRegenFaster,           // the hearth-calm settles after P1s instead of 10s (Kalyptra)

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

// Data-driven legendary registry. Ids are globally unique and never reused (framework §10). The
// 270 entries now live per-family in Families/*.cs; FamilyRegistry aggregates them (id-ascending)
// and this class is the array-backed lookup facade. The family/slot constants stay here because
// the family definitions and content code reference them.
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
    // The five body relics (0-4) plus the five hat relics (8-11, one shape shared) — indices match
    // ClothingFamily.Factories order exactly.
    public const byte ClothingPieceBodySash = 0;
    public const byte ClothingPieceFancyShirt = 1;
    public const byte ClothingPieceKilt = 2;
    public const byte ClothingPieceRobe = 3;
    public const byte ClothingPieceCloak = 4;
    public const byte ClothingPieceStrawHat = 8;
    public const byte ClothingPieceWideBrimHat = 9;
    public const byte ClothingPieceFeatheredHat = 10;
    public const byte ClothingPieceCap = 11;

    // Jewelry BaseIndex — the slot every jewelry legendary is bound to (20-jewelry.md §1).
    public const byte JewelrySlotRing = 0;
    public const byte JewelrySlotBracelet = 1;
    public const byte JewelrySlotNecklace = 2;
    public const byte JewelrySlotEarrings = 3;

    private static readonly LegendaryEntry[] _entries = FamilyRegistry.Legendaries;

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
