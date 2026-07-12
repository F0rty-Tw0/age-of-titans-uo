using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Server.Tests;
using Xunit;

namespace UOContent.Tests;

// Part B mechanics overhaul: legendary indestructibility, the durability/repair clause
// replacements, on-kill stacking-waste spill, armor-pen overflow conversion, cross-domain patron
// devotion, and the Divine Resonance re-equip/source-name fix. Shared world boot, same pattern as
// WornEffectStateTests.
[Collection("Sequential UOContent Tests")]
public class RarityMechanicsPartBTests
{
    private static PlayerMobile CreatePlayerMobile(Point3D location, AccessLevel access = AccessLevel.GameMaster)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = access;
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    // ---- B1: legendary indestructibility -------------------------------------------------

    [Fact]
    public void LegendaryArmor_TakesNoDurabilityLoss()
    {
        var chest = new LeatherChest();

        try
        {
            RarityEffects.ApplyVariant(chest, VariantRoot.Panika, ItemRarity.Legendary);
            chest.MaxHitPoints = 100;
            chest.HitPoints = 100;

            var weapon = new Dagger();

            // The wear roll is a 25% chance per hit; a legendary skips it entirely, so even a long
            // barrage never chips it.
            for (var i = 0; i < 200; i++)
            {
                ((IWearableDurability)chest).OnHit(weapon, 20);
            }

            Assert.Equal(100, chest.HitPoints);
            weapon.Delete();
        }
        finally
        {
            chest.Delete();
        }
    }

    [Fact]
    public void LegendaryClothing_TakesNoDurabilityLoss()
    {
        var robe = new Robe();

        try
        {
            RarityEffects.ApplyVariant(robe, VariantRoot.Hestian, ItemRarity.Legendary);
            robe.MaxHitPoints = 50;
            robe.HitPoints = 50;

            var weapon = new Dagger();

            for (var i = 0; i < 200; i++)
            {
                ((IWearableDurability)robe).OnHit(weapon, 20);
            }

            Assert.Equal(50, robe.HitPoints);
            weapon.Delete();
        }
        finally
        {
            robe.Delete();
        }
    }

    // ---- B2: durability/repair replacements ---------------------------------------------

    [Fact]
    public void Telamon_ForcedMiss_ZeroesNextHitChance_Once()
    {
        var attacker = CreatePlayerMobile(new Point3D(4620, 700, 0), AccessLevel.Player);
        var defender = CreatePlayerMobile(new Point3D(4621, 700, 0));
        var weapon = new Longsword();

        try
        {
            // A parry threw the attacker off balance: the next swing auto-misses, then the flag clears.
            CombatFxState.ArmForcedMiss(attacker);

            var chance = 0.9;
            RarityEffects.AdjustHitChance(weapon, attacker, defender, ref chance);
            Assert.Equal(0.0, chance);

            var chance2 = 0.9;
            RarityEffects.AdjustHitChance(weapon, attacker, defender, ref chance2);
            Assert.NotEqual(0.0, chance2); // one-shot — the second swing is unaffected
        }
        finally
        {
            CombatFxState.Evict(attacker);
            CombatFxState.Evict(defender);
            weapon.Delete();
            attacker.Delete();
            defender.Delete();
        }
    }

    [Fact]
    public void DeflectSecondary_ReboundsTheMarkOntoTheAttacker()
    {
        var attacker = CreatePlayerMobile(new Point3D(4640, 700, 0), AccessLevel.Player);
        var defender = CreatePlayerMobile(new Point3D(4641, 700, 0));
        var axe = new DoubleAxe();

        try
        {
            RarityEffects.ApplyLegendary(axe, 23); // Sagaris — MarkFirstHit (guaranteed first-hit mark)

            var ctx = RarityEffects.BeginWeaponHit(axe, attacker, defender);
            Assert.True(ctx.IsFirstHit);

            // The defender's first hit taken armed the deflect (simulated here) — the incoming mark
            // rebounds onto the attacker instead of landing on the defender.
            WornEffectState.ArmSecondaryDeflect(defender);
            RarityEffects.EndWeaponHit(axe, attacker, defender, 10, ctx);

            Assert.True(CombatFxState.IsMarkedBy(attacker, defender));
            Assert.False(CombatFxState.IsMarked(defender));
        }
        finally
        {
            CombatFxState.Evict(attacker);
            CombatFxState.Evict(defender);
            axe.Delete();
            attacker.Delete();
            defender.Delete();
        }
    }

    // ---- B3: stacking-waste conversions --------------------------------------------------

    [Fact]
    public void RestoreWithSpill_FullStat_SpillsHalfIntoHealth()
    {
        var m = CreatePlayerMobile(new Point3D(4660, 700, 0));

        try
        {
            m.RawStr = 125;
            m.RawDex = 125;

            var hitsMax = m.HitsMax;
            Assert.True(hitsMax >= 30, $"test needs HP headroom, HitsMax was {hitsMax}");

            m.Stam = m.StamMax;      // stamina already full — a stam restore has no headroom
            m.Hits = hitsMax - 20;   // and health has room to absorb the spill

            RarityEffects.RestoreWithSpill(m, 'S', 20);

            Assert.Equal(m.StamMax, m.Stam);      // stamina unchanged (was full)
            Assert.Equal(hitsMax - 10, m.Hits);   // half of the wasted 20 spilled into health
        }
        finally
        {
            m.Delete();
        }
    }

    [Fact]
    public void RestoreWithSpill_WithHeadroom_AppliesFullyAndDoesNotSpill()
    {
        var m = CreatePlayerMobile(new Point3D(4661, 700, 0));

        try
        {
            m.RawStr = 125;
            m.RawDex = 125;

            var hitsMax = m.HitsMax;
            m.Hits = hitsMax;                 // health full — proves the applied portion doesn't spill
            m.Stam = m.StamMax - 15;          // 15 stamina headroom

            RarityEffects.RestoreWithSpill(m, 'S', 10);

            Assert.Equal(m.StamMax - 5, m.Stam); // 10 landed on stamina
            Assert.Equal(hitsMax, m.Hits);       // nothing spilled (all 10 was used)
        }
        finally
        {
            m.Delete();
        }
    }

    [Fact]
    public void ArmorPen_OverflowPast100_ConvertsToBonusDamage()
    {
        var attacker = CreatePlayerMobile(new Point3D(4680, 700, 0), AccessLevel.Player);
        var defender = CreatePlayerMobile(new Point3D(4681, 700, 0));
        var kryss = new Kryss();

        try
        {
            // Sybaris (Kentron fencing): row ArmorPenPct 20 + the NthHitFullArmorPen signature's +100
            // on every 3rd hit -> pen 120. The 3rd hit is not a cadence crit (CritArmorPen fires on
            // the 5th), so no crit pen — a clean 120 that clamps to 100, spilling (120-100)/2 = 10
            // into this hit's damage bonus. The Kentron row carries no base damage/ramp, so the
            // bonus at hit 3 is exactly the overflow.
            RarityEffects.ApplyLegendary(kryss, 166);
            RarityEffects.ConsumePendingArmorPen(); // drain stale

            var ctx = default(RarityEffects.WeaponHitContext);

            for (var i = 0; i < 3; i++)
            {
                ctx = RarityEffects.BeginWeaponHit(kryss, attacker, defender);
            }

            Assert.False(ctx.IsCrit);
            Assert.Equal(10, ctx.DamageBonusPercent);
            Assert.Equal(100, RarityEffects.ConsumePendingArmorPen()); // clamped
        }
        finally
        {
            CombatFxState.Evict(attacker);
            CombatFxState.Evict(defender);
            kryss.Delete();
            attacker.Delete();
            defender.Delete();
        }
    }

    // ---- B4: patron devotion stacks across domains --------------------------------------

    [SkippableFact]
    public void TwoOffenseDomains_BothPledge_AndDamagePerkSums()
    {
        TileDataRequirement.SkipIfMissing();
        var player = CreatePlayerMobile(new Point3D(4700, 700, 0));
        var chest = new RingmailChest();
        var arms = new RingmailArms();
        var legs = new RingmailLegs();
        var ring = new GoldRing();
        var bracelet = new GoldBracelet();
        var necklace = new GoldNecklace();

        try
        {
            // Three War-domain ringmail legendaries + three Sky-domain (Olympian) jewelry legendaries.
            RarityEffects.ApplyLegendary(chest, 186);     // Kekrops (Hoplites, War)
            RarityEffects.ApplyLegendary(arms, 189);      // Perdix (Zoster, War)
            RarityEffects.ApplyLegendary(legs, 195);      // Nereus (Taxis, War)
            RarityEffects.ApplyLegendary(ring, 246);      // Hyperion (Olympian, Sky)
            RarityEffects.ApplyLegendary(bracelet, 247);  // Ouranos (Olympian, Sky)
            RarityEffects.ApplyLegendary(necklace, 248);  // Aither (Olympian, Sky)

            Assert.True(player.EquipItem(chest));
            Assert.True(player.EquipItem(arms));
            Assert.True(player.EquipItem(legs));
            Assert.True(player.EquipItem(ring));
            Assert.True(player.EquipItem(bracelet));
            Assert.True(player.EquipItem(necklace));

            var agg = WornEffectState.GetAggregate(player);

            Assert.True(agg.HasDevotion);
            Assert.True(agg.IsDevotedTo(PantheonDomain.War));
            Assert.True(agg.IsDevotedTo(PantheonDomain.Sky));
            Assert.False(agg.IsExarch);
            Assert.Equal(8, agg.DevotionDamagePct); // War (+4) + Sky (+4), both offense domains

            // Breaking one domain below 3 drops only that pledge.
            player.RemoveItem(necklace);
            var demoted = WornEffectState.GetAggregate(player);
            Assert.True(demoted.IsDevotedTo(PantheonDomain.War));
            Assert.False(demoted.IsDevotedTo(PantheonDomain.Sky));
            Assert.Equal(4, demoted.DevotionDamagePct);
        }
        finally
        {
            chest.Delete();
            arms.Delete();
            legs.Delete();
            ring.Delete();
            bracelet.Delete();
            necklace.Delete();
            player.Delete();
        }
    }

    // ---- B5: Divine Resonance re-equip + per-item source names --------------------------

    [SkippableFact]
    public void Resonance_NamesSources_AndRebuildsOnReEquip()
    {
        TileDataRequirement.SkipIfMissing();
        var player = CreatePlayerMobile(new Point3D(4720, 700, 0));
        // Kekrops (Ringmail chest) + Zethos (buckler) both carry HpRegenBurstOnCritTaken after the
        // 2026-07-12 arming-group split moved shields off the ReflectFirstHit lane — the surviving
        // co-wearable defensive-resonance pair in the registry.
        var chest = new RingmailChest();
        var buckler = new Buckler();

        try
        {
            RarityEffects.ApplyLegendary(chest, 186);   // Kekrops — HpRegenBurstOnCritTaken
            RarityEffects.ApplyLegendary(buckler, 223); // Zethos — HpRegenBurstOnCritTaken (defensive resonance)

            Assert.True(player.EquipItem(chest));
            Assert.True(player.EquipItem(buckler));

            var text = WornEffectState.GetAggregate(player).ResonanceText;
            Assert.NotNull(text);
            Assert.Contains("Kekrops", text);
            Assert.Contains("Zethos", text);
            Assert.Contains("damage reduction", text);

            // The equip bug: unequipping and re-equipping must dissolve then rebuild the readout,
            // not leave stale text behind.
            player.RemoveItem(buckler);
            Assert.Null(WornEffectState.GetAggregate(player).ResonanceText);

            Assert.True(player.EquipItem(buckler));
            var rebuilt = WornEffectState.GetAggregate(player).ResonanceText;
            Assert.NotNull(rebuilt);
            Assert.Contains("Kekrops", rebuilt);
            Assert.Contains("Zethos", rebuilt);
        }
        finally
        {
            chest.Delete();
            buckler.Delete();
            player.Delete();
        }
    }
}
