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

// Whole-BUILD simulator: complete presets (weapon + suit + shield + relics — real archetypes a
// player would assemble, across weapon types incl. crossbow, halberd, staff, spear) measured on
// BOTH axes through the real pipeline:
//   offense  — preset attacker vs reference dummy (as RarityDpsSimTests)
//   defense  — preset defender vs reference katana attacker (as RarityDefenseSimTests)
// Composite power = DPS x sqrt(EHP factor), EHP factor = naked damage-taken / preset damage-taken
// (sqrt so mitigation-stacking tanks don't dominate the scale). The always-on threshold test is
// the BALANCE BAND: every preset must land within [0.45x, 2.2x] of the preset median — wide
// enough for archetype identity and RNG, tight enough that a runaway combo (or a gutted one)
// fails the suite loudly. Env-gated table: RARITY_BUILD_SIM=<path>.
[Collection("Sequential UOContent Tests")]
public class RarityBuildSimTests
{
    private const string EnvVar = "RARITY_BUILD_SIM";

    // N=2000: per-run sigma at N=800 measured ~5% (heavy-tailed damage chains); 2000 keeps the
    // band test far from flake while the whole 8-preset sweep stays under ~10s.
    private const int Swings = 2000;

    static RarityBuildSimTests()
    {
        Mobile.SkillCheckDirectLocationHandler ??= (m, _, chance) => chance >= Utility.RandomDouble();
        Mobile.SkillCheckDirectTargetHandler ??= (m, _, _, chance) => chance >= Utility.RandomDouble();
    }

    private sealed record BuildPreset(string Name, ushort WeaponId, Action<PlayerMobile> Dress);

    private static void DressLegendaries(PlayerMobile m, params ushort[] ids)
    {
        foreach (var id in ids)
        {
            Assert.True(LegendaryRegistry.TryGet(id, out var entry));

            // Armor legendaries construct a RANDOM piece of their material — reroll until it
            // lands on a free slot (two ringmail legendaries must not both roll the chest).
            var equipped = false;

            for (var attempt = 0; attempt < 12 && !equipped; attempt++)
            {
                var item = LootRoller.ConstructForLegendary(entry);
                RarityEffects.ApplyLegendary(item, id);
                equipped = m.EquipItem(item);

                if (!equipped)
                {
                    item.Delete();
                }
            }

            Assert.True(equipped, $"could not equip {entry.Name} on a free slot after 12 rolls");
        }
    }

    private static void DressSuit(PlayerMobile m, ArmorMaterialType material, VariantRoot root)
    {
        Assert.True(FamilyRegistry.ArmorFamilyByMaterial.TryGetValue(material, out var family));

        foreach (var factory in family.SlotFactories)
        {
            var piece = (BaseArmor)factory();
            RarityEffects.ApplyVariant(piece, root, ItemRarity.Legendary);
            Assert.True(m.EquipItem(piece), $"could not equip {piece.GetType().Name}");
        }
    }

    private static void DressShield(PlayerMobile m, VariantRoot root)
    {
        var shield = new HeaterShield();
        RarityEffects.ApplyVariant(shield, root, ItemRarity.Legendary);
        Assert.True(m.EquipItem(shield));
    }

    // The 8 archetypes — deliberately spanning weapon types: 1H sword, 2H spear, 1H hammer pick,
    // 2H halberd, heavy crossbow, 2H staff, and both shield and shieldless builds.
    private static BuildPreset[] Presets() => new BuildPreset[]
    {
        new("War Exarch (sword+board, 5x Ares)", 48, m => // Meriones
            DressLegendaries(m, 186, 195, 222, 268, 272)), // Kekrops+Nereus+Amphion+Atropos+Kisseus
        new("Wind Duelist (spear + dodge leather)", 146, m => // Peleus
            DressSuit(m, ArmorMaterialType.Leather, VariantRoot.Panika)),
        new("Stone Bulwark (pick + plate + Aegis)", 116, m => // Skeptron
        {
            DressSuit(m, ArmorMaterialType.Plate, VariantRoot.Adamas);
            DressShield(m, VariantRoot.Aegis);
        }),
        new("Grave Reaper (halberd + bone)", 82, m => // Thoon
            DressSuit(m, ArmorMaterialType.Bone, VariantRoot.Tymbos)),
        new("Far Huntress (heavy xbow + studded)", 179, m => // Penthesileia
            DressSuit(m, ArmorMaterialType.Studded, VariantRoot.Kynegis)),
        new("Storm Seer (staff + chain ward)", 131, m => // Melampos
            DressSuit(m, ArmorMaterialType.Chainmail, VariantRoot.Phrourion)),
        new("Glass Flurry (spear, no armor)", 146, null), // Peleus naked
        new("Sunlit Skirmisher (sword + leather + Amyntor)", 56, m => // Chrysaor
        {
            DressSuit(m, ArmorMaterialType.Leather, VariantRoot.Naias);
            DressShield(m, VariantRoot.Amyntor);
        })
    };

    private static PlayerMobile CreateCombatant(Point3D location, AccessLevel access)
    {
        var m = new PlayerMobile(World.NewMobile);
        m.DefaultMobileInit();
        m.AccessLevel = access;
        m.RawStr = 150;
        m.RawDex = 100;
        m.RawInt = 25;
        m.Hits = m.HitsMax;
        m.Stam = m.StamMax;
        m.MoveToWorld(location, Map.Felucca);
        return m;
    }

    private static void DeleteAll(PlayerMobile m)
    {
        for (var i = m.Items.Count - 1; i >= 0; i--)
        {
            m.Items[i].Delete();
        }

        m.Delete();
    }

    // Offense half: the dressed preset attacker vs a deep-HP raw-Mobile dummy.
    private static double MeasureDps(in BuildPreset preset, Point3D location)
    {
        var prior = Core.Expansion;
        Core.Expansion = Expansion.T2A;

        var attacker = CreateCombatant(location, AccessLevel.GameMaster);
        var defender = new Mobile(World.NewMobile);
        defender.DefaultMobileInit();
        defender.RawStr = 5000;
        defender.Hits = defender.HitsMax;
        defender.MoveToWorld(new Point3D(location.X + 1, location.Y, location.Z), Map.Felucca);

        try
        {
            Assert.True(LegendaryRegistry.TryGet(preset.WeaponId, out var entry));
            var item = LootRoller.ConstructForLegendary(entry);
            RarityEffects.ApplyLegendary(item, entry.Id);
            var weapon = (BaseWeapon)item;

            attacker.Skills[weapon.Skill].Base = 100;
            attacker.Skills[SkillName.Tactics].Base = 100;
            attacker.Skills[SkillName.Anatomy].Base = 100;
            Assert.True(attacker.EquipItem(weapon));
            preset.Dress?.Invoke(attacker);

            if (weapon is BaseRanged ranged)
            {
                attacker.AddToBackpack(new Arrow(60000));
                attacker.AddToBackpack(new Bolt(60000));
                attacker.MoveToWorld(new Point3D(location.X - Math.Min(4, ranged.MaxRange - 1), location.Y, location.Z), Map.Felucca);
                attacker.LastMoveTime = Core.TickCount - 2000;
            }

            var seconds = 0.0;
            long damage = 0;

            for (var i = 0; i < Swings; i++)
            {
                if (!defender.Alive)
                {
                    defender.Resurrect();
                }

                defender.Hits = defender.HitsMax;
                attacker.Hits = attacker.HitsMax;
                attacker.Stam = attacker.StamMax;
                RarityDpsSimTests.RefitDurability(attacker);

                seconds += weapon.OnSwing(attacker, defender).TotalSeconds;
                damage += defender.HitsMax - defender.Hits;
            }

            return damage / seconds;
        }
        finally
        {
            Core.Expansion = prior;
            defender.Delete();
            DeleteAll(attacker);
        }
    }

    // Defense half: a reference katana attacker vs the dressed preset defender (weapon included —
    // its worn-side folds, dodge and block clauses all count). Returns damage taken per swing.
    private static double MeasureTakenPerSwing(in BuildPreset preset, Point3D location, bool naked = false)
    {
        var prior = Core.Expansion;
        Core.Expansion = Expansion.T2A;

        var attacker = CreateCombatant(location, AccessLevel.GameMaster);
        var defender = CreateCombatant(new Point3D(location.X + 1, location.Y, location.Z), AccessLevel.Player);
        var katana = new Katana();

        try
        {
            attacker.Skills[SkillName.Swords].Base = 100;
            attacker.Skills[SkillName.Tactics].Base = 100;
            attacker.Skills[SkillName.Anatomy].Base = 100;
            Assert.True(attacker.EquipItem(katana));

            defender.Skills[SkillName.Parry].Base = 100;

            if (!naked)
            {
                Assert.True(LegendaryRegistry.TryGet(preset.WeaponId, out var entry));
                var item = LootRoller.ConstructForLegendary(entry);
                RarityEffects.ApplyLegendary(item, entry.Id);
                var weapon = (BaseWeapon)item;
                defender.Skills[weapon.Skill].Base = 100;
                Assert.True(defender.EquipItem(weapon));
                preset.Dress?.Invoke(defender);
            }
            else
            {
                // The naked baseline still holds a plain weapon at skill: an unarmed test dummy
                // has no Fists bootstrap, so its defense skill would read 0 and saturate CheckHit.
                var plain = new Katana();
                defender.Skills[SkillName.Swords].Base = 100;
                Assert.True(defender.EquipItem(plain));
            }

            long taken = 0;

            for (var i = 0; i < Swings; i++)
            {
                if (!defender.Alive)
                {
                    defender.Resurrect();
                }

                defender.Hits = defender.HitsMax;
                attacker.Hits = attacker.HitsMax;
                attacker.Stam = attacker.StamMax;
                RarityDpsSimTests.RefitDurability(attacker);
                RarityDpsSimTests.RefitDurability(defender);

                katana.OnSwing(attacker, defender);
                taken += defender.HitsMax - defender.Hits;
            }

            return (double)taken / Swings;
        }
        finally
        {
            Core.Expansion = prior;
            katana.Delete();
            DeleteAll(defender);
            DeleteAll(attacker);
        }
    }

    private readonly record struct BuildResult(string Name, double Dps, double TakenPerSwing, double Power);

    private static List<BuildResult> MeasureAll(Point3D location)
    {
        var presets = Presets();
        var nakedTaken = MeasureTakenPerSwing(presets[0], location, naked: true);
        var results = new List<BuildResult>(presets.Length);

        foreach (var preset in presets)
        {
            var dps = MeasureDps(preset, location);
            var taken = MeasureTakenPerSwing(preset, location);
            var ehpFactor = nakedTaken / Math.Max(0.05, taken);
            var power = dps * Math.Sqrt(ehpFactor);

            results.Add(new BuildResult(preset.Name, dps, taken, power));
        }

        return results;
    }

    // THE BALANCE BAND — the deliverable threshold: after the 2026-07-11 tuning pass every
    // archetype's composite power sits within [0.45x, 2.2x] of the preset median. A future data
    // change that lets one build lap the field (or guts one) fails here, with names and numbers.
    [SkippableFact]
    public void AllBuildPresets_WithinPowerBand()
    {
        TileDataRequirement.SkipIfMissing();
        var results = MeasureAll(new Point3D(5200, 600, 0));

        var powers = new List<double>(results.Count);

        foreach (var r in results)
        {
            powers.Add(r.Power);
        }

        powers.Sort();
        var median = powers[powers.Count / 2];

        foreach (var r in results)
        {
            Assert.True(
                r.Power > median * 0.45 && r.Power < median * 2.2,
                $"build '{r.Name}' power {r.Power:F1} is outside the balance band " +
                $"[{median * 0.45:F1} .. {median * 2.2:F1}] (median {median:F1}, dps {r.Dps:F1}, taken/swing {r.TakenPerSwing:F2})"
            );
        }
    }

    [Fact]
    public void FullReport_WhenEnvSet()
    {
        var path = Environment.GetEnvironmentVariable(EnvVar);

        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        var results = MeasureAll(new Point3D(5220, 600, 0));

        var lines = new List<string>
        {
            "build preset | DPS | taken/swing | composite power (dps x sqrt(EHP factor))",
            ""
        };

        foreach (var r in results)
        {
            lines.Add($"{r.Name} | {r.Dps:F2} | {r.TakenPerSwing:F2} | {r.Power:F1}");
        }

        File.WriteAllLines(path, lines);
    }
}
