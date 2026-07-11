using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Engines.Rarity;
using Server.Items;
using Server.Mobiles;
using Xunit;

namespace UOContent.Tests;

// Defense-side twin of RarityDpsSimTests: a fixed reference attacker swings a plain katana at a
// defender wearing a candidate defensive loadout, through the REAL pipeline (CheckHit incl. the
// rarity dodge subtraction -> AbsorbDamage -> shrug/DR/reflect/flame riders), and we measure
// what the SUIT changes: damage taken per swing, zero-damage swing rate (miss + dodge + full
// absorb), and reflect damage returned to the attacker.
//
// Always-on smoke asserts pin the three core defensive promises: armor mitigates, the dodge
// pool changes real hit rates, thorns really bite back. The full material x lane sweep is
// env-gated like the DPS sweep: RARITY_DEFENSE_SIM=<path> writes the tuning table.
//
// Era note: every rarity defense hook (dodge subtraction aside) lives on the PRE-AOS branches
// (BaseWeapon.AbsorbDamage / BaseShield.OnHit) — the shard's real paths. The test world boots
// the latest expansion, so the sim flips Core.Expansion to T2A for its measurement window
// (restored in finally; Sequential collection = nothing runs concurrently). The reference
// attacker is 100 weapon skill vs the defender's 100 Wrestling — T2A hit chance 0.5, leaving
// dodge real headroom to show up (vs a no-skill dummy the chance saturates above 1.0 and dodge
// mathematically cannot surface — that is live behavior too: dodge does nothing for you against
// a grossly over-skilled attacker).
[Collection("Sequential UOContent Tests")]
public class RarityDefenseSimTests
{
    private const string EnvVar = "RARITY_DEFENSE_SIM";

    static RarityDefenseSimTests()
    {
        // Same fallback as RarityDpsSimTests — without a handler every CheckHit auto-misses.
        Mobile.SkillCheckDirectLocationHandler ??= (m, _, chance) => chance >= Utility.RandomDouble();
        Mobile.SkillCheckDirectTargetHandler ??= (m, _, _, chance) => chance >= Utility.RandomDouble();
    }

    private readonly record struct DefenseResult(double DamagePerSwing, double ZeroDamageRate, double ReflectPerSwing);

    private static PlayerMobile CreateCombatant(Point3D location, AccessLevel access)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = access;
        m.RawStr = 150; // meets every armor str requirement; HP pool comfortably above one hit
        m.RawDex = 100;
        m.RawInt = 25;
        m.Hits = m.HitsMax;
        m.Stam = m.StamMax;
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    // Swings a plain (non-variant) reference weapon `swings` times at a defender dressed by
    // `dress`. The attacker is the CONSTANT within a comparison; only the defender's loadout
    // varies. Two reference weapons matter: the katana (light hits — avoid/regen lanes show
    // best) and the halberd (heavy hits — mitigation percentages decompress; plate/bone/ring
    // gaps read true). Default stays the katana for the smokes.
    private static DefenseResult SimulateDefense(
        Action<PlayerMobile> dress, int swings, Point3D location, Func<BaseWeapon> referenceWeapon = null
    )
    {
        var priorExpansion = Core.Expansion;
        Core.Expansion = Expansion.T2A; // measure under the shard's real era (restored below)

        var attacker = CreateCombatant(location, AccessLevel.GameMaster);
        var defender = CreateCombatant(new Point3D(location.X + 1, location.Y, location.Z), AccessLevel.Player);
        var katana = referenceWeapon?.Invoke() ?? new Katana();

        try
        {
            attacker.Skills[katana.Skill].Base = 100;
            attacker.Skills[SkillName.Tactics].Base = 100;
            attacker.Skills[SkillName.Anatomy].Base = 100;
            Assert.True(attacker.EquipItem(katana));

            // The dummy holds its own katana at 100 skill: the test world has no Fists bootstrap,
            // so an unarmed defender's defense skill reads 0 and T2A hit chance saturates at 1.5
            // (nothing, dodge included, could surface). Armed 100-vs-100 puts chance at 0.5 —
            // real PvP-parity headroom.
            var defenderWeapon = new Katana();
            defender.Skills[SkillName.Swords].Base = 100;
            defender.Skills[SkillName.Parry].Base = 100; // exercises the shield-parry path when a shield is worn
            Assert.True(defender.EquipItem(defenderWeapon));
            dress?.Invoke(defender);

            long damageTaken = 0;
            long reflectTaken = 0;
            var zeroDamageSwings = 0;

            for (var i = 0; i < swings; i++)
            {
                if (!defender.Alive)
                {
                    defender.Resurrect();
                }

                defender.Hits = defender.HitsMax;
                attacker.Hits = attacker.HitsMax;
                attacker.Stam = attacker.StamMax;
                RarityDpsSimTests.RefitDurability(attacker);
                RarityDpsSimTests.RefitDurability(defender); // the suit under test must not wear out mid-measurement

                katana.OnSwing(attacker, defender);

                var dealt = defender.HitsMax - defender.Hits;
                damageTaken += dealt;
                reflectTaken += attacker.HitsMax - attacker.Hits;

                if (dealt == 0)
                {
                    zeroDamageSwings++;
                }
            }

            return new DefenseResult(
                (double)damageTaken / swings,
                (double)zeroDamageSwings / swings,
                (double)reflectTaken / swings
            );
        }
        finally
        {
            Core.Expansion = priorExpansion;
            katana.Delete();

            for (var i = defender.Items.Count - 1; i >= 0; i--)
            {
                defender.Items[i].Delete();
            }

            attacker.Delete();
            defender.Delete();
        }
    }

    // Dresses the defender in a FULL suit of one material's pieces, all carrying `root` at the
    // given rarity — the registry's own slot factories, so every material's real shapes are used.
    private static void DressSuit(PlayerMobile defender, ArmorMaterialType material, VariantRoot root, ItemRarity rarity)
    {
        Assert.True(FamilyRegistry.ArmorFamilyByMaterial.TryGetValue(material, out var family));

        foreach (var factory in family.SlotFactories)
        {
            var piece = (BaseArmor)factory();
            RarityEffects.ApplyVariant(piece, root, rarity);
            Assert.True(defender.EquipItem(piece), $"could not equip {piece.GetType().Name}");
        }
    }

    [Fact]
    public void LegendaryPlateSuit_TakesLessDamageThanNaked_Smoke()
    {
        var loc = new Point3D(5100, 600, 0);
        var naked = SimulateDefense(null, 400, loc);
        var plated = SimulateDefense(d => DressSuit(d, ArmorMaterialType.Plate, VariantRoot.Adamas, ItemRarity.Legendary), 400, loc);

        Assert.True(
            plated.DamagePerSwing < naked.DamagePerSwing,
            $"Adamas plate did not mitigate: naked {naked.DamagePerSwing:F2}/swing vs plated {plated.DamagePerSwing:F2}"
        );
    }

    [Fact]
    public void DodgeSuit_RaisesZeroDamageRate_InRealCombat_Smoke()
    {
        // The §9.8 dodge cap is 12% — a full Legendary Panika (dodge-lane) suit should show up
        // as a measurably higher zero-damage swing rate through the REAL CheckHit path.
        var loc = new Point3D(5120, 600, 0);
        var naked = SimulateDefense(null, 1200, loc);
        var dodging = SimulateDefense(d =>
        {
            DressSuit(d, ArmorMaterialType.Leather, VariantRoot.Panika, ItemRarity.Legendary);
            Assert.Equal(12, WornEffectState.GetAggregate(d).DodgePct); // pool at the framework cap
        }, 1200, loc);

        // True delta is ~12pp; 4pp margin over N=1200 keeps noise ~4 sigma away.
        Assert.True(
            dodging.ZeroDamageRate > naked.ZeroDamageRate + 0.04,
            $"dodge suit did not change real hit rate: naked {naked.ZeroDamageRate:P1} vs dodge {dodging.ZeroDamageRate:P1}"
        );
    }

    [Fact]
    public void ThornsSuit_ReflectsDamageToAttacker_Smoke()
    {
        var loc = new Point3D(5140, 600, 0);
        var bramble = SimulateDefense(d => DressSuit(d, ArmorMaterialType.Studded, VariantRoot.Batos, ItemRarity.Legendary), 400, loc);

        Assert.True(bramble.ReflectPerSwing > 0, "Batos thorns suit returned no damage to the attacker");
    }

    // Full defensive sweep — env-gated tuning tool, mirrors the DPS sweep.
    [Fact]
    public void FullReport_WhenEnvSet()
    {
        var path = Environment.GetEnvironmentVariable(EnvVar);

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        const int swings = 2000; // ~5% sigma at 800 (heavy-tailed) — 2000 tightens the sweep
        var loc = new Point3D(5160, 600, 0);
        var lines = new List<string>();

        // Two reference attackers: light hits saturate mitigation percentages near the top —
        // the heavy section is where the plate/bone/ring gaps read true.
        var references = new (string Label, Func<BaseWeapon> Factory)[]
        {
            ("reference: KATANA (light hits)", () => new Katana()),
            ("reference: HALBERD (heavy hits)", () => new Halberd())
        };

        foreach (var (label, factory) in references)
        {
            var naked = SimulateDefense(null, swings, loc, factory);

            lines.Add($"== {label} ==");
            lines.Add("loadout | dmg/swing | mitigation vs naked | zero-dmg swings | reflect/swing");
            lines.Add($"naked | {naked.DamagePerSwing:F2} | - | {naked.ZeroDamageRate:P1} | {naked.ReflectPerSwing:F2}");
            lines.Add("note: Legendary 4+ piece suits include their material capstone; shield rows depend on era-gated parry code.");
            lines.Add("");

            foreach (var (material, family) in FamilyRegistry.ArmorFamilyByMaterial)
            {
                foreach (var root in FamilyRegistry.LaneRoots(family.Lanes))
                {
                    var result = SimulateDefense(d => DressSuit(d, material, root, ItemRarity.Legendary), swings, loc, factory);
                    var mitigation = 1 - result.DamagePerSwing / naked.DamagePerSwing;

                    lines.Add(
                        $"{material} {root} x{family.SlotFactories.Length} | {result.DamagePerSwing:F2} | " +
                        $"{mitigation:P1} | {result.ZeroDamageRate:P1} | {result.ReflectPerSwing:F2}"
                    );
                }

                lines.Add("");
            }

            // Shield lanes on an otherwise naked defender (Parry 100).
            foreach (var root in new[] { VariantRoot.Aegis, VariantRoot.Amyntor, VariantRoot.Probolos, VariantRoot.Herkos, VariantRoot.Pnoe })
            {
                var result = SimulateDefense(d =>
                {
                    var shield = new HeaterShield();
                    RarityEffects.ApplyVariant(shield, root, ItemRarity.Legendary);
                    Assert.True(d.EquipItem(shield));
                }, swings, loc, factory);

                var mitigation = 1 - result.DamagePerSwing / naked.DamagePerSwing;
                lines.Add($"HeaterShield {root} | {result.DamagePerSwing:F2} | {mitigation:P1} | {result.ZeroDamageRate:P1} | {result.ReflectPerSwing:F2}");
            }

            lines.Add("");
        }

        File.WriteAllLines(path, lines);
    }
}
