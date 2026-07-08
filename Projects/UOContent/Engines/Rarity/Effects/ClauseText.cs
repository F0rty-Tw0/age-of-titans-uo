namespace Server.Engines.Rarity;

// Player-facing "unique clause" tooltip text for every ClauseType (framework §6 OPL order:
// name -> rarity line -> theme effects -> unique clause). Cold-path display code, called once
// per OPL rebuild from RarityEffects.AddVariantProperties — plain string interpolation returning
// a built string is fine here (mirrors BuildWeaponSummary/BuildArmorSummary/BuildAccessorySummary).
public static class ClauseText
{
    // Graceful degradation for params whose engine code (RarityEffects.cs) substitutes a hidden
    // default when a row/legendary leaves them at 0 (the "p > 0 ? p : N" pattern) — this file has
    // no access to that engine-side default, so it prints a neutral placeholder instead of "0".
    private static string PctOr(short p) => p > 0 ? $"{p}%" : "some";
    private static string SecsOr(short p) => p > 0 ? $"{p}s" : "a while";
    private static string AmountOr(short p) => p > 0 ? p.ToString() : "some";
    private static string Element(short kind) => kind == 1 ? "fire" : "lightning";
    private static string FirstHitAddendum(short p3) => p3 == 1 ? ", plus the first hit of every fight" : "";

    public static string Describe(ClauseType clause, short p1, short p2, short p3) => clause switch
    {
        ClauseType.None => null,

        // Extra-swing family (Zephyr)
        ClauseType.ExtraSwingEveryN => $"every {p1}th hit grants an extra swing",
        ClauseType.ExtraSwingOnParry => "parrying a hit grants an extra swing",
        ClauseType.ExtraSwingFirstHit => "the first hit of every fight grants an extra swing",
        ClauseType.DoubleStrikeEveryN => $"every {p1}th hit becomes a double strike; the second strike always crits",

        // Crit family (Phobos)
        ClauseType.CritFirstHit => p2 > 0
            ? $"the first hit of every fight is a guaranteed crit, splashing {p2}% to up to {p3} targets"
            : "the first hit of every fight is a guaranteed crit",
        ClauseType.CritEveryN => $"every {p1}th hit is a guaranteed crit",
        ClauseType.CritSplash => $"every {p1}th hit crits and splashes {p2}% damage to up to {p3} nearby targets",
        ClauseType.CritArmorPen => $"every {p1}th hit crits and ignores {p2}% of the target's armor",
        ClauseType.CritExecuteUnder15 => $"every {p1}th hit crits; crits deal double damage vs targets under 15% health",

        // Mark family (Agrotera)
        ClauseType.MarkFirstHit => "the first hit of every fight marks the target for bonus damage",
        ClauseType.MarkOnCrit => "landing a crit marks the target for bonus damage",
        ClauseType.MarkAllSources25 => $"a mark also raises damage taken from all sources by {p1}%",
        ClauseType.MarkSpreadOnDeath => $"a mark spreads to up to {p1} nearby allies when the target dies",
        ClauseType.MarkNearbyAllies => $"a mark also spreads to up to {p1} nearby allies at half bonus",
        ClauseType.MarkHealBlock => $"landing a crit marks the target and heal-blocks it for {SecsOr(p1)}",

        // Defense family (Pallas)
        ClauseType.BlockFirstHit => "the first hit taken each fight is always blocked",
        ClauseType.BlockCritStun => "blocking a crit briefly stuns the attacker",
        ClauseType.ReflectFirstHit => $"the first hit taken each fight reflects {p1}% damage back and staggers the attacker",

        // Drain family (Stygian)
        ClauseType.LifestealOnCrit => "crits heal the wielder for a portion of the damage dealt",
        ClauseType.OnKillRestore => p1 >= 2 ? "on-kill: restores stamina and mana" : "on-kill: restores stamina",
        ClauseType.StamDrainOnCrit => "crits drain the target's stamina",
        ClauseType.PoisonTickDoubled => "the first hit of every fight marks the target; poison ticks vs a mark are doubled",

        // ---- P2 additions: weapon families 1-6 (swords/polearms/maces/staves/fencing/archery) ----

        // Extra-swing riders (Zephyr lines)
        ClauseType.ExtraSwingSplash => $"every {p1}th hit grants an extra swing that splashes {p2}% to up to {p3} targets",
        ClauseType.ExtraSwingGuaranteedHit => $"every {p1}th hit grants an extra swing that always lands",
        ClauseType.ExtraSwingManaLeech => $"every {p1}th hit grants an extra swing that leeches {p2}% of the target's mana",
        ClauseType.ExtraSwingElemental => p1 > 0
            ? $"every {p1}th hit grants an extra swing with a {Element(p2)} proc"
            : $"the first hit of every fight grants an extra swing with a {Element(p2)} proc",
        ClauseType.ExtraSwingHealBlock => $"every {p1}th hit grants an extra swing that heal-blocks the target for {SecsOr(p2)}",
        ClauseType.ExtraSwingStackingHit => $"the first hit grants an extra swing; each later swing gains +{p1}% hit chance, up to {p2}%",

        // Crit riders (Phobos / caster lines)
        ClauseType.CritPoisonTick => $"every {p1}th hit crits and applies a poison tick",
        ClauseType.CritStagger => $"every {p1}th hit crits and briefly staggers the target",
        ClauseType.CritElemental => p1 > 0
            ? $"every {p1}th hit crits and procs {Element(p2)}{FirstHitAddendum(p3)}"
            : $"natural crits proc {Element(p2)}{FirstHitAddendum(p3)}",
        ClauseType.CritManaLeech => p1 > 0
            ? $"every {p1}th hit crits and leeches {PctOr(p2)} mana{FirstHitAddendum(p3)}"
            : $"natural crits leech {PctOr(p2)} mana{FirstHitAddendum(p3)}",
        ClauseType.CritHealBlock => p1 > 0
            ? $"every {p1}th hit crits and heal-blocks the target for {SecsOr(p2)}{FirstHitAddendum(p3)}"
            : $"natural crits heal-block the target for {SecsOr(p2)}{FirstHitAddendum(p3)}",
        ClauseType.CritFullHpDouble => p1 > 0
            ? $"every {p1}th hit crits and deals double damage vs full-health targets{FirstHitAddendum(p3)}"
            : $"natural crits deal double damage vs full-health targets{FirstHitAddendum(p3)}",

        // Mark riders (Agrotera caster lines)
        ClauseType.MarkManaLeech => $"the first hit marks the target; each hit vs the mark leeches {p1}% mana",
        ClauseType.MarkElemental => $"the first hit of every fight marks the target with a {Element(p1)} proc",
        ClauseType.MarkHealBlockFirstHit => $"the first hit of every fight marks the target and heal-blocks it for {SecsOr(p1)}",

        // Block / reflect riders (Pallas lines)
        ClauseType.BlockRestoreStam => $"the first hit taken each fight is blocked, restoring {p1}% max stamina",
        ClauseType.BlockDrainStam => $"the first hit taken each fight is blocked, draining {AmountOr(p1)} stamina from the attacker",
        ClauseType.BlockManaLeech => $"the first hit taken each fight is blocked, leeching {p1}% mana from the attacker",
        ClauseType.BlockElemental => $"the first hit taken each fight is blocked, firing a {Element(p1)} proc back at the attacker",
        ClauseType.BlockNextShotCrit => "the first hit taken each fight is blocked, and the next hit is a guaranteed crit",
        ClauseType.ReflectHealBlock => $"the first hit taken reflects {p1}% damage and heal-blocks the attacker for {SecsOr(p2)}",

        // ---- P3a additions: armor + shields (families 7-9) ----------------------------------

        // Polias/Aegis shrug riders (metal + light armor)
        ClauseType.ShrugStunAttacker => "a fully shrugged blow briefly stuns the attacker",
        ClauseType.ShrugReflect => $"a fully shrugged blow reflects {p1}% of its damage back",
        ClauseType.ShrugFirstHitGuaranteed => "the first hit taken each fight is always shrugged",

        // Cyclopean flame-proc riders
        ClauseType.FlameProcDoubleFirstHit => "the flame proc chance doubles vs the first hit of any fight",
        ClauseType.FlameProcHealBlock => $"the flame proc also heal-blocks the attacker for {SecsOr(p1)}",
        ClauseType.FlameProcBoostLowHp => $"the flame proc chance rises to {PctOr(p1)} while under {p2}% health",
        ClauseType.FlameProcPoison => "the flame proc also applies a poison tick to the attacker",
        ClauseType.FlameProcSplash => $"the flame proc also splashes to {p1} extra nearby attackers",
        ClauseType.FlameProcDoubleLowDurability => $"the flame proc chance doubles while the shield is under {p1}% durability",
        ClauseType.FlameProcEveryN => $"every {p1}th hit taken guarantees a flame proc",

        // Paean mending riders
        ClauseType.AutoCureRestoresStamMana => $"the auto-cure tick also restores {PctOr(p1)} stamina and mana",
        ClauseType.OnKillRestoreMissingHpPct => $"on-kill: restores {PctOr(p1)} of missing health",
        ClauseType.AutoCureClearsDebuffsOnce => "the auto-cure tick also clears mark and heal-block, once per fight",
        ClauseType.AutoCureRestoresHpPct => $"the auto-cure tick also restores {PctOr(p1)} missing health",
        ClauseType.LowHpEmergencyCure => $"once per fight below {p1}% health: a free cure and {PctOr(p2)} health restored",
        ClauseType.EmergencyRegenTick => $"once per fight, a hit dropping the wearer under {p1}% health fires a full regen tick",

        // Tritonian ward riders
        ClauseType.ParaResistBoostsSpellDr => $"resisting a paralyze raises spell resist to {p1}% for {SecsOr(p2)}",
        ClauseType.ParaResistStunsAttacker => "a resisted paralyze or stun briefly stuns the attacker instead",
        ClauseType.ResistSkillDoubleLowHp => $"the resisting spells bonus becomes {p1} while under {p2}% health",
        ClauseType.ResistSkillBoostLowHp => $"+{p1} additional resisting spells while under {p2}% health",
        ClauseType.FirstParaAutoFails => "the first paralyze or stun attempt of any fight automatically fails",
        ClauseType.SpellDrVsPoisonDot => "spell resist also reduces poison damage-over-time ticks",
        ClauseType.RerollFirstResist => "the first failed paralyze or stun resist roll each fight rerolls once",
        ClauseType.SpellDrBoostFirstHit => $"spell resist rises to {p1}% vs the first spell hit of any fight",
        ClauseType.SpellDrBurstOnCritTaken => $"spell resist doubles for {SecsOr(p1)} after taking a crit",
        ClauseType.ParaResistBoostsResistSkill => $"resisting a paralyze grants +{p1} resisting spells for {SecsOr(p2)}",
        ClauseType.FirstHitNoSecondaryEffect => "the first hit taken each fight applies no secondary effect",

        // Talarian stride riders
        ClauseType.DodgeRefundStam => $"a dodge refunds {p1} stamina",
        ClauseType.OnKillDodgeDoubleDuration => $"on kill, dodge chance doubles for {SecsOr(p1)}",
        ClauseType.DodgeRestoreMana => $"a successful dodge also restores {p1}% mana",
        ClauseType.DodgeDoubleFirstAttack => "dodge chance doubles vs the first attack of any fight",
        ClauseType.DodgeRegenBurst => $"a successful dodge grants +{p1}% stamina regen for {SecsOr(p2)}",
        ClauseType.WeightReductionSuiteBurstOnDodge => $"weight reduction applies suit-wide for {SecsOr(p1)} after a dodge",

        // Aegis shield-parry riders
        ClauseType.ParryFirstHitGuaranteed => "guaranteed parry vs the first hit of any fight",
        ClauseType.ParryCritStun => "parrying a crit briefly stuns the attacker",
        ClauseType.ParryExtraReflect => $"every parry reflects an extra {p1}% on top of thorns",
        ClauseType.ParryRepairsEveryN => $"every {p1}th parry repairs 1 durability point",
        ClauseType.LowHpGuaranteedParry => $"while under {p1}% health, the first hit taken is guaranteed-parried",
        ClauseType.ParryFirstHitGuaranteedStun => "guaranteed parry and stun vs the first hit of any fight",

        // Shield-only riders (Cyclopean/Paean/Tritonian/Talarian on a shield)
        ClauseType.ReflectBoostFirstHit => $"reflect rises to {p1}% vs the first hit of any fight",
        ClauseType.SelfRepairBurstOnCritBlock => $"self-repair rate doubles for {SecsOr(p1)} after blocking a crit",
        ClauseType.SelfRepairRestoresHp => $"each self-repair tick also restores {PctOr(p1)} max health",
        ClauseType.ReflectCritStun => "reflect damage from a blocked crit briefly stuns the attacker",
        ClauseType.HpRegenBurstOnCritTaken => $"health regen rate triples for {SecsOr(p1)} after taking a crit",
        ClauseType.OnKillRestoreExtraHp => $"on the wearer's first kill each fight, restores an extra {PctOr(p1)} max health",
        ClauseType.HealBlockOnFirstHitLanded => $"the first hit landed each fight heal-blocks its target for {SecsOr(p1)}",
        ClauseType.ManaRegenMirrorsHp => "mana regen rate gains the same bonus as health regen",
        ClauseType.StamRegenMirrorsHp => "stamina regen rate gains the same bonus as health regen",
        ClauseType.OnKillRestoreHpPct => $"on-kill: restores {PctOr(p1)} of max health",
        ClauseType.StamRegenMirrorsManaHalf => "stamina regen ticks also restore mana",
        ClauseType.OnKillStamRestoreExtendImmunity => $"on-kill: full stamina restore and extends stun immunity by {SecsOr(p1)}",

        // ---- P3b additions: jewelry (family 10) + clothing (family 11) ----------------------

        // Olympian riders (jewelry — lightning proc on the wearer's own melee hits)
        ClauseType.LightningProcRefundStam => $"the lightning proc also refunds {AmountOr(p1)} stamina",
        ClauseType.LightningProcChanceRestoreMana => $"the lightning proc has a {PctOr(p1)} chance to also restore {AmountOr(p2)} mana",
        ClauseType.StatBonusSplashSecondStat => "the rolled stat bonus also applies at half value to a second stat",
        ClauseType.LightningProcResistBurst => $"the lightning proc grants +{p1}% spell resist for {SecsOr(p2)}",

        // Hecatean riders (jewelry — mana leech on spell damage)
        ClauseType.ManaLeechRestoresStam => $"the mana leech proc also restores {AmountOr(p1)} stamina",
        ClauseType.OnKillFullManaRestore => "on-kill: full mana restore",
        ClauseType.ManaRegenDoubleLowMana => $"mana regen bonus doubles while under {PctOr(p1)} mana",
        ClauseType.ManaLeechResistBurst => $"the mana leech proc grants +{p1}% spell resist for {SecsOr(p2)}",

        // Tychean riders (jewelry — incoming hit halved / attacker miss-reroll)
        ClauseType.HitHalvedRegenPulse => $"a halved hit grants a {SecsOr(p1)} health/stamina/mana regen pulse",
        ClauseType.MissRerollGrazeRestoreStam => $"a re-rolled miss that fails again still restores {AmountOr(p1)} stamina",
        ClauseType.HitHalvedDurabilityImmunity => $"no durability loss for {SecsOr(p1)} after a halved hit",
        ClauseType.HitHalvedResistBurst => $"a halved hit grants +{p1}% spell resist for {SecsOr(p2)}",

        // Nyxian riders (jewelry — hide/stealth)
        ClauseType.StealthBreakRefundStam => "opening a fight from stealth refunds the swing's stamina cost",
        ClauseType.RegenDoubleWhileHidden => "health/stamina/mana regen doubles while hidden",
        ClauseType.HideRestoresMana => $"successfully hiding restores {AmountOr(p1)} mana",
        ClauseType.PoisonResistDoubleWhileHidden => "poison resist bonus doubles while hidden",

        // Demetrian riders (jewelry — potions)
        ClauseType.PotionRestoresStam => $"potions also restore {p1} stamina",
        ClauseType.RegenDoubleAfterPotion => $"regen bonus doubles for {SecsOr(p1)} after drinking any potion",
        ClauseType.PotionRestoresMana => $"potions also restore {p1} mana",
        ClauseType.OnKillTriggerHeldPotion => "on-kill: instantly gain the effect of the strongest held heal potion",

        // Clothing relics (family 11 — one per theme god, each bound to one piece shape)
        ClauseType.OnKillStamRegenBurstStacking => $"on-kill: +{p1}% stamina regen for {SecsOr(p2)}, stacking once on a second kill",
        ClauseType.AnimalTamingSkillBonus => $"+{p1} animal taming while worn",
        ClauseType.FrenzyStaggerChance => $"while frenzied, hits carry a {p1}% chance to stagger the target",
        ClauseType.DurabilityLossImmunity => "immune to durability loss while worn",
        ClauseType.DodgeReflectDamage => $"a successful dodge reflects {p1}% of the avoided damage",

        // ---- P2 (re-theme 2026-07-07): per-family Epic signature clauses ---------------------
        ClauseType.RampMaxStacksSplash => $"when a consecutive-hit ramp maxes out, splashes {p1}% to {p2} targets",
        ClauseType.OnKillFullStamNextHitCrit => $"on-kill: full stamina and the next hit crits within {SecsOr(p1)}",
        ClauseType.PoisonedTakeBonusDamage => $"hits deal +{p1}% damage vs a poisoned target",
        ClauseType.BlockGrantsDrBurst => $"after a block, gain +{p1}% damage reduction for {SecsOr(p2)}",
        ClauseType.NthHitSplash => $"every {p1}th hit splashes {p2}% to {p3} targets",
        ClauseType.NthHitFullArmorPen => $"every {p1}th hit ignores armor entirely",
        ClauseType.ExtraSwingChain => $"every {p1}th hit grants an extra swing that may itself chain once more",
        ClauseType.CritFirstHitStamRefund => "the first hit of every fight is a guaranteed crit and refunds its swing's stamina cost",
        ClauseType.PoisonedTargetsMarked => $"targets the wielder poisons are marked, taking +{p1}% damage",
        ClauseType.DodgeGrantsCounterWindow => $"for {SecsOr(p1)} after a dodge, the wielder's next swing crits",
        ClauseType.ParryRestoresStam => $"a successful parry restores {p1}% max stamina",

        _ => null
    };
}
