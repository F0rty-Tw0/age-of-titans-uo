using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Engines.LootBags;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Server.Tests;
using Xunit;

namespace UOContent.Tests;

// Offline DPS simulator — drives the REAL swing pipeline (BaseWeapon.OnSwing -> CheckHit ->
// Begin/EndWeaponHit -> absorb) against a healed-every-swing dummy, so crits, cadence procs,
// ramps, extra swings, marks and resonance/devotion damage all land exactly as in game.
//
// Two modes:
//  - Always-on smoke asserts: the pipeline produces damage and sane numbers (cheap N).
//  - Full tuning report, env-gated like the golden harness: set RARITY_DPS_SIM=<output path>
//    to sweep every weapon family's TOP-base legendary across its five lanes and write a table
//    of measured DPS (raw + naked-vs-devotion composed case). Run:
//      RARITY_DPS_SIM=dps.txt MODERNUO_TEST_DATA_DIR=... dotnet test --filter RarityDpsSim
//
// Honest limits (stated in the report header too): poison DoT ticks and on-kill restores never
// fire (the dummy neither ticks nor dies), so poison/execute lanes read slightly low. The test
// world boots the latest expansion, but the sim flips Core.Expansion to T2A for its measurement
// window (restored in finally; the Sequential collection means nothing runs concurrently), so
// hit/damage/delay all follow the SHARD'S real era paths. Compare lanes RELATIVELY, not to the
// framework's 8.70 theoretical target.
[Collection("Sequential UOContent Tests")]
public class RarityDpsSimTests
{
    private const string EnvVar = "RARITY_DPS_SIM";

    static RarityDpsSimTests()
    {
        // The test world registers no skill-check handlers, so Mobile.CheckSkill — and therefore
        // every CheckHit — silently fails. Install a minimal chance-roll fallback (no skill gain);
        // ??= keeps any real handler that a future fixture might register.
        Mobile.SkillCheckDirectLocationHandler ??= (m, _, chance) => chance >= Utility.RandomDouble();
        Mobile.SkillCheckDirectTargetHandler ??= (m, _, _, chance) => chance >= Utility.RandomDouble();
    }

    // Thousands of continuous swings grind gear durability to nothing (degrading, then breaking
    // it) — a decay no real fight reaches. Re-fitting every swing keeps the measurement about
    // the DESIGN numbers, not about wear. (Found empirically: N=4000 runs measured ~half the
    // DPS of N=800 runs before this existed.)
    internal static void RefitDurability(Mobile m)
    {
        for (var i = m.Items.Count - 1; i >= 0; i--)
        {
            if (m.Items[i] is IDurability { MaxHitPoints: > 0 } worn)
            {
                worn.HitPoints = worn.MaxHitPoints;
            }
        }
    }

    private static PlayerMobile CreateCombatant(Point3D location, int str, int dex, int skills, AccessLevel access)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = access; // the dummy must be Player-level — staff cannot be harmed
        m.RawStr = str;
        m.RawDex = dex; // reference stamina — swing speed is stamina-anchored
        m.RawInt = 25;
        m.Skills[SkillName.Tactics].Base = skills;
        m.Skills[SkillName.Anatomy].Base = skills;
        m.Hits = m.HitsMax;
        m.Stam = m.StamMax;
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }


    // Simulates `swings` real swings of the given legendary weapon and returns measured DPS.
    // `dress` optionally equips extra gear on the attacker (devotion/resonance composition).
    private static double SimulateDps(in LegendaryEntry entry, int swings, Point3D location, Action<PlayerMobile> dress = null)
    {
        var priorExpansion = Core.Expansion;
        Core.Expansion = Expansion.T2A; // measure under the shard's real era (restored below)

        var attacker = CreateCombatant(location, 100, 100, 100, AccessLevel.GameMaster);

        // The dummy is a RAW Mobile, not a PlayerMobile: PlayerMobile.OnDeath needs client/
        // account state the test world doesn't have, and a deep crit chain can kill anything.
        // Deep HP pool + per-swing resurrect guard keep the measurement loop unconditional.
        var defender = new Mobile(World.NewMobile);
        defender.DefaultMobileInit(); // initializes the item/skill state MoveToWorld touches
        defender.RawStr = 5000; // HitsMax ~2550 — no single swing chain gets close
        defender.RawDex = 25;
        defender.RawInt = 25;
        defender.Hits = defender.HitsMax;
        defender.MoveToWorld(new Point3D(location.X + 1, location.Y, location.Z), Map.Felucca);
        var item = LootRoller.ConstructForLegendary(entry);

        try
        {
            RarityEffects.ApplyLegendary(item, entry.Id);

            var weapon = Assert.IsAssignableFrom<BaseWeapon>(item);
            attacker.Skills[weapon.Skill].Base = 100;
            Assert.True(attacker.EquipItem(weapon));
            dress?.Invoke(attacker);

            // Ranged weapons need ammunition, shooting distance, and the pre-AOS "stood still for
            // 1s" gate satisfied — or every shot silently fails.
            if (weapon is BaseRanged ranged)
            {
                attacker.AddToBackpack(new Arrow(60000));
                attacker.AddToBackpack(new Bolt(60000));
                attacker.MoveToWorld(new Point3D(location.X - Math.Min(4, ranged.MaxRange - 1), location.Y, location.Z), Map.Felucca);
                attacker.LastMoveTime = Core.TickCount - 2000;
            }

            var seconds = 0.0;
            long damage = 0;

            for (var i = 0; i < swings; i++)
            {
                if (!defender.Alive)
                {
                    defender.Resurrect(); // belt-and-braces — the HP pool should make this unreachable
                }

                defender.Hits = defender.HitsMax;
                attacker.Hits = attacker.HitsMax;
                attacker.Stam = attacker.StamMax; // hold the stamina anchor across the run
                RefitDurability(attacker);

                // Under T2A, OnSwing's returned delay IS the live value (GetDelay ends with the
                // rarity swing-speed adjustment) — no manual timing math needed.
                seconds += weapon.OnSwing(attacker, defender).TotalSeconds;
                damage += defender.HitsMax - defender.Hits;
            }

            Assert.True(seconds > 0, "swing pipeline returned zero elapsed time");
            return damage / seconds;
        }
        finally
        {
            Core.Expansion = priorExpansion;
            item.Delete();

            // Attacker may have been dressed — delete whatever was equipped along the way.
            for (var i = attacker.Items.Count - 1; i >= 0; i--)
            {
                attacker.Items[i].Delete();
            }

            attacker.Delete();
            defender.Delete();
        }
    }

    [Fact]
    public void SwingPipeline_ProducesDamage_Smoke()
    {
        Assert.True(LegendaryRegistry.TryGet(12, out var labrys)); // Labrys, double axe

        var dps = SimulateDps(labrys, 150, new Point3D(5000, 600, 0));

        // Wide sanity band only — this guards "the pipeline still deals damage", not tuning.
        Assert.InRange(dps, 0.5, 100.0);
    }

    [SkippableFact]
    public void DevotionDamage_MeasurablyRaisesDps_Smoke()
    {
        TileDataRequirement.SkipIfMissing();
        Assert.True(LegendaryRegistry.TryGet(12, out var labrys));

        var loc = new Point3D(5020, 600, 0);
        var naked = SimulateDps(labrys, 400, loc);
        var devoted = SimulateDps(labrys, 400, loc, attacker =>
        {
            // War devotion trio — Labrys is Zephyr (Wind), so the +4% is domain-agnostic damage.
            var chest = new RingmailChest();
            var legs = new RingmailLegs();
            var kilt = new Kilt();
            RarityEffects.ApplyLegendary(chest, 186); // Kekrops
            RarityEffects.ApplyLegendary(legs, 195);  // Nereus
            RarityEffects.ApplyLegendary(kilt, 268);  // Atropos
            Assert.True(attacker.EquipItem(chest));
            Assert.True(attacker.EquipItem(legs));
            Assert.True(attacker.EquipItem(kilt));
            Assert.Equal(4, WornEffectState.GetAggregate(attacker).DevotionDamagePct);
        });

        // +4% damage over 400 swings should not read as a DECREASE beyond noise.
        Assert.True(devoted > naked * 0.97, $"devotion build measured lower: naked {naked:F2} vs devoted {devoted:F2}");
    }

    // Full tuning sweep — writes a report instead of asserting (env-gated, manual tool).
    [Fact]
    public void FullReport_WhenEnvSet()
    {
        var path = Environment.GetEnvironmentVariable(EnvVar);

        if (string.IsNullOrEmpty(path))
        {
            return; // not requested — the smoke tests above keep the pipeline honest in CI
        }

        var lines = new List<string>
        {
            "family | base | weapon type | lane root | legendary | DPS (measured, 100-skill vs 25)",
            "FULL base x lane matrix — every weapon TYPE (bows, crossbows, both polearms, every axe...)",
            "poison DoT / on-kill effects excluded (dummy never ticks or dies); full-HP-gated clauses",
            "read HIGH (dummy healed to full every swing); ~5% CI at N=4000 — compare lanes relatively.",
            ""
        };

        var families = FamilyRegistry.WeaponFamilies;
        var entries = LegendaryRegistry.Entries;
        var loc = new Point3D(5040, 600, 0);

        for (var f = 0; f < families.Length; f++)
        {
            var family = families[f];

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];

                if (entry.Family != family.Family)
                {
                    continue;
                }

                var typeName = family.LadderTypes[entry.BaseIndex].Name;
                // N=4000: measured per-run sigma at N=800 was ~5% (heavy-tailed damage chains —
                // time is deterministic); 4000 brings the 95% CI under ~5%, enough to trust
                // ladder ordering. Full-HP-gated clauses read high (the dummy is healed to full
                // every swing) — note kept in the header.
                var dps = SimulateDps(entry, 4000, loc);
                lines.Add($"{family.Family} | {entry.BaseIndex} | {typeName} | {entry.Root} | {entry.Name} | {dps:F2}");
            }

            lines.Add("");
        }

        // Pure-ladder section: Epic VARIANTS carry lane effects but no unique clause, so within
        // one lane the bases isolate the §7 ladder (ratio/speed) — the legendary matrix above
        // cannot (each base's legendary carries a different clause). One lane per family.
        lines.Add("== pure ladder (Epic variant, first lane per family — no unique clauses) ==");
        lines.Add("");

        for (var f = 0; f < families.Length; f++)
        {
            var family = families[f];
            var root = FamilyRegistry.LaneRoots(family.Lanes)[0];

            for (var b = 0; b < family.LadderTypes.Length; b++)
            {
                var dps = SimulateVariantDps(family.Factories[b], root, 4000, loc);
                lines.Add($"{family.Family} | {b} | {family.LadderTypes[b].Name} | {root} | {dps:F2}");
            }

            lines.Add("");
        }

        File.WriteAllLines(path, lines);
    }

    // Epic-variant twin of SimulateDps — same measurement loop, drop-variant instead of legendary.
    private static double SimulateVariantDps(Func<Item> factory, VariantRoot root, int swings, Point3D location)
    {
        var priorExpansion = Core.Expansion;
        Core.Expansion = Expansion.T2A;

        var attacker = CreateCombatant(location, 100, 100, 100, AccessLevel.GameMaster);
        var defender = new Mobile(World.NewMobile);
        defender.DefaultMobileInit();
        defender.RawStr = 5000;
        defender.Hits = defender.HitsMax;
        defender.MoveToWorld(new Point3D(location.X + 1, location.Y, location.Z), Map.Felucca);

        var item = factory();

        try
        {
            RarityEffects.ApplyVariant(item, root, ItemRarity.Epic);

            var weapon = Assert.IsAssignableFrom<BaseWeapon>(item);
            attacker.Skills[weapon.Skill].Base = 100;
            Assert.True(attacker.EquipItem(weapon));

            if (weapon is BaseRanged ranged)
            {
                attacker.AddToBackpack(new Arrow(60000));
                attacker.AddToBackpack(new Bolt(60000));
                attacker.MoveToWorld(new Point3D(location.X - Math.Min(4, ranged.MaxRange - 1), location.Y, location.Z), Map.Felucca);
                attacker.LastMoveTime = Core.TickCount - 2000;
            }

            var seconds = 0.0;
            long damage = 0;

            for (var i = 0; i < swings; i++)
            {
                if (!defender.Alive)
                {
                    defender.Resurrect();
                }

                defender.Hits = defender.HitsMax;
                attacker.Hits = attacker.HitsMax;
                attacker.Stam = attacker.StamMax;
                RefitDurability(attacker);

                seconds += weapon.OnSwing(attacker, defender).TotalSeconds;
                damage += defender.HitsMax - defender.Hits;
            }

            return damage / seconds;
        }
        finally
        {
            Core.Expansion = priorExpansion;
            item.Delete();

            for (var i = attacker.Items.Count - 1; i >= 0; i--)
            {
                attacker.Items[i].Delete();
            }

            attacker.Delete();
            defender.Delete();
        }
    }

    // Weapon-TYPE coverage guard: every family — including the ranged ones and both polearms —
    // must produce damage through the real pipeline. This is the class of bug the ranged sim
    // found (no-ammo / stand-still gate / NetState NRE): had this test existed, "bows deal
    // nothing" could never have shipped silently.
    [SkippableFact]
    public void EveryWeaponFamily_ProducesDamage_Smoke()
    {
        TileDataRequirement.SkipIfMissing();
        var families = FamilyRegistry.WeaponFamilies;
        var entries = LegendaryRegistry.Entries;
        var loc = new Point3D(5060, 600, 0);

        for (var f = 0; f < families.Length; f++)
        {
            var family = families[f];
            var found = false;

            for (var i = 0; i < entries.Count && !found; i++)
            {
                var entry = entries[i];

                if (entry.Family != family.Family || entry.BaseIndex != 0)
                {
                    continue;
                }

                found = true;
                var dps = SimulateDps(entry, 150, loc);

                Assert.True(
                    dps > 0.5,
                    $"family {family.Family} ({family.LadderTypes[0].Name}, {entry.Name}) produced {dps:F2} DPS — a weapon TYPE is broken in the pipeline"
                );
            }

            Assert.True(found, $"family {family.Family} has no base-0 legendary to smoke-test");
        }
    }
}
