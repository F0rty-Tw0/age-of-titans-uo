using System;
using System.Collections.Generic;
using System.Linq;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// Guards the hand-written ClauseType dispatch switches against silent drift. Adding a clause to
// the ClauseType enum and forgetting to dispatch it (or declaring its ClauseTraits flags but
// leaving it out of the "HandledBy*" list beside the switch) compiles clean and does nothing at
// runtime — these tests turn that into a loud, named failure instead.
//
// The map below is the single wiring point: each dispatch trigger flag points at the "HandledBy*"
// coverage lists kept literally beside the switches that implement it (in RarityEffects.*.cs /
// WornEffectState.cs). The tests then require, in BOTH directions, that the clauses declaring a
// flag are exactly the clauses named in that flag's lists.
public class ClauseDispatchCoverageTests
{
    // ClauseTrigger flag -> the coverage lists whose union must equal {clauses declaring the flag}.
    // Deferred is intentionally absent: it labels clauses with NO live dispatch (asserted separately).
    private static readonly IReadOnlyDictionary<ClauseTrigger, ClauseType[][]> FlagToHandledLists =
        new Dictionary<ClauseTrigger, ClauseType[][]>
        {
            [ClauseTrigger.WeaponHitArm] = new[] { RarityEffects.HandledByCritSwingArm, RarityEffects.HandledByHitDamage, RarityEffects.HandledByRowNumericProcs },
            [ClauseTrigger.PostHitProc] = new[] { RarityEffects.HandledByClauseProcs },
            [ClauseTrigger.MarkRider] = new[] { RarityEffects.HandledByMark, RarityEffects.HandledByMarkSpread },
            [ClauseTrigger.ExtraSwingRider] = new[] { RarityEffects.HandledByExtraSwingRider },
            [ClauseTrigger.WeaponBlock] = new[] { RarityEffects.HandledByWeaponBlock },
            [ClauseTrigger.ArmorDefense] = new[] { RarityEffects.HandledByArmorDefense },
            [ClauseTrigger.ShieldParry] = new[] { RarityEffects.HandledByShieldParry },
            [ClauseTrigger.RegenTick] = new[] { RarityEffects.HandledByHitsRegen, RarityEffects.HandledByHitsTickSideEffects, RarityEffects.HandledBySelfRepair, RarityEffects.HandledByStamRegen, RarityEffects.HandledByManaRegen },
            [ClauseTrigger.SpellDr] = new[] { RarityEffects.HandledBySpellDrPoisonDot, RarityEffects.HandledBySpellDr },
            [ClauseTrigger.ParaResist] = new[] { RarityEffects.HandledByParaResist },
            [ClauseTrigger.OnKill] = new[] { RarityEffects.HandledByOnKillWeapon, RarityEffects.HandledByOnKillWorn },
            [ClauseTrigger.Potion] = new[] { RarityEffects.HandledByPotion },
            [ClauseTrigger.Hide] = new[] { RarityEffects.HandledByHide },
            [ClauseTrigger.MissReroll] = new[] { RarityEffects.HandledByMissReroll },
            [ClauseTrigger.PoisonResist] = new[] { RarityEffects.HandledByPoisonResist },
            [ClauseTrigger.Dodge] = new[] { RarityEffects.HandledByHitChance, RarityEffects.HandledByMeleeMiss },
            [ClauseTrigger.SpellManaLeech] = new[] { RarityEffects.HandledBySpellManaLeech },
            [ClauseTrigger.LightningProc] = new[] { RarityEffects.HandledByLightningProc },
            [ClauseTrigger.ArmorHitRider] = new[] { RarityEffects.HandledByArmorHitRider },
            [ClauseTrigger.WornStatMod] = new[] { WornEffectState.HandledByRebuildAggregate, WornEffectState.HandledByAnimalTaming, WornEffectState.HandledByHealsReceived },
            // The one dispatch site outside Engines/Rarity — its coverage list lives beside the site.
            [ClauseTrigger.DurabilityLoss] = new[] { BaseClothing.HandledByDurabilityLoss }
        };

    private static IEnumerable<ClauseType> AllClausesExceptNone() =>
        Enum.GetValues<ClauseType>().Where(c => c != ClauseType.None);

    // Every real clause must be classified. A new enum member with no ClauseTraits entry returns
    // ClauseTrigger.None here and fails — forcing whoever adds the clause to declare its triggers.
    [Fact]
    public void EveryClauseType_ExceptNone_HasAtLeastOneTraitFlag()
    {
        foreach (var clause in AllClausesExceptNone())
        {
            Assert.True(
                ClauseTraits.Get(clause) != ClauseTrigger.None,
                $"{clause} has no ClauseTraits entry — add it to ClauseTraits.Build() with its trigger flag(s)"
            );
        }
    }

    // The core invariant, both directions: for each dispatch flag, {clauses declaring it} must
    // equal the union of that flag's HandledBy* lists.
    //   - declared-but-not-handled  => a clause was flagged but never wired into a switch's list.
    //   - handled-but-not-declared  => a clause sits in a switch's list without the matching flag.
    [Fact]
    public void EachTriggerFlag_DeclaredClauses_MatchHandledLists()
    {
        foreach (var (flag, lists) in FlagToHandledLists)
        {
            var declared = AllClausesExceptNone().Where(c => (ClauseTraits.Get(c) & flag) != 0).ToHashSet();
            var handled = lists.SelectMany(l => l).ToHashSet();

            var declaredNotHandled = declared.Except(handled).ToList();
            var handledNotDeclared = handled.Except(declared).ToList();

            Assert.True(
                declaredNotHandled.Count == 0,
                $"{flag}: clauses flagged {flag} but missing from its HandledBy* list(s) — " +
                $"either dispatch them or drop the flag: {string.Join(", ", declaredNotHandled)}"
            );

            Assert.True(
                handledNotDeclared.Count == 0,
                $"{flag}: clauses in a HandledBy* list without the {flag} trait — " +
                $"add {flag} to their ClauseTraits entry: {string.Join(", ", handledNotDeclared)}"
            );
        }
    }

    // A clause has a live dispatch iff it is NOT labelled Deferred. This closes the gap where a
    // new clause is given only the Deferred flag (so it passes the "has a flag" test) yet is a real
    // behavior that should have been wired: Deferred must be reserved for genuinely-undispatched
    // clauses, and those must appear in no HandledBy* list.
    [Fact]
    public void DeferredFlag_ExactlyMarksClausesWithNoDispatch()
    {
        var everyHandledClause = FlagToHandledLists.Values
            .SelectMany(lists => lists.SelectMany(l => l))
            .ToHashSet();

        foreach (var clause in AllClausesExceptNone())
        {
            var deferred = ClauseTraits.IsDeferred(clause);
            var hasDispatch = ClauseTraits.HasDispatch(clause);

            Assert.False(
                deferred && hasDispatch,
                $"{clause} is flagged Deferred yet also carries a dispatch flag — pick one"
            );

            Assert.Equal(deferred, !hasDispatch);

            if (deferred)
            {
                Assert.DoesNotContain(clause, everyHandledClause);
            }
        }
    }

    // Snapshot of the currently-undispatched clauses. Not a correctness rule — a tripwire: if this
    // list changes, someone either implemented one of these (good — remove it here and wire
    // ClauseTraits) or parked a new clause as Deferred (needs a conscious sign-off), so the change
    // shows up in review instead of hiding.
    [Fact]
    public void DeferredClauses_AreTheKnownThree()
    {
        var deferred = AllClausesExceptNone().Where(ClauseTraits.IsDeferred).OrderBy(c => c.ToString()).ToArray();

        Assert.Equal(
            new[]
            {
                ClauseType.BlockCritStun,
                ClauseType.ExtraSwingOnParry,
                ClauseType.StealthBreakRefundStam
            },
            deferred
        );
    }
}
