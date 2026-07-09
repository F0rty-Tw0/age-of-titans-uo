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
    // ---- Combat hooks (BaseWeapon, T2A path) --------------------------------------------

    // P1 — swing speed. Scales the computed swing delay by the weapon's SwingSpeedPct, plus
    // Maenad's Epic frenzy swing-speed rider — which is wielder-driven (clothing), not weapon-
    // driven, so it applies regardless of whether the weapon itself carries a rarity variant.
    public static double AdjustSwingDelay(BaseWeapon weapon, Mobile wielder, double delaySeconds)
    {
        var frenzySwingPct = CombatFxState.GetFrenzySwingPct(wielder);

        if (weapon is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return frenzySwingPct == 0 ? delaySeconds : delaySeconds * 100.0 / (100 + frenzySwingPct);
        }

        var (root, rarity) = ResolveRootRarity(variant, weapon.Rarity);
        var pct = WeaponEffectTable.Get(root, rarity).SwingSpeedPct + frenzySwingPct;

        return pct == 0 ? delaySeconds : delaySeconds * 100.0 / (100 + pct);
    }

    // P3 — hit chance. Adds the weapon's HitChancePct to the to-hit roll (chance is 0..1), then
    // subtracts the defender's worn Talarian dodge% (armor/shield P3a).
    public static void AdjustHitChance(BaseWeapon weapon, Mobile attacker, Mobile defender, ref double chance)
    {
        // A guaranteed extra swing (Antilochos/Peleus) forces its follow-up to land.
        if (ForceHit)
        {
            chance = 1.0;
            return;
        }

        if (weapon is IVariantItem variant && (variant.VariantRoot != VariantRoot.None || variant.LegendaryId != 0))
        {
            var (root, rarity) = ResolveRootRarity(variant, weapon.Rarity);
            var pct = WeaponEffectTable.Get(root, rarity).HitChancePct;

            if (pct != 0)
            {
                chance += pct / 100.0;
            }

            // Kyknos: hit chance stacks with each follow-up swing this fight (§ ExtraSwingStackingHit).
            var stack = CombatFxState.GetHitStack(attacker);

            if (stack != 0)
            {
                chance += stack / 100.0;
            }
        }

        var agg = WornEffectState.GetAggregate(defender);
        var dodge = agg.DodgePct;

        if (dodge <= 0)
        {
            return;
        }

        var legendaries = WornEffectState.GetLegendaries(defender);
        var fightFresh = CombatFxState.IsFightFreshForDefender(defender);

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.DodgeDoubleFirstAttack && fightFresh)
            {
                dodge *= 2; // Kyllene
            }
        }

        chance -= dodge / 100.0;

        if (chance < 0.01)
        {
            chance = 0.01;
        }
    }

    // A defender wearing any Talarian dodge% reskins a suffered miss as "Dodge"; BaseWeapon.OnMiss
    // reads this to suppress the generic "Miss" float for them.
    public static bool HasDodgePackage(Mobile defender) =>
        WornEffectState.GetAggregate(defender).DodgePct > 0;

    // Called from BaseWeapon.CheckHit only when the swing actually missed. A miss suffered by a
    // defender with no Talarian dodge% is an ordinary whiff, not a themed "dodge" — riders only
    // fire when the defender is actually wearing the dodge package.
    public static void OnMeleeMiss(Mobile attacker, Mobile defender)
    {
        if (!HasDodgePackage(defender))
        {
            return;
        }

        var legendaries = WornEffectState.GetLegendaries(defender);

        // Ophis (DodgeGrantsCounterWindow) arms the dodger's next swing to crit. Fold that into
        // the single dodge float ("Dodge, Crit Ready") instead of a second overhead line.
        var counterWindow = false;

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.DodgeGrantsCounterWindow)
            {
                counterWindow = true;
                break;
            }
        }

        FloatingCombatText.ShowSelfStatus(defender, counterWindow ? "Dodge, Crit Ready" : "Dodge");

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.DodgeRefundStam:
                    {
                        var before = defender.Stam;
                        defender.Stam = Math.Min(defender.StamMax, defender.Stam + entry.P1);
                        FloatingCombatText.ShowRestore(defender, 'S', defender.Stam - before);
                        break;
                    }
                case ClauseType.DodgeRestoreMana:
                    {
                        var before = defender.Mana;
                        defender.Mana = Math.Min(defender.ManaMax, defender.Mana + defender.ManaMax * entry.P1 / 100);
                        FloatingCombatText.ShowRestore(defender, 'M', defender.Mana - before);
                        break;
                    }
                case ClauseType.DodgeRegenBurst:
                    {
                        WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 3));
                        break;
                    }
                case ClauseType.WeightReductionSuiteBurstOnDodge:
                    {
                        WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 3));
                        break;
                    }
                case ClauseType.DodgeReflectDamage: // Penelope — the swing missed entirely, so
                    // there is no live "damage that would have landed" value at this hook; a small
                    // flat reflect stands in (matches the existing "small flat thorns" precedent).
                    {
                        AOS.Damage(attacker, defender, 5, 100, 0, 0, 0, 0);
                        FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
                        break;
                    }
                case ClauseType.DodgeGrantsCounterWindow: // Ophis signature: the dodger's next swing
                    // crits; the "Crit Ready" cue is already folded into the dodge float above.
                    {
                        CombatFxState.SetNextHitCrit(defender);
                        break;
                    }
            }
        }
    }

    // Computed once per landed hit. Rolls crit + legendary cadence and returns the damage-bonus
    // percent to fold into OnHit's percentageBonus, plus the context for the post-hit procs.
    public static WeaponHitContext BeginWeaponHit(BaseWeapon weapon, Mobile attacker, Mobile defender)
    {
        if (weapon is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return default;
        }

        var (root, rarity) = ResolveRootRarity(variant, weapon.Rarity);
        var clause = ClauseType.None;
        short p1 = 0, p2 = 0, p3 = 0;

        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            clause = entry.Clause;
            p1 = entry.P1;
            p2 = entry.P2;
            p3 = entry.P3;
        }

        var row = WeaponEffectTable.Get(root, rarity);
        var signature = row.Signature;
        short s1 = row.S1, s2 = row.S2, s3 = row.S3;
        var hitCount = CombatFxState.RegisterHit(attacker, out var firstHit);

        var isCrit = row.CritChancePct > 0 && Utility.Random(100) < row.CritChancePct;
        var extraSwing = row.ExtraSwingPct > 0 && Utility.Random(100) < row.ExtraSwingPct;

        // Kydon: a prior block armed this attacker's next hit to crit.
        if (CombatFxState.ConsumeNextHitCrit(attacker))
        {
            isCrit = true;
        }

        // Dual-clause seam: the lane signature and the legendary's unique clause each get a pass
        // (bool ORs are idempotent; the "unique type != signature type within a lane" invariant
        // keeps a non-idempotent stack mutation from double-firing). Signature first, unique second.
        ArmClauseCritSwing(signature, s1, s2, s3, hitCount, firstHit, attacker, ref isCrit, ref extraSwing);
        ArmClauseCritSwing(clause, p1, p2, p3, hitCount, firstHit, attacker, ref isCrit, ref extraSwing);

        // P28 ramp: consecutive same-target hits add row.RampPerStackPct each.
        var rampReachedMax = false;
        var rampStacks = 0;

        if (row.RampPerStackPct > 0)
        {
            rampStacks = CombatFxState.RegisterRampHit(attacker, defender, row.RampMaxStacks, out rampReachedMax);
        }

        var bonus = row.DamagePct;

        if (isCrit)
        {
            bonus += 50 + row.CritDamagePct; // crit = x1.5 base, +crit-damage additive
        }

        if (rampStacks > 0)
        {
            bonus += rampStacks * row.RampPerStackPct;
        }

        if (row.NthHitN > 0 && hitCount % row.NthHitN == 0)
        {
            bonus += row.NthHitBonusPct;
        }

        if (firstHit)
        {
            bonus += row.FirstHitBonusPct;
        }

        // Haima signature: the wielder's hits deal more against a poisoned target.
        if (signature == ClauseType.PoisonedTakeBonusDamage && defender.Poisoned)
        {
            bonus += s1;
        }

        bonus += CombatFxState.GetMarkBonusFrom(defender, attacker);

        if (isCrit && clause == ClauseType.CritExecuteUnder15 &&
            IsUnderHpFraction(defender, (p2 > 0 ? p2 : 15) / 100.0))
        {
            bonus += 100; // double damage vs low-HP targets (P2 = threshold %, default 15)
        }

        if (isCrit && clause == ClauseType.CritFullHpDouble && IsFullHp(defender))
        {
            bonus += 100; // Pandaros: opening crit doubles vs full-HP targets
        }

        // P27 armor-pen for this hit — stashed for BaseWeapon's AR-absorb step to consume.
        // Always-on row pen; NthHitFullArmorPen (either slot) forces 100 at cadence; CritArmorPen's
        // P2 applies only when the hit actually crits. Cross-family clause reuse is allowed, so the
        // Nth-pen check honours both the signature slot and a legendary's unique clause.
        var pen = row.ArmorPenPct;

        if (signature == ClauseType.NthHitFullArmorPen && s1 > 0 && hitCount % s1 == 0 ||
            clause == ClauseType.NthHitFullArmorPen && p1 > 0 && hitCount % p1 == 0)
        {
            pen = 100;
        }

        if (isCrit && clause == ClauseType.CritArmorPen)
        {
            pen = Math.Max(pen, p2);
        }

        SetPendingArmorPen(Math.Clamp(pen, 0, 100));

        return new WeaponHitContext(
            root, rarity, row, clause, p1, p2, p3, isCrit, firstHit, extraSwing, bonus,
            signature, s1, s2, s3, hitCount, rampReachedMax
        );
    }

    // Dual-invoked crit/extra-swing arming for one clause slot. Only touches the ref bools (safe
    // to run twice); the ExtraSwingStackingHit stack mutation is guarded by its clause matching
    // just one slot (signatures never carry that clause).
    private static void ArmClauseCritSwing(
        ClauseType clause, short p1, short p2, short p3, int hitCount, bool firstHit,
        Mobile attacker, ref bool isCrit, ref bool extraSwing
    )
    {
        switch (clause)
        {
            case ClauseType.CritFirstHit:
            case ClauseType.CritFirstHitStamRefund: // Ephodos signature: first hit is a guaranteed crit
                {
                    isCrit |= firstHit;
                    break;
                }
            case ClauseType.CritEveryN:
            case ClauseType.CritSplash:
            case ClauseType.CritArmorPen:
            case ClauseType.CritExecuteUnder15:
            case ClauseType.CritPoisonTick:
            case ClauseType.CritStagger:
                {
                    isCrit |= p1 > 0 && hitCount % p1 == 0;
                    break;
                }
            case ClauseType.CritElemental:
            case ClauseType.CritManaLeech:
            case ClauseType.CritHealBlock:
            case ClauseType.CritFullHpDouble:
                {
                    isCrit |= p1 > 0 && hitCount % p1 == 0 || p3 == 1 && firstHit;
                    break;
                }
            case ClauseType.ExtraSwingEveryN:
            case ClauseType.DoubleStrikeEveryN:
            case ClauseType.ExtraSwingSplash:
            case ClauseType.ExtraSwingGuaranteedHit:
            case ClauseType.ExtraSwingManaLeech:
            case ClauseType.ExtraSwingHealBlock:
            case ClauseType.ExtraSwingChain: // chained swing may proc one more (depth-2 guard)
                {
                    extraSwing |= p1 > 0 && hitCount % p1 == 0;
                    break;
                }
            case ClauseType.ExtraSwingElemental:
                {
                    extraSwing |= p1 > 0 ? hitCount % p1 == 0 : firstHit;
                    break;
                }
            case ClauseType.ExtraSwingFirstHit:
                {
                    extraSwing |= firstHit;
                    break;
                }
            case ClauseType.ExtraSwingStackingHit:
                {
                    extraSwing |= firstHit;

                    if (firstHit)
                    {
                        CombatFxState.ResetHitStack(attacker);
                    }
                    else
                    {
                        CombatFxState.AddHitStack(attacker, p1 > 0 ? p1 : 5, p2 > 0 ? p2 : 20);
                    }

                    break;
                }
        }
    }

    // Runs after damage is applied: lifesteal, stamina, mark application, poison, legendary
    // riders (splash/on-kill/stam-drain), and the extra-swing proc.
    public static void EndWeaponHit(
        BaseWeapon weapon, Mobile attacker, Mobile defender, int damageGiven, in WeaponHitContext ctx
    )
    {
        // Olympian: lightning proc on the wearer's own landed melee hit. Jewelry-driven, so this
        // must run regardless of whether the weapon itself carries a rarity variant.
        ApplyOlympianLightningProc(attacker, defender);

        if (!ctx.Active)
        {
            return;
        }

        var row = ctx.Row;

        if (damageGiven > 0 && row.LifestealPct > 0)
        {
            var pct = row.LifestealPct;

            if (row.LifestealExecute && (!defender.Alive || IsUnderHpFraction(defender, 0.30)))
            {
                pct *= 2;
            }

            var heal = AOS.Scale(damageGiven, pct);

            if (heal > 0)
            {
                attacker.Hits += heal;
                FloatingCombatText.ShowRestore(attacker, 'L', heal);
            }
        }

        if (damageGiven > 0 && row.StamRegenPct > 0)
        {
            var gain = AOS.Scale(damageGiven, row.StamRegenPct);
            attacker.Stam += gain;
            FloatingCombatText.ShowRestore(attacker, 'S', gain);
        }

        ApplyMark(attacker, defender, in ctx);
        RunClauseProcs(attacker, defender, damageGiven, ctx.Signature, ctx.S1, ctx.S2, ctx.S3, in ctx);
        RunClauseProcs(attacker, defender, damageGiven, ctx.Clause, ctx.P1, ctx.P2, ctx.P3, in ctx);
        RunRowNumericProcs(attacker, defender, damageGiven, in ctx);
        ApplyArmorHitRiders(attacker, defender, in ctx);

        // ExtraSwingChain (either slot) relaxes the re-entrancy guard to depth 2 so the chained
        // swing may itself proc ONE more; every other extra swing is depth 1.
        var maxDepth = ctx.Clause == ClauseType.ExtraSwingChain || ctx.Signature == ClauseType.ExtraSwingChain
            ? 2
            : 1;

        if (ctx.ExtraSwing && _extraSwingDepth < maxDepth && defender.Alive)
        {
            DoExtraSwing(weapon, attacker, defender, in ctx);
            ApplyExtraSwingRider(attacker, defender, damageGiven, ctx.Signature, ctx.S2, ctx.S3);
            ApplyExtraSwingRider(attacker, defender, damageGiven, ctx.Clause, ctx.P2, ctx.P3);
        }
    }

    // Always-on numeric lane hooks (row fields) plus the cadence/ramp/poison signatures that ride
    // the landed hit. Runs once per hit (fields are slot-independent).
    private static void RunRowNumericProcs(Mobile attacker, Mobile defender, int damageGiven, in WeaponHitContext ctx)
    {
        var row = ctx.Row;

        if (row.SplashPct > 0 && damageGiven > 0)
        {
            Splash(attacker, defender, damageGiven * row.SplashPct / 100, 3);
        }

        if (ctx.Signature == ClauseType.NthHitSplash && ctx.S1 > 0 && ctx.HitCount % ctx.S1 == 0 && damageGiven > 0)
        {
            Splash(attacker, defender, damageGiven * ctx.S2 / 100, ctx.S3 > 0 ? ctx.S3 : 3);
        }

        if (ctx.Signature == ClauseType.RampMaxStacksSplash && ctx.RampReachedMax && damageGiven > 0)
        {
            Splash(attacker, defender, damageGiven * ctx.S1 / 100, ctx.S2 > 0 ? ctx.S2 : 3);
        }

        if (row.StaggerProcPct > 0 && Utility.Random(100) < row.StaggerProcPct)
        {
            if (CombatFxState.TryStun(defender, TimeSpan.FromSeconds(1))) // §9.2 caps enforced in TryStun
            {
                FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned");
            }
        }

        if (row.PoisonApplyPct > 0 && defender.Alive && Utility.Random(100) < row.PoisonApplyPct)
        {
            defender.ApplyPoison(attacker, row.PoisonTier >= 1 ? Poison.Regular : Poison.Lesser);
            FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Poisoned", FloatingCombatText.PoisonHue);

            // Toxikon signature: targets the wielder poisons are marked (+S1% taken).
            if (ctx.Signature == ClauseType.PoisonedTargetsMarked)
            {
                CombatFxState.SetMark(defender, attacker, ctx.S1, false, MarkDuration);
            }
        }

        if (row.ManaLeechPct > 0)
        {
            LeechMana(defender, attacker, row.ManaLeechPct);
            FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Mana Drain");
        }

        if (row.ElementalProcPct > 0 && Utility.Random(100) < row.ElementalProcPct)
        {
            ElementalProc(defender, attacker, damageGiven, row.ElementalKind, row.ElementalKind == 1 ? "Burn" : "Shock");
        }

        if (row.HealBlockProcPct > 0 && Utility.Random(100) < row.HealBlockProcPct)
        {
            CombatFxState.SetHealBlock(defender, TimeSpan.FromSeconds(3)); // 3s cap enforced in SetHealBlock
            FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Heal Block");
        }

        if (row.DefenderStamDrainFlat > 0)
        {
            defender.Stam = Math.Max(0, defender.Stam - row.DefenderStamDrainFlat);
            FloatingCombatText.ShowOffensiveStatus(defender, attacker, "-Stam");
        }

        if (row.OnKillStamPct > 0 && !defender.Alive)
        {
            var before = attacker.Stam;
            attacker.Stam = Math.Min(attacker.StamMax, attacker.Stam + attacker.StamMax * row.OnKillStamPct / 100);
            FloatingCombatText.ShowRestore(attacker, 'S', attacker.Stam - before);
        }

        // Ephodos signature: the guaranteed first-hit crit refunds the swing's stamina cost. No
        // explicit per-swing stamina cost exists on the T2A path, so a flat refund stands in.
        if (ctx.Signature == ClauseType.CritFirstHitStamRefund && ctx.IsFirstHit)
        {
            var before = attacker.Stam;
            attacker.Stam = Math.Min(attacker.StamMax, attacker.Stam + (ctx.S1 > 0 ? ctx.S1 : 10));
            FloatingCombatText.ShowRestore(attacker, 'S', attacker.Stam - before);
        }
    }

    // Olympian: a % chance the wearer's own landed melee hit also triggers a lightning proc.
    private static void ApplyOlympianLightningProc(Mobile attacker, Mobile defender)
    {
        var agg = WornEffectState.GetAggregate(attacker);

        if (agg.LightningProcPct <= 0 || Utility.Random(100) >= agg.LightningProcPct)
        {
            return;
        }

        ElementalProc(defender, attacker, 0, 0, "Lightning"); // lightning

        var legendaries = WornEffectState.GetLegendaries(attacker);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.LightningProcRefundStam: // Hyperion
                    {
                        var before = attacker.Stam;
                        attacker.Stam = Math.Min(attacker.StamMax, attacker.Stam + (entry.P1 > 0 ? entry.P1 : 10));
                        FloatingCombatText.ShowRestore(attacker, 'S', attacker.Stam - before);
                        break;
                    }
                case ClauseType.LightningProcChanceRestoreMana: // Ouranos
                    {
                        if (Utility.Random(100) < (entry.P1 > 0 ? entry.P1 : 50))
                        {
                            var before = attacker.Mana;
                            attacker.Mana = Math.Min(attacker.ManaMax, attacker.Mana + (entry.P2 > 0 ? entry.P2 : 10));
                            FloatingCombatText.ShowRestore(attacker, 'M', attacker.Mana - before);
                        }

                        break;
                    }
                case ClauseType.LightningProcResistBurst: // Astraios
                    {
                        WornEffectState.ArmClauseBurst(attacker, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 3));
                        FloatingCombatText.ShowSelfStatus(attacker, "Warded");
                        break;
                    }
            }
        }
    }

    // Armor/shield riders that key off a landed melee hit rather than the weapon's own variant:
    // crit-taken bursts on the defender's gear, and the attacker's own on-kill / first-hit-landed
    // shield clauses. Gated the same as the rest of this function (ctx.Active) — a known scope
    // limit is that these only fire when the attacker's own weapon also carries a rarity variant,
    // since that is what makes BeginWeaponHit populate ctx at all (flagged in the P3a report).
    private static void ApplyArmorHitRiders(Mobile attacker, Mobile defender, in WeaponHitContext ctx)
    {
        if (ctx.IsCrit)
        {
            var defenderLegendaries = WornEffectState.GetLegendaries(defender);

            for (var i = 0; i < defenderLegendaries.Count; i++)
            {
                var entry = defenderLegendaries[i];

                if (entry.Clause == ClauseType.HpRegenBurstOnCritTaken) // Phylakos
                {
                    WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 5));
                }
                else if (entry.Clause == ClauseType.SpellDrBurstOnCritTaken) // Palaimon
                {
                    WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 3));
                }
            }
        }

        var attackerLegendaries = WornEffectState.GetLegendaries(attacker);

        for (var i = 0; i < attackerLegendaries.Count; i++)
        {
            var entry = attackerLegendaries[i];

            if (entry.Clause == ClauseType.HealBlockOnFirstHitLanded && ctx.IsFirstHit) // Aiakos
            {
                CombatFxState.SetHealBlock(defender, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 3));
                FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Heal Block");
            }
            else if (entry.Clause == ClauseType.FrenzyStaggerChance && CombatFxState.GetFrenzyDamagePct(attacker) > 0)
            {
                // Atropos: while frenzied (Maenad clothing), landed hits carry a stagger chance.
                if (Utility.Random(100) < (entry.P1 > 0 ? entry.P1 : 4))
                {
                    if (CombatFxState.TryStun(defender, TimeSpan.FromSeconds(1)))
                    {
                        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned");
                    }
                }
            }
        }

        if (defender.Alive)
        {
            return;
        }

        for (var i = 0; i < attackerLegendaries.Count; i++)
        {
            var entry = attackerLegendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.OnKillRestoreMissingHpPct: // Podaleirios
                    {
                        var missing = attacker.HitsMax - attacker.Hits;

                        if (missing > 0)
                        {
                            var gain = missing * (entry.P1 > 0 ? entry.P1 : 50) / 100;
                            attacker.Hits += gain;
                            FloatingCombatText.ShowRestore(attacker, 'L', gain);
                        }

                        break;
                    }
                case ClauseType.OnKillRestoreExtraHp: // Autonoos
                    {
                        var gain = attacker.HitsMax * (entry.P1 > 0 ? entry.P1 : 15) / 100;
                        attacker.Hits += gain;
                        FloatingCombatText.ShowRestore(attacker, 'L', gain);
                        break;
                    }
                case ClauseType.OnKillRestoreHpPct: // Asklepios
                    {
                        var gain = attacker.HitsMax * (entry.P1 > 0 ? entry.P1 : 25) / 100;
                        attacker.Hits += gain;
                        FloatingCombatText.ShowRestore(attacker, 'L', gain);
                        break;
                    }
                case ClauseType.OnKillDodgeDoubleDuration: // Patroklos
                    {
                        WornEffectState.ArmClauseBurst(attacker, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 5));
                        break;
                    }
                case ClauseType.OnKillStamRestoreExtendImmunity: // Melanippos
                    {
                        var before = attacker.Stam;
                        attacker.Stam = attacker.StamMax;
                        CombatFxState.ExtendStunImmunity(attacker, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 5));
                        FloatingCombatText.ShowRestore(attacker, 'S', attacker.Stam - before);
                        break;
                    }
            }
        }
    }

    private static void DoExtraSwing(BaseWeapon weapon, Mobile attacker, Mobile defender, in WeaponHitContext ctx)
    {
        var stagger = ctx.Clause == ClauseType.ExtraSwingEveryN && ctx.P2 == 1; // Notos
        var doubleStrikeCrits = ctx.Clause == ClauseType.DoubleStrikeEveryN;
        var guaranteedHit = ctx.Clause == ClauseType.ExtraSwingGuaranteedHit; // Antilochos/Peleus

        var priorForceHit = ForceHit;
        _extraSwingDepth++;
        ForceHit = guaranteedHit;

        try
        {
            attacker.NextCombatTime = Core.TickCount + (int)weapon.OnSwing(attacker, defender).TotalMilliseconds;
        }
        finally
        {
            _extraSwingDepth--;
            ForceHit = priorForceHit;
        }

        FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Extra Swing");

        if (stagger)
        {
            if (CombatFxState.TryStun(defender, TimeSpan.FromSeconds(1)))
            {
                FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Stunned");
            }
        }

        // ponytail: DoubleStrikeEveryN's "second strike always crits" rider is deferred to P2 —
        // the extra swing rolls its own crit through the normal path for now.
        _ = doubleStrikeCrits;
    }

    // Post-swing riders that ride the guaranteed extra swing (splash/mana-leech/heal-block/
    // elemental). Dual-invoked per hit — signature slot (S2/S3) then unique slot (P2/P3).
    private static void ApplyExtraSwingRider(Mobile attacker, Mobile defender, int damageGiven, ClauseType clause, short p2, short p3)
    {
        switch (clause)
        {
            case ClauseType.ExtraSwingSplash:
                {
                    if (damageGiven > 0)
                    {
                        Splash(attacker, defender, damageGiven * p2 / 100, p3 > 0 ? p3 : 3);
                    }

                    break;
                }
            case ClauseType.ExtraSwingManaLeech:
                {
                    LeechMana(defender, attacker, p2);
                    FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Mana Drain");
                    break;
                }
            case ClauseType.ExtraSwingHealBlock:
                {
                    CombatFxState.SetHealBlock(defender, TimeSpan.FromSeconds(p2 > 0 ? p2 : 3));
                    FloatingCombatText.ShowOffensiveStatus(defender, attacker, "Heal Block");
                    break;
                }
            case ClauseType.ExtraSwingElemental:
                {
                    ElementalProc(defender, attacker, damageGiven, p2, p2 == 1 ? "Burn" : "Shock");
                    break;
                }
        }
    }
}
