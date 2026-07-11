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
    // Coverage list for AbsorbForDefender: the BlockGrantsDrBurst window, the reflect clauses, the
    // blockFirstClause set, and RunBlockClause's switch below (ClauseDispatchCoverageTests).
    internal static readonly ClauseType[] HandledByWeaponBlock =
    {
        ClauseType.BlockGrantsDrBurst, ClauseType.ReflectFirstHit, ClauseType.ReflectHealBlock,
        ClauseType.BlockFirstHit, ClauseType.BlockRestoreStam, ClauseType.BlockDrainStam,
        ClauseType.BlockManaLeech, ClauseType.BlockElemental, ClauseType.BlockNextShotCrit,
        ClauseType.ExtraSwingOnParry
    };

    // Pre-AOS defensive absorb (Pallas). Called from BaseWeapon.AbsorbDamage; reads the DEFENDER's
    // wielded weapon (the block/parry belongs to the defender, not the attacking weapon).
    public static int AbsorbForDefender(Mobile attacker, Mobile defender, int damage)
    {
        if (defender.Weapon is not BaseWeapon weapon || weapon is not IVariantItem variant ||
            variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return damage;
        }

        var (root, rarity) = ResolveRootRarity(variant, weapon.Rarity);
        var clause = ClauseType.None;
        short p1 = 0, p2 = 0;

        if (variant.LegendaryId != 0 && LegendaryRegistry.TryGet(variant.LegendaryId, out var entry))
        {
            clause = entry.Clause;
            p1 = entry.P1;
            p2 = entry.P2;
        }

        var row = WeaponEffectTable.Get(root, rarity);
        var signature = row.Signature;

        // Phalanx/Eryma signature: a block armed earlier opened a brief DR window; while active,
        // reduce this incoming hit by the signature's S1%. P6: the window shares the §9.8 DR
        // budget with worn armor DR (applied later in AbsorbForDefenderArmor), so it is clamped
        // to the remaining headroom under the 12% cap — a suit already at the cap gains nothing.
        if (signature == ClauseType.BlockGrantsDrBurst && row.S1 > 0 &&
            WornEffectState.IsClauseBurstActive(defender, ClauseType.BlockGrantsDrBurst))
        {
            var drBurst = Math.Min((int)row.S1, WornEffectState.DrCap - WornEffectState.GetAggregate(defender).DrPct);

            if (drBurst > 0)
            {
                damage -= damage * drBurst / 100;
            }
        }

        // Reflect clauses (Gorgoneion / Helenos): reflect a % of the first hit taken and stagger,
        // optionally heal-blocking the attacker.
        if (clause is ClauseType.ReflectFirstHit or ClauseType.ReflectHealBlock &&
            CombatFxState.RegisterHitTaken(defender, out var reflectFirst) && reflectFirst)
        {
            var reflect = ReflectAmount(damage, p1 > 0 ? p1 : 20);

            if (reflect > 0)
            {
                AOS.Damage(attacker, defender, reflect, 100, 0, 0, 0, 0);
                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
            }

            if (clause == ClauseType.ReflectHealBlock)
            {
                CombatFxState.SetHealBlock(attacker, TimeSpan.FromSeconds(p2 > 0 ? p2 : 3));
                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Heal Block");
            }

            if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
            {
                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
            }

            PantheonFx.PlayWornProc(defender, root);

            return damage;
        }

        var blocked = row.BlockPct > 0 && Utility.Random(100) < row.BlockPct;

        // Block-first-hit clauses (Itonia line + rider variants): the first hit taken is always blocked.
        var blockFirstClause = clause is ClauseType.BlockFirstHit or ClauseType.BlockRestoreStam
            or ClauseType.BlockDrainStam or ClauseType.BlockManaLeech or ClauseType.BlockElemental
            or ClauseType.BlockNextShotCrit;

        if (blockFirstClause && CombatFxState.RegisterHitTaken(defender, out var blockFirst) && blockFirst)
        {
            blocked = true;
        }

        if (!blocked)
        {
            return damage;
        }

        FloatingCombatText.ShowSelfStatus(defender, "Block");

        // The block's mitigation is its DR-on-block; Uncommon Pallas lists block with no DR, so
        // its block is currently a no-op numerically (doc gap — flagged for tuning).
        if (row.BlockDrPct > 0)
        {
            damage -= damage * row.BlockDrPct / 100;
        }

        // Epic Pallas "thorns after block" is a melee-only follow-up — it never fires for a bow or
        // crossbow (07-archery.md §2), so gate it out when the defender is wielding a ranged weapon.
        if (row.BlockThorns && weapon is not BaseRanged)
        {
            AOS.Damage(attacker, defender, 5, 100, 0, 0, 0, 0); // small flat thorns
            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Thorns");
        }

        // Dual-clause seam: on-block riders from both the lane signature and the unique clause.
        RunBlockClause(signature, row.S1, row.S2, damage, attacker, defender);
        RunBlockClause(clause, p1, p2, damage, attacker, defender);

        // Aello/Elektor: a successful block answers with an immediate counter-swing.
        if (clause == ClauseType.ExtraSwingOnParry || signature == ClauseType.ExtraSwingOnParry)
        {
            CounterSwingOnParry(defender, attacker);
        }

        // Pantheon flourish on the blocker when their weapon legendary's clause rode this block.
        if (IsBlockClause(clause) || clause == ClauseType.ExtraSwingOnParry)
        {
            PantheonFx.PlayWornProc(defender, root);
        }

        return Math.Max(damage, 0);
    }

    // Aello/Elektor (ExtraSwingOnParry): a successful block/parry answers with an immediate
    // counter-swing from the defender's weapon. Shares the extra-swing depth guard so a counter
    // can never chain another counter (or ride an extra swing), and only fires while the attacker
    // is still inside the defender's weapon range — an archer blocked at distance draws no riposte.
    private static void CounterSwingOnParry(Mobile defender, Mobile attacker)
    {
        if (_extraSwingDepth > 0 || attacker is not { Alive: true } ||
            defender.Weapon is not BaseWeapon weapon ||
            !defender.InRange(attacker.Location, weapon.MaxRange) || !defender.CanBeHarmful(attacker, false))
        {
            return;
        }

        _extraSwingDepth++;

        try
        {
            // Same reduced-strength scalar as a granted extra swing — a riposte is a free hit too.
            defender.NextCombatTime = Core.TickCount +
                (int)weapon.OnSwing(defender, attacker, ExtraSwingDamageScalar).TotalMilliseconds;
        }
        finally
        {
            _extraSwingDepth--;
        }

        FloatingCombatText.ShowSelfStatus(defender, "Riposte");
    }

    // Reflect/thorns amount with a 1-damage floor (2026-07-11 balance pass): integer math made
    // every percentage reflect round to ZERO against small post-DR hits — thorns were dead
    // weight vs weak attackers. If a reflect effect is active and the hit dealt damage, it
    // always stings for at least 1.
    private static int ReflectAmount(int damage, int pct) =>
        damage <= 0 || pct <= 0 ? 0 : Math.Max(1, damage * pct / 100);

    // Dual-invoked on-block rider for one clause slot (runs only after a block landed).
    private static void RunBlockClause(ClauseType clause, short p1, short p2, int damage, Mobile attacker, Mobile defender)
    {
        switch (clause)
        {
            case ClauseType.BlockRestoreStam:
                {
                    var before = defender.Stam;
                    defender.Stam += defender.StamMax * (p1 > 0 ? p1 : 10) / 100;
                    FloatingCombatText.ShowRestore(defender, 'S', defender.Stam - before);
                    break;
                }
            case ClauseType.BlockDrainStam:
                {
                    attacker.Stam -= p1 > 0 ? p1 : 2;
                    FloatingCombatText.ShowOffensiveStatus(attacker, defender, "-Stam");
                    break;
                }
            case ClauseType.BlockManaLeech:
                {
                    LeechMana(attacker, defender, p1 > 0 ? p1 : 10);
                    FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Mana Drain");
                    break;
                }
            case ClauseType.BlockElemental:
                {
                    // Halved flat (see ElementalProc): a per-block passive punish, not a crit proc.
                    ElementalProc(attacker, defender, damage, p1, p1 == 1 ? "Burn" : "Shock", 5);
                    break;
                }
            case ClauseType.BlockNextShotCrit:
                {
                    CombatFxState.SetNextHitCrit(defender);
                    FloatingCombatText.ShowSelfStatus(defender, "Crit Ready");
                    break;
                }
            case ClauseType.BlockGrantsDrBurst: // Phalanx/Eryma signature: open a DR window
                {
                    WornEffectState.ArmClauseBurst(defender, ClauseType.BlockGrantsDrBurst, TimeSpan.FromSeconds(p2 > 0 ? p2 : 3));
                    break;
                }
        }
    }

    // P16 — bonus AR. Per-piece, not pooled (see WornAggregate's doc comment for the flagged
    // conflict with framework §9.8's suit-wide AR cap). Called from BaseArmor.ArmorRating.
    public static int GetBonusArmorRating(BaseArmor armor)
    {
        if (armor is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return 0;
        }

        var (root, rarity) = ResolveRootRarity(variant, armor.Rarity);
        return ArmorEffectTable.Get(root, rarity, armor is BaseShield).BonusAr;
    }

    // Armor/shield defensive package (Polias bulwark + Cyclopean forge + their legendary
    // riders). Runs after the weapon-side AbsorbForDefender in BaseWeapon.AbsorbDamage. Order
    // per the P3a brief: shrug roll (halved, + legendary shrug riders) -> DR% -> reflect/flame-proc.
    // Coverage list for AbsorbForDefenderArmor: the guaranteed-shrug set, the shrug-rider switch,
    // the ShrugFirstHitDrBurst DR window, reflect-boost/crit-stun, the flame-proc chance modifiers
    // and post-proc switch, and the EmergencyRegenTick check.
    internal static readonly ClauseType[] HandledByArmorDefense =
    {
        ClauseType.ShrugFirstHitGuaranteed, ClauseType.ShrugFirstHitPoisonAttacker,
        ClauseType.ShrugFirstHitDrainStam, ClauseType.ShrugFirstHitDrBurst, ClauseType.ShrugStunAttacker,
        ClauseType.ShrugReflect, ClauseType.ShrugReflectStun, ClauseType.HitHalvedRegenPulse,
        ClauseType.HitHalvedDurabilityImmunity, ClauseType.HitHalvedResistBurst, ClauseType.ReflectBoostFirstHit,
        ClauseType.ReflectCritStun, ClauseType.FlameProcDoubleFirstHit, ClauseType.FlameProcBoostLowHp,
        ClauseType.FlameProcDoubleLowDurability, ClauseType.FlameProcEveryN, ClauseType.FlameProcHealBlock,
        ClauseType.FlameProcPoison, ClauseType.FlameProcSplash, ClauseType.EmergencyRegenTick
    };

    public static int AbsorbForDefenderArmor(Mobile attacker, Mobile defender, int damage)
    {
        var agg = WornEffectState.GetAggregate(defender);
        var legendaries = WornEffectState.GetLegendaries(defender);

        if (agg is { ShrugPct: 0, DrPct: 0, ReflectPct: 0, FlameProcPct: 0, FrenzyChancePct: 0, HasCapstone: false } &&
            legendaries.Count == 0)
        {
            return damage;
        }

        CombatFxState.RegisterHitTaken(defender, out var firstHit, out var hitCount);

        // ---- shrug ----
        var shrugged = agg.ShrugPct > 0 && Utility.Random(100) < agg.ShrugPct;

        for (var i = 0; i < legendaries.Count; i++)
        {
            // Kadmos / Nemea, plus the Option A chest signatures — all guarantee the first shrug;
            // the chest signatures' riders dispatch below once the shrug lands.
            if (firstHit && legendaries[i].Clause is ClauseType.ShrugFirstHitGuaranteed
                or ClauseType.ShrugFirstHitPoisonAttacker or ClauseType.ShrugFirstHitDrainStam
                or ClauseType.ShrugFirstHitDrBurst)
            {
                shrugged = true;
            }
        }

        // Pantheon flourish root for this absorb — set by whichever armor clause acts below,
        // played once at the end (PantheonFx throttles repeats anyway).
        var fxRoot = VariantRoot.None;

        if (shrugged)
        {
            damage /= 2;
            // "Shrugged" is folded into this hit's damage number by BaseWeapon.OnHit, which consumes
            // this flag right before the main AOS.Damage. Only a plain bool is set here — the display
            // context is applied later in OnHit, so the riders' own reflect/flame damage below never
            // inherits the shrug label.
            _pendingShrugDisplay = true;

            for (var i = 0; i < legendaries.Count; i++)
            {
                var entry = legendaries[i];

                switch (entry.Clause)
                {
                    case ClauseType.ShrugStunAttacker: // Kekrops / Kithairon
                        {
                            if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
                            {
                                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
                            }

                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.ShrugReflect: // Erechtheus / Erymanthos
                        {
                            var reflected = ReflectAmount(damage, entry.P1);

                            if (reflected > 0)
                            {
                                AOS.Damage(attacker, defender, reflected, 100, 0, 0, 0, 0);
                                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
                            }

                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.ShrugReflectStun: // Plate Arms signature
                        {
                            var reflected = ReflectAmount(damage, entry.P1);

                            if (reflected > 0)
                            {
                                AOS.Damage(attacker, defender, reflected, 100, 0, 0, 0, 0);
                                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
                            }

                            if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
                            {
                                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
                            }

                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.ShrugFirstHitPoisonAttacker when firstHit: // Studded Chest signature
                        {
                            attacker.ApplyPoison(defender, Poison.Lesser);
                            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Poisoned", FloatingCombatText.PoisonHue);
                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.ShrugFirstHitDrainStam when firstHit: // Bone Chest signature
                        {
                            attacker.Stam -= entry.P1 > 0 ? entry.P1 : 5;
                            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "-Stam");
                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.ShrugFirstHitDrBurst when firstHit: // Ringmail Chest signature
                        {
                            WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 3));
                            FloatingCombatText.ShowSelfStatus(defender, "Fortified");
                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.HitHalvedRegenPulse: // Ananke
                        {
                            WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 3));
                            fxRoot = entry.Root;
                            break;
                        }
                    case ClauseType.HitHalvedDurabilityImmunity: // Nemesis — burst armed correctly;
                        // NOT wired into every durability-loss path (armor/weapon/clothing each
                        // reduce HP independently across several files) — flagged in the P3b report.
                        {
                            WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 3));
                            break;
                        }
                    case ClauseType.HitHalvedResistBurst: // Themis
                        {
                            WornEffectState.ArmClauseBurst(defender, entry.Clause, TimeSpan.FromSeconds(entry.P2 > 0 ? entry.P2 : 3));
                            FloatingCombatText.ShowSelfStatus(defender, "Warded");
                            fxRoot = entry.Root;
                            break;
                        }
                }
            }
        }

        // Maenad: a % chance any hit taken triggers a short frenzy buff (+dmg%, Epic +swing%).
        if (agg.FrenzyChancePct > 0 && Utility.Random(100) < agg.FrenzyChancePct)
        {
            CombatFxState.ArmFrenzy(defender, agg.FrenzyDamagePct, agg.FrenzySwingPct, TimeSpan.FromSeconds(5));
            FloatingCombatText.ShowSelfStatus(defender, "Frenzy");
        }

        // ---- damage reduction ----
        var drPct = agg.DrPct;

        // Ward-Surge (chainmail set capstone): while the crit-taken window is open, DR rises to
        // the suit-wide cap (the +12% burst, pre-clamped per plan cap-fix #2).
        if (agg is { HasCapstone: true, CapstoneMaterial: ArmorMaterialType.Chainmail } &&
            CombatFxState.IsWardSurgeActive(defender))
        {
            drPct = WornEffectState.DrCap;
        }

        // Ringmail Chest signature (ShrugFirstHitDrBurst): while the first-hit window is open,
        // later hits gain +P1% DR, clamped to the suit-wide cap (P6). The arming hit itself
        // (firstHit) is excluded — it was already halved by the shrug.
        if (!firstHit)
        {
            for (var i = 0; i < legendaries.Count; i++)
            {
                var entry = legendaries[i];

                if (entry.Clause == ClauseType.ShrugFirstHitDrBurst && entry.P1 > 0 &&
                    WornEffectState.IsClauseBurstActive(defender, ClauseType.ShrugFirstHitDrBurst))
                {
                    drPct = Math.Min(drPct + entry.P1, WornEffectState.DrCap);
                }
            }
        }

        if (drPct > 0)
        {
            damage -= damage * drPct / 100;
        }

        // ---- reflect (Cyclopean thorns) ----
        var reflectPct = agg.ReflectPct;

        // Phalanx-Thorns (ringmail set capstone): +8% reflect on every hit taken, clamped to the
        // suit-wide reflect cap. The plan frames it as a burst armed by the hit; since the burst
        // would be re-armed by every hit and reflect only matters while being hit, the inline
        // bonus is behaviorally identical with no burst state.
        if (agg is { HasCapstone: true, CapstoneMaterial: ArmorMaterialType.Ringmail })
        {
            reflectPct = Math.Min(reflectPct + 8, WornEffectState.ReflectCap);
        }

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.ReflectBoostFirstHit && firstHit)
            {
                reflectPct = Math.Max(reflectPct, legendaries[i].P1); // Proitos
                fxRoot = legendaries[i].Root;
            }
        }

        if (reflectPct > 0 && damage > 0)
        {
            var reflectDamage = ReflectAmount(damage, reflectPct);

            if (reflectDamage > 0)
            {
                AOS.Damage(attacker, defender, reflectDamage, 100, 0, 0, 0, 0);
                FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Reflect");
            }

            if (ConsumePendingHitCrit())
            {
                for (var i = 0; i < legendaries.Count; i++)
                {
                    if (legendaries[i].Clause == ClauseType.ReflectCritStun) // Akrisios
                    {
                        if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
                        {
                            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Stunned");
                        }

                        fxRoot = legendaries[i].Root;
                    }
                }
            }
        }

        // ---- flame-burst proc (Cyclopean) ----
        var procChance = agg.FlameProcPct;

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.FlameProcDoubleFirstHit && firstHit)
            {
                procChance *= 2; // Perdix / Teumessos
            }
            else if (entry.Clause == ClauseType.FlameProcBoostLowHp &&
                     IsUnderHpFraction(defender, (entry.P2 > 0 ? entry.P2 : 30) / 100.0))
            {
                procChance = Math.Max(procChance, entry.P1); // Talos
            }
            else if (entry.Clause == ClauseType.FlameProcDoubleLowDurability &&
                     defender.FindItemOnLayer<BaseShield>(Layer.TwoHanded) is { MaxHitPoints: > 0 } shield &&
                     shield.HitPoints * 100 / shield.MaxHitPoints < (entry.P1 > 0 ? entry.P1 : 50))
            {
                procChance *= 2; // Amphion
            }
        }

        var procced = procChance > 0 && Utility.Random(100) < procChance;

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.FlameProcEveryN)
            {
                var n = legendaries[i].P1 > 0 ? legendaries[i].P1 : 5;

                if (hitCount % n == 0) // Tiryns — guaranteed proc every Nth hit taken
                {
                    procced = true;
                }
            }
        }

        if (procced)
        {
            // Cyclopean flame-burst is retaliation: the fire hits the ATTACKER (per clause text),
            // not the defender who was struck. (Previously targeted `defender` — self-damage bug.)
            ElementalProc(attacker, defender, damage, 1, "Burn"); // fire

            for (var i = 0; i < legendaries.Count; i++)
            {
                var entry = legendaries[i];

                switch (entry.Clause)
                {
                    case ClauseType.FlameProcHealBlock: // Erichthonios
                        {
                            CombatFxState.SetHealBlock(attacker, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 3));
                            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Heal Block");
                            break;
                        }
                    case ClauseType.FlameProcPoison: // Khimaira
                        {
                            attacker.ApplyPoison(defender, Poison.Lesser);
                            FloatingCombatText.ShowOffensiveStatus(attacker, defender, "Poisoned", FloatingCombatText.PoisonHue);
                            break;
                        }
                    case ClauseType.FlameProcSplash: // Echidna
                        {
                            Splash(defender, attacker, Math.Max(1, damage / 4), 1 + entry.P1);
                            break;
                        }
                }
            }
        }

        // Keryneia: once per fight, a hit that would drop the wearer under the threshold instead
        // fires an immediate emergency regen tick (a flat chunk of max HP).
        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            if (entry.Clause == ClauseType.EmergencyRegenTick && defender.HitsMax > 0 &&
                defender.Hits - damage < defender.HitsMax * (entry.P1 > 0 ? entry.P1 : 10) / 100 &&
                WornEffectState.TryUseOncePerFight(defender, entry.Clause, firstHit))
            {
                var gain = Math.Max(1, defender.HitsMax / 10);
                defender.Hits += gain;
                FloatingCombatText.ShowRestore(defender, 'L', gain);
                fxRoot = entry.Root;
            }
        }

        // Pantheon flourish on the wearer for whichever armor clause acted this absorb. The
        // Cyclopean flame proc is deliberately excluded — its retaliation fire is already its FX.
        if (fxRoot != VariantRoot.None)
        {
            PantheonFx.PlayWornProc(defender, fxRoot);
        }

        return Math.Max(damage, 0);
    }

    // ---- Shield parry hooks (Aegis bulwark) — pre-AOS Parry-skill path, BaseShield.OnHit ----

    // Additive Aegis parry% + guaranteed-parry clause riders. Called from BaseShield.OnHit
    // before the Parry skill check. Registers this hit-taken for the owner so "first hit of
    // fight" and "every Nth parry" clauses share the same counter the armor absorb step uses.
    // Coverage list for the shield-parry path: AdjustShieldParryChance's guaranteed-parry checks
    // (incl. the IsBlockClause set gated by IsShieldSourced) + FirstHitNoSecondaryEffect, and
    // OnShieldParried's rider switch below.
    internal static readonly ClauseType[] HandledByShieldParry =
    {
        ClauseType.ParryFirstHitGuaranteed, ClauseType.ParryFirstHitGuaranteedStun, ClauseType.BlockFirstHit,
        ClauseType.BlockRestoreStam, ClauseType.BlockDrainStam, ClauseType.BlockManaLeech,
        ClauseType.BlockElemental, ClauseType.BlockNextShotCrit, ClauseType.LowHpGuaranteedParry,
        ClauseType.FirstHitNoSecondaryEffect, ClauseType.ParryExtraReflect, ClauseType.ParryCritStun,
        ClauseType.ParryRepairsEveryN, ClauseType.SelfRepairBurstOnCritBlock, ClauseType.ParryRestoresStam,
        ClauseType.ExtraSwingOnParry
    };

    public static double AdjustShieldParryChance(Mobile owner, double chance)
    {
        var agg = WornEffectState.GetAggregate(owner);
        var legendaries = WornEffectState.GetLegendaries(owner);

        CombatFxState.RegisterHitTaken(owner, out var firstHit, out _);

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            // "Block" on a shield is the parry event (re-theme: Probolos/Pnoe lines carry Block*
            // clauses). Only REAL shield legendaries (Id != 0) grant the first-hit guarantee —
            // lane signatures (synthetic, Id 0) contribute their on-parry rider only, and
            // weapon-sourced Block* entries keep dispatching through AbsorbForDefender instead.
            var guaranteedFirst = firstHit &&
                (entry.Clause is ClauseType.ParryFirstHitGuaranteed or ClauseType.ParryFirstHitGuaranteedStun ||
                 entry.Id != 0 && IsShieldSourced(entry) && IsBlockClause(entry.Clause));

            var guaranteedLowHp = entry.Clause == ClauseType.LowHpGuaranteedParry && owner.HitsMax > 0 &&
                owner.Hits < owner.HitsMax * (entry.P1 > 0 ? entry.P1 : 30) / 100;

            if (guaranteedFirst || guaranteedLowHp)
            {
                PantheonFx.PlayWornProc(owner, entry.Root);
                return 1.0; // Ankyle / Aias / Abderos / Kerberos / Sakos
            }

            // Hyperbios: the first hit taken each fight applies no secondary effect (mark/
            // poison). Narrowly scoped to the weapon-side Agrotera mark/poison path (ApplyMark)
            // — a fully general "no secondary effect" across every subsystem is out of scope.
            if (entry.Clause == ClauseType.FirstHitNoSecondaryEffect && firstHit)
            {
                WornEffectState.ArmSecondaryEffectSuppression(owner);
            }
        }

        return agg.ParryPct > 0 ? chance + agg.ParryPct / 100.0 : chance;
    }

    // Runs after a successful parry: Aegis DR-on-parry, thorns/extra-reflect, and the
    // durability/stun/self-repair-burst riders. `attacker` may be null if the incoming weapon
    // has no wielder on record — riders that need it simply no-op in that edge case.
    public static int OnShieldParried(BaseShield shield, Mobile attacker, Mobile owner, int damage)
    {
        var agg = WornEffectState.GetAggregate(owner);
        var legendaries = WornEffectState.GetLegendaries(owner);

        // "Parried" is folded into this hit's surviving damage number by BaseWeapon.OnHit.
        _pendingParryDisplay = true;

        if (agg.ParryDrPct > 0)
        {
            damage -= damage * agg.ParryDrPct / 100;
        }

        var extraReflectPct = 0;

        for (var i = 0; i < legendaries.Count; i++)
        {
            if (legendaries[i].Clause == ClauseType.ParryExtraReflect)
            {
                extraReflectPct = legendaries[i].P1; // Salamis
            }
        }

        if ((agg.ParryThorns || extraReflectPct > 0) && attacker != null)
        {
            // 5% base thorns + rider, floored to 1 so a parried weak hit still stings.
            var reflect = ReflectAmount(Math.Max(1, damage), 5 + extraReflectPct);

            if (reflect > 0)
            {
                AOS.Damage(attacker, owner, reflect, 100, 0, 0, 0, 0);
                FloatingCombatText.ShowOffensiveStatus(attacker, owner, "Reflect");
            }
        }

        var crit = ConsumePendingHitCrit();
        var fxRoot = VariantRoot.None;

        for (var i = 0; i < legendaries.Count; i++)
        {
            var entry = legendaries[i];

            switch (entry.Clause)
            {
                case ClauseType.ParryCritStun when crit && attacker != null: // Oiliades
                    {
                        if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
                        {
                            FloatingCombatText.ShowOffensiveStatus(attacker, owner, "Stunned");
                        }

                        fxRoot = entry.Root;
                        break;
                    }
                case ClauseType.ParryFirstHitGuaranteedStun when attacker != null: // Aias
                    {
                        if (CombatFxState.TryStun(attacker, TimeSpan.FromSeconds(1)))
                        {
                            FloatingCombatText.ShowOffensiveStatus(attacker, owner, "Stunned");
                        }

                        fxRoot = entry.Root;
                        break;
                    }
                case ClauseType.ParryRepairsEveryN: // Telamon
                    {
                        var n = entry.P1 > 0 ? entry.P1 : 10;

                        CombatFxState.RegisterHitTaken(owner, out _, out var count);

                        if (count % n == 0 && shield.HitPoints < shield.MaxHitPoints)
                        {
                            shield.HitPoints++;
                            FloatingCombatText.ShowSelfStatus(owner, "Repair");
                            fxRoot = entry.Root;
                        }

                        break;
                    }
                case ClauseType.SelfRepairBurstOnCritBlock when crit: // Zethos
                    {
                        WornEffectState.ArmClauseBurst(owner, entry.Clause, TimeSpan.FromSeconds(entry.P1 > 0 ? entry.P1 : 5));
                        fxRoot = entry.Root;
                        break;
                    }
                case ClauseType.ParryRestoresStam: // Aegis-line signature: restore P1% max stam on parry
                    {
                        var before = owner.Stam;
                        owner.Stam = Math.Min(owner.StamMax, owner.Stam + owner.StamMax * (entry.P1 > 0 ? entry.P1 : 10) / 100);
                        FloatingCombatText.ShowRestore(owner, 'S', owner.Stam - before);
                        break;
                    }
                case ClauseType.ExtraSwingOnParry when attacker != null: // Aello / Elektor
                    {
                        // The wielder's weapon clause riding the shield parry: answer with an
                        // immediate counter-swing (depth-guarded, range-gated).
                        CounterSwingOnParry(owner, attacker);
                        fxRoot = entry.Root;
                        break;
                    }
                case ClauseType.BlockRestoreStam or ClauseType.BlockDrainStam or ClauseType.BlockManaLeech
                    or ClauseType.BlockElemental or ClauseType.BlockNextShotCrit
                    when IsShieldSourced(entry) && attacker != null:
                    {
                        // Shield-held Block* clauses ride the parry (re-theme: Probolos line,
                        // Aiakos, Probolos' BlockElemental signature). Same rider semantics as
                        // the weapon-side block path.
                        RunBlockClause(entry.Clause, entry.P1, entry.P2, damage, attacker, owner);
                        fxRoot = entry.Root;
                        break;
                    }
            }
        }

        // Pantheon flourish on the parrier for whichever legendary clause rode this parry.
        if (fxRoot != VariantRoot.None)
        {
            PantheonFx.PlayWornProc(owner, fxRoot);
        }

        return Math.Max(damage, 0);
    }

    // A worn-list entry came from a shield if it is a real shield-family legendary or a
    // synthetic lane-signature entry whose root is one of the five shield roots.
    internal static bool IsShieldSourced(in LegendaryEntry entry) =>
        entry.Family == LegendaryRegistry.FamilyShields ||
        entry.Root is VariantRoot.Aegis or VariantRoot.Amyntor or VariantRoot.Probolos
            or VariantRoot.Herkos or VariantRoot.Pnoe;

    private static bool IsBlockClause(ClauseType clause) =>
        clause is ClauseType.BlockFirstHit or ClauseType.BlockRestoreStam or ClauseType.BlockDrainStam
            or ClauseType.BlockManaLeech or ClauseType.BlockElemental or ClauseType.BlockNextShotCrit;
}
