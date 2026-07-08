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
    public static TimeSpan AdjustHitsRegenRate(Mobile m, TimeSpan baseRate)
    {
        var agg = WornEffectState.GetAggregate(m);
        var legendaries = WornEffectState.GetLegendaries(m);

        RunHitsTickSideEffects(m, agg, legendaries);

        var pct = agg.HpRegenPct;

        if (agg.StationaryRegenPct > 0 && IsStationary(m))
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

        return TimeSpan.FromSeconds(baseRate.TotalSeconds * 100.0 / (100 + pct));
    }

    // Hestian: the stationary-regen bonus only applies once the wearer hasn't moved in >=10s.
    private static bool IsStationary(Mobile m) => Core.TickCount - m.LastMoveTime >= 10_000;

    private static void RunHitsTickSideEffects(Mobile m, in WornAggregate agg, IReadOnlyList<LegendaryEntry> legendaries)
    {
        if (agg.SelfRepair)
        {
            TryArmorSelfRepairTick(m);
        }

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
        }
    }

    // Cyclopean "slow self-repair" (Rare+). No exact rate is specified in the design docs, so
    // this rolls a modest 10% chance per HP-regen tick per qualifying piece — a deliberately
    // slow trickle, tunable later. Danaos additionally mirrors 1% max HP per tick on its shield.
    private static void TryArmorSelfRepairTick(Mobile m)
    {
        var items = m.Items;

        for (var i = 0; i < items.Count; i++)
        {
            if (items[i] is not BaseArmor armor || armor is not IVariantItem variant ||
                variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
            {
                continue;
            }

            var (root, rarity) = ResolveRootRarity(variant, armor.Rarity);
            var row = ArmorEffectTable.Get(root, rarity, armor is BaseShield);

            if (!row.SelfRepair || armor.MaxHitPoints <= 0 || armor.HitPoints >= armor.MaxHitPoints)
            {
                continue;
            }

            var repairChance = WornEffectState.IsClauseBurstActive(m, ClauseType.SelfRepairBurstOnCritBlock) ? 20 : 10; // Zethos

            if (Utility.Random(100) >= repairChance)
            {
                continue;
            }

            armor.HitPoints++;

            if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry) &&
                entry.Clause == ClauseType.SelfRepairRestoresHp) // Danaos
            {
                m.Hits += Math.Max(1, m.HitsMax * (entry.P1 > 0 ? entry.P1 : 1) / 100);
            }
        }
    }

    // Talarian stam regen + Boutes' "mirrors HP regen" + dodge-regen bursts. Lasthenes' "ticks
    // also restore mana" rides the same cadence (approximated at the same rate rather than a
    // literal half — there is no fractional-tick primitive to halve against).
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
    public static TimeSpan AdjustManaRegenRate(Mobile m, TimeSpan baseRate)
    {
        var agg = WornEffectState.GetAggregate(m);
        var legendaries = WornEffectState.GetLegendaries(m);
        var pct = agg.ManaRegenPct;

        if (agg.StationaryAppliesMana && agg.StationaryRegenPct > 0 && IsStationary(m))
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
}
