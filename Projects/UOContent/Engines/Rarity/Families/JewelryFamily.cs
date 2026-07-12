using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Jewelry — family 10. No damage/AR ladder (framework §7): one legendary per (theme x slot).
// Namespace: 20-jewelry.md §3. Own Legendary column, a step above Epic (framework §4).
public static class JewelryFamily
{
    public static readonly AccessoryFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyJewelry,
        IsClothing = false,
        Factories = new Func<Item>[]
        {
            () => new GoldRing(), () => new GoldBracelet(), () => new GoldNecklace(), () => new GoldEarrings()
        },
        Lanes = new[]
        {
            // Olympian — Zeus (might)
            new LaneDefinition
            {
                Root = VariantRoot.Olympian, DisplayName = "olympian", MythTag = "Zeus", BaseHue = 2213,
                Jewelry = new[]
                {
                    new AccessoryEffectRow { StatBonus = 2 },
                    new AccessoryEffectRow { StatBonus = 4 },
                    new AccessoryEffectRow { StatBonus = 6, LightningProcPct = 3 },
                    new AccessoryEffectRow { StatBonus = 9, LightningProcPct = 5 }
                }
            },
            // Hecatean — Hecate (sorcery)
            new LaneDefinition
            {
                Root = VariantRoot.Hecatean, DisplayName = "hecatean", MythTag = "Hecate", BaseHue = 896, StackGroup = StackGroup.Sorcery,
                Jewelry = new[]
                {
                    new AccessoryEffectRow { ManaRegenPct = 8 },
                    new AccessoryEffectRow { ManaRegenPct = 12, SpellDamagePct = 4 },
                    new AccessoryEffectRow { ManaRegenPct = 18, SpellDamagePct = 7, ManaLeechPct = 3 },
                    new AccessoryEffectRow { ManaRegenPct = 25, SpellDamagePct = 10, ManaLeechPct = 5 }
                }
            },
            // Tychean — Tyche (fortune)
            new LaneDefinition
            {
                Root = VariantRoot.Tychean, DisplayName = "tychean", MythTag = "Tyche", BaseHue = 66,
                Jewelry = new[]
                {
                    new AccessoryEffectRow { HitHalvedPct = 2 },
                    new AccessoryEffectRow { HitHalvedPct = 3 },
                    new AccessoryEffectRow { HitHalvedPct = 4, MissRerollPct = 4 },
                    new AccessoryEffectRow { HitHalvedPct = 5, MissRerollPct = 6 }
                }
            },
            // Nyxian — Nyx (night)
            new LaneDefinition
            {
                Root = VariantRoot.Nyxian, DisplayName = "nyxian", MythTag = "Nyx", BaseHue = 1109, StackGroup = StackGroup.Night,
                Jewelry = new[]
                {
                    new AccessoryEffectRow { HidingBonus = 5 },
                    new AccessoryEffectRow { HidingBonus = 5, StealthBonus = 5 },
                    new AccessoryEffectRow { HidingBonus = 10, StealthBonus = 10, NightSight = true },
                    new AccessoryEffectRow { HidingBonus = 15, StealthBonus = 15, NightSight = true, PoisonResistPct = 10 }
                }
            },
            // Demetrian — Demeter (harvest)
            new LaneDefinition
            {
                Root = VariantRoot.Demetrian, DisplayName = "demetrian", MythTag = "Demeter", BaseHue = 51,
                Jewelry = new[]
                {
                    new AccessoryEffectRow { AllRegenPct = 4 },
                    new AccessoryEffectRow { AllRegenPct = 6 },
                    new AccessoryEffectRow { AllRegenPct = 8, PotionEffectPct = 10 },
                    new AccessoryEffectRow { AllRegenPct = 12, PotionEffectPct = 15 }
                }
            }
        },
        Legendaries = new[]
        {
            // Olympian line (Zeus)
            new LegendaryEntry(246, "Hyperion", VariantRoot.Olympian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotRing, ClauseType.LightningProcRefundStam, 10, 0, 0, 0),
            new LegendaryEntry(247, "Ouranos", VariantRoot.Olympian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotBracelet, ClauseType.LightningProcChanceRestoreMana, 50, 10, 0, 0),
            new LegendaryEntry(248, "Aither", VariantRoot.Olympian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotNecklace, ClauseType.StatBonusSplashSecondStat, 0, 0, 0, 0),
            new LegendaryEntry(249, "Astraios", VariantRoot.Olympian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotEarrings, ClauseType.LightningProcResistBurst, 10, 3, 0, 0),

            // Hecatean line (Hecate)
            new LegendaryEntry(250, "Selene", VariantRoot.Hecatean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotRing, ClauseType.ManaLeechRestoresStam, 5, 0, 0, 0),
            new LegendaryEntry(251, "Asteria", VariantRoot.Hecatean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotBracelet, ClauseType.OnKillFullManaRestore, 0, 0, 0, 0),
            new LegendaryEntry(252, "Phoibe", VariantRoot.Hecatean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotNecklace, ClauseType.ManaRegenDoubleLowMana, 25, 0, 0, 0),
            new LegendaryEntry(253, "Theia", VariantRoot.Hecatean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotEarrings, ClauseType.ManaLeechResistBurst, 10, 3, 0, 0),

            // Tychean line (Tyche)
            new LegendaryEntry(254, "Ananke", VariantRoot.Tychean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotRing, ClauseType.HitHalvedRegenPulse, 3, 0, 0, 0),
            new LegendaryEntry(255, "Metis", VariantRoot.Tychean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotBracelet, ClauseType.MissRerollGrazeRestoreStam, 10, 0, 0, 0),
            new LegendaryEntry(256, "Nemesis", VariantRoot.Tychean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotNecklace, ClauseType.HitHalvedReflectSpared, 0, 0, 0, 0),
            new LegendaryEntry(257, "Themis", VariantRoot.Tychean, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotEarrings, ClauseType.HitHalvedResistBurst, 10, 3, 0, 0),

            // Nyxian line (Nyx)
            new LegendaryEntry(258, "Hypnos", VariantRoot.Nyxian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotRing, ClauseType.StealthBreakRefundStam, 0, 0, 0, 0),
            new LegendaryEntry(259, "Khaos", VariantRoot.Nyxian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotBracelet, ClauseType.RegenDoubleWhileHidden, 0, 0, 0, 0),
            new LegendaryEntry(260, "Moros", VariantRoot.Nyxian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotNecklace, ClauseType.HideRestoresMana, 10, 0, 0, 0),
            new LegendaryEntry(261, "Achlys", VariantRoot.Nyxian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotEarrings, ClauseType.PoisonResistDoubleWhileHidden, 0, 0, 0, 0),

            // Demetrian line (Demeter)
            new LegendaryEntry(262, "Gaia", VariantRoot.Demetrian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotRing, ClauseType.PotionRestoresStam, 10, 0, 0, 0),
            new LegendaryEntry(263, "Rhea", VariantRoot.Demetrian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotBracelet, ClauseType.RegenDoubleAfterPotion, 5, 0, 0, 0),
            new LegendaryEntry(264, "Tethys", VariantRoot.Demetrian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotNecklace, ClauseType.PotionRestoresMana, 10, 0, 0, 0),
            new LegendaryEntry(265, "Okeanos", VariantRoot.Demetrian, LegendaryRegistry.FamilyJewelry, LegendaryRegistry.JewelrySlotEarrings, ClauseType.OnKillTriggerHeldPotion, 0, 0, 0, 0)
        }
    };
}
