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
    private static void ApplyMark(Mobile attacker, Mobile defender, in WeaponHitContext ctx)
    {
        if (WornEffectState.ConsumeSecondaryEffectSuppression(defender)) // Hyperbios
        {
            return;
        }

        var row = ctx.Row;
        var doMark = row.MarkChancePct > 0 && Utility.Random(100) < row.MarkChancePct;
        var allSources = false;
        var bonus = row.MarkBonusPct;

        // Dual-clause seam: both slots may raise the mark. Signature first, unique second.
        AccumulateMark(ctx.Signature, ctx.S1, ctx.IsFirstHit, ctx.IsCrit, ref doMark, ref allSources, ref bonus);
        AccumulateMark(ctx.Clause, ctx.P1, ctx.IsFirstHit, ctx.IsCrit, ref doMark, ref allSources, ref bonus);

        if (!doMark)
        {
            return;
        }

        CombatFxState.SetMark(defender, attacker, bonus, allSources, MarkDuration);
        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Marked");

        // Poison tick fires at most once even if both the always-on flag and a PoisonTickDoubled
        // clause are present (preserves the original `||` semantics).
        if (row.MarkPoisonTick || ctx.Signature == ClauseType.PoisonTickDoubled || ctx.Clause == ClauseType.PoisonTickDoubled)
        {
            defender.ApplyPoison(attacker, Poison.Lesser);
            FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Poisoned", FloatingCombatText.PoisonHue);
        }

        RunMarkRider(ctx.Signature, ctx.S1, attacker, defender, bonus);
        RunMarkRider(ctx.Clause, ctx.P1, attacker, defender, bonus);
    }

    // Dual-invoked accumulator: one clause slot's contribution to whether/how a mark lands.
    private static void AccumulateMark(
        ClauseType clause, short p1, bool isFirstHit, bool isCrit, ref bool doMark, ref bool allSources, ref int bonus
    )
    {
        switch (clause)
        {
            case ClauseType.MarkFirstHit:
            case ClauseType.MarkNearbyAllies:
            case ClauseType.MarkSpreadOnDeath:
            case ClauseType.PoisonTickDoubled:
            case ClauseType.MarkManaLeech:
            case ClauseType.MarkElemental:
            case ClauseType.MarkHealBlockFirstHit:
                {
                    doMark |= isFirstHit;
                    break;
                }
            case ClauseType.MarkAllSources25:
                {
                    if (isFirstHit)
                    {
                        doMark = true;
                        allSources = true;
                        bonus = Math.Max(bonus, p1);
                    }

                    break;
                }
            case ClauseType.MarkOnCrit:
            case ClauseType.MarkHealBlock:
                {
                    doMark |= isCrit;
                    break;
                }
        }
    }

    // Dual-invoked per-slot mark rider (heal-block / nearby-allies spread / elemental). Poison-tick
    // is handled once by the caller to avoid a double application.
    private static void RunMarkRider(ClauseType clause, short p1, Mobile attacker, Mobile defender, int bonus)
    {
        switch (clause)
        {
            case ClauseType.MarkHealBlock:
            case ClauseType.MarkHealBlockFirstHit:
                {
                    CombatFxState.SetHealBlock(defender, TimeSpan.FromSeconds(p1 > 0 ? p1 : 3));
                    FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Heal Block");
                    break;
                }
            case ClauseType.MarkNearbyAllies:
                {
                    MarkNearbyAllies(attacker, defender, bonus / 2, p1 > 0 ? p1 : 3);
                    break;
                }
            case ClauseType.MarkElemental:
                {
                    // P1 = element (0 lightning / 1 fire)
                    ElementalProc(defender, attacker, 0, p1, p1 == 1 ? "Burn" : "Shock");
                    break;
                }
        }
    }

    // Dual-invoked per hit — once for the lane signature slot, once for the legendary's unique
    // clause. Each side effect fires at most once per hit because the signature-type and
    // unique-type differ within a lane (audit invariant). ctx supplies the shared IsCrit/Row.
    private static void RunClauseProcs(
        Mobile attacker, Mobile defender, int damageGiven, ClauseType clause, short p1, short p2, short p3,
        in WeaponHitContext ctx
    )
    {
        switch (clause)
        {
            case ClauseType.CritFirstHit:
            case ClauseType.CritSplash:
                {
                    if (ctx.IsCrit && p2 > 0 && damageGiven > 0)
                    {
                        Splash(attacker, defender, damageGiven * p2 / 100, p3 > 0 ? p3 : 3);
                    }

                    break;
                }
            case ClauseType.LifestealOnCrit:
                {
                    if (ctx.IsCrit && damageGiven > 0)
                    {
                        var heal = AOS.Scale(damageGiven, ctx.Row.LifestealPct);

                        if (heal > 0)
                        {
                            attacker.Hits += heal;
                            FloatingCombatText.ShowRestore(attacker, 'L', heal);
                        }
                    }

                    break;
                }
            case ClauseType.StamDrainOnCrit:
                {
                    if (ctx.IsCrit)
                    {
                        defender.Stam = 0;
                        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "-Stam");
                    }

                    break;
                }
            case ClauseType.OnKillRestore:
                {
                    if (!defender.Alive)
                    {
                        var beforeStam = attacker.Stam;
                        attacker.Stam = attacker.StamMax;
                        FloatingCombatText.ShowRestore(attacker, 'S', attacker.Stam - beforeStam);

                        if (p1 >= 2)
                        {
                            var beforeMana = attacker.Mana;
                            attacker.Mana = attacker.ManaMax;
                            FloatingCombatText.ShowRestore(attacker, 'M', attacker.Mana - beforeMana);
                        }
                    }

                    break;
                }
            case ClauseType.CritPoisonTick:
                {
                    if (ctx.IsCrit)
                    {
                        defender.ApplyPoison(attacker, Poison.Lesser);
                        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Poisoned", FloatingCombatText.PoisonHue);
                    }

                    break;
                }
            case ClauseType.CritStagger:
                {
                    if (ctx.IsCrit)
                    {
                        if (CombatFxState.TryStun(defender, TimeSpan.FromSeconds(1)))
                        {
                            FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned");
                        }
                    }

                    break;
                }
            case ClauseType.CritElemental:
                {
                    if (ctx.IsCrit)
                    {
                        ElementalProc(defender, attacker, damageGiven, p2, p2 == 1 ? "Burn" : "Shock");
                    }

                    break;
                }
            case ClauseType.CritManaLeech:
                {
                    if (ctx.IsCrit)
                    {
                        LeechMana(defender, attacker, p2);
                        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Mana Drain");
                    }

                    break;
                }
            case ClauseType.CritHealBlock:
                {
                    if (ctx.IsCrit)
                    {
                        CombatFxState.SetHealBlock(defender, TimeSpan.FromSeconds(p2 > 0 ? p2 : 3));
                        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Heal Block");
                    }

                    break;
                }
            case ClauseType.MarkManaLeech:
                {
                    if (CombatFxState.IsMarkedBy(defender, attacker))
                    {
                        LeechMana(defender, attacker, p1);
                        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Mana Drain");
                    }

                    break;
                }
        }
    }

    private static void Splash(Mobile attacker, Mobile center, int splashDamage, int targetCap)
    {
        if (splashDamage <= 0 || center.Map == null)
        {
            return;
        }

        using var queue = PooledRefQueue<Mobile>.Create();

        foreach (var m in center.GetMobilesInRange<Mobile>(2))
        {
            if (m == attacker || m == center || !attacker.CanBeHarmful(m, false))
            {
                continue;
            }

            queue.Enqueue(m);

            if (queue.Count >= targetCap)
            {
                break;
            }
        }

        while (queue.Count > 0)
        {
            var m = queue.Dequeue();
            attacker.DoHarmful(m, true);
            AOS.Damage(m, attacker, splashDamage, 100, 0, 0, 0, 0);
        }
    }

    // P8: drain pct% of the target's current mana and hand it to the leecher.
    private static void LeechMana(Mobile target, Mobile to, int pct)
    {
        if (pct <= 0 || target.Mana <= 0)
        {
            return;
        }

        var amount = Math.Max(1, target.Mana * pct / 100);

        target.Mana -= amount;
        to.Mana = Math.Min(to.ManaMax, to.Mana + amount);
    }

    // P24: elemental FX + flat bonus damage. element: 0 = lightning, 1 = fire. The typed damage
    // renders inline as "-N (label)" in spell color (SetSpellContext) rather than a separate float.
    private static void ElementalProc(Mobile target, Mobile from, int baseDamage, int element, string label)
    {
        if (target.Map == null || !target.Alive)
        {
            return;
        }

        if (element == 1)
        {
            target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            target.PlaySound(0x208);
        }
        else
        {
            target.BoltEffect(0);
            target.PlaySound(0x29);
        }

        from.DoHarmful(target, true);

        FloatingCombatText.SetSpellContext(label);
        AOS.Damage(target, from, 10 + baseDamage / 10, 100, 0, 0, 0, 0);
        FloatingCombatText.ClearContext();
    }

    private static void MarkNearbyAllies(Mobile attacker, Mobile center, int bonus, int cap)
    {
        if (center.Map == null)
        {
            return;
        }

        using var queue = PooledRefQueue<Mobile>.Create();

        foreach (var m in center.GetMobilesInRange<Mobile>(4))
        {
            if (m == attacker || m == center || !attacker.CanBeHarmful(m, false))
            {
                continue;
            }

            queue.Enqueue(m);

            if (queue.Count >= cap)
            {
                break;
            }
        }

        while (queue.Count > 0)
        {
            CombatFxState.SetMark(queue.Dequeue(), attacker, bonus, false, MarkDuration);
        }
    }

    private static void SpreadMark(Mobile marker, Mobile deadTarget, int cap, int bonus)
    {
        if (deadTarget.Map == null)
        {
            return;
        }

        using var queue = PooledRefQueue<Mobile>.Create();

        foreach (var m in deadTarget.GetMobilesInRange<Mobile>(4))
        {
            if (m == marker || m == deadTarget || !marker.CanBeHarmful(m, false))
            {
                continue;
            }

            queue.Enqueue(m);

            if (queue.Count >= cap)
            {
                break;
            }
        }

        while (queue.Count > 0)
        {
            CombatFxState.SetMark(queue.Dequeue(), marker, bonus, false, MarkDuration);
        }
    }
}
