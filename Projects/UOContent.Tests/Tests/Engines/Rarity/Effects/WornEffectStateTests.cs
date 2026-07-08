using System.Collections.Generic;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Uses the shared UOContentFixture world boot (via the collection), same rationale as
// RarityEffectsTests: keeps these item/mobile-touching tests off the parallel double-boot race.
[Collection("Sequential UOContent Tests")]
public class WornEffectStateTests
{
    private static PlayerMobile CreatePlayerMobile(Point3D location)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = AccessLevel.GameMaster; // bypass stat/race equip gates — not under test here
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    [Fact]
    public void TwoTalarianLegendaryPieces_StrongestFullSecondHalf()
    {
        var player = CreatePlayerMobile(new Point3D(4500, 600, 0));
        var chest = new PlateChest();
        var legs = new PlateLegs();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Talarian, ItemRarity.Legendary);
            RarityEffects.ApplyVariant(legs, VariantRoot.Talarian, ItemRarity.Legendary);

            Assert.True(player.EquipItem(chest));
            Assert.True(player.EquipItem(legs));

            var agg = WornEffectState.GetAggregate(player);

            // Legendary Talarian: DodgePct 5 / StamRegenPct 10 per piece. Strongest instance
            // counts in full (100%), the second same-root instance at half (framework §9.4):
            // dodge 5 + 2 = 7, stam regen 10 + 5 = 15.
            Assert.Equal(7, agg.DodgePct);
            Assert.Equal(15, agg.StamRegenPct);
        }
        finally
        {
            chest.Delete();
            legs.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void SixTalarianLegendaryPieces_DodgeClampsAtFrameworkCeiling()
    {
        var player = CreatePlayerMobile(new Point3D(4520, 600, 0));
        var pieces = new BaseArmor[]
        {
            new PlateChest(), new PlateLegs(), new PlateArms(),
            new PlateGorget(), new PlateGloves(), new PlateHelm()
        };

        try
        {
            foreach (var piece in pieces)
            {
                RarityEffects.ApplyVariant(piece, VariantRoot.Talarian, ItemRarity.Legendary);
                Assert.True(player.EquipItem(piece));
            }

            var agg = WornEffectState.GetAggregate(player);

            Assert.Equal(12, agg.DodgePct); // framework §9.8 hard ceiling
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
    public void PoliasLegendaryArmorPiece_DoesNotAddBonusArToAggregate()
    {
        // Bonus AR is applied per-piece (RarityEffects.GetBonusArmorRating), not pooled into
        // WornAggregate — this is the flagged conflict with framework §9.8's suit-wide AR cap.
        var player = CreatePlayerMobile(new Point3D(4540, 600, 0));
        var chest = new PlateChest();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Polias, ItemRarity.Legendary);
            Assert.True(player.EquipItem(chest));

            Assert.Equal(3, WornEffectState.GetAggregate(player).DrPct);
            Assert.Equal(4, RarityEffects.GetBonusArmorRating(chest));
        }
        finally
        {
            chest.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void TwoOlympianLegendaryPieces_StrongestFullSecondHalf()
    {
        var player = CreatePlayerMobile(new Point3D(4580, 600, 0));
        var ring = new GoldRing();
        var bracelet = new GoldBracelet();

        try
        {
            var baseStr = player.RawStr;

            RarityEffects.ApplyVariant(ring, VariantRoot.Olympian, ItemRarity.Legendary);
            ring.OlympianStat = StatType.Str;
            RarityEffects.ApplyVariant(bracelet, VariantRoot.Olympian, ItemRarity.Legendary);
            bracelet.OlympianStat = StatType.Str;

            Assert.True(player.EquipItem(ring));
            Assert.True(player.EquipItem(bracelet));

            // Legendary Olympian: StatBonus 9 per piece. Strongest instance counts in full
            // (100%), the second same-root instance at half (framework §9.4): 9 + 9*50/100 = 13.
            Assert.Equal(baseStr + 13, player.Str);

            // Legendary Olympian: 5% lightning proc per piece, same §9.4 stacking as the stat
            // bonus above: 5 + 5*50/100 = 7.
            var agg = WornEffectState.GetAggregate(player);
            Assert.Equal(7, agg.LightningProcPct);
        }
        finally
        {
            ring.Delete();
            bracelet.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void RemovingArmor_RebuildsAggregateBackToEmpty()
    {
        var player = CreatePlayerMobile(new Point3D(4560, 600, 0));
        var chest = new PlateChest();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Talarian, ItemRarity.Legendary);
            Assert.True(player.EquipItem(chest));
            Assert.Equal(5, WornEffectState.GetAggregate(player).DodgePct);

            player.RemoveItem(chest);

            Assert.Equal(0, WornEffectState.GetAggregate(player).DodgePct);
        }
        finally
        {
            chest.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void StackGroup_StrongerDifferentRootTakesFullWeight_SecondHalved()
    {
        var player = CreatePlayerMobile(new Point3D(4620, 600, 0));
        var chest = new PlateChest();     // Polias (armor bulwark)
        var shield = new HeaterShield();  // Aegis (shield bulwark)

        try
        {
            // Polias and Aegis are different roots that share the bulwark stack-group (framework §9.4).
            Assert.Equal(
                VariantRootInfo.GetStackGroup(VariantRoot.Polias),
                VariantRootInfo.GetStackGroup(VariantRoot.Aegis)
            );

            RarityEffects.ApplyVariant(chest, VariantRoot.Polias, ItemRarity.Legendary); // DrPct 3, group's top rarity
            RarityEffects.ApplyVariant(shield, VariantRoot.Aegis, ItemRarity.Epic);      // ParryPct 7 / ParryDrPct 10

            Assert.True(player.EquipItem(chest));
            Assert.True(player.EquipItem(shield));

            var agg = WornEffectState.GetAggregate(player);

            // The Legendary Polias piece holds the bulwark group's full-weight slot, so the Epic
            // Aegis shield — a DIFFERENT root in the SAME group — is weighted at 50%: ParryPct
            // 7 -> 3, ParryDrPct 10 -> 5. Under the old per-root keying it would have applied full.
            Assert.Equal(3, agg.DrPct);
            Assert.Equal(3, agg.ParryPct);
            Assert.Equal(5, agg.ParryDrPct);
        }
        finally
        {
            chest.Delete();
            shield.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void HeldLegendaryWeapon_JoinsWornLegendaries_AndRevertsOnUnequip()
    {
        var player = CreatePlayerMobile(new Point3D(4640, 600, 0));
        var axe = new DoubleAxe();

        try
        {
            RarityEffects.ApplyLegendary(axe, 12); // Labrys

            Assert.True(player.EquipItem(axe));
            Assert.Contains(WornEffectState.GetLegendaries(player), e => e.Id == 12);

            player.RemoveItem(axe);
            Assert.DoesNotContain(WornEffectState.GetLegendaries(player), e => e.Id == 12);
        }
        finally
        {
            axe.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void HeldWeaponWornFold_RunsAlongsideArmor_WithoutDisturbingHealsReceivedOrAutoCure()
    {
        // The weapon worn-side fold (spell DR / mana regen / dodge / resist skill / heals-received
        // / auto-cure) runs for a held weapon. Every production weapon row is worn-side-zero this
        // phase, so a held weapon must fold in exactly nothing: a Paean chest's heals-received +
        // auto-cure survive equipping a weapon unchanged. Non-zero weapon worn-fold values arrive
        // with the Phase 3 staff (Manteia) table.
        var player = CreatePlayerMobile(new Point3D(4660, 600, 0));
        var chest = new PlateChest(); // Paean: HealsReceivedPct 10, AutoCure at Legendary
        var axe = new DoubleAxe();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Paean, ItemRarity.Legendary);
            Assert.True(player.EquipItem(chest));

            var before = WornEffectState.GetAggregate(player);
            Assert.Equal(10, before.HealsReceivedPct);
            Assert.True(before.AutoCure);

            RarityEffects.ApplyVariant(axe, VariantRoot.Zephyr, ItemRarity.Legendary); // no worn-side fields
            Assert.True(player.EquipItem(axe));

            var after = WornEffectState.GetAggregate(player);
            Assert.Equal(10, after.HealsReceivedPct); // weapon fold added 0, armor value intact
            Assert.True(after.AutoCure);               // weapon fold OR'd in false
        }
        finally
        {
            chest.Delete();
            axe.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void AppendSignature_BuildsSyntheticEntry_AndSkipsNone()
    {
        List<LegendaryEntry> list = null;

        WornEffectState.AppendSignature(ref list, VariantRoot.Menis, ClauseType.RampMaxStacksSplash, 10, 3, 0);

        Assert.NotNull(list);
        Assert.Single(list);

        var entry = list[0];
        Assert.Equal((ushort)0, entry.Id);
        Assert.Null(entry.Name);
        Assert.Equal(VariantRoot.Menis, entry.Root);
        Assert.Equal((byte)0xFF, entry.Family);
        Assert.Equal(ClauseType.RampMaxStacksSplash, entry.Clause);
        Assert.Equal((short)10, entry.P1);
        Assert.Equal((short)3, entry.P2);
        Assert.Equal((short)0, entry.P3);

        // A None signature contributes nothing.
        WornEffectState.AppendSignature(ref list, VariantRoot.Menis, ClauseType.None, 0, 0, 0);
        Assert.Single(list);
    }
}
