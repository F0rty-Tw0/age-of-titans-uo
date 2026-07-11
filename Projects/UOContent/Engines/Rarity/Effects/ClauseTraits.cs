using System.Collections.Generic;

namespace Server.Engines.Rarity;

// Which combat/worn trigger category each ClauseType participates in. This is the declarative
// counterpart to the hand-written dispatch switches scattered across RarityEffects.*.cs and
// WornEffectState.cs: a clause added to the ClauseType enum but forgotten at its dispatch site
// compiles clean and silently does nothing today, so ClauseDispatchCoverageTests cross-checks
// this table against the "HandledBy*" lists kept beside each switch and fails loudly on drift.
//
// A clause may fire at several sites, so the flags compose ([Flags]). Every ClauseType except
// None has an entry with at least one flag. `Deferred` is the honest label for a clause that is
// declared and tooltip-visible but has NO live dispatch yet (a known gap, see the four members
// below) — it is mutually exclusive with the dispatch flags.
[System.Flags]
public enum ClauseTrigger : uint
{
    None = 0,

    // Weapon offense
    WeaponHitArm    = 1u << 0,  // crit/extra-swing arming + hit damage/armor-pen (RarityEffects.WeaponHit.cs)
    PostHitProc     = 1u << 1,  // post-damage proc switch (RunClauseProcs, RarityEffects.Procs.cs)
    MarkRider       = 1u << 2,  // mark application/riders + on-death spread (Procs.cs / Hooks.cs)
    ExtraSwingRider = 1u << 3,  // rides the granted extra swing (ApplyExtraSwingRider / DoExtraSwing)
    Dodge           = 1u << 4,  // hit-chance dodge + on-dodge riders (AdjustHitChance / OnMeleeMiss)

    // Defense taken
    WeaponBlock     = 1u << 5,  // defender's weapon block/reflect (AbsorbForDefender)
    ArmorDefense    = 1u << 6,  // worn armor shrug/reflect/flame on a hit taken (AbsorbForDefenderArmor)
    ShieldParry     = 1u << 7,  // shield parry chance + on-parry riders (AdjustShieldParryChance / OnShieldParried)
    ArmorHitRider   = 1u << 8,  // worn armor riders keyed off a landed hit (ApplyArmorHitRiders)

    // Worn / periodic / lifecycle
    RegenTick       = 1u << 9,  // HP/stam/mana regen hooks + tick side-effects (RarityEffects.Worn.cs)
    SpellDr         = 1u << 10, // spell-DR query + poison-DoT DR enabler (Hooks.cs)
    ParaResist      = 1u << 11, // paralyze/stun resist (TryResistParalyze)
    OnKill          = 1u << 12, // on-kill riders (weapon-side + LastKiller-driven worn/jewelry/clothing)
    Potion          = 1u << 13, // potion-heal riders (AdjustPotionHeal)
    Hide            = 1u << 14, // on-successful-hide (OnSuccessfulHide)
    MissReroll      = 1u << 15, // miss-reroll graze (OnMissRerollFailed)
    PoisonResist    = 1u << 16, // poison-application resist (TryResistPoisonApplication)
    SpellManaLeech  = 1u << 17, // Hecatean spell mana-leech riders (ApplyHecateanSpellManaLeech)
    LightningProc   = 1u << 18, // Olympian lightning-proc riders (ApplyOlympianLightningProc)
    WornStatMod     = 1u << 19, // stat/skill mods folded in on Rebuild (WornEffectState.cs)
    DurabilityLoss  = 1u << 20, // durability-loss-immunity query on the wear path (Items/Clothing/BaseClothing.cs OnHit)

    // Declared but not dispatched anywhere yet — a known gap, not a category.
    Deferred        = 1u << 21
}

// ClauseType -> the trigger categories it is dispatched at. See ClauseTraits.cs header.
public static class ClauseTraits
{
    private const ClauseTrigger DispatchMask = (ClauseTrigger)((1u << 21) - 1); // every flag except Deferred

    // Alias so the (necessarily long) table below stays scannable one clause per line.
    private static readonly IReadOnlyDictionary<ClauseType, ClauseTrigger> _traits = Build();

    public static ClauseTrigger Get(ClauseType clause) =>
        _traits.TryGetValue(clause, out var t) ? t : ClauseTrigger.None;

    public static bool IsDeferred(ClauseType clause) => (Get(clause) & ClauseTrigger.Deferred) != 0;

    public static bool HasDispatch(ClauseType clause) => (Get(clause) & DispatchMask) != 0;

    public static IReadOnlyDictionary<ClauseType, ClauseTrigger> All => _traits;

    private static Dictionary<ClauseType, ClauseTrigger> Build()
    {
        // One line per clause; every ClauseType except None appears exactly once.
        return new Dictionary<ClauseType, ClauseTrigger>
        {
            [ClauseType.ExtraSwingEveryN]                  = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.ExtraSwingOnParry]                 = ClauseTrigger.Deferred,
            [ClauseType.ExtraSwingFirstHit]                = ClauseTrigger.WeaponHitArm,
            [ClauseType.DoubleStrikeEveryN]                = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.CritFirstHit]                      = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritEveryN]                        = ClauseTrigger.WeaponHitArm,
            [ClauseType.CritSplash]                        = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritArmorPen]                      = ClauseTrigger.WeaponHitArm,
            [ClauseType.CritExecuteUnder15]                = ClauseTrigger.WeaponHitArm,
            [ClauseType.MarkFirstHit]                      = ClauseTrigger.MarkRider,
            [ClauseType.MarkOnCrit]                        = ClauseTrigger.MarkRider,
            [ClauseType.MarkAllSources25]                  = ClauseTrigger.MarkRider,
            [ClauseType.MarkSpreadOnDeath]                 = ClauseTrigger.MarkRider,
            [ClauseType.MarkNearbyAllies]                  = ClauseTrigger.MarkRider,
            [ClauseType.MarkHealBlock]                     = ClauseTrigger.MarkRider,
            [ClauseType.BlockFirstHit]                     = ClauseTrigger.WeaponBlock | ClauseTrigger.ShieldParry,
            [ClauseType.BlockCritStun]                     = ClauseTrigger.Deferred,
            [ClauseType.ReflectFirstHit]                   = ClauseTrigger.WeaponBlock,
            [ClauseType.LifestealOnCrit]                   = ClauseTrigger.PostHitProc,
            [ClauseType.OnKillRestore]                     = ClauseTrigger.PostHitProc,
            [ClauseType.StamDrainOnCrit]                   = ClauseTrigger.PostHitProc,
            [ClauseType.PoisonTickDoubled]                 = ClauseTrigger.MarkRider,
            [ClauseType.ExtraSwingSplash]                  = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.ExtraSwingGuaranteedHit]           = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.ExtraSwingManaLeech]               = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.ExtraSwingElemental]               = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.ExtraSwingHealBlock]               = ClauseTrigger.WeaponHitArm | ClauseTrigger.ExtraSwingRider,
            [ClauseType.ExtraSwingStackingHit]             = ClauseTrigger.WeaponHitArm,
            [ClauseType.CritPoisonTick]                    = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritStagger]                       = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritElemental]                     = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritManaLeech]                     = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritHealBlock]                     = ClauseTrigger.WeaponHitArm | ClauseTrigger.PostHitProc,
            [ClauseType.CritFullHpDouble]                  = ClauseTrigger.WeaponHitArm,
            [ClauseType.MarkManaLeech]                     = ClauseTrigger.PostHitProc | ClauseTrigger.MarkRider,
            [ClauseType.MarkElemental]                     = ClauseTrigger.MarkRider,
            [ClauseType.MarkHealBlockFirstHit]             = ClauseTrigger.MarkRider,
            [ClauseType.BlockRestoreStam]                  = ClauseTrigger.WeaponBlock | ClauseTrigger.ShieldParry,
            [ClauseType.BlockDrainStam]                    = ClauseTrigger.WeaponBlock | ClauseTrigger.ShieldParry,
            [ClauseType.BlockManaLeech]                    = ClauseTrigger.WeaponBlock | ClauseTrigger.ShieldParry,
            [ClauseType.BlockElemental]                    = ClauseTrigger.WeaponBlock | ClauseTrigger.ShieldParry,
            [ClauseType.BlockNextShotCrit]                 = ClauseTrigger.WeaponBlock | ClauseTrigger.ShieldParry,
            [ClauseType.ReflectHealBlock]                  = ClauseTrigger.WeaponBlock,
            [ClauseType.ShrugStunAttacker]                 = ClauseTrigger.ArmorDefense,
            [ClauseType.ShrugReflect]                      = ClauseTrigger.ArmorDefense,
            [ClauseType.ShrugFirstHitGuaranteed]           = ClauseTrigger.ArmorDefense,
            [ClauseType.ShrugFirstHitPoisonAttacker]       = ClauseTrigger.ArmorDefense,
            [ClauseType.ShrugFirstHitDrainStam]            = ClauseTrigger.ArmorDefense,
            [ClauseType.ShrugFirstHitDrBurst]              = ClauseTrigger.ArmorDefense,
            [ClauseType.ShrugReflectStun]                  = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcDoubleFirstHit]           = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcHealBlock]                = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcBoostLowHp]               = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcPoison]                   = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcSplash]                   = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcDoubleLowDurability]      = ClauseTrigger.ArmorDefense,
            [ClauseType.FlameProcEveryN]                   = ClauseTrigger.ArmorDefense,
            [ClauseType.AutoCureRestoresStamMana]          = ClauseTrigger.RegenTick,
            [ClauseType.OnKillRestoreMissingHpPct]         = ClauseTrigger.OnKill,
            [ClauseType.AutoCureClearsDebuffsOnce]         = ClauseTrigger.RegenTick,
            [ClauseType.AutoCureRestoresHpPct]             = ClauseTrigger.RegenTick,
            [ClauseType.LowHpEmergencyCure]                = ClauseTrigger.RegenTick,
            [ClauseType.EmergencyRegenTick]                = ClauseTrigger.ArmorDefense,
            [ClauseType.ParaResistBoostsSpellDr]           = ClauseTrigger.SpellDr | ClauseTrigger.ParaResist,
            [ClauseType.ParaResistStunsAttacker]           = ClauseTrigger.ParaResist,
            [ClauseType.ResistSkillDoubleLowHp]            = ClauseTrigger.RegenTick,
            [ClauseType.ResistSkillBoostLowHp]             = ClauseTrigger.RegenTick,
            [ClauseType.FirstParaAutoFails]                = ClauseTrigger.ParaResist,
            [ClauseType.SpellDrVsPoisonDot]                = ClauseTrigger.SpellDr,
            [ClauseType.RerollFirstResist]                 = ClauseTrigger.ParaResist,
            [ClauseType.SpellDrBoostFirstHit]              = ClauseTrigger.SpellDr,
            [ClauseType.SpellDrBurstOnCritTaken]           = ClauseTrigger.SpellDr | ClauseTrigger.ArmorHitRider,
            [ClauseType.ParaResistBoostsResistSkill]       = ClauseTrigger.RegenTick | ClauseTrigger.ParaResist,
            [ClauseType.FirstHitNoSecondaryEffect]         = ClauseTrigger.ShieldParry,
            [ClauseType.DodgeRefundStam]                   = ClauseTrigger.Dodge,
            [ClauseType.OnKillDodgeDoubleDuration]         = ClauseTrigger.OnKill,
            [ClauseType.DodgeRestoreMana]                  = ClauseTrigger.Dodge,
            [ClauseType.DodgeDoubleFirstAttack]            = ClauseTrigger.Dodge,
            [ClauseType.DodgeRegenBurst]                   = ClauseTrigger.RegenTick | ClauseTrigger.Dodge,
            [ClauseType.WeightReductionSuiteBurstOnDodge]  = ClauseTrigger.Dodge,
            [ClauseType.ParryFirstHitGuaranteed]           = ClauseTrigger.ShieldParry,
            [ClauseType.ParryCritStun]                     = ClauseTrigger.ShieldParry,
            [ClauseType.ParryExtraReflect]                 = ClauseTrigger.ShieldParry,
            [ClauseType.ParryRepairsEveryN]                = ClauseTrigger.ShieldParry,
            [ClauseType.LowHpGuaranteedParry]              = ClauseTrigger.ShieldParry,
            [ClauseType.ParryFirstHitGuaranteedStun]       = ClauseTrigger.ShieldParry,
            [ClauseType.ReflectBoostFirstHit]              = ClauseTrigger.ArmorDefense,
            [ClauseType.SelfRepairBurstOnCritBlock]        = ClauseTrigger.ShieldParry | ClauseTrigger.RegenTick,
            [ClauseType.SelfRepairRestoresHp]              = ClauseTrigger.RegenTick,
            [ClauseType.ReflectCritStun]                   = ClauseTrigger.ArmorDefense,
            [ClauseType.HpRegenBurstOnCritTaken]           = ClauseTrigger.RegenTick | ClauseTrigger.ArmorHitRider,
            [ClauseType.OnKillRestoreExtraHp]              = ClauseTrigger.OnKill,
            [ClauseType.HealBlockOnFirstHitLanded]         = ClauseTrigger.ArmorHitRider,
            [ClauseType.ManaRegenMirrorsHp]                = ClauseTrigger.RegenTick,
            [ClauseType.StamRegenMirrorsHp]                = ClauseTrigger.RegenTick,
            [ClauseType.OnKillRestoreHpPct]                = ClauseTrigger.OnKill,
            [ClauseType.StamRegenMirrorsManaHalf]          = ClauseTrigger.RegenTick,
            [ClauseType.OnKillStamRestoreExtendImmunity]   = ClauseTrigger.OnKill,
            [ClauseType.LightningProcRefundStam]           = ClauseTrigger.LightningProc,
            [ClauseType.LightningProcChanceRestoreMana]    = ClauseTrigger.LightningProc,
            [ClauseType.StatBonusSplashSecondStat]         = ClauseTrigger.WornStatMod,
            [ClauseType.LightningProcResistBurst]          = ClauseTrigger.SpellDr | ClauseTrigger.LightningProc,
            [ClauseType.ManaLeechRestoresStam]             = ClauseTrigger.SpellManaLeech,
            [ClauseType.OnKillFullManaRestore]             = ClauseTrigger.OnKill,
            [ClauseType.ManaRegenDoubleLowMana]            = ClauseTrigger.RegenTick,
            [ClauseType.ManaLeechResistBurst]              = ClauseTrigger.SpellDr | ClauseTrigger.SpellManaLeech,
            [ClauseType.HitHalvedRegenPulse]               = ClauseTrigger.ArmorDefense | ClauseTrigger.RegenTick,
            [ClauseType.MissRerollGrazeRestoreStam]        = ClauseTrigger.MissReroll,
            [ClauseType.HitHalvedDurabilityImmunity]       = ClauseTrigger.ArmorDefense,
            [ClauseType.HitHalvedResistBurst]              = ClauseTrigger.ArmorDefense | ClauseTrigger.SpellDr,
            [ClauseType.StealthBreakRefundStam]            = ClauseTrigger.Deferred,
            [ClauseType.RegenDoubleWhileHidden]            = ClauseTrigger.RegenTick,
            [ClauseType.HideRestoresMana]                  = ClauseTrigger.Hide,
            [ClauseType.PoisonResistDoubleWhileHidden]     = ClauseTrigger.PoisonResist,
            [ClauseType.PotionRestoresStam]                = ClauseTrigger.Potion,
            [ClauseType.RegenDoubleAfterPotion]            = ClauseTrigger.RegenTick | ClauseTrigger.Potion,
            [ClauseType.PotionRestoresMana]                = ClauseTrigger.Potion,
            [ClauseType.OnKillTriggerHeldPotion]           = ClauseTrigger.OnKill,
            [ClauseType.OnKillStamRegenBurstStacking]      = ClauseTrigger.RegenTick | ClauseTrigger.OnKill,
            [ClauseType.AnimalTamingSkillBonus]            = ClauseTrigger.WornStatMod,
            [ClauseType.FrenzyStaggerChance]               = ClauseTrigger.ArmorHitRider,
            [ClauseType.DurabilityLossImmunity]            = ClauseTrigger.DurabilityLoss,
            [ClauseType.DodgeSnare]                        = ClauseTrigger.Dodge,
            [ClauseType.HealsReceivedBonusPct]             = ClauseTrigger.WornStatMod,
            [ClauseType.StationaryRegenFaster]             = ClauseTrigger.RegenTick,
            [ClauseType.RampMaxStacksSplash]               = ClauseTrigger.WeaponHitArm,
            [ClauseType.OnKillFullStamNextHitCrit]         = ClauseTrigger.OnKill,
            [ClauseType.PoisonedTakeBonusDamage]           = ClauseTrigger.WeaponHitArm,
            [ClauseType.BlockGrantsDrBurst]                = ClauseTrigger.WeaponBlock,
            [ClauseType.NthHitSplash]                      = ClauseTrigger.WeaponHitArm,
            [ClauseType.NthHitFullArmorPen]                = ClauseTrigger.WeaponHitArm,
            [ClauseType.ExtraSwingChain]                   = ClauseTrigger.WeaponHitArm,
            [ClauseType.CritFirstHitStamRefund]            = ClauseTrigger.WeaponHitArm,
            [ClauseType.PoisonedTargetsMarked]             = ClauseTrigger.WeaponHitArm,
            [ClauseType.DodgeGrantsCounterWindow]          = ClauseTrigger.Dodge,
            [ClauseType.ParryRestoresStam]                 = ClauseTrigger.ShieldParry,
        };
    }
}
