namespace Server.Engines.Rarity;

// Player-facing "unique clause" tooltip text for every ClauseType (framework §6 OPL order:
// name -> rarity line -> theme effects -> unique clause). Cold-path display code, called once
// per OPL rebuild from RarityEffects.AddVariantProperties — plain string interpolation returning
// a built string is fine here (mirrors BuildWeaponSummary/BuildArmorSummary/BuildAccessorySummary).
public static class ClauseText
{
    // English ordinal for a cadence count ("1st", "2nd", "3rd", "4th"...) so "every 3rd hit" reads
    // correctly instead of "every 3th hit". Internal so RarityEffects.Tooltips shares it.
    internal static string Ord(int n)
    {
        var suffix = n % 100 is >= 11 and <= 13
            ? "th"
            : (n % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };

        return $"{n}{suffix}";
    }

    // Graceful degradation for params whose engine code (RarityEffects.cs) substitutes a hidden
    // default when a row/legendary leaves them at 0 (the "p > 0 ? p : N" pattern) — this file has
    // no access to that engine-side default, so it prints a neutral placeholder instead of "0".
    private static string PctOr(short p) => p > 0 ? $"{p}%" : "some";
    private static string SecsOr(short p) => p > 0 ? $"{p}s" : "a while";
    private static string AmountOr(short p) => p > 0 ? p.ToString() : "some";
    private static string Element(short kind) => kind == 1 ? "fire" : "lightning";
    private static string FirstHitAddendum(short p3) => p3 == 1 ? ", plus the first hit of every fight" : "";

    // Mark-family riders state the bonus damage a mark inflicts. The Tooltips call sites inject the
    // weapon row's MarkBonusPct into the clause's free p-slot before Describe runs, so `p` is that
    // value; a still-zero slot degrades to the old value-less phrasing.
    private static string MarkTaken(short p) => p > 0 ? $": +{p}% damage taken" : " for bonus damage";

    // Extra-swing clauses with no cadence N (p1 == 0) fall back to a natural-crit trigger (the engine
    // mirrors the crit-rider convention). `rider` describes what the additional strike also does. The
    // wording is weapon-agnostic ("strikes an additional time"), so bows read the same as blades.
    private static string ExtraSwingText(short p1, string rider) =>
        p1 > 0 ? $"every {Ord(p1)} hit strikes an additional time{rider}" : $"critical hits strike an additional time{rider}";

    // Ranged overload kept for call-site compatibility: all clause wording is now weapon-agnostic, so
    // ranged and melee read identically (no "swing"->"shot" rewrite).
    public static string Describe(ClauseType clause, short p1, short p2, short p3, bool ranged) =>
        Describe(clause, p1, p2, p3);

    public static string Describe(ClauseType clause, short p1, short p2, short p3) => clause switch
    {
        ClauseType.None => null,

        // Extra-swing family (Zephyr)
        ClauseType.ExtraSwingEveryN => ExtraSwingText(p1, ""),
        ClauseType.ExtraSwingOnParry => "parrying or dodging a hit answers with an immediate additional strike",
        ClauseType.ExtraSwingFirstHit => "the first hit of every fight strikes an additional time",
        ClauseType.DoubleStrikeEveryN => p1 > 0
            ? $"every {Ord(p1)} hit becomes a double strike; the second strike always crits"
            : "critical hits become a double strike; the second strike always crits",

        // Crit family (Phobos)
        ClauseType.CritFirstHit => p2 > 0
            ? $"the first hit of every fight is a guaranteed crit, splashing {p2}% to up to {p3} targets"
            : "the first hit of every fight is a guaranteed crit",
        ClauseType.CritEveryN => $"every {Ord(p1)} hit is a guaranteed crit",
        ClauseType.CritSplash => p1 > 0
            ? $"every {Ord(p1)} hit crits and splashes {p2}% damage to up to {p3} nearby targets"
            : $"critical hits splash {p2}% damage to up to {p3} nearby targets",
        ClauseType.CritArmorPen => p1 > 0
            ? $"every {Ord(p1)} hit crits and penetrates {p2}% of armor"
            : $"critical hits penetrate {p2}% of armor",
        ClauseType.CritExecuteUnder15 => p1 > 0
            ? $"every {Ord(p1)} hit crits, and crits deal double damage vs targets under {(p2 > 0 ? p2 : 15)}% health"
            : $"crits deal double damage vs targets under {(p2 > 0 ? p2 : 15)}% health",

        // Mark family (Agrotera)
        ClauseType.MarkFirstHit => $"the first hit of every fight marks the target{MarkTaken(p1)}",
        ClauseType.MarkOnCrit => $"landing a crit marks the target{MarkTaken(p1)}",
        ClauseType.MarkAllSources25 => $"a mark also raises damage taken from all sources by {p1}%",
        ClauseType.MarkSpreadOnDeath => $"a mark spreads to up to {p1} nearby allies when the target dies",
        ClauseType.MarkNearbyAllies => $"a mark also spreads to up to {p1} nearby allies at half bonus",
        ClauseType.MarkHealBlock => $"landing a crit marks the target and heal-blocks it for {SecsOr(p1)}",

        // Defense family (Pallas)
        ClauseType.BlockFirstHit => "the first hit taken each fight is always blocked",
        ClauseType.ReflectFirstHit => $"the first hit taken each fight reflects {p1}% damage back and staggers the attacker",

        // Drain family (Stygian)
        ClauseType.LifestealOnCrit => $"critical hits heal you for {PctOr(p1)} of the damage dealt",
        ClauseType.OnKillRestore => p1 >= 2 ? "on kill: restores full stamina and mana" : "on kill: restores full stamina",
        ClauseType.StamDrainOnCrit => "crits drain the target's stamina",
        ClauseType.PoisonTickDoubled => $"the first hit of every fight marks the target{MarkTaken(p1)} and poison ticks vs marked target are doubled",

        // ---- P2 additions: weapon families 1-6 (swords/polearms/maces/staves/fencing/archery) ----

        // Extra-swing riders (Zephyr lines)
        ClauseType.ExtraSwingSplash => ExtraSwingText(p1, $" that splashes {p2}% to up to {p3} targets"),
        ClauseType.ExtraSwingGuaranteedHit => ExtraSwingText(p1, " that always hits"),
        ClauseType.ExtraSwingManaLeech => ExtraSwingText(p1, $" that leeches {p2}% of the target's mana"),
        ClauseType.ExtraSwingElemental => p1 > 0
            ? $"every {Ord(p1)} hit strikes an additional time with a {Element(p2)} proc"
            : $"the first hit of every fight strikes an additional time with a {Element(p2)} proc",
        ClauseType.ExtraSwingHealBlock => ExtraSwingText(p1, $" that heal-blocks the target for {SecsOr(p2)}"),
        ClauseType.ExtraSwingStackingHit => $"the first hit strikes an additional time and each later hit gains +{p1}% chance to hit, up to {p2}%",

        // Crit riders (Phobos / caster lines)
        ClauseType.CritPoisonTick => p1 > 0
            ? $"every {Ord(p1)} hit crits and applies poison"
            : "critical hits apply poison",
        ClauseType.CritStagger => p1 > 0
            ? $"every {Ord(p1)} hit crits and briefly staggers the target"
            : "critical hits briefly stagger the target",
        ClauseType.CritElemental => p1 > 0
            ? $"every {Ord(p1)} hit crits and procs {Element(p2)}{FirstHitAddendum(p3)}"
            : $"critical hits proc {Element(p2)}{FirstHitAddendum(p3)}",
        ClauseType.CritManaLeech => p1 > 0
            ? $"every {Ord(p1)} hit crits and leeches {PctOr(p2)} mana{FirstHitAddendum(p3)}"
            : $"critical hits leech {PctOr(p2)} mana{FirstHitAddendum(p3)}",
        ClauseType.CritHealBlock => p1 > 0
            ? $"every {Ord(p1)} hit crits and heal-blocks the target for {SecsOr(p2)}{FirstHitAddendum(p3)}"
            : $"critical hits heal-block the target for {SecsOr(p2)}{FirstHitAddendum(p3)}",
        ClauseType.CritFullHpDouble => p1 > 0
            ? $"every {Ord(p1)} hit crits and deals double damage vs full-health targets{FirstHitAddendum(p3)}"
            : $"critical hits deal double damage vs full-health targets{FirstHitAddendum(p3)}",

        // Mark riders (Agrotera caster lines)
        ClauseType.MarkManaLeech => $"the first hit marks the target{MarkTaken(p2)} and each hit vs the mark leeches {p1}% mana",
        ClauseType.MarkElemental => $"the first hit of every fight marks the target{MarkTaken(p2)} with a {Element(p1)} proc",
        ClauseType.MarkHealBlockFirstHit => $"the first hit of every fight marks the target{MarkTaken(p2)} and heal-blocks it for {SecsOr(p1)}",

        // Block / reflect riders (Pallas lines)
        ClauseType.BlockRestoreStam => $"the first hit taken each fight is blocked, restoring {p1}% max stamina",
        ClauseType.BlockDrainStam => $"the first hit taken each fight is blocked, draining {AmountOr(p1)} stamina from the attacker",
        ClauseType.BlockManaLeech => $"the first hit taken each fight is blocked, leeching {p1}% mana from the attacker",
        ClauseType.BlockElemental => $"the first hit taken each fight is blocked, firing a {Element(p1)} proc back at the attacker",
        ClauseType.BlockNextShotCrit => "the first hit taken each fight is blocked, and your next hit is a guaranteed crit",
        ClauseType.ReflectHealBlock => $"the first hit taken reflects {p1}% damage and heal-blocks the attacker for {SecsOr(p2)}",

        // ---- P3a additions: armor + shields (families 7-9) ----------------------------------

        // Polias/Aegis shrug riders (metal + light armor). A shrug halves the hit (never negates
        // it fully — RarityEffects.Defense damage /= 2), so no "fully" qualifier.
        ClauseType.ShrugStunAttacker => "a shrugged hit briefly stuns the attacker",
        ClauseType.ShrugReflect => $"a shrugged hit reflects {p1}% of its damage back at the attacker",
        ClauseType.ShrugFirstHitGuaranteed => "the first hit taken each fight is always shrugged",

        // Option A slot-set signatures (armor-slotsets plan, Phase 3)
        ClauseType.ShrugFirstHitPoisonAttacker => "the first hit taken each fight is shrugged, poisoning the attacker",
        ClauseType.ShrugFirstHitDrainStam => $"the first hit taken each fight is shrugged, draining {AmountOr(p1)} stamina from the attacker",
        ClauseType.ShrugFirstHitDrBurst => $"the first hit taken each fight is shrugged, granting +{p1}% damage reduction for {SecsOr(p2)}",
        ClauseType.ShrugReflectStun => $"a shrugged hit reflects {p1}% of its damage back and briefly stuns the attacker",

        // Cyclopean flame-proc riders
        ClauseType.FlameProcDoubleFirstHit => "the flame proc chance doubles vs the first hit of any fight",
        ClauseType.FlameProcHealBlock => $"the flame proc also heal-blocks the attacker for {SecsOr(p1)}",
        ClauseType.FlameProcBoostLowHp => $"the flame proc chance rises to {PctOr(p1)} while under {p2}% health",
        ClauseType.FlameProcPoison => "the flame proc also applies a poison tick to the attacker",
        ClauseType.FlameProcSplash => $"the flame proc also splashes to {p1} extra nearby attackers",
        ClauseType.FlameProcEveryN => $"every {Ord(p1)} hit taken guarantees a flame proc",

        // Paean mending riders
        ClauseType.AutoCureRestoresStamMana => $"the auto-cure tick also restores {PctOr(p1)} stamina and mana",
        ClauseType.OnKillRestoreMissingHpPct => $"on kill: restores {PctOr(p1)} of missing health",
        ClauseType.AutoCureClearsDebuffsOnce => "the auto-cure tick also clears any marks and heal-blocks, once per fight",
        ClauseType.AutoCureRestoresHpPct => $"the auto-cure tick also restores {PctOr(p1)} missing health",
        ClauseType.LowHpEmergencyCure => $"once per fight when below {p1}% health: you are cured and {PctOr(p2)} health restored",
        ClauseType.EmergencyRegenTick => $"once per fight, a hit that would drop you below {p1}% health, restores 10% of your max health",

        // Tritonian ward riders
        // "to", not "by" — the engine takes Math.Max(current, P1), it does not add.
        ClauseType.ParaResistBoostsSpellDr => $"resisting a paralyze reduces spell damage taken by {p1}% for {SecsOr(p2)}",
        ClauseType.ParaResistStunsAttacker => "a resisted paralyze or stun briefly stuns the attacker instead",
        // These add their P1 on top of the base ResistSkillBonus (fully stacking, duplicates included).
        // Active at/below P2% health, drops at P2+1% and re-applies on the next crossing.
        ClauseType.ResistSkillDoubleLowHp => $"increases Magic Resistance by {p1} while under {p2}% health",
        ClauseType.ResistSkillBoostLowHp => $"+{p1} Magic Resistance while under {p2}% health",
        ClauseType.FirstParaAutoFails => "once per fight, completely resists the first paralyze or stun",
        ClauseType.SpellDrVsPoisonDot => "your spell damage reduction also applies to poison damage over time",
        ClauseType.RerollFirstResist => "reapplies the resisted paralyze or stun, once per fight",
        ClauseType.SpellDrBoostFirstHit => $"once per fight, reduces spell damage taken by {p1}% against the first spell",
        ClauseType.SpellDrBurstOnCritTaken => $"spell damage reduction doubles for {SecsOr(p1)} after taking a crit",
        ClauseType.ParaResistBoostsResistSkill => $"resisting a paralyze grants +{p1} Magic Resistance for {SecsOr(p2)}",
        ClauseType.DeflectSecondaryFirstHit => "the first mark, poison, or heal-block aimed at you is redirected onto the attacker",

        // Talarian stride riders
        ClauseType.DodgeRefundStam => $"a dodge refunds {p1} stamina",
        ClauseType.OnKillDodgeDoubleDuration => $"on kill: dodge chance doubles for {SecsOr(p1)}",
        ClauseType.DodgeRestoreMana => $"a successful dodge also restores {p1}% mana",
        ClauseType.DodgeDoubleFirstAttack => "dodge chance doubles vs the first attack of any fight",
        ClauseType.DodgeRegenBurst => $"a successful dodge grants +{p1}% stamina regen for {SecsOr(p2)}",
        ClauseType.DodgeRefundStamPct => $"a successful dodge refunds {(p1 > 0 ? p1 : 10)}% of your max stamina",

        // Aegis shield-parry riders
        ClauseType.ParryFirstHitGuaranteed => "guaranteed parry vs the first hit of any fight",
        ClauseType.ParryCritStun => "parrying a crit briefly stuns the attacker",
        ClauseType.ParryExtraReflect => $"every parry reflects an extra {p1}% of the damage back at the attacker",
        ClauseType.ParryForcesMissEveryN => $"every {Ord(p1)} parry makes the attacker to miss their next hit",
        ClauseType.LowHpGuaranteedParry => $"after falling below {p1}% health, the next hit taken is guaranteed-parried",
        ClauseType.ParryFirstHitGuaranteedStun => "guaranteed parry and stun vs the first hit of any fight",

        // Shield-only riders (Cyclopean/Paean/Tritonian/Talarian on a shield)
        ClauseType.ReflectBoostFirstHit => $"reflect increased by {p1}% against the first hit of a fight",
        ClauseType.ReflectBurstOnCritBlock => $"reflect doubles for {SecsOr(p1)} after blocking a crit",
        ClauseType.BlockRestoresHp => $"a block restores {PctOr(p1)} of your max health",
        ClauseType.ReflectCritStun => "reflect damage from a blocked crit briefly stuns the attacker",
        ClauseType.HpRegenBurstOnCritTaken => $"health regen rate triples for {SecsOr(p1)} after taking a crit",
        ClauseType.OnKillRestoreExtraHp => $"on kill: restores {PctOr(p1)} max health",
        ClauseType.HealBlockOnFirstHitLanded => $"the first hit landed each fight heal-blocks its target for {SecsOr(p1)}",
        ClauseType.ManaRegenMirrorsHp => "mana regen rate gains the same bonus as health regen",
        ClauseType.StamRegenMirrorsHp => "stamina regen rate gains the same bonus as health regen",
        ClauseType.OnKillRestoreHpPct => $"on kill: restores {PctOr(p1)} of max health",
        ClauseType.StamRegenMirrorsManaHalf => "stamina regen ticks also restore mana",
        ClauseType.OnKillStamRestoreExtendImmunity => $"on kill: restores full stamina and extends stun immunity by {SecsOr(p1)}",

        // ---- P3b additions: jewelry (family 10) + clothing (family 11) ----------------------

        // Olympian riders (jewelry — lightning proc on the wearer's own melee hits)
        ClauseType.LightningProcRefundStam => $"the lightning proc also refunds {AmountOr(p1)} stamina",
        ClauseType.LightningProcChanceRestoreMana => $"the lightning proc has a {PctOr(p1)} chance to also restore {AmountOr(p2)} mana",
        ClauseType.StatBonusSplashSecondStat => "the rolled stat bonus also applies at half value to a second stat",
        ClauseType.LightningProcResistBurst => $"the lightning proc reduces spell damage taken by {p1}% for {SecsOr(p2)}",

        // Hecatean riders (jewelry — mana leech on spell damage)
        ClauseType.ManaLeechRestoresStam => $"the mana leech proc also restores {AmountOr(p1)} stamina",
        ClauseType.OnKillFullManaRestore => "on kill: restores full mana",
        ClauseType.ManaRegenDoubleLowMana => $"mana regen bonus doubles while under {PctOr(p1)} mana",
        ClauseType.ManaLeechResistBurst => $"the mana leech proc reduces spell damage taken by {p1}% for {SecsOr(p2)}",

        // Tychean riders (jewelry — incoming hit halved / attacker miss-reroll)
        ClauseType.HitHalvedRegenPulse => $"shrugging a hit boosts your health, stamina, and mana regen by 20% for {SecsOr(p1)}",
        ClauseType.MissRerollGrazeRestoreStam => $"a rerolled miss that fails again still restores {AmountOr(p1)} stamina",
        ClauseType.HitHalvedReflectSpared => "a shrugged hit reflects the damage back at the attacker",
        ClauseType.HitHalvedResistBurst => $"shrugging a hit reduces spell damage taken by {p1}% for {SecsOr(p2)}",

        // Nyxian riders (jewelry — hide/stealth)
        ClauseType.StealthBreakRefundStam => "opening a fight from stealth refunds the hit stamina cost",
        ClauseType.RegenDoubleWhileHidden => "health/stamina/mana regen doubles while hidden",
        ClauseType.HideRestoresMana => $"successfully hiding restores {AmountOr(p1)} mana",
        ClauseType.PoisonResistDoubleWhileHidden => "poison resist bonus doubles while hidden",

        // Demetrian riders (jewelry — potions)
        ClauseType.PotionRestoresStam => $"potions also restore {p1} stamina",
        ClauseType.RegenDoubleAfterPotion => $"regen bonus doubles for {SecsOr(p1)} after drinking any potion",
        ClauseType.PotionRestoresMana => $"potions also restore {p1} mana",
        ClauseType.OnKillTriggerHeldPotion => "on kill: instantly gain the effect of the strongest heal potion in the backpack",

        // Clothing relics (family 11 — one per theme god, each bound to one piece shape)
        ClauseType.OnKillStamRegenBurstStacking => $"on kill: gain +{p1}% stamina regen for {SecsOr(p2)}, stacking once on a second kill",
        ClauseType.AnimalTamingSkillBonus => $"+{p1} animal taming while worn",
        ClauseType.FrenzyStaggerChance => $"while frenzied, hits have a {p1}% chance to stagger the target",
        ClauseType.LowHpDodgeBurst => $"once per fight below 25% health: gain +{p1}% dodge for {SecsOr(p2)}",
        ClauseType.DodgeSnare => $"a successful dodge webs the attacker, slowing their hits by {p1}% for {SecsOr(p2)}",

        // Hat-bound clothing relics (displacing-cloth cycle)
        ClauseType.HealsReceivedBonusPct => $"+{p1}% to all healing received while worn",
        ClauseType.StationaryRegenFaster => $"regen while standing still kicks in after {SecsOr(p1)} instead of 10s",

        // ---- P2 (re-theme 2026-07-07): per-family Epic signature clauses ---------------------
        ClauseType.RampMaxStacksSplash => $"once your consecutive-hit damage ramp is fully stacked, the hit also splashes {p1}% damage to {p2} nearby targets",
        ClauseType.OnKillFullStamNextHitCrit => $"on kill: restores full stamina and your next hit crits within {SecsOr(p1)}",
        ClauseType.PoisonedTakeBonusDamage => $"hits deal +{p1}% damage vs a poisoned target",
        ClauseType.BlockGrantsDrBurst => $"after a block, gain +{p1}% damage reduction for {SecsOr(p2)}",
        ClauseType.NthHitSplash => $"every {Ord(p1)} hit splashes {p2}% to {p3} targets",
        ClauseType.NthHitFullArmorPen => $"every {Ord(p1)} hit ignores all armor",
        ClauseType.ExtraSwingChain => ExtraSwingText(p1, " that may itself chain once more"),
        ClauseType.CritFirstHitStamRefund => "the first hit of every fight is a guaranteed crit and refunds its hit's stamina cost",
        ClauseType.PoisonedTargetsMarked => $"targets you poison are marked, taking +{p1}% damage",
        ClauseType.DodgeGrantsCounterWindow => $"for {SecsOr(p1)} after a dodge, your next hit crits",
        ClauseType.ParryRestoresStam => $"a successful parry restores {p1}% max stamina",

        _ => null
    };
}
