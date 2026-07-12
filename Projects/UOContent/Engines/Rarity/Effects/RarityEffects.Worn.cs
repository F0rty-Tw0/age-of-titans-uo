using System;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;
using Server.Collections;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Text;

namespace Server.Engines.Rarity;

public static partial class RarityEffects
{
    // ---- Worn passive-effect hooks (armor/shields P3a; jewelry/clothing P3b) -------------

    public static void OnWornAdded(Item item, Mobile wearer)
    {
        if (item is BaseArmor or BaseJewel or BaseClothing or BaseWeapon)
        {
            WornEffectState.Rebuild(wearer);
        }
    }

    public static void OnWornRemoved(Item item, Mobile wearer)
    {
        if (item is BaseArmor or BaseJewel or BaseClothing or BaseWeapon)
        {
            WornEffectState.Rebuild(wearer);
        }
    }

    // ---- Regen rate hooks (Paean HP regen / Talarian stam regen / mana mirrors) -----------

    // Called from Misc.RegenRates every time the mobile's HP-regen interval is computed — which
    // is every real regen tick, since Server.Mobile's HitsTimer recomputes its interval via this
    // handler right after each tick fires. Doubles as the periodic hook for the auto-cure roll
    // and the low-HP emergency-cure clause so no second per-mobile timer is needed (CLAUDE.md
    // rule 6). `baseRate` already reflects era (AOS vs pre-AOS) — only the rarity bonus is added.
    // Coverage list for AdjustHitsRegenRate's clause reads (hidden-double + the burst multipliers).
    internal static readonly ClauseType[] HandledByHitsRegen =
    {
        ClauseType.RegenDoubleWhileHidden, ClauseType.HpRegenBurstOnCritTaken, ClauseType.HitHalvedRegenPulse,
        ClauseType.RegenDoubleAfterPotion, ClauseType.StationaryRegenFaster
    };

    public static TimeSpan AdjustHitsRegenRate(Mobile m, TimeSpan baseRate)
    {
        var agg = WornEffectState.GetAggregate(m);
        var legendaries = WornEffectState.GetLegendaries(m);

        RunHitsTickSideEffects(m, agg, legendaries);

        var pct = agg.HpRegenPct;

        if (agg.StationaryRegenPct > 0 && IsStationary(m, legendaries))
        {
            pct += agg.StationaryRegenPct; // Hestian
        }

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.RegenDoubleWhileHidden && m.Hidden) // Khaos
            {
                pct *= 2;
                break;
            }
        }

        if (pct <= 0)
        {
            return baseRate;
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.HpRegenBurstOnCritTaken))
        {
            pct *= 3; // Phylakos: HP regen rate triples for 5s after taking a crit
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.HitHalvedRegenPulse))
        {
            pct += 20; // Ananke — flat pulse bump; doc gives no exact magnitude, tunable
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.RegenDoubleAfterPotion))
        {
            pct *= 2; // Rhea
        }

        // P6 cap fix: the burst multipliers above stack AFTER Rebuild's suit-wide clamp, so a
        // 25% suit under Phylakos ×3 (or hidden ×2 + potion ×2) would sail past the framework's
        // 60% ceiling. The cap is law — clamp the final effective bonus.
        pct = Math.Min(pct, WornEffectState.HpRegenCap);

        return TimeSpan.FromSeconds(baseRate.TotalSeconds * 100.0 / (100 + pct));
    }

    // Hestian: the stationary-regen bonus only applies once the wearer hasn't moved in >=10s.
    // Kalyptra (Hestian hat relic, StationaryRegenFaster) lowers that threshold to P1 seconds.
    private static bool IsStationary(Mobile m, IReadOnlyList<LegendaryEntry> legendaries)
    {
        var thresholdMs = 10_000;

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.StationaryRegenFaster) // Kalyptra
            {
                thresholdMs = (legendaries[i].P1 > 0 ? legendaries[i].P1 : 5) * 1000;
                break;
            }
        }

        return Core.TickCount - m.LastMoveTime >= thresholdMs;
    }

    // Coverage list for RunHitsTickSideEffects' auto-cure riders + low-HP cure + resist-skill switches.
    internal static readonly ClauseType[] HandledByHitsTickSideEffects =
    {
        ClauseType.AutoCureRestoresStamMana, ClauseType.AutoCureClearsDebuffsOnce, ClauseType.AutoCureRestoresHpPct,
        ClauseType.LowHpEmergencyCure, ClauseType.ResistSkillDoubleLowHp, ClauseType.ParaResistBoostsResistSkill,
        ClauseType.ResistSkillBoostLowHp, ClauseType.LowHpDodgeBurst
    };

    private static void RunHitsTickSideEffects(Mobile m, in WornAggregate agg, IReadOnlyList<LegendaryEntry> legendaries)
    {
        if (agg.AutoCure && m.Poisoned && Utility.Random(100) < 5)
        {
            m.CurePoison(m);

            for (var i = 0; i < legendaries.Count; i++)
            {
                var entry = legendaries[i];

                switch (entry.Clause)
                {
                    case ClauseType.AutoCureRestoresStamMana: // Machaon
                        {
                            var pct = entry.P1 > 0 ? entry.P1 : 10;
                            m.Stam = Math.Min(m.StamMax, m.Stam + m.StamMax * pct / 100);
                            m.Mana = Math.Min(m.ManaMax, m.Mana + m.ManaMax * pct / 100);
                            break;
                        }
                    case ClauseType.AutoCureClearsDebuffsOnce: // Iapyx
                        {
                            if (WornEffectState.TryUseOncePerFight(m, entry.Clause, CombatFxState.IsFightFreshForDefender(m)))
                            {
                                CombatFxState.ClearMark(m);
                            }

                            break;
                        }
                    case ClauseType.AutoCureRestoresHpPct: // Kyrene
                        {
                            var missing = m.HitsMax - m.Hits;

                            if (missing > 0)
                            {
                                m.Hits += missing * (entry.P1 > 0 ? entry.P1 : 5) / 100;
                            }

                            break;
                        }
                }
            }
        }

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.LowHpEmergencyCure && m.HitsMax > 0 &&
                m.Hits < m.HitsMax * (entry.P1 > 0 ? entry.P1 : 30) / 100)
            {
                if (WornEffectState.TryUseOncePerFight(m, entry.Clause, CombatFxState.IsFightFreshForDefender(m)))
                {
                    if (m.Poisoned)
                    {
                        m.CurePoison(m);
                    }

                    m.Hits += m.HitsMax * (entry.P2 > 0 ? entry.P2 : 10) / 100; // Daphne
                }
            }
            else if (entry.Clause == ClauseType.ResistSkillDoubleLowHp)
            {
                var threshold = entry.P2 > 0 ? entry.P2 : 50;
                var boosted = m.HitsMax > 0 && m.Hits < m.HitsMax * threshold / 100;

                WornEffectState.BoostResistSkill(m, boosted ? entry.P1 > 0 ? entry.P1 : 10 : agg.ResistSkillBonus); // Glaukos
            }
            else if (entry.Clause == ClauseType.ParaResistBoostsResistSkill)
            {
                var boosted = WornEffectState.IsClauseBurstActive(m, entry.Clause);

                WornEffectState.BoostResistSkill(m, boosted ? entry.P1 > 0 ? entry.P1 : 10 : agg.ResistSkillBonus); // Alkathous
            }
            else if (entry.Clause == ClauseType.ResistSkillBoostLowHp) // Ilion
            {
                var threshold = entry.P2 > 0 ? entry.P2 : 50;
                var boosted = m.HitsMax > 0 && m.Hits < m.HitsMax * threshold / 100;

                WornEffectState.BoostResistSkill(m, agg.ResistSkillBonus + (boosted ? entry.P1 > 0 ? entry.P1 : 5 : 0));
            }
            else if (entry.Clause == ClauseType.LowHpDodgeBurst && m.HitsMax > 0 && m.Hits < m.HitsMax / 4) // Ariadne
            {
                // Once per fight below 25% health: a short dodge burst (read in AdjustHitChance).
                if (WornEffectState.TryUseOncePerFight(m, entry.Clause, CombatFxState.IsFightFreshForDefender(m)))
                {
                    WornEffectState.ArmClauseBurst(m, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 5));
                    FloatingCombatText.ShowSelfStatus(m, "Evasion");
                }
            }
        }
    }

    // Talarian stam regen + Boutes' "mirrors HP regen" + dodge-regen bursts. Lasthenes' "ticks
    // also restore mana" rides the same cadence (approximated at the same rate rather than a
    // literal half — there is no fractional-tick primitive to halve against).
    // Coverage list for AdjustStamRegenRate's clause switch below.
    internal static readonly ClauseType[] HandledByStamRegen =
    {
        ClauseType.StamRegenMirrorsHp, ClauseType.DodgeRegenBurst, ClauseType.StamRegenMirrorsManaHalf,
        ClauseType.RegenDoubleWhileHidden, ClauseType.OnKillStamRegenBurstStacking, ClauseType.HitHalvedRegenPulse,
        ClauseType.RegenDoubleAfterPotion
    };

    public static TimeSpan AdjustStamRegenRate(Mobile m, TimeSpan baseRate)
    {
        var agg = WornEffectState.GetAggregate(m);
        var legendaries = WornEffectState.GetLegendaries(m);

        var pct = agg.StamRegenPct;

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.StamRegenMirrorsHp) // Boutes
            {
                pct += agg.HpRegenPct;
            }
            else if (entry.Clause == ClauseType.DodgeRegenBurst && WornEffectState.IsClauseBurstActive(m, entry.Clause))
            {
                pct += entry.P1 > 0 ? entry.P1 : 10; // Kalydon / Panoptes
            }
            else if (entry.Clause == ClauseType.StamRegenMirrorsManaHalf && m.Stam < m.StamMax) // Lasthenes
            {
                m.Mana = Math.Min(m.ManaMax, m.Mana + 1);
            }
            else if (entry.Clause == ClauseType.RegenDoubleWhileHidden && m.Hidden) // Khaos
            {
                pct *= 2;
            }
            else if (entry.Clause == ClauseType.OnKillStamRegenBurstStacking) // Klotho
            {
                var stacks = WornEffectState.GetKlothoStacks(m);

                if (stacks > 0)
                {
                    pct += (entry.P1 > 0 ? entry.P1 : 20) * stacks;
                }
            }
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.HitHalvedRegenPulse))
        {
            pct += 20; // Ananke
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.RegenDoubleAfterPotion))
        {
            pct *= 2; // Rhea
        }

        return pct > 0 ? TimeSpan.FromSeconds(baseRate.TotalSeconds * 100.0 / (100 + pct)) : baseRate;
    }

    // Aristaios: mana regen rate gains the same bonus as HP regen. Also carries Hecatean's own
    // mana-regen% (P3a's "regen mechanism" family), Phoibe's low-mana double, and the same
    // Khaos/Ananke/Rhea burst riders as the other two regen hooks.
    // Coverage list for AdjustManaRegenRate's clause switch below.
    internal static readonly ClauseType[] HandledByManaRegen =
    {
        ClauseType.ManaRegenMirrorsHp, ClauseType.ManaRegenDoubleLowMana, ClauseType.RegenDoubleWhileHidden,
        ClauseType.HitHalvedRegenPulse, ClauseType.RegenDoubleAfterPotion
    };

    public static TimeSpan AdjustManaRegenRate(Mobile m, TimeSpan baseRate)
    {
        var agg = WornEffectState.GetAggregate(m);
        var legendaries = WornEffectState.GetLegendaries(m);
        var pct = agg.ManaRegenPct;

        if (agg.StationaryAppliesMana && agg.StationaryRegenPct > 0 && IsStationary(m, legendaries))
        {
            pct += agg.StationaryRegenPct; // Hestian Epic
        }

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.ManaRegenMirrorsHp) // Aristaios
            {
                pct += agg.HpRegenPct;
            }
            else if (entry.Clause == ClauseType.ManaRegenDoubleLowMana && m.ManaMax > 0 &&
                     m.Mana < m.ManaMax * (entry.P1 > 0 ? entry.P1 : 25) / 100) // Phoibe
            {
                pct *= 2;
            }
            else if (entry.Clause == ClauseType.RegenDoubleWhileHidden && m.Hidden) // Khaos
            {
                pct *= 2;
            }
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.HitHalvedRegenPulse))
        {
            pct += 20; // Ananke
        }

        if (WornEffectState.IsClauseBurstActive(m, ClauseType.RegenDoubleAfterPotion))
        {
            pct *= 2; // Rhea
        }

        return pct > 0 ? TimeSpan.FromSeconds(baseRate.TotalSeconds * 100.0 / (100 + pct)) : baseRate;
    }

    // ---- Spell interaction hooks (Paean heals-received / Tritonian spell DR + para resist) --

    // Called from SpellHelper.Heal and BandageContext — the two heal choke points on this shard.
    public static int AdjustHealAmount(Mobile target, int amount)
    {
        var pct = WornEffectState.GetAggregate(target).HealsReceivedPct;
        return pct > 0 ? amount + amount * pct / 100 : amount;
    }

    // Light-armor "carry capacity" lane: the extra weight a wearer can bear, as a % of their base
    // MaxWeight. Folded into PlayerMobile.MaxWeight so the whole carried load is lightened, not the
    // armor piece's own weight (user directive 2026-07-12). Suit-capped in WornEffectState.
    public static int CarryWeightBonus(Mobile m, int baseMaxWeight)
    {
        var pct = WornEffectState.GetAggregate(m).CarryWeightBonusPct;
        return pct > 0 ? baseMaxWeight * pct / 100 : 0;
    }
}
