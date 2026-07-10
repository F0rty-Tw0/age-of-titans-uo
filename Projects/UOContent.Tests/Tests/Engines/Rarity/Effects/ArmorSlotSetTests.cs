using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// P3-P6 armor slot-set coverage: signature redirect, capstone set detection, clause dedupe,
// and the cap clamps. World-booted (equips real items) — same collection rationale as
// WornEffectStateTests.
[Collection("Sequential UOContent Tests")]
public class ArmorSlotSetTests
{
    private static PlayerMobile CreatePlayerMobile(Point3D location)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = AccessLevel.GameMaster; // bypass stat/race equip gates — not under test here
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    // ---- P1/P2: signature redirect ----

    [Fact]
    public void EpicArmor_SignatureComesFromSlotTable_NotRoot()
    {
        var player = CreatePlayerMobile(new Point3D(4700, 600, 0));
        var chest = new RingmailChest();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Polias, ItemRarity.Epic);
            Assert.True(player.EquipItem(chest));

            // (Ringmail x Chest) => ShrugFirstHitDrBurst(8, 3) as a synthetic (Id 0) entry,
            // regardless of the piece's root.
            Assert.Contains(
                WornEffectState.GetLegendaries(player),
                e => e.Id == 0 && e.Clause == ClauseType.ShrugFirstHitDrBurst && e.P1 == 8 && e.P2 == 3
            );
        }
        finally
        {
            chest.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void SubEpicArmor_DoesNotGetSlotSignature()
    {
        var player = CreatePlayerMobile(new Point3D(4702, 600, 0));
        var chest = new RingmailChest();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Polias, ItemRarity.Rare);
            Assert.True(player.EquipItem(chest));

            Assert.DoesNotContain(
                WornEffectState.GetLegendaries(player),
                e => e.Clause == ClauseType.ShrugFirstHitDrBurst
            );
        }
        finally
        {
            chest.Delete();
            player.Delete();
        }
    }

    // ---- P4: capstone set detection ----

    [Fact]
    public void FourEpicPlatePieces_CompleteSet_BreaksWhenPieceRemoved()
    {
        var player = CreatePlayerMobile(new Point3D(4704, 600, 0));
        var pieces = new BaseArmor[] { new PlateChest(), new PlateLegs(), new PlateArms(), new PlateGloves() };

        try
        {
            foreach (var piece in pieces)
            {
                RarityEffects.ApplyVariant(piece, VariantRoot.Polias, ItemRarity.Epic);
                Assert.True(player.EquipItem(piece));
            }

            var agg = WornEffectState.GetAggregate(player);
            Assert.True(agg.HasCapstone);
            Assert.Equal(ArmorMaterialType.Plate, agg.CapstoneMaterial);

            player.RemoveItem(pieces[0]); // 3 of 4 — set broken

            Assert.False(WornEffectState.GetAggregate(player).HasCapstone);
        }
        finally
        {
            foreach (var piece in pieces)
            {
                piece.Delete();
            }

            player.Delete();
        }
    }

    [Fact]
    public void SubEpicPiece_DoesNotCountTowardSet()
    {
        var player = CreatePlayerMobile(new Point3D(4706, 600, 0));
        var pieces = new BaseArmor[] { new PlateChest(), new PlateLegs(), new PlateArms(), new PlateGloves() };

        try
        {
            for (var i = 0; i < pieces.Length; i++)
            {
                // Three Epics + one Rare: the Rare piece must not count.
                RarityEffects.ApplyVariant(pieces[i], VariantRoot.Polias, i == 3 ? ItemRarity.Rare : ItemRarity.Epic);
                Assert.True(player.EquipItem(pieces[i]));
            }

            Assert.False(WornEffectState.GetAggregate(player).HasCapstone);
        }
        finally
        {
            foreach (var piece in pieces)
            {
                piece.Delete();
            }

            player.Delete();
        }
    }

    [Fact]
    public void ChainmailSet_ThresholdIsThree()
    {
        // Chainmail has only 3 piece shapes, so its threshold is min(4, 3) = 3.
        var player = CreatePlayerMobile(new Point3D(4708, 600, 0));
        var pieces = new BaseArmor[] { new ChainCoif(), new ChainChest(), new ChainLegs() };

        try
        {
            foreach (var piece in pieces)
            {
                RarityEffects.ApplyVariant(piece, VariantRoot.Polias, ItemRarity.Epic);
                Assert.True(player.EquipItem(piece));
            }

            var agg = WornEffectState.GetAggregate(player);
            Assert.True(agg.HasCapstone);
            Assert.Equal(ArmorMaterialType.Chainmail, agg.CapstoneMaterial);
        }
        finally
        {
            foreach (var piece in pieces)
            {
                piece.Delete();
            }

            player.Delete();
        }
    }

    [Fact]
    public void LeatherCapstone_AddsSixDodge()
    {
        // Polias rows carry no DodgePct, so the aggregate's dodge must be exactly the
        // capstone's +6 (Evasion), folded in before the cap clamp.
        var player = CreatePlayerMobile(new Point3D(4710, 600, 0));
        var pieces = new BaseArmor[] { new LeatherChest(), new LeatherLegs(), new LeatherArms(), new LeatherGloves() };

        try
        {
            foreach (var piece in pieces)
            {
                RarityEffects.ApplyVariant(piece, VariantRoot.Polias, ItemRarity.Epic);
                Assert.True(player.EquipItem(piece));
            }

            var agg = WornEffectState.GetAggregate(player);
            Assert.True(agg.HasCapstone);
            Assert.Equal(ArmorMaterialType.Leather, agg.CapstoneMaterial);
            Assert.Equal(6, agg.DodgePct);
        }
        finally
        {
            foreach (var piece in pieces)
            {
                piece.Delete();
            }

            player.Delete();
        }
    }

    // ---- P5: clause dedupe (pure list tests) ----

    [Fact]
    public void Dedupe_SameClause_StrongestP1Wins()
    {
        var list = new List<LegendaryEntry>
        {
            new(0, null, VariantRoot.Alkimos, 0xFF, 0, ClauseType.ShrugReflect, 10, 0, 0, 0), // synthetic signature
            new(187, "Erechtheus", VariantRoot.Phylax, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.ShrugReflect, 15, 0, 0, 0)
        };

        WornEffectState.DedupeClauses(list);

        var entry = Assert.Single(list);
        Assert.Equal((short)15, entry.P1);
        Assert.Equal((ushort)187, entry.Id);
    }

    [Fact]
    public void Dedupe_EveryNthClause_SmallerIntervalWins()
    {
        var list = new List<LegendaryEntry>
        {
            new(1, "A", VariantRoot.Polias, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.FlameProcEveryN, 5, 0, 0, 0),
            new(2, "B", VariantRoot.Polias, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.FlameProcEveryN, 3, 0, 0, 0)
        };

        WornEffectState.DedupeClauses(list);

        Assert.Equal((short)3, Assert.Single(list).P1); // every-3rd fires more often than every-5th
    }

    [Fact]
    public void Dedupe_Tie_PrefersShieldSourced_ThenRealLegendary()
    {
        // Tie on params: the shield-sourced entry must survive (parry-path gates key off source).
        var shieldTie = new List<LegendaryEntry>
        {
            new(100, "Weapon", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 0, ClauseType.BlockDrainStam, 5, 0, 0, 0),
            new(0, null, VariantRoot.Aegis, 0xFF, 0, ClauseType.BlockDrainStam, 5, 0, 0, 0)
        };

        WornEffectState.DedupeClauses(shieldTie);
        Assert.Equal(VariantRoot.Aegis, Assert.Single(shieldTie).Root);

        // Tie on params and source: the real legendary (Id != 0) beats the synthetic signature.
        var idTie = new List<LegendaryEntry>
        {
            new(0, null, VariantRoot.Hoplites, 0xFF, 0, ClauseType.ShrugStunAttacker, 0, 0, 0, 0),
            new(186, "Kekrops", VariantRoot.Hoplites, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.ShrugStunAttacker, 0, 0, 0, 0)
        };

        WornEffectState.DedupeClauses(idTie);
        Assert.Equal((ushort)186, Assert.Single(idTie).Id);
    }

    [Fact]
    public void Dedupe_DifferentClauses_Untouched()
    {
        var list = new List<LegendaryEntry>
        {
            new(0, null, VariantRoot.Alkimos, 0xFF, 0, ClauseType.ShrugReflect, 10, 0, 0, 0),
            new(0, null, VariantRoot.Hoplites, 0xFF, 0, ClauseType.ShrugStunAttacker, 0, 0, 0, 0)
        };

        WornEffectState.DedupeClauses(list);

        Assert.Equal(2, list.Count);
    }

    // ---- P3 + P6: ShrugFirstHitDrBurst end-to-end with the DR cap clamp ----

    [Fact]
    public void RingmailChestSignature_ShrugsFirstHit_ThenDrWindowReducesNextHit()
    {
        var defender = CreatePlayerMobile(new Point3D(4712, 600, 0));
        var attacker = CreatePlayerMobile(new Point3D(4713, 600, 0));
        var chest = new RingmailChest();

        try
        {
            // Talarian carries no shrug/DR/reflect/flame fields, so the absorb math below is
            // driven purely by the slot signature and fully deterministic.
            RarityEffects.ApplyVariant(chest, VariantRoot.Talarian, ItemRarity.Epic);
            Assert.True(defender.EquipItem(chest));

            var agg = WornEffectState.GetAggregate(defender);
            Assert.Equal(0, agg.ShrugPct);
            Assert.Equal(0, agg.DrPct);

            // First hit of the fight: guaranteed shrug (halved), arms the 3s DR window.
            Assert.Equal(50, RarityEffects.AbsorbForDefenderArmor(attacker, defender, 100));

            // Advance the game clock: RegisterHitTaken is deliberately idempotent within one
            // Core.TickCount value (weapon-side + armor-side absorb share a single incoming
            // hit), so the second swing must land on a later tick — as in a real fight.
            Core._tickCount += 100;

            // Second hit inside the window: not shrugged, +8% DR (clamped vs the 12 cap).
            Assert.Equal(92, RarityEffects.AbsorbForDefenderArmor(attacker, defender, 100));
        }
        finally
        {
            chest.Delete();
            attacker.Delete();
            defender.Delete();
        }
    }

    [Fact]
    public void HpRegenBurst_ClampsToSixtyPercentCap()
    {
        var player = CreatePlayerMobile(new Point3D(4714, 600, 0));
        var chest = new PlateChest();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Paean, ItemRarity.Legendary);
            Assert.True(player.EquipItem(chest));
            Assert.Equal(25, WornEffectState.GetAggregate(player).HpRegenPct);

            var baseRate = TimeSpan.FromSeconds(10);

            // 25% bonus: 10s * 100/125 = 8s.
            Assert.Equal(TimeSpan.FromSeconds(8), RarityEffects.AdjustHitsRegenRate(player, baseRate));

            // Crit-taken burst triples the 25% to 75%, which must clamp to the 60% ceiling:
            // 10s * 100/160 = 6.25s (NOT 100/175).
            WornEffectState.ArmClauseBurst(player, ClauseType.HpRegenBurstOnCritTaken, TimeSpan.FromSeconds(5));
            Assert.Equal(TimeSpan.FromSeconds(6.25), RarityEffects.AdjustHitsRegenRate(player, baseRate));
        }
        finally
        {
            chest.Delete();
            player.Delete();
        }
    }
}
