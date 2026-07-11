using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// P5b Divine Resonance (duplicate clauses echo instead of vanishing) + the debuff repeat-stacking
// rules in CombatFxState (strongest mark wins, heal-block extends only). Same shared world boot
// as WornEffectStateTests.
[Collection("Sequential UOContent Tests")]
public class ResonanceAndStackingTests
{
    private static PlayerMobile CreatePlayerMobile(Point3D location)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = AccessLevel.GameMaster; // bypass stat/race equip gates — not under test here
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    private static LegendaryEntry Entry(ushort id, ClauseType clause, short p1) =>
        new(id, $"Test{id}", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 0, clause, p1, 0, 0, 0);

    [Fact]
    public void DedupeCounted_OffensiveDuplicate_CountsOffense_AndKeepsStrongest()
    {
        // CritFirstHit dispatches at WeaponHitArm/PostHitProc -> offensive resonance.
        var list = new List<LegendaryEntry> { Entry(9001, ClauseType.CritFirstHit, 10), Entry(9002, ClauseType.CritFirstHit, 25) };

        var (offense, defense, utility) = WornEffectState.DedupeClausesCounted(list);

        Assert.Equal(1, offense);
        Assert.Equal(0, defense);
        Assert.Equal(0, utility);
        Assert.Single(list);
        Assert.Equal(25, list[0].P1); // strongest survives
    }

    [Fact]
    public void DedupeCounted_DefensiveAndUtilityDuplicates_CountPerCategory()
    {
        // ReflectFirstHit -> WeaponBlock (defense); AutoCureRestoresStamMana -> RegenTick (utility).
        var list = new List<LegendaryEntry>
        {
            Entry(9003, ClauseType.ReflectFirstHit, 20),
            Entry(9004, ClauseType.ReflectFirstHit, 15),
            Entry(9005, ClauseType.AutoCureRestoresStamMana, 10),
            Entry(9006, ClauseType.AutoCureRestoresStamMana, 10)
        };

        var (offense, defense, utility) = WornEffectState.DedupeClausesCounted(list);

        Assert.Equal(0, offense);
        Assert.Equal(1, defense);
        Assert.Equal(1, utility);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void FullLegendaryPair_SameClauseAcrossWeaponAndShield_Resonates()
    {
        // Ladon (kryss, ReflectFirstHit 20) + Amphion (buckler, ReflectFirstHit 15) are the real
        // co-wearable duplicate from the registry: one dispatch survives, the duplicate echoes
        // as +2% DR (defensive resonance) instead of being silently wasted.
        var player = CreatePlayerMobile(new Point3D(4700, 600, 0));
        var kryss = new Kryss();
        var buckler = new Buckler();

        try
        {
            RarityEffects.ApplyLegendary(kryss, 160);   // Ladon
            RarityEffects.ApplyLegendary(buckler, 222); // Amphion

            Assert.True(player.EquipItem(kryss));
            Assert.True(player.EquipItem(buckler));

            var legendaries = WornEffectState.GetLegendaries(player);
            Assert.Single(legendaries, e => e.Clause == ClauseType.ReflectFirstHit);
            Assert.Contains(legendaries, e => e.Id == 160); // stronger P1 (20 > 15) survives

            var agg = WornEffectState.GetAggregate(player);
            Assert.Equal(1, agg.ResonanceDefense);
            Assert.Equal(0, agg.ResonanceOffense);
            Assert.Equal(0, agg.ResonanceDamagePct);
            Assert.True(agg.DrPct >= 2); // the echo fed the (capped) DR pool

            // Breaking the pair dissolves the resonance.
            player.RemoveItem(buckler);
            Assert.Equal(0, WornEffectState.GetAggregate(player).ResonanceDefense);
        }
        finally
        {
            kryss.Delete();
            buckler.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void Mark_WeakerForeignMark_DoesNotOverwriteStronger()
    {
        var target = CreatePlayerMobile(new Point3D(4720, 600, 0));
        var strongMarker = CreatePlayerMobile(new Point3D(4721, 600, 0));
        var weakMarker = CreatePlayerMobile(new Point3D(4722, 600, 0));

        try
        {
            CombatFxState.SetMark(target, strongMarker, 25, true, TimeSpan.FromSeconds(5));
            CombatFxState.SetMark(target, weakMarker, 10, false, TimeSpan.FromSeconds(5));

            Assert.True(CombatFxState.TryGetMark(target, out var bonus));
            Assert.Equal(25, bonus);
            Assert.True(CombatFxState.IsMarkedBy(target, strongMarker));

            // The owner of the strong mark may always refresh/replace their own.
            CombatFxState.SetMark(target, strongMarker, 12, false, TimeSpan.FromSeconds(5));
            Assert.True(CombatFxState.TryGetMark(target, out bonus));
            Assert.Equal(12, bonus);

            // And an equal-or-stronger rival takes the mark over.
            CombatFxState.SetMark(target, weakMarker, 12, false, TimeSpan.FromSeconds(5));
            Assert.True(CombatFxState.IsMarkedBy(target, weakMarker));

            CombatFxState.ClearMark(target);
        }
        finally
        {
            target.Delete();
            strongMarker.Delete();
            weakMarker.Delete();
        }
    }

    [Fact]
    public void HealBlock_ShorterReapply_NeverShortensActiveWindow()
    {
        var target = CreatePlayerMobile(new Point3D(4740, 600, 0));

        try
        {
            CombatFxState.SetHealBlock(target, TimeSpan.FromSeconds(3));
            var before = CombatFxState.GetHealBlockRemaining(target);

            CombatFxState.SetHealBlock(target, TimeSpan.FromSeconds(1)); // e.g. bone capstone re-tag

            var after = CombatFxState.GetHealBlockRemaining(target);
            Assert.True(after >= before - 50, $"heal-block shortened: {before}ms -> {after}ms");
            Assert.True(CombatFxState.IsHealBlocked(target));
        }
        finally
        {
            target.Delete();
        }
    }

    [Fact]
    public void ResonanceCategory_CoversEveryDispatchedClause()
    {
        // Every non-deferred clause must classify into one of the three echo categories without
        // throwing; deferred clauses never reach DedupeClausesCounted's counters in production
        // but must not break it either.
        foreach (var clause in Enum.GetValues<ClauseType>())
        {
            if (clause == ClauseType.None)
            {
                continue;
            }

            var list = new List<LegendaryEntry> { Entry(9101, clause, 5), Entry(9102, clause, 5) };
            var (offense, defense, utility) = WornEffectState.DedupeClausesCounted(list);

            Assert.Equal(1, offense + defense + utility);
            Assert.Single(list);
        }
    }
}
