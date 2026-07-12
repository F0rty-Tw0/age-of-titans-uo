using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Clothing — family 11. No ladder (21-clothing.md §1): drops cap at Epic; Legendary exists only as
// the bound relics (one body piece + one hat per theme), and the Epic row is duplicated into the
// Legendary slot. Factory order matches LegendaryRegistry.ClothingPieceXxx (body 0-4, hats 8-11).
//
// Two magnitude classes by opportunity cost (21-clothing.md §1): "Clothing" is the over-armor cloth
// (robe/cloak/sash/doublet/tunic/shirts/kilt/skirt/feet), which coexists with armor. "ClothingDisplacing"
// is exactly 2x those values and is served to the armor-DISPLACING shapes — hats (Layer.Helm, replace
// helms) and cloth pants (Layer.Pants, replace leg armor) — since equipping one sacrifices an armor
// slot (AR + material root + slot signature + capstone progress) for a jewelry-level bonus. The 2x is
// DATA (declared below), not a runtime multiplier: booleans carry over unchanged; no clothing field
// touches a framework §9.8 shared-pool ceiling even doubled (max dodge 8 < 12, stationary regen 40
// folds under the 60% HP-regen consume cap), so no cap-clamps apply.
public static class ClothingFamily
{
    public static readonly AccessoryFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyClothing,
        IsClothing = true,
        Factories = new Func<Item>[]
        {
            () => new BodySash(), () => new FancyShirt(), () => new Kilt(), () => new Robe(), () => new Cloak(),
            () => new Doublet(), () => new Tunic(), () => new Skirt(),
            () => new StrawHat(), () => new WideBrimHat(), () => new FeatheredHat(), () => new Cap(),
            () => new Boots(), () => new ThighBoots(), () => new FurBoots(), () => new Shoes(), () => new Sandals(),
            () => new LongPants(), () => new ShortPants()
        },
        Lanes = new[]
        {
            // Laurel — Nike (victory)
            new LaneDefinition
            {
                Root = VariantRoot.Laurel, DisplayName = "laurel", MythTag = "Nike", BaseHue = 66,
                Clothing = new[]
                {
                    new AccessoryEffectRow { OnKillStamina = 5 },
                    new AccessoryEffectRow { OnKillStamina = 10 },
                    new AccessoryEffectRow { OnKillStamina = 10, OnKillHp = 5 },
                    new AccessoryEffectRow { OnKillStamina = 10, OnKillHp = 5 }
                },
                ClothingDisplacing = new[]
                {
                    new AccessoryEffectRow { OnKillStamina = 10 },
                    new AccessoryEffectRow { OnKillStamina = 20 },
                    new AccessoryEffectRow { OnKillStamina = 20, OnKillHp = 10 },
                    new AccessoryEffectRow { OnKillStamina = 20, OnKillHp = 10 }
                }
            },
            // Charis — Aphrodite (charm)
            new LaneDefinition
            {
                Root = VariantRoot.Charis, DisplayName = "charis", MythTag = "Aphrodite", BaseHue = 25,
                Clothing = new[]
                {
                    new AccessoryEffectRow { KarmaGainPct = 5 },
                    new AccessoryEffectRow { KarmaGainPct = 10, VendorPricePct = 3 },
                    new AccessoryEffectRow { KarmaGainPct = 15, VendorPricePct = 5 },
                    new AccessoryEffectRow { KarmaGainPct = 15, VendorPricePct = 5 }
                },
                ClothingDisplacing = new[]
                {
                    new AccessoryEffectRow { KarmaGainPct = 10 },
                    new AccessoryEffectRow { KarmaGainPct = 20, VendorPricePct = 6 },
                    new AccessoryEffectRow { KarmaGainPct = 30, VendorPricePct = 10 },
                    new AccessoryEffectRow { KarmaGainPct = 30, VendorPricePct = 10 }
                }
            },
            // Maenad — Dionysos (frenzy)
            new LaneDefinition
            {
                Root = VariantRoot.Maenad, DisplayName = "maenad", MythTag = "Dionysus", BaseHue = 15,
                Clothing = new[]
                {
                    new AccessoryEffectRow { FrenzyChancePct = 2, FrenzyDamagePct = 10 },
                    new AccessoryEffectRow { FrenzyChancePct = 3, FrenzyDamagePct = 10 },
                    new AccessoryEffectRow { FrenzyChancePct = 4, FrenzyDamagePct = 10, FrenzySwingPct = 10 },
                    new AccessoryEffectRow { FrenzyChancePct = 4, FrenzyDamagePct = 10, FrenzySwingPct = 10 }
                },
                ClothingDisplacing = new[]
                {
                    new AccessoryEffectRow { FrenzyChancePct = 4, FrenzyDamagePct = 20 },
                    new AccessoryEffectRow { FrenzyChancePct = 6, FrenzyDamagePct = 20 },
                    new AccessoryEffectRow { FrenzyChancePct = 8, FrenzyDamagePct = 20, FrenzySwingPct = 20 },
                    new AccessoryEffectRow { FrenzyChancePct = 8, FrenzyDamagePct = 20, FrenzySwingPct = 20 }
                }
            },
            // Hestian — Hestia (hearth)
            new LaneDefinition
            {
                Root = VariantRoot.Hestian, DisplayName = "hestian", MythTag = "Hestia", BaseHue = 43,
                Clothing = new[]
                {
                    new AccessoryEffectRow { StationaryRegenPct = 10 },
                    new AccessoryEffectRow { StationaryRegenPct = 15 },
                    new AccessoryEffectRow { StationaryRegenPct = 20, StationaryAppliesMana = true },
                    new AccessoryEffectRow { StationaryRegenPct = 20, StationaryAppliesMana = true }
                },
                ClothingDisplacing = new[]
                {
                    new AccessoryEffectRow { StationaryRegenPct = 20 },
                    new AccessoryEffectRow { StationaryRegenPct = 30 },
                    new AccessoryEffectRow { StationaryRegenPct = 40, StationaryAppliesMana = true },
                    new AccessoryEffectRow { StationaryRegenPct = 40, StationaryAppliesMana = true }
                }
            },
            // Arachne — Arachne (web)
            new LaneDefinition
            {
                Root = VariantRoot.Arachne, DisplayName = "arachne", MythTag = "Arachne", BaseHue = 907,
                Clothing = new[]
                {
                    new AccessoryEffectRow { DodgePct = 2 },
                    new AccessoryEffectRow { DodgePct = 3, PoisonResistPct = 5 },
                    new AccessoryEffectRow { DodgePct = 4, PoisonResistPct = 10 },
                    new AccessoryEffectRow { DodgePct = 4, PoisonResistPct = 10 }
                },
                ClothingDisplacing = new[]
                {
                    new AccessoryEffectRow { DodgePct = 4 },
                    new AccessoryEffectRow { DodgePct = 6, PoisonResistPct = 10 },
                    new AccessoryEffectRow { DodgePct = 8, PoisonResistPct = 20 },
                    new AccessoryEffectRow { DodgePct = 8, PoisonResistPct = 20 }
                }
            }
        },
        Legendaries = new[]
        {
            // Body-piece relics (the Fates and weavers) — over-armor cloth.
            new LegendaryEntry(266, "Klotho", VariantRoot.Laurel, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceBodySash, ClauseType.OnKillStamRegenBurstStacking, 20, 5, 0, 0),
            new LegendaryEntry(267, "Lachesis", VariantRoot.Charis, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceFancyShirt, ClauseType.AnimalTamingSkillBonus, 15, 0, 0, 0),
            new LegendaryEntry(268, "Atropos", VariantRoot.Maenad, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceKilt, ClauseType.FrenzyStaggerChance, 4, 0, 0, 0),
            new LegendaryEntry(269, "Ariadne", VariantRoot.Hestian, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceRobe, ClauseType.LowHpDodgeBurst, 10, 5, 0, 0),
            new LegendaryEntry(270, "Penelope", VariantRoot.Arachne, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceCloak, ClauseType.DodgeSnare, 30, 3, 0, 0),

            // Hat relics (crowns/veils) — armor-displacing cloth; carry the displacing Epic package
            // (via ResolveRootRarity -> Legendary row) plus the clause below.
            new LegendaryEntry(271, "Kotinos", VariantRoot.Laurel, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceStrawHat, ClauseType.OnKillFullStamNextHitCrit, 5, 0, 0, 0),
            new LegendaryEntry(272, "Kisseus", VariantRoot.Maenad, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceWideBrimHat, ClauseType.FrenzyStaggerChance, 6, 0, 0, 0),
            new LegendaryEntry(273, "Diadema", VariantRoot.Charis, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceFeatheredHat, ClauseType.HealsReceivedBonusPct, 15, 0, 0, 0),
            new LegendaryEntry(274, "Kalyptra", VariantRoot.Hestian, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceCap, ClauseType.StationaryRegenFaster, 5, 0, 0, 0),
            new LegendaryEntry(275, "Kalathos", VariantRoot.Arachne, LegendaryRegistry.FamilyClothing, LegendaryRegistry.ClothingPieceStrawHat, ClauseType.DodgeRegenBurst, 20, 5, 0, 0)
        }
    };
}
