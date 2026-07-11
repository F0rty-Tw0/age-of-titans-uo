using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Axes — family 0. The exemplar weapon family: it keeps the original five shared weapon roots
// (framework §3). Ladder (BaseIndex): hatchet 0, axe 1, battle axe 2, double axe 3, executioner's
// axe 4, two-handed axe 5, large battle axe 6, ornate axe 7. Legendary namespace: Harpies/wind.
public static class AxesFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyAxes,
        LadderTypes = new[]
        {
            typeof(Hatchet), typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe),
            typeof(ExecutionersAxe), typeof(TwoHandedAxe), typeof(LargeBattleAxe), typeof(OrnateAxe)
        },
        Ratios = new[] { 0.65, 0.70, 0.75, 0.80, 0.85, 0.90, 0.95, 1.00 },
        SwingSeconds = new[] { 2.75, 2.85, 2.95, 3.05, 3.15, 3.25, 3.35, 3.45 },
        Factories = new Func<Item>[]
        {
            () => new Hatchet(), () => new Axe(), () => new BattleAxe(), () => new DoubleAxe(),
            () => new ExecutionersAxe(), () => new TwoHandedAxe(), () => new LargeBattleAxe(), () => new OrnateAxe()
        },
        Lanes = new[]
        {
            // Zephyr — Hermes (speed)
            new LaneDefinition
            {
                Root = VariantRoot.Zephyr, DisplayName = "zephyr", MythTag = "Hermes", BaseHue = 89,
                Weapon = new[]
                {
                    new WeaponEffectRow { SwingSpeedPct = 8 },
                    new WeaponEffectRow { SwingSpeedPct = 8, HitChancePct = 6 },
                    new WeaponEffectRow { SwingSpeedPct = 10, HitChancePct = 8, ExtraSwingPct = 10 },
                    new WeaponEffectRow { SwingSpeedPct = 10, HitChancePct = 8, ExtraSwingPct = 10 }
                }
            },
            // Phobos — Ares (damage)
            new LaneDefinition
            {
                Root = VariantRoot.Phobos, DisplayName = "phobos", MythTag = "Ares", BaseHue = 34,
                Weapon = new[]
                {
                    new WeaponEffectRow { DamagePct = 8 },
                    new WeaponEffectRow { DamagePct = 8, CritChancePct = 8 },
                    new WeaponEffectRow { DamagePct = 10, CritChancePct = 10, CritDamagePct = 20 },
                    new WeaponEffectRow { DamagePct = 10, CritChancePct = 10, CritDamagePct = 20 }
                }
            },
            // Agrotera — Artemis (mark)
            new LaneDefinition
            {
                Root = VariantRoot.Agrotera, DisplayName = "agrotera", MythTag = "Artemis", BaseHue = 64,
                Weapon = new[]
                {
                    new WeaponEffectRow { MarkChancePct = 6, MarkBonusPct = 8 },
                    new WeaponEffectRow { MarkChancePct = 8, MarkBonusPct = 10 },
                    new WeaponEffectRow { MarkChancePct = 10, MarkBonusPct = 14, MarkPoisonTick = true },
                    new WeaponEffectRow { MarkChancePct = 10, MarkBonusPct = 14, MarkPoisonTick = true }
                }
            },
            // Pallas — Athena (defense)
            new LaneDefinition
            {
                Root = VariantRoot.Pallas, DisplayName = "pallas", MythTag = "Athena", BaseHue = 1151,
                Weapon = new[]
                {
                    new WeaponEffectRow { BlockPct = 6 },
                    new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true }
                }
            },
            // Stygian — Hades (drain)
            new LaneDefinition
            {
                Root = VariantRoot.Stygian, DisplayName = "stygian", MythTag = "Hades", BaseHue = 1106,
                Weapon = new[]
                {
                    new WeaponEffectRow { LifestealPct = 6 },
                    new WeaponEffectRow { LifestealPct = 6, StamRegenPct = 6 },
                    new WeaponEffectRow { LifestealPct = 8, StamRegenPct = 8, LifestealExecute = true },
                    new WeaponEffectRow { LifestealPct = 8, StamRegenPct = 8, LifestealExecute = true }
                }
            }
        },
        Legendaries = new[]
        {
            // Zephyr line (Hermes)
            new LegendaryEntry(1, "Aello", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 0, ClauseType.ExtraSwingOnParry, 0, 0, 0, 0),
            new LegendaryEntry(2, "Boreas", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 1, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
            new LegendaryEntry(3, "Podarge", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 2, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
            new LegendaryEntry(4, "Ocypete", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 3, ClauseType.DoubleStrikeEveryN, 5, 0, 0, 0),
            new LegendaryEntry(5, "Euros", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 4, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(6, "Notos", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 5, ClauseType.ExtraSwingEveryN, 5, 1, 0, 0), // P2=1 stagger
            new LegendaryEntry(7, "Celaeno", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 6, ClauseType.ExtraSwingEveryN, 5, 10, 0, 0), // P2=armor pen %
            new LegendaryEntry(8, "Aellopos", VariantRoot.Zephyr, LegendaryRegistry.FamilyAxes, 7, ClauseType.ExtraSwingEveryN, 5, 0, 5, 0), // P3=stam leech %

            // Phobos line (Ares)
            new LegendaryEntry(9, "Ker", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 0, ClauseType.CritFirstHit, 0, 10, 3, 0),
            new LegendaryEntry(10, "Enyo", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 1, ClauseType.CritArmorPen, 5, 10, 0, 0),
            new LegendaryEntry(11, "Alala", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 2, ClauseType.CritSplash, 6, 10, 3, 0),
            new LegendaryEntry(12, "Labrys", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 3, ClauseType.CritSplash, 5, 15, 3, 0),
            new LegendaryEntry(13, "Polemos", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 4, ClauseType.CritExecuteUnder15, 6, 0, 0, 0),
            new LegendaryEntry(14, "Deimos", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 5, ClauseType.CritSplash, 6, 10, 3, 0),
            new LegendaryEntry(15, "Eris", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 6, ClauseType.CritSplash, 5, 10, 3, 0),
            new LegendaryEntry(16, "Enyalios", VariantRoot.Phobos, LegendaryRegistry.FamilyAxes, 7, ClauseType.CritEveryN, 6, 0, 0, 0),

            // Agrotera line (Artemis)
            new LegendaryEntry(17, "Taygete", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 0, ClauseType.MarkNearbyAllies, 3, 0, 0, 0),
            new LegendaryEntry(18, "Kallisto", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 1, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(19, "Britomartis", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 2, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0),
            new LegendaryEntry(20, "Oupis", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 3, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(21, "Aktaion", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 4, ClauseType.MarkHealBlock, 3, 0, 0, 0),
            new LegendaryEntry(22, "Orion", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 5, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(23, "Sagaris", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 6, ClauseType.MarkFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(24, "Atalanta", VariantRoot.Agrotera, LegendaryRegistry.FamilyAxes, 7, ClauseType.PoisonTickDoubled, 0, 0, 0, 0),

            // Pallas line (Athena)
            new LegendaryEntry(25, "Itonia", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(26, "Alalkomeneis", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 1, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(27, "Promachos", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 2, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(28, "Gorgoneion", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 3, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
            new LegendaryEntry(29, "Glaukopis", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 4, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(30, "Tritogeneia", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 5, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
            new LegendaryEntry(31, "Hippia", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 6, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
            new LegendaryEntry(32, "Parthenos", VariantRoot.Pallas, LegendaryRegistry.FamilyAxes, 7, ClauseType.BlockFirstHit, 0, 0, 0, 0),

            // Stygian line (Hades)
            new LegendaryEntry(33, "Lethe", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 0, ClauseType.StamDrainOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(34, "Acheron", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 1, ClauseType.OnKillRestore, 2, 0, 0, 0),
            new LegendaryEntry(35, "Kokytos", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 2, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(36, "Phlegethon", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 3, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(37, "Charon", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 4, ClauseType.OnKillRestore, 2, 0, 0, 0),
            new LegendaryEntry(38, "Erebos", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 5, ClauseType.OnKillRestore, 2, 0, 0, 0),
            new LegendaryEntry(39, "Tartaros", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 6, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(40, "Thanatos", VariantRoot.Stygian, LegendaryRegistry.FamilyAxes, 7, ClauseType.OnKillRestore, 2, 0, 0, 0)
        }
    };
}
