using System;
using System.Collections.Generic;
using System.Linq;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// PERMANENT guard (replaced the throwaway audit dump, 2026-07-12). Every legendary item dispatches
// BOTH its lane/slot SIGNATURE clause and its own UNIQUE clause on the same physical item, so the
// two must never double-apply the same resource on one event. This test enumerates every legendary
// against every signature it can be co-resident with — weapons: the single weapon-lane signature;
// armor: EVERY slot signature of its material (a legendary rolls onto a random slot, so any slot's
// signature is a possible pairing); shields: the shield-lane signature — and asserts two invariants.
//
// (1) Signature clause TYPE != unique clause TYPE. A clause equal to a co-resident signature would
//     dispatch twice on one item (the worn-clause loop fires it once per source).
//
// (2) Resource-disjointness. A small, explicit per-clause resource-tag map (below) tags ONLY the
//     resources the 2026-07-12 de-overlap pass separated: Pen, SplashDamage, SpellDr, ResistSkill,
//     SelfStamRestore (self-stamina on a defense event), ReflectBoost, HpRegenBurst. No signature/
//     unique pair on one item may share a tag. ARMING clauses (crit / extra-swing / block / parry /
//     shrug / dodge arming — "the first hit is a guaranteed crit", "always shrug the first hit",
//     etc.) carry NO tag on purpose: they set an idempotent boolean/next-hit flag, so two of them on
//     one item bool-OR to the same single effect and stacking is harmless by design. Only true
//     accumulating resources (armor-pen %, splash %, spell-DR %, a stamina refund, an HP-regen
//     multiplier) are tagged, because those are what actually double when two sources apply them.
[Collection("Sequential UOContent Tests")]
public class LegendaryClauseStackGuardTests
{
    // Per-clause resource tag. A pair of clauses sharing a tag would double-apply that resource if
    // both landed on one item and fired on the same event. Only the resources de-overlapped by the
    // 2026-07-12 pass are tagged; everything else (arming flags, distinct resources) is untagged.
    private static readonly IReadOnlyDictionary<ClauseType, string> ResourceTag = new Dictionary<ClauseType, string>
    {
        // Pen — armor penetration folded into the same landed hit.
        [ClauseType.NthHitFullArmorPen] = "Pen",
        [ClauseType.CritArmorPen]       = "Pen",

        // SplashDamage — AoE splash damage carried by the hit / extra swing.
        [ClauseType.CritSplash]          = "SplashDamage",
        [ClauseType.NthHitSplash]        = "SplashDamage",
        [ClauseType.ExtraSwingSplash]    = "SplashDamage",
        [ClauseType.RampMaxStacksSplash] = "SplashDamage",

        // SpellDr — spell damage-reduction buffs.
        [ClauseType.SpellDrBoostFirstHit]    = "SpellDr",
        [ClauseType.SpellDrBurstOnCritTaken] = "SpellDr",
        [ClauseType.ParaResistBoostsSpellDr] = "SpellDr",
        [ClauseType.SpellDrVsPoisonDot]      = "SpellDr",

        // ResistSkill — Resisting Spells skill boosts.
        [ClauseType.ParaResistBoostsResistSkill] = "ResistSkill",
        [ClauseType.ResistSkillDoubleLowHp]      = "ResistSkill",
        [ClauseType.ResistSkillBoostLowHp]       = "ResistSkill",

        // SelfStamRestore — self-stamina restored on a defense event (block / parry / dodge).
        [ClauseType.DodgeRefundStam]   = "SelfStamRestore",
        [ClauseType.BlockRestoreStam]  = "SelfStamRestore",
        [ClauseType.ParryRestoresStam] = "SelfStamRestore",

        // ReflectBoost — "reflect % rises vs the first hit" resource.
        [ClauseType.ReflectBoostFirstHit] = "ReflectBoost",

        // HpRegenBurst — "HP regen multiplies after taking a crit" resource.
        [ClauseType.HpRegenBurstOnCritTaken] = "HpRegenBurst",
    };

    // Arming-group membership (2026-07-12 full arming-group split). Unlike ResourceTag — which only
    // catches accumulating resources — this tags the EFFECT-FAMILY LANE a clause belongs to. Two
    // clauses in the same group are "the same trick" (both crit riders, both mark riders, both shrug
    // riders, …), and the split's rule is that a legendary's slot/lane SIGNATURE and its own UNIQUE
    // clause must never share a lane: their group-sets must be disjoint. A clause may belong to
    // several groups (e.g. DoubleStrikeEveryN is CRIT + EXTRA_SWING); collision = ANY shared group.
    // Clauses absent from the map carry no lane (utility/worn passives) and are always safe targets.
    // Built once from the per-group member lists so multi-group clauses accumulate their tags.
    private static readonly IReadOnlyDictionary<string, ClauseType[]> ArmingGroupMembers =
        new Dictionary<string, ClauseType[]>
        {
            ["CRIT"] = new[]
            {
                ClauseType.CritFirstHit, ClauseType.CritFirstHitStamRefund, ClauseType.CritEveryN,
                ClauseType.CritSplash, ClauseType.CritArmorPen, ClauseType.CritExecuteUnder15,
                ClauseType.CritPoisonTick, ClauseType.CritStagger, ClauseType.CritElemental,
                ClauseType.CritManaLeech, ClauseType.CritHealBlock, ClauseType.CritFullHpDouble,
                ClauseType.DoubleStrikeEveryN, ClauseType.LifestealOnCrit, ClauseType.StamDrainOnCrit
            },
            ["EXTRA_SWING"] = new[]
            {
                ClauseType.ExtraSwingEveryN, ClauseType.ExtraSwingFirstHit, ClauseType.ExtraSwingSplash,
                ClauseType.ExtraSwingGuaranteedHit, ClauseType.ExtraSwingManaLeech, ClauseType.ExtraSwingElemental,
                ClauseType.ExtraSwingHealBlock, ClauseType.ExtraSwingStackingHit, ClauseType.ExtraSwingChain,
                ClauseType.ExtraSwingOnParry, ClauseType.DoubleStrikeEveryN
            },
            ["MARK"] = new[]
            {
                ClauseType.MarkFirstHit, ClauseType.MarkOnCrit, ClauseType.MarkAllSources25,
                ClauseType.MarkSpreadOnDeath, ClauseType.MarkNearbyAllies, ClauseType.MarkHealBlock,
                ClauseType.MarkManaLeech, ClauseType.MarkElemental, ClauseType.MarkHealBlockFirstHit,
                ClauseType.PoisonTickDoubled, ClauseType.PoisonedTargetsMarked
            },
            ["BLOCK_PARRY"] = new[]
            {
                ClauseType.BlockFirstHit, ClauseType.BlockRestoreStam, ClauseType.BlockDrainStam,
                ClauseType.BlockManaLeech, ClauseType.BlockElemental, ClauseType.BlockNextShotCrit,
                ClauseType.BlockGrantsDrBurst, ClauseType.ReflectFirstHit, ClauseType.ReflectHealBlock,
                ClauseType.ParryFirstHitGuaranteed, ClauseType.ParryFirstHitGuaranteedStun, ClauseType.ParryCritStun,
                ClauseType.ParryExtraReflect, ClauseType.ParryForcesMissEveryN, ClauseType.LowHpGuaranteedParry,
                ClauseType.ParryRestoresStam, ClauseType.ReflectBurstOnCritBlock, ClauseType.ReflectBoostFirstHit,
                ClauseType.ReflectCritStun, ClauseType.BlockRestoresHp, ClauseType.ExtraSwingOnParry
            },
            ["SHRUG"] = new[]
            {
                ClauseType.ShrugStunAttacker, ClauseType.ShrugReflect, ClauseType.ShrugFirstHitGuaranteed,
                ClauseType.ShrugFirstHitPoisonAttacker, ClauseType.ShrugFirstHitDrainStam, ClauseType.ShrugFirstHitDrBurst,
                ClauseType.ShrugReflectStun, ClauseType.HitHalvedRegenPulse, ClauseType.HitHalvedReflectSpared,
                ClauseType.HitHalvedResistBurst
            },
            ["DODGE"] = new[]
            {
                ClauseType.DodgeRefundStam, ClauseType.DodgeRestoreMana, ClauseType.DodgeRegenBurst,
                ClauseType.DodgeRefundStamPct, ClauseType.DodgeSnare, ClauseType.DodgeGrantsCounterWindow,
                ClauseType.DodgeDoubleFirstAttack, ClauseType.OnKillDodgeDoubleDuration, ClauseType.LowHpDodgeBurst
            },
            ["REGEN_BURST"] = new[]
            {
                ClauseType.HpRegenBurstOnCritTaken, ClauseType.DodgeRegenBurst, ClauseType.HitHalvedRegenPulse,
                ClauseType.RegenDoubleAfterPotion, ClauseType.RegenDoubleWhileHidden, ClauseType.EmergencyRegenTick,
                ClauseType.OnKillStamRegenBurstStacking, ClauseType.StationaryRegenFaster
            },
            ["SPELL_DR"] = new[]
            {
                ClauseType.SpellDrBoostFirstHit, ClauseType.SpellDrBurstOnCritTaken, ClauseType.ParaResistBoostsSpellDr,
                ClauseType.SpellDrVsPoisonDot, ClauseType.HitHalvedResistBurst, ClauseType.LightningProcResistBurst,
                ClauseType.ManaLeechResistBurst, ClauseType.ParaResistStunsAttacker, ClauseType.FirstParaAutoFails,
                ClauseType.RerollFirstResist
            },
            ["RESIST_SKILL"] = new[]
            {
                ClauseType.ParaResistBoostsResistSkill, ClauseType.ResistSkillDoubleLowHp, ClauseType.ResistSkillBoostLowHp
            },
            // FlameProcStacksHeat listed in the brief has no enum member (dropped with the heat model);
            // the six live flame-proc riders are the whole FLAME lane.
            ["FLAME"] = new[]
            {
                ClauseType.FlameProcDoubleFirstHit, ClauseType.FlameProcHealBlock, ClauseType.FlameProcBoostLowHp,
                ClauseType.FlameProcPoison, ClauseType.FlameProcSplash, ClauseType.FlameProcEveryN
            },
            ["ON_KILL"] = new[]
            {
                ClauseType.OnKillRestore, ClauseType.OnKillRestoreMissingHpPct, ClauseType.OnKillRestoreExtraHp,
                ClauseType.OnKillRestoreHpPct, ClauseType.OnKillStamRestoreExtendImmunity, ClauseType.OnKillFullManaRestore,
                ClauseType.OnKillTriggerHeldPotion, ClauseType.OnKillFullStamNextHitCrit, ClauseType.OnKillDodgeDoubleDuration,
                ClauseType.OnKillStamRegenBurstStacking
            }
        };

    // ClauseType -> set of arming groups it belongs to (accumulated across ArmingGroupMembers).
    private static readonly IReadOnlyDictionary<ClauseType, HashSet<string>> ArmingGroups = BuildArmingGroups();

    private static Dictionary<ClauseType, HashSet<string>> BuildArmingGroups()
    {
        var map = new Dictionary<ClauseType, HashSet<string>>();

        foreach (var (group, clauses) in ArmingGroupMembers)
        {
            foreach (var clause in clauses)
            {
                if (!map.TryGetValue(clause, out var set))
                {
                    set = new HashSet<string>();
                    map[clause] = set;
                }

                set.Add(group);
            }
        }

        return map;
    }

    // (1) No legendary's slot/lane signature clause TYPE may equal its unique clause TYPE.
    [Fact]
    public void SignatureTypeNeverEqualsUniqueType()
    {
        var violators = new List<string>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            foreach (var (slot, sig, _, _, _) in ResolveSignatures(entry))
            {
                if (sig != ClauseType.None && sig == entry.Clause)
                {
                    violators.Add($"#{entry.Id} {entry.Name} (fam {entry.Family}, {slot}): {sig}");
                }
            }
        }

        Assert.True(violators.Count == 0, "signature type == unique type on:\n" + string.Join("\n", violators));
    }

    // (2) No co-resident signature/unique pair may share a tagged resource.
    [Fact]
    public void SignatureAndUniqueShareNoTaggedResource()
    {
        var violators = new List<string>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (!ResourceTag.TryGetValue(entry.Clause, out var uniqueTag))
            {
                continue; // untagged unique cannot collide on a tagged resource
            }

            foreach (var (slot, sig, _, _, _) in ResolveSignatures(entry))
            {
                if (sig != ClauseType.None && ResourceTag.TryGetValue(sig, out var sigTag) && sigTag == uniqueTag)
                {
                    violators.Add($"#{entry.Id} {entry.Name} (fam {entry.Family}, {slot}): {sig} + {entry.Clause} both tag '{uniqueTag}'");
                }
            }
        }

        Assert.True(violators.Count == 0, "signature/unique share a tagged resource on:\n" + string.Join("\n", violators));
    }

    // (3) Arming-group disjointness (2026-07-12 full split). For every legendary and every co-resident
    // signature, the signature's arming-group set and the unique clause's arming-group set must share
    // NO group. This is stricter than (1)/(2): it forbids a unique from being the same effect-family
    // lane as a co-resident signature even when the exact ClauseType differs and no accumulating
    // resource is shared (e.g. a crit-lane weapon signature paired with any other crit rider unique).
    [Fact]
    public void SignatureAndUniqueShareNoArmingGroup()
    {
        var violators = new List<string>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (!ArmingGroups.TryGetValue(entry.Clause, out var uniqueGroups))
            {
                continue; // ungrouped unique (utility/worn passive) can never collide on a lane
            }

            foreach (var (slot, sig, _, _, _) in ResolveSignatures(entry))
            {
                if (sig == ClauseType.None || !ArmingGroups.TryGetValue(sig, out var sigGroups))
                {
                    continue;
                }

                if (sigGroups.Overlaps(uniqueGroups))
                {
                    var shared = string.Join(",", sigGroups.Intersect(uniqueGroups));
                    violators.Add(
                        $"#{entry.Id} {entry.Name} (fam {entry.Family}, {slot}): sig {sig} + unique {entry.Clause} share [{shared}]"
                    );
                }
            }
        }

        Assert.True(violators.Count == 0, "signature/unique share an arming group on:\n" + string.Join("\n", violators));
    }

    // ---- Signature resolution, mirroring production display/dispatch ---------------------------
    // Returns (slotLabel, signature, s1, s2, s3) tuples; armor yields one per material slot, so a
    // legendary is checked against every slot signature it could roll onto.
    private static IEnumerable<(string, ClauseType, short, short, short)> ResolveSignatures(LegendaryEntry entry)
    {
        if (entry.Family <= LegendaryRegistry.FamilyArchery)
        {
            var row = WeaponEffectTable.Get(entry.Root, ItemRarity.Legendary);
            yield return ("weapon", row.Signature, row.S1, row.S2, row.S3);
            yield break;
        }

        if (entry.Family == LegendaryRegistry.FamilyShields)
        {
            var row = ArmorEffectTable.Get(entry.Root, ItemRarity.Legendary, isShield: true);
            yield return ("shield", row.Signature, row.S1, row.S2, row.S3);
            yield break;
        }

        if (entry.Family is LegendaryRegistry.FamilyMetalArmor or LegendaryRegistry.FamilyLightArmor)
        {
            var families = entry.Family == LegendaryRegistry.FamilyMetalArmor
                ? FamilyRegistry.MetalArmorFamilies
                : FamilyRegistry.LightArmorFamilies;
            var def = families[entry.BaseIndex]; // BaseIndex = material ladder index for armor

            foreach (var factory in def.SlotFactories)
            {
                var item = factory();

                try
                {
                    if (item is BaseArmor armor)
                    {
                        var (sig, s1, s2, s3) = ArmorSlotSignatureTable.Get(armor.MaterialType, armor.BodyPosition);
                        yield return ($"{armor.MaterialType}/{armor.BodyPosition}", sig, s1, s2, s3);
                    }
                }
                finally
                {
                    item.Delete();
                }
            }

            yield break;
        }

        // Jewelry / clothing: AccessoryEffectRow carries no signature.
        yield return ("accessory", ClauseType.None, 0, 0, 0);
    }
}
