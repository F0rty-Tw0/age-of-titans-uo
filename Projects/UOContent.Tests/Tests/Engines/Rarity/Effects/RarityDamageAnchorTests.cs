using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// The runtime weapon damage/speed anchors (framework §2/§7; D = 10/13/17/22/30 directive 2026-07-11).
// Variant weapons resolve their base min/max damage and swing speed from their §7 ladder ratio and
// rarity at read time — nothing is serialized. Uses the shared world fixture for item construction.
[Collection("Sequential UOContent Tests")]
public class RarityDamageAnchorTests
{
    [Theory]
    // battle axe ratio .75: Rare anchor .75x17 = 12.75 -> min round(11.475)=11, max round(14.025)=14.
    [InlineData(typeof(BattleAxe), ItemRarity.Rare, 11, 14)]
    // fencing dagger ratio .50: Rare anchor .50x17 = 8.5 -> min round(7.65)=8, max round(9.35)=9.
    [InlineData(typeof(Dagger), ItemRarity.Rare, 8, 9)]
    // archery bow ratio .70: Epic anchor .70x22 = 15.4 -> min round(13.86)=14, max round(16.94)=17.
    [InlineData(typeof(Bow), ItemRarity.Epic, 14, 17)]
    public void VariantWeapon_MinMaxDamage_MatchesAnchorSpread(Type weaponType, ItemRarity rarity, int expectedMin, int expectedMax)
    {
        var weapon = (BaseWeapon)Activator.CreateInstance(weaponType);

        try
        {
            // Pallas/Ios/Skopos would be family-correct, but the anchor keys off the weapon TYPE +
            // rarity only, so any variant root exercises it. Use each weapon's own family lane where
            // the row has no DamagePct so MinDamage/MaxDamage read the raw anchor (no theme fold).
            RarityEffects.ApplyVariant(weapon, RootFor(weaponType), rarity);

            Assert.Equal(expectedMin, weapon.MinDamage);
            Assert.Equal(expectedMax, weapon.MaxDamage);
        }
        finally
        {
            weapon.Delete();
        }
    }

    [Fact]
    public void OrnateAxeLegendary_AnchorsAtThirtyWithTenPercentSpread()
    {
        var weapon = new OrnateAxe();

        try
        {
            RarityEffects.ApplyLegendary(weapon, 16); // Enyalios — Phobos ornate axe (base 7), Legendary

            // ratio 1.00 x D[Legendary] 30 = 30 -> min round(27)=27, max round(33)=33.
            Assert.Equal(27, weapon.MinDamage);
            Assert.Equal(33, weapon.MaxDamage);
        }
        finally
        {
            weapon.Delete();
        }
    }

    [Fact]
    public void PlainWeapon_MinMaxSpeed_UnchangedByAnchor()
    {
        var plain = new BattleAxe();
        var plain2 = new BattleAxe();
        var variant = new BattleAxe();

        try
        {
            RarityEffects.ApplyVariant(variant, VariantRoot.Pallas, ItemRarity.Rare);

            // The anchor only fires for variants; a plain weapon returns false and keeps whatever the
            // stock era getters yield (expansion-agnostic assertion — the fixture boots EJ, not T2A).
            Assert.False(RarityEffects.TryGetAnchorDamage(plain, out _, out _));
            Assert.False(RarityEffects.TryGetAnchorSpeed(plain, out _));
            Assert.True(RarityEffects.TryGetAnchorDamage(variant, out _, out _));

            // Two identical plain axes agree; the variant of the same base diverges (proves the plain
            // path is genuinely skipped, not coincidentally equal to the anchor).
            Assert.Equal(plain.MinDamage, plain2.MinDamage);
            Assert.Equal(plain.Speed, plain2.Speed);
            Assert.NotEqual(plain.MinDamage, variant.MinDamage);
            Assert.NotEqual(plain.Speed, variant.Speed);
        }
        finally
        {
            plain.Delete();
            plain2.Delete();
            variant.Delete();
        }
    }

    [Fact]
    public void VariantWeapon_SwingDelay_AtReferenceStamina_MatchesLadderSeconds()
    {
        // GetDelay's live formula is expansion-gated; the shard runs T2A (pre-AOS branch), while the
        // test fixture boots EJ. Toggle to T2A for this assertion so we exercise the real live path,
        // then restore (Sequential collection makes the swap safe).
        var previous = Core.Expansion;
        var weapon = new BattleAxe();
        var player = new PlayerMobile(World.NewMobile);
        player.DefaultMobileInit();
        player.RawDex = 125;
        player.Stam = 100; // reference stamina

        try
        {
            Core.Expansion = Expansion.T2A;

            // Pallas (defense) has no SwingSpeedPct, so GetDelay shows the raw speed anchor.
            RarityEffects.ApplyVariant(weapon, VariantRoot.Pallas, ItemRarity.Rare);

            var delay = weapon.GetDelay(player).TotalSeconds;

            // battle axe SwingSeconds 2.95 -> speed 75/2.95 = 25.42; the pre-AOS formula int-truncates
            // speed to 25 at stam 100 -> delay 3.0s, within one stat-int of the 2.95 design value.
            Assert.True(Math.Abs(delay - 2.95) <= 0.1, $"variant battle axe delay {delay:0.000}s not near the 2.95s anchor");

            // A plain battle axe's delay is unchanged by the anchor.
            var plain = new BattleAxe();
            try
            {
                var plainSpeed = (int)plain.Speed;
                var expectedPlain = 15000.0 / ((player.Stam + 100) * plainSpeed);
                Assert.Equal(expectedPlain, plain.GetDelay(player).TotalSeconds, 3);
            }
            finally
            {
                plain.Delete();
            }
        }
        finally
        {
            Core.Expansion = previous;
            weapon.Delete();
            player.Delete();
        }
    }

    [Fact]
    public void StatsLine_ShowsAnchorDerivedRange_Automatically()
    {
        var weapon = new BattleAxe();

        try
        {
            // Pallas Rare row carries no DamagePct, so the stats line shows the raw 11-14 anchor.
            RarityEffects.ApplyVariant(weapon, VariantRoot.Pallas, ItemRarity.Rare);

            var lines = new List<string>();
            RarityEffects.CollectSingleClickLines(weapon, lines);

            Assert.StartsWith("Damage 11-14", lines[0]);
        }
        finally
        {
            weapon.Delete();
        }
    }

    [Fact]
    public void TopBaseLegendaryDps_MatchesRecomputedParityTargets()
    {
        // §7 parity: melee top bases cluster at ~8.70 DPS (recomputed for D[Legendary]=30); the noted
        // outliers are maces -1.5%, staves -8%, archery -16%. A ratio/seconds typo shifts one of these.
        // WeaponFamilies is family-id order: axes, swords, polearms, maces, staves, fencing, archery.
        var expected = new[] { 8.70, 8.77, 8.65, 8.57, 8.00, 8.82, 7.30 };
        var families = FamilyRegistry.WeaponFamilies;

        Assert.Equal(expected.Length, families.Length);

        for (var f = 0; f < families.Length; f++)
        {
            var fam = families[f];
            var top = fam.LadderTypes.Length - 1;
            var dps = RarityDamageAnchors.Anchor(fam.Ratios[top], ItemRarity.Legendary) / fam.SwingSeconds[top];

            Assert.True(
                Math.Abs(dps - expected[f]) <= 0.05,
                $"family {fam.Family} top-base Legendary DPS {dps:0.00} off target {expected[f]:0.00}"
            );
        }
    }

    // Each ladder weapon type's own-family lane root with a DamagePct-free row, so the anchor reads
    // unmodified by the theme fold. (Any variant root would trip the anchor; these keep it clean.)
    private static VariantRoot RootFor(Type weaponType) =>
        weaponType == typeof(BattleAxe) ? VariantRoot.Pallas :   // axes defense lane (no DamagePct)
        weaponType == typeof(Dagger) ? VariantRoot.Ophis :       // fencing evasion lane
        weaponType == typeof(Bow) ? VariantRoot.Skopos :         // archery warden lane
        VariantRoot.Pallas;
}
