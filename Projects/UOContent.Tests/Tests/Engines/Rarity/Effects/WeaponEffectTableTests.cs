using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

public class WeaponEffectTableTests
{
    [Fact]
    public void PhobosEpic_MatchesDocMagnitudes()
    {
        var row = WeaponEffectTable.Get(VariantRoot.Phobos, ItemRarity.Epic);

        Assert.Equal(10, row.DamagePct);
        Assert.Equal(10, row.CritChancePct);
        Assert.Equal(20, row.CritDamagePct);
    }

    [Theory]
    [InlineData(ItemRarity.Uncommon, 8, 0, 0)]
    [InlineData(ItemRarity.Rare, 8, 6, 0)]
    [InlineData(ItemRarity.Epic, 10, 8, 10)]
    public void Zephyr_MatchesDocMagnitudes(ItemRarity rarity, int swing, int hit, int extraSwing)
    {
        var row = WeaponEffectTable.Get(VariantRoot.Zephyr, rarity);

        Assert.Equal(swing, row.SwingSpeedPct);
        Assert.Equal(hit, row.HitChancePct);
        Assert.Equal(extraSwing, row.ExtraSwingPct);
    }

    [Fact]
    public void AgroteraEpic_MarksWithPoisonTick()
    {
        var row = WeaponEffectTable.Get(VariantRoot.Agrotera, ItemRarity.Epic);

        Assert.Equal(10, row.MarkChancePct);
        Assert.Equal(14, row.MarkBonusPct);
        Assert.True(row.MarkPoisonTick);
    }

    [Fact]
    public void PallasEpic_BlocksWithDrAndThorns()
    {
        var row = WeaponEffectTable.Get(VariantRoot.Pallas, ItemRarity.Epic);

        Assert.Equal(8, row.BlockPct);
        Assert.Equal(12, row.BlockDrPct);
        Assert.True(row.BlockThorns);
    }

    [Fact]
    public void StygianEpic_LifestealWithExecute()
    {
        var row = WeaponEffectTable.Get(VariantRoot.Stygian, ItemRarity.Epic);

        Assert.Equal(8, row.LifestealPct);
        Assert.Equal(8, row.StamRegenPct);
        Assert.True(row.LifestealExecute);
    }

    [Theory]
    [InlineData(VariantRoot.Zephyr)]
    [InlineData(VariantRoot.Phobos)]
    [InlineData(VariantRoot.Agrotera)]
    [InlineData(VariantRoot.Pallas)]
    [InlineData(VariantRoot.Stygian)]
    public void LegendaryColumn_EqualsEpic(VariantRoot root)
    {
        var epic = WeaponEffectTable.Get(root, ItemRarity.Epic);
        var legendary = WeaponEffectTable.Get(root, ItemRarity.Legendary);

        Assert.Equal(epic, legendary);
    }

    [Fact]
    public void CommonAndNone_AreEmpty()
    {
        Assert.True(WeaponEffectTable.Get(VariantRoot.Phobos, ItemRarity.Common).IsEmpty);
        Assert.True(WeaponEffectTable.Get(VariantRoot.None, ItemRarity.Epic).IsEmpty);
    }

    [Fact]
    public void NewNumericFields_DefaultZero_KeepRowEmpty()
    {
        // A row with none of the P2 re-theme fields set is still empty (guards IsEmpty coverage).
        Assert.True(default(WeaponEffectRow).IsEmpty);
    }

    [Fact]
    public void RowWithOnlySignature_IsNotEmpty()
    {
        var row = new WeaponEffectRow { Signature = ClauseType.NthHitFullArmorPen };

        Assert.False(row.IsEmpty);
    }

    [Fact]
    public void RowWithOnlyNewNumericField_IsNotEmpty()
    {
        Assert.False(new WeaponEffectRow { ArmorPenPct = 10 }.IsEmpty);
        Assert.False(new WeaponEffectRow { SplashPct = 25 }.IsEmpty);
        Assert.False(new WeaponEffectRow { DodgePct = 3 }.IsEmpty);
        Assert.False(new WeaponEffectRow { HealsReceivedPct = 8 }.IsEmpty); // Manteia staff worn-side lane
    }

    // The 30 per-family weapon roots wired in Phase 3 (02-07-*.md §2). Axes keep the original five.
    private static readonly VariantRoot[] NewWeaponRoots =
    {
        VariantRoot.Phoibos, VariantRoot.Areia, VariantRoot.Menis, VariantRoot.Aristeia, VariantRoot.Haima,
        VariantRoot.Ennosigaios, VariantRoot.Kataigis, VariantRoot.Rhaistes, VariantRoot.Eryma, VariantRoot.Kamatos,
        VariantRoot.Theristes, VariantRoot.Sarisa, VariantRoot.Phalanx, VariantRoot.Horme, VariantRoot.Zophos,
        VariantRoot.Empousa, VariantRoot.Prester, VariantRoot.Alexikakos, VariantRoot.Manteia, VariantRoot.Baskania,
        VariantRoot.Ios, VariantRoot.Ephodos, VariantRoot.Aiolos, VariantRoot.Kentron, VariantRoot.Ophis,
        VariantRoot.Hekatos, VariantRoot.Belos, VariantRoot.Pede, VariantRoot.Toxikon, VariantRoot.Skopos
    };

    [Fact]
    public void EveryNewWeaponRoot_HasFullLadder_WithLegendaryDuplicatingEpic()
    {
        foreach (var root in NewWeaponRoots)
        {
            Assert.True(WeaponEffectTable.Get(root, ItemRarity.Common).IsEmpty, $"{root} Common should be empty");
            Assert.False(WeaponEffectTable.Get(root, ItemRarity.Uncommon).IsEmpty, $"{root} Uncommon is empty");
            Assert.False(WeaponEffectTable.Get(root, ItemRarity.Rare).IsEmpty, $"{root} Rare is empty");
            Assert.False(WeaponEffectTable.Get(root, ItemRarity.Epic).IsEmpty, $"{root} Epic is empty");
            Assert.False(WeaponEffectTable.Get(root, ItemRarity.Legendary).IsEmpty, $"{root} Legendary is empty");

            // Framework §4 weapon rule: the Legendary column duplicates Epic verbatim (incl. Signature).
            Assert.Equal(WeaponEffectTable.Get(root, ItemRarity.Epic), WeaponEffectTable.Get(root, ItemRarity.Legendary));
        }
    }

    [Fact]
    public void NewWeaponRoot_Signatures_AppearOnEpicOnly_NotUncommonOrRare()
    {
        foreach (var root in NewWeaponRoots)
        {
            Assert.Equal(ClauseType.None, WeaponEffectTable.Get(root, ItemRarity.Uncommon).Signature);
            Assert.Equal(ClauseType.None, WeaponEffectTable.Get(root, ItemRarity.Rare).Signature);
        }
    }

    [Theory]
    // Swords
    [InlineData(VariantRoot.Phoibos, ClauseType.CritFirstHit)]
    [InlineData(VariantRoot.Areia, ClauseType.BlockNextShotCrit)]
    [InlineData(VariantRoot.Menis, ClauseType.RampMaxStacksSplash)]
    [InlineData(VariantRoot.Aristeia, ClauseType.OnKillFullStamNextHitCrit)]
    [InlineData(VariantRoot.Haima, ClauseType.PoisonedTakeBonusDamage)]
    // Maces
    [InlineData(VariantRoot.Ennosigaios, ClauseType.NthHitSplash)]
    [InlineData(VariantRoot.Kataigis, ClauseType.CritStagger)]
    [InlineData(VariantRoot.Rhaistes, ClauseType.NthHitFullArmorPen)]
    [InlineData(VariantRoot.Eryma, ClauseType.BlockGrantsDrBurst)]
    [InlineData(VariantRoot.Kamatos, ClauseType.None)] // low-stam-damage signature unwireable
    // Polearms
    [InlineData(VariantRoot.Theristes, ClauseType.CritSplash)]
    [InlineData(VariantRoot.Sarisa, ClauseType.CritFirstHit)]
    [InlineData(VariantRoot.Phalanx, ClauseType.BlockGrantsDrBurst)]
    [InlineData(VariantRoot.Horme, ClauseType.ExtraSwingEveryN)]
    [InlineData(VariantRoot.Zophos, ClauseType.None)] // execute is the LifestealExecute bool
    // Staves
    [InlineData(VariantRoot.Empousa, ClauseType.CritManaLeech)]
    [InlineData(VariantRoot.Prester, ClauseType.None)] // proc chance is the numeric field
    [InlineData(VariantRoot.Alexikakos, ClauseType.SpellDrBoostFirstHit)]
    [InlineData(VariantRoot.Manteia, ClauseType.None)] // heals-received + auto-cure are numeric
    [InlineData(VariantRoot.Baskania, ClauseType.CritHealBlock)]
    // Fencing
    [InlineData(VariantRoot.Ios, ClauseType.None)] // poison tier-up is the numeric field
    [InlineData(VariantRoot.Ephodos, ClauseType.CritFirstHitStamRefund)]
    [InlineData(VariantRoot.Aiolos, ClauseType.ExtraSwingChain)]
    [InlineData(VariantRoot.Kentron, ClauseType.NthHitFullArmorPen)]
    [InlineData(VariantRoot.Ophis, ClauseType.DodgeGrantsCounterWindow)]
    // Archery
    [InlineData(VariantRoot.Hekatos, ClauseType.CritFirstHit)]
    [InlineData(VariantRoot.Belos, ClauseType.NthHitSplash)]
    [InlineData(VariantRoot.Pede, ClauseType.CritStagger)]
    [InlineData(VariantRoot.Toxikon, ClauseType.PoisonedTargetsMarked)]
    [InlineData(VariantRoot.Skopos, ClauseType.BlockNextShotCrit)]
    public void EpicSignature_MatchesFamilyDoc(VariantRoot root, ClauseType expectedSignature)
    {
        Assert.Equal(expectedSignature, WeaponEffectTable.Get(root, ItemRarity.Epic).Signature);
        Assert.Equal(expectedSignature, WeaponEffectTable.Get(root, ItemRarity.Legendary).Signature);
    }

    [Fact]
    public void AxeRoots_StillCarryNoSignature()
    {
        // Axes are the exemplar family — they predate the signature concept and stay signature-free.
        foreach (var root in new[]
                 {
                     VariantRoot.Zephyr, VariantRoot.Phobos, VariantRoot.Agrotera, VariantRoot.Pallas, VariantRoot.Stygian
                 })
        {
            for (var rarity = ItemRarity.Common; rarity <= ItemRarity.Legendary; rarity++)
            {
                Assert.Equal(ClauseType.None, WeaponEffectTable.Get(root, rarity).Signature);
            }
        }
    }
}
