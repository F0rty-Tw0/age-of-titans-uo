using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Round-2 coverage: the ExtraSwingOnParry riposte (nested OnSwing inside parry resolution — the
// riskiest new path), the Daphne suit-weight stamina refund, Patron God devotion detection, and
// the legendary-only gate on Divine Resonance. Shared world boot, same as WornEffectStateTests.
[Collection("Sequential UOContent Tests")]
public class RiposteAndDevotionTests
{
    private static PlayerMobile CreatePlayerMobile(Point3D location, AccessLevel access = AccessLevel.GameMaster)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = access;
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    [Fact]
    public void Riposte_OnShieldParry_SwingsBackThroughRealPipeline()
    {
        // Elektor (war mace) is the one-handed ExtraSwingOnParry carrier — the only one that can
        // actually hold a shield (Aello's hatchet is two-handed; it rides the dodge path below).
        var owner = CreatePlayerMobile(new Point3D(4800, 600, 0));
        var attacker = CreatePlayerMobile(new Point3D(4801, 600, 0), AccessLevel.Player);
        var mace = new WarMace();
        var buckler = new Buckler();

        try
        {
            RarityEffects.ApplyLegendary(mace, 96); // Elektor — ExtraSwingOnParry

            Assert.True(owner.EquipItem(mace));
            Assert.True(owner.EquipItem(buckler));

            const long sentinel = 12345;
            owner.NextCombatTime = sentinel;

            // The parry rider drives a full nested OnSwing (depth-guarded); the riposte swing
            // re-schedules the owner's combat timer — the observable proof it actually swung.
            RarityEffects.OnShieldParried(buckler, attacker, owner, 10);

            Assert.NotEqual(sentinel, owner.NextCombatTime);
        }
        finally
        {
            mace.Delete();
            buckler.Delete();
            owner.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void Riposte_AttackerOutOfWeaponRange_DoesNotSwing()
    {
        var owner = CreatePlayerMobile(new Point3D(4820, 600, 0));
        var attacker = CreatePlayerMobile(new Point3D(4830, 600, 0), AccessLevel.Player); // 10 tiles away
        var mace = new WarMace();
        var buckler = new Buckler();

        try
        {
            RarityEffects.ApplyLegendary(mace, 96); // Elektor

            Assert.True(owner.EquipItem(mace));
            Assert.True(owner.EquipItem(buckler));

            const long sentinel = 12345;
            owner.NextCombatTime = sentinel;

            RarityEffects.OnShieldParried(buckler, attacker, owner, 10);

            Assert.Equal(sentinel, owner.NextCombatTime); // range gate held — no phantom melee riposte
        }
        finally
        {
            mace.Delete();
            buckler.Delete();
            owner.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void Riposte_AelloTwoHander_AnswersAThemedDodge()
    {
        // Aello's hatchet is two-handed — no shield, no Zephyr block — so its riposte answers a
        // themed dodge instead (the fix this test pinned down: without the dodge path the clause
        // was unreachable on its only axe carrier).
        var owner = CreatePlayerMobile(new Point3D(4880, 600, 0));
        var attacker = CreatePlayerMobile(new Point3D(4881, 600, 0), AccessLevel.Player);
        var hatchet = new Hatchet();
        var chest = new LeatherChest();

        try
        {
            RarityEffects.ApplyLegendary(hatchet, 1);                                  // Aello
            RarityEffects.ApplyVariant(chest, VariantRoot.Panika, ItemRarity.Legendary); // dodge package

            Assert.True(owner.EquipItem(hatchet));
            Assert.True(owner.EquipItem(chest));
            Assert.True(WornEffectState.GetAggregate(owner).DodgePct > 0);

            const long sentinel = 12345;
            owner.NextCombatTime = sentinel;

            RarityEffects.OnMeleeMiss(attacker, owner); // a suffered miss = themed dodge

            Assert.NotEqual(sentinel, owner.NextCombatTime);
        }
        finally
        {
            hatchet.Delete();
            chest.Delete();
            owner.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void Daphne_DodgeRefundsStamFromSuitWeight()
    {
        var defender = CreatePlayerMobile(new Point3D(4840, 600, 0));
        var attacker = CreatePlayerMobile(new Point3D(4841, 600, 0), AccessLevel.Player);
        var chest = new StuddedChest();

        try
        {
            RarityEffects.ApplyLegendary(chest, 208); // Daphne — DodgeRefundStamSuitWeight
            Assert.True(defender.EquipItem(chest));
            Assert.True(WornEffectState.GetAggregate(defender).DodgePct > 0); // dodge package live

            defender.RawDex = 50; // fresh test mobiles have StamMax 0 — give the refund headroom
            defender.Stam = 1;
            RarityEffects.OnMeleeMiss(attacker, defender); // a suffered miss = themed dodge

            Assert.True(defender.Stam > 1, $"expected suit-weight stam refund, stam is {defender.Stam}");
        }
        finally
        {
            chest.Delete();
            defender.Delete();
            attacker.Delete();
        }
    }

    [Fact]
    public void ThreeWarLegendaries_PledgeThePatron_AndBreakOnRemoval()
    {
        // Kekrops (Hoplites ringmail) + Nereus (Taxis ringmail) + Amphion (Amyntor buckler) —
        // three distinct clauses (no resonance), all War-domain roots -> Patron: Ares, +4% damage.
        var player = CreatePlayerMobile(new Point3D(4860, 600, 0));
        var chest = new RingmailChest();
        var legs = new RingmailLegs();
        var buckler = new Buckler();

        try
        {
            RarityEffects.ApplyLegendary(chest, 186);   // Kekrops
            RarityEffects.ApplyLegendary(legs, 195);    // Nereus
            RarityEffects.ApplyLegendary(buckler, 222); // Amphion

            Assert.True(player.EquipItem(chest));
            Assert.True(player.EquipItem(legs));
            Assert.True(player.EquipItem(buckler));

            var agg = WornEffectState.GetAggregate(player);

            Assert.True(agg.HasDevotion);
            Assert.Equal(PantheonDomain.War, agg.DevotionDomain);
            Assert.Equal(4, agg.DevotionDamagePct); // War is an offense domain
            Assert.Equal(0, agg.ResonanceOffense + agg.ResonanceDefense + agg.ResonanceUtility);

            // Two legendaries are no pledge.
            player.RemoveItem(buckler);
            Assert.False(WornEffectState.GetAggregate(player).HasDevotion);
        }
        finally
        {
            chest.Delete();
            legs.Delete();
            buckler.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void FiveWarLegendaries_ReachExarchTier_PerkDoubles()
    {
        // War trio (Kekrops/Nereus/Amphion) + the two Maenad relics (Atropos kilt, Kisseus hat)
        // = 5 War-domain legendaries -> Exarch: the +4% damage perk doubles to +8%.
        var player = CreatePlayerMobile(new Point3D(4900, 600, 0));
        var chest = new RingmailChest();
        var legs = new RingmailLegs();
        var buckler = new Buckler();
        var kilt = new Kilt();
        var hat = new WideBrimHat();

        try
        {
            RarityEffects.ApplyLegendary(chest, 186);   // Kekrops
            RarityEffects.ApplyLegendary(legs, 195);    // Nereus
            RarityEffects.ApplyLegendary(buckler, 222); // Amphion
            RarityEffects.ApplyLegendary(kilt, 268);    // Atropos
            RarityEffects.ApplyLegendary(hat, 272);     // Kisseus

            Assert.True(player.EquipItem(chest));
            Assert.True(player.EquipItem(legs));
            Assert.True(player.EquipItem(buckler));
            Assert.True(player.EquipItem(kilt));
            Assert.True(player.EquipItem(hat));

            var agg = WornEffectState.GetAggregate(player);

            Assert.True(agg.HasDevotion);
            Assert.True(agg.IsExarch);
            Assert.Equal(PantheonDomain.War, agg.DevotionDomain);
            Assert.Equal(8, agg.DevotionDamagePct); // doubled at Exarch

            // Dropping to 4 pieces falls back to Patron tier, not zero.
            player.RemoveItem(hat);
            var demoted = WornEffectState.GetAggregate(player);
            Assert.True(demoted.HasDevotion);
            Assert.False(demoted.IsExarch);
            Assert.Equal(4, demoted.DevotionDamagePct);
        }
        finally
        {
            chest.Delete();
            legs.Delete();
            buckler.Delete();
            kilt.Delete();
            hat.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void PlayerSnare_LockedOutWhileActive_NoPermaChain()
    {
        var target = CreatePlayerMobile(new Point3D(4920, 600, 0));
        target.Player = true; // the PvP lockout keys off Mobile.Player, not the concrete type

        try
        {
            CombatFxState.SetSnare(target, 30, TimeSpan.FromSeconds(3));
            Assert.Equal(30, CombatFxState.GetSnarePct(target));

            // A second snare inside the lockout window bounces — no chain-refresh on players.
            CombatFxState.SetSnare(target, 50, TimeSpan.FromSeconds(3));
            Assert.Equal(30, CombatFxState.GetSnarePct(target));
        }
        finally
        {
            target.Delete();
        }
    }

    [Fact]
    public void PlayerHealBlock_LockedOutWhileActive_NoPermaChain()
    {
        var target = CreatePlayerMobile(new Point3D(4940, 600, 0));
        target.Player = true; // the PvP lockout keys off Mobile.Player, not the concrete type

        try
        {
            CombatFxState.SetHealBlock(target, TimeSpan.FromSeconds(2));
            var first = CombatFxState.GetHealBlockRemaining(target);
            Assert.True(first > 0);

            // Re-application inside the lockout cannot extend the window on a player.
            CombatFxState.SetHealBlock(target, TimeSpan.FromSeconds(3));
            var second = CombatFxState.GetHealBlockRemaining(target);
            Assert.True(second <= first, $"heal-block chain-extended on a player: {first}ms -> {second}ms");
        }
        finally
        {
            target.Delete();
        }
    }

    [Fact]
    public void SyntheticSignaturePair_Dedupes_ButDoesNotResonate()
    {
        // Two Id-0 lane-signature synthetics with the same clause (an Epic-only overlap): the
        // dispatch dedupe still collapses them, but Divine Resonance is legendary-tier — no echo.
        var list = new List<LegendaryEntry>
        {
            new(0, null, VariantRoot.Menis, 0xFF, 0, ClauseType.BlockGrantsDrBurst, 8, 3, 0, 0),
            new(0, null, VariantRoot.Eryma, 0xFF, 0, ClauseType.BlockGrantsDrBurst, 6, 3, 0, 0)
        };

        var (offense, defense, utility) = WornEffectState.DedupeClausesCounted(list);

        Assert.Equal(0, offense + defense + utility);
        Assert.Single(list);
        Assert.Equal((short)8, list[0].P1); // strongest still wins the dispatch slot
    }
}
