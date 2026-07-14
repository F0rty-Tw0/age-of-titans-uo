using System;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;
using Server.Collections;
using Server.Engines.Leveling;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Text;

namespace Server.Engines.Rarity;

public static partial class RarityEffects
{
    // Called from SpellHelper's Damage() family for hostile spell damage against `defender`.
    public static int ReduceSpellDamage(Mobile defender, int damage)
    {
        var pct = GetSpellDrPct(defender);
        return pct > 0 ? damage - damage * pct / 100 : damage;
    }

    // SpellDrVsPoisonDot (Skylla/Megareus legendaries, Dryas signature, Studded Helm cell): an
    // ENABLER clause — while worn, the wearer's spell DR also reduces poison damage-over-time.
    // Called from PoisonImpl.PoisonTimer on the merged tick total. With no spell DR from the
    // rest of the suit it correctly reduces nothing.
    // Coverage list for ReducePoisonTickDamage's SpellDrVsPoisonDot enabler check below.
    internal static readonly ClauseType[] HandledBySpellDrPoisonDot = { ClauseType.SpellDrVsPoisonDot };

    public static int ReducePoisonTickDamage(Mobile defender, int damage)
    {
        var legendaries = WornEffectState.GetLegendaries(defender);

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.SpellDrVsPoisonDot)
            {
                var pct = GetSpellDrPct(defender);
                return pct > 0 ? damage - damage * pct / 100 : damage;
            }
        }

        return damage;
    }

    // Hecatean: the caster's own spell-damage% bonus — applied at the SAME pre-AOS choke points
    // ReduceSpellDamage already uses (symmetric: boost outgoing before reducing for the target).
    public static int BoostSpellDamage(Mobile caster, int damage)
    {
        if (caster == null)
        {
            return damage;
        }

        var pct = WornEffectState.GetAggregate(caster).SpellDamagePct;
        return pct > 0 ? damage + damage * pct / 100 : damage;
    }

    // Hecatean: mana leech on spell damage — runs at the same choke points, right after the
    // damage lands.
    // Coverage list for ApplyHecateanSpellManaLeech's rider switch below.
    internal static readonly ClauseType[] HandledBySpellManaLeech =
        { ClauseType.ManaLeechRestoresStam, ClauseType.ManaLeechResistBurst };

    public static void ApplyHecateanSpellManaLeech(Mobile caster, Mobile target, int damageGiven)
    {
        if (caster == null || target == null || damageGiven <= 0)
        {
            return;
        }

        var agg = WornEffectState.GetAggregate(caster);

        if (agg.ManaLeechPct <= 0)
        {
            return;
        }

        LeechMana(target, caster, agg.ManaLeechPct);
        FloatingCombatText.ShowOffensiveStatus(target, caster, "Mana Drain");

        var legendaries = WornEffectState.GetLegendaries(caster);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.ManaLeechRestoresStam: // Selene
                    {
                        var before = caster.Stam;
                        caster.Stam = Math.Min(caster.StamMax, caster.Stam + (entry.P1 > 0 ? entry.P1 : 5));
                        FloatingCombatText.ShowRestore(caster, 'S', caster.Stam - before);
                        break;
                    }
                case ClauseType.ManaLeechResistBurst: // Theia
                    {
                        WornEffectState.ArmClauseBurst(caster, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 3));
                        FloatingCombatText.ShowSelfStatus(caster, "Warded");
                        break;
                    }
            }
        }
    }

    // Demetrian: healing-potion effect % + Gaia/Tethys flat stam/mana riders + Rhea's
    // regen-double burst. Called from BaseHealPotion.DoHeal — the single choke point shared by
    // every heal potion tier on this shard.
    // Coverage list for AdjustPotionHeal's rider switch below.
    internal static readonly ClauseType[] HandledByPotion =
        { ClauseType.PotionRestoresStam, ClauseType.PotionRestoresMana, ClauseType.RegenDoubleAfterPotion };

    public static int AdjustPotionHeal(Mobile from, int amount)
    {
        var agg = WornEffectState.GetAggregate(from);
        var boosted = agg.PotionEffectPct > 0 ? amount + amount * agg.PotionEffectPct / 100 : amount;

        var legendaries = WornEffectState.GetLegendaries(from);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.PotionRestoresStam: // Gaia
                    {
                        var before = from.Stam;
                        from.Stam = Math.Min(from.StamMax, from.Stam + (entry.P1 > 0 ? entry.P1 : 10));
                        FloatingCombatText.ShowRestore(from, 'S', from.Stam - before);
                        break;
                    }
                case ClauseType.PotionRestoresMana: // Tethys
                    {
                        var before = from.Mana;
                        from.Mana = Math.Min(from.ManaMax, from.Mana + (entry.P1 > 0 ? entry.P1 : 10));
                        FloatingCombatText.ShowRestore(from, 'M', from.Mana - before);
                        break;
                    }
                case ClauseType.RegenDoubleAfterPotion: // Rhea
                    {
                        WornEffectState.ArmClauseBurst(from, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 5));
                        break;
                    }
            }
        }

        return boosted;
    }

    // Charis: karma-gain %. Called from Titles.AwardKarma for positive offsets only ("karma
    // gained" — the doc doesn't extend this to karma losses).
    public static int AdjustKarmaGain(Mobile m, int offset)
    {
        if (offset <= 0)
        {
            return offset;
        }

        var pct = WornEffectState.GetAggregate(m).KarmaGainPct;
        return pct > 0 ? offset + offset * pct / 100 : offset;
    }

    // Charis: vendor prices % better. Called from BaseVendor's buy/sell choke points.
    public static int AdjustVendorBuyPrice(Mobile buyer, int totalCost)
    {
        var pct = WornEffectState.GetAggregate(buyer).VendorPricePct;
        return pct > 0 ? totalCost - totalCost * pct / 100 : totalCost;
    }

    public static int AdjustVendorSellPrice(Mobile seller, int giveGold)
    {
        var pct = WornEffectState.GetAggregate(seller).VendorPricePct;
        return pct > 0 ? giveGold + giveGold * pct / 100 : giveGold;
    }

    // Nyxian/Arachne: a % chance to fully shrug a poison application (Achlys doubles it while
    // hidden). Called from PlayerMobile/BaseCreature's existing CheckPoisonImmunity overrides.
    // Coverage list for TryResistPoisonApplication's PoisonResistDoubleWhileHidden check below.
    internal static readonly ClauseType[] HandledByPoisonResist = { ClauseType.PoisonResistDoubleWhileHidden };

    public static bool TryResistPoisonApplication(Mobile defender)
    {
        var agg = WornEffectState.GetAggregate(defender);

        if (agg.PoisonResistPct <= 0)
        {
            return false;
        }

        var pct = agg.PoisonResistPct;

        if (defender.Hidden)
        {
            var legendaries = WornEffectState.GetLegendaries(defender);

            for (var i = 0; i < legendaries.Count; i++)
            {
                if (legendaries[i].Clause == ClauseType.PoisonResistDoubleWhileHidden) // Achlys
                {
                    pct *= 2;
                    break;
                }
            }
        }

        var resisted = Utility.Random(100) < pct;

        if (resisted)
        {
            FloatingCombatText.ShowSelfStatus(defender, "Resisted");
        }

        return resisted;
    }

    // Moros: successfully hiding restores mana. Called from Skills/Hiding.cs's single "hide
    // succeeded" branch.
    // Coverage list for OnSuccessfulHide's HideRestoresMana check below.
    internal static readonly ClauseType[] HandledByHide = { ClauseType.HideRestoresMana };

    public static void OnSuccessfulHide(Mobile m)
    {
        var legendaries = WornEffectState.GetLegendaries(m);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.HideRestoresMana)
            {
                var before = m.Mana;
                m.Mana = Math.Min(m.ManaMax, m.Mana + (entry.P1 > 0 ? entry.P1 : 10));
                FloatingCombatText.ShowRestore(m, 'M', m.Mana - before);
            }
        }
    }

    // Tychean: a % chance the attacker's own missed swing re-rolls once — independent of the
    // attacker's weapon variant. Called from BaseWeapon.CheckHit only on an initial miss.
    public static bool TryRerollMiss(Mobile attacker)
    {
        var agg = WornEffectState.GetAggregate(attacker);
        var reroll = agg.MissRerollPct > 0 && Utility.Random(100) < agg.MissRerollPct;

        if (reroll)
        {
            FloatingCombatText.ShowSelfStatus(attacker, "Reroll");
        }

        return reroll;
    }

    // Metis: when the reroll itself also misses, the swing still counts as a guaranteed graze
    // that restores stamina rather than a total whiff.
    // Coverage list for OnMissRerollFailed's MissRerollGrazeRestoreStam check below.
    internal static readonly ClauseType[] HandledByMissReroll = { ClauseType.MissRerollGrazeRestoreStam };

    public static void OnMissRerollFailed(Mobile attacker)
    {
        var legendaries = WornEffectState.GetLegendaries(attacker);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.MissRerollGrazeRestoreStam)
            {
                var before = attacker.Stam;
                attacker.Stam = Math.Min(attacker.StamMax, attacker.Stam + (entry.P1 > 0 ? entry.P1 : 10));
                FloatingCombatText.ShowRestore(attacker, 'S', attacker.Stam - before);
            }
        }
    }

    // Coverage list for GetSpellDrPct's burst/boost switch below (the spell-DR aggregate query).
    internal static readonly ClauseType[] HandledBySpellDr =
    {
        ClauseType.SpellDrBurstOnCritTaken, ClauseType.SpellDrBoostFirstHit, ClauseType.ParaResistBoostsSpellDr,
        ClauseType.LightningProcResistBurst, ClauseType.ManaLeechResistBurst, ClauseType.HitHalvedResistBurst
    };

    private static int GetSpellDrPct(Mobile defender)
    {
        var agg = WornEffectState.GetAggregate(defender);
        var pct = agg.SpellDrPct;
        var legendaries = WornEffectState.GetLegendaries(defender);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.SpellDrBurstOnCritTaken && WornEffectState.IsClauseBurstActive(defender, entry.Clause))
            {
                pct *= 2; // Palaimon
            }
            else if (entry.Clause == ClauseType.SpellDrBoostFirstHit && CombatFxState.IsFightFreshForDefender(defender))
            {
                pct = Math.Max(pct, entry.P1); // Laomedon
            }
            else if (entry.Clause == ClauseType.ParaResistBoostsSpellDr && WornEffectState.IsClauseBurstActive(defender, entry.Clause))
            {
                pct = Math.Max(pct, entry.P1); // Nereus — spell DR rises for 5s after resisting a paralyze
            }
            // Jewelry riders that grant "+magic resist" all map onto this same spell-DR field —
            // there's no separate magic-resist mechanic on this shard to layer on top of.
            else if (entry.Clause == ClauseType.LightningProcResistBurst && WornEffectState.IsClauseBurstActive(defender, entry.Clause))
            {
                pct = Math.Max(pct, entry.P1); // Astraios
            }
            else if (entry.Clause == ClauseType.ManaLeechResistBurst && WornEffectState.IsClauseBurstActive(defender, entry.Clause))
            {
                pct = Math.Max(pct, entry.P1); // Theia
            }
            else if (entry.Clause == ClauseType.HitHalvedResistBurst && WornEffectState.IsClauseBurstActive(defender, entry.Clause))
            {
                pct = Math.Max(pct, entry.P1); // Themis
            }
        }

        return pct;
    }

    // Tritonian paralyze/stun resist. Returns true when the attempt is fully resisted (the
    // caller — ParalyzeSpell — then skips applying the paralysis). This is the only paralyze
    // effect live on this pre-AOS T2A shard, so "stun resist" in the design docs maps to it.
    // Coverage list for TryResistParalyze: the auto-fail/reroll checks + the resisted-para rider
    // switch below.
    internal static readonly ClauseType[] HandledByParaResist =
    {
        ClauseType.FirstParaAutoFails, ClauseType.RerollFirstResist, ClauseType.ParaResistStunsAttacker,
        ClauseType.ParaResistBoostsSpellDr, ClauseType.ParaResistBoostsResistSkill
    };

    public static bool TryResistParalyze(Mobile attacker, Mobile defender)
    {
        var agg = WornEffectState.GetAggregate(defender);
        var legendaries = WornEffectState.GetLegendaries(defender);

        if (agg.ParaResistPct <= 0 && legendaries.Count == 0)
        {
            return false;
        }

        var fightFresh = CombatFxState.IsFightFreshForDefender(defender);
        var autoFails = false;

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.FirstParaAutoFails && fightFresh)
            {
                autoFails = true; // Arethousa
            }
        }

        var resisted = autoFails || Utility.Random(100) < agg.ParaResistPct;

        if (!resisted)
        {
            for (var i = 0; i < legendaries.Count; i++)
            {
                if (legendaries[i].Clause == ClauseType.RerollFirstResist && fightFresh &&
                    WornEffectState.TryUseOncePerFight(defender, legendaries[i].Clause, fightFresh))
                {
                    resisted = Utility.Random(100) < agg.ParaResistPct; // Krommyon
                }
            }
        }

        if (!resisted)
        {
            return false;
        }

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.ParaResistStunsAttacker: // Proteus
                    {
                        if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
                        {
                            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
                        }

                        break;
                    }
                case ClauseType.ParaResistBoostsSpellDr: // Nereus
                    {
                        WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 5));
                        FloatingCombatText.ShowSelfStatus(defender, "Warded");
                        break;
                    }
                case ClauseType.ParaResistBoostsResistSkill: // Alkathous
                    {
                        WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 5));
                        FloatingCombatText.ShowSelfStatus(defender, "Warded");
                        break;
                    }
            }
        }

        FloatingCombatText.ShowSelfStatus(defender, "Resisted");
        return true;
    }

    // ---- Death / delete: mark spread + eviction -----------------------------------------

    // Coverage list for OnMobileGone's MarkSpreadOnDeath spread check (the mark's second dispatch
    // site; its arming site is in Procs.cs, covered by HandledByMark).
    internal static readonly ClauseType[] HandledByMarkSpread = { ClauseType.MarkSpreadOnDeath };

    [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
    [OnEvent(nameof(PlayerMobile.PlayerDeletedEvent))]
    [OnEvent(nameof(BaseCreature.CreatureDeathEvent))]
    [OnEvent(nameof(BaseCreature.CreatureDeletedEvent))]
    public static void OnMobileGone(Mobile m)
    {
        if (m == null)
        {
            return;
        }

        // General on-kill dispatch for clothing/jewelry effects (Laurel/Klotho, Hecatean's
        // Asteria, Demetrian's Okeanos) — these aren't tied to the killer's own weapon the way
        // P2's on-kill weapon riders are, so they hook the mobile-gone event via LastKiller
        // instead, firing on ANY kill (melee, spell, poison, ...). Level-0 victims grant
        // nothing — including the fight reset, which would re-arm first-hit clauses.
        if (m.LastKiller is { Deleted: false } killer && killer != m && GrantsKillBenefits(m))
        {
            ApplyOnKillEffects(killer);
            CombatFxState.ResetFight(killer); // a kill ends the killer's fight → next foe is a fresh first-hit
        }

        // Britomartis: when a marked target dies, the mark jumps to nearby enemies.
        if (CombatFxState.TryGetMarker(m, out var marker) && marker.Alive &&
            marker.Weapon is BaseWeapon w && w is IVariantItem v && v.LegendaryId != 0 &&
            LegendaryRegistry.TryGet(v.LegendaryId, out var entry) && entry.Clause == ClauseType.MarkSpreadOnDeath)
        {
            SpreadMark(marker, m, entry.P1 > 0 ? entry.P1 : 3, WeaponEffectTable.Get(entry.Root, ItemRarity.Legendary).MarkBonusPct);
        }

        CombatFxState.Evict(m);
        WornEffectState.Evict(m);
        PantheonFx.Evict(m);
    }

    // Worn effects (Olympian stat mods, resist/skill mods, night sight, and the whole defensive
    // aggregate) are applied via OnWornAdded on equip — but StatMods/SkillMods are transient and a
    // world save/reload rebuilds the mobile with empty mod lists. OnAdded does NOT fire on
    // deserialization, so without this hook a relogged player keeps none of it until they re-seat
    // a piece. Rebuild here (login fires after items are attached) restores the full aggregate.
    [OnEvent(nameof(PlayerMobile.PlayerLoginEvent))]
    public static void OnPlayerLogin(PlayerMobile pm) => WornEffectState.Rebuild(pm);

    // Laurel's flat on-kill restores + the Klotho/Asteria/Okeanos legendary riders.
    // Coverage list for ApplyOnKillEffects' rider switch below (LastKiller-driven worn/jewelry/
    // clothing on-kill). The weapon-side on-kill block lives in WeaponHit.cs (HandledByOnKillWeapon).
    internal static readonly ClauseType[] HandledByOnKillWorn =
    {
        ClauseType.OnKillFullManaRestore, ClauseType.OnKillStamRegenBurstStacking,
        ClauseType.OnKillTriggerHeldPotion, ClauseType.OnKillFullStamNextHitCrit
    };

    // Level-0 mobs (LevelConfig.MobLevelOverrides ambient/farm pins — 0 XP, gray tag) grant no
    // killer benefits: a chicken kill must not refill mana/stamina or re-arm first-hit clauses.
    // Player victims always grant them (PvP kills stay live).
    internal static bool GrantsKillBenefits(Mobile victim) =>
        victim is not BaseCreature bc || LevelConfig.GetMobLevel(bc) > 0;

    private static void ApplyOnKillEffects(Mobile killer)
    {
        var agg = WornEffectState.GetAggregate(killer);

        // B3: every on-kill restore spills its unusable remainder into health (see RestoreWithSpill),
        // so overlapping restores in the same kill (e.g. a full-stam clause + a flat stam clause) no
        // longer waste the second.
        if (agg.OnKillStamina > 0)
        {
            RestoreWithSpill(killer, 'S', agg.OnKillStamina);
        }

        if (agg.OnKillHp > 0)
        {
            RestoreWithSpill(killer, 'L', agg.OnKillHp);
        }

        var legendaries = WornEffectState.GetLegendaries(killer);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.OnKillFullManaRestore: // Asteria
                    {
                        RestoreWithSpill(killer, 'M', killer.ManaMax - killer.Mana);
                        break;
                    }
                case ClauseType.OnKillStamRegenBurstStacking: // Klotho
                    {
                        WornEffectState.ArmOrStackKlothoBurst(killer, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 5), 2);
                        break;
                    }
                case ClauseType.OnKillTriggerHeldPotion: // Okeanos
                    {
                        TriggerStrongestHeldPotion(killer);
                        break;
                    }
                case ClauseType.OnKillFullStamNextHitCrit: // Aristeia signature (via a held weapon's synthetic entry)
                    {
                        RestoreWithSpill(killer, 'S', killer.StamMax - killer.Stam);
                        CombatFxState.SetNextHitCrit(killer); // P1s crit window ~ a single pending crit
                        FloatingCombatText.ShowSelfStatus(killer, "Crit Ready");
                        break;
                    }
            }
        }
    }

    // Okeanos: "instantly gain the effect of your strongest held regen potion" — this shard's
    // only regen-potion family is heal potions, so the strongest held heal potion (by MaxHeal)
    // is found, drunk, and consumed.
    private static void TriggerStrongestHeldPotion(Mobile killer)
    {
        var pack = killer.Backpack;

        if (pack == null)
        {
            return;
        }

        BaseHealPotion best = null;

        foreach (var potion in pack.FindItemsByType<BaseHealPotion>())
        {
            if (best == null || potion.MaxHeal > best.MaxHeal)
            {
                best = potion;
            }
        }

        if (best == null)
        {
            return;
        }

        killer.Heal(AdjustPotionHeal(killer, Utility.RandomMinMax(best.MinHeal, best.MaxHeal)));
        FloatingCombatText.ShowSelfStatus(killer, "Heal");
        best.Consume();
    }
}
