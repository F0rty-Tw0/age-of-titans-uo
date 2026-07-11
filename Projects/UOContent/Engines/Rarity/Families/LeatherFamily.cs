using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Leather — light armor (family 8), material ladder 0. Namespace: 11-armor-light.md §3-4.
// "The nymph's hide." Armor Legendary is a numeric step above Epic (framework §4).
public static class LeatherFamily
{
    public static readonly ArmorFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyLightArmor,
        Material = ArmorMaterialType.Leather,
        LadderIndex = 0,
        CapstoneThreshold = 4,
        CapstoneName = "Evasion",
        CapstoneIcon = BuffIcon.Evasion,
        SlotFactories = new Func<Item>[]
        {
            () => new LeatherChest(), () => new LeatherLegs(), RollLeatherHelm,
            () => new LeatherArms(), () => new LeatherGorget(), () => new LeatherGloves()
        },
        SetPieces = new Func<Item>[]
        {
            () => new LeatherCap(), () => new LeatherGorget(), () => new LeatherChest(),
            () => new LeatherArms(), () => new LeatherGloves(), () => new LeatherLegs()
        },
        SlotSignatures = new[]
        {
            new SlotSignature(ArmorBodyType.Helmet, ClauseType.SpellDrBoostFirstHit, 10, 0, 0),
            new SlotSignature(ArmorBodyType.Gorget, ClauseType.RerollFirstResist, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Chest, ClauseType.FirstHitNoSecondaryEffect, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Arms, ClauseType.ReflectBoostFirstHit, 10, 0, 0),
            new SlotSignature(ArmorBodyType.Gloves, ClauseType.DodgeGrantsCounterWindow, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Legs, ClauseType.DodgeRegenBurst, 20, 5, 0)
        },
        Lanes = new[]
        {
            // Naias — mending (HP regen + heals received)
            new LaneDefinition
            {
                Root = VariantRoot.Naias, DisplayName = "naias", MythTag = "the naiads", BaseHue = 65, StackGroup = StackGroup.Mending,
                Armor = new[]
                {
                    new ArmorEffectRow { HpRegenPct = 8 },
                    new ArmorEffectRow { HpRegenPct = 12, HealsReceivedPct = 5 },
                    new ArmorEffectRow { HpRegenPct = 18, HealsReceivedPct = 8 },
                    new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10 }
                }
            },
            // Dryas — briar (thorns + poison resist)
            new LaneDefinition
            {
                Root = VariantRoot.Dryas, DisplayName = "dryas", MythTag = "the dryads", BaseHue = 67, StackGroup = StackGroup.Forge,
                Armor = new[]
                {
                    new ArmorEffectRow { ReflectPct = 2, PoisonResistPct = 5 },
                    new ArmorEffectRow { ReflectPct = 3, PoisonResistPct = 8 },
                    new ArmorEffectRow { ReflectPct = 5, PoisonResistPct = 10 },
                    new ArmorEffectRow { ReflectPct = 6, PoisonResistPct = 12 }
                }
            },
            // Oreias — stride (weight + stam regen)
            new LaneDefinition
            {
                Root = VariantRoot.Oreias, DisplayName = "oreias", MythTag = "the oreads", BaseHue = 69, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { WeightReductionPct = 10, StamRegenPct = 4 },
                    new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 6 },
                    new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8 },
                    new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10 }
                }
            },
            // Melissa — balm (auto-cure + heals received)
            new LaneDefinition
            {
                Root = VariantRoot.Melissa, DisplayName = "melissa", MythTag = "Melissa", BaseHue = 71, StackGroup = StackGroup.Mending,
                Armor = new[]
                {
                    new ArmorEffectRow { HealsReceivedPct = 5 },
                    new ArmorEffectRow { HealsReceivedPct = 5, AutoCure = true },
                    new ArmorEffectRow { HealsReceivedPct = 8, AutoCure = true },
                    new ArmorEffectRow { HealsReceivedPct = 10, AutoCure = true }
                }
            },
            // Panika — startle (dodge)
            new LaneDefinition
            {
                Root = VariantRoot.Panika, DisplayName = "panika", MythTag = "Pan", BaseHue = 73, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { DodgePct = 3 },
                    new ArmorEffectRow { DodgePct = 4 },
                    new ArmorEffectRow { DodgePct = 5 },
                    new ArmorEffectRow { DodgePct = 8 }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(201, "Nemea", VariantRoot.Naias, LegendaryRegistry.FamilyLightArmor, 0, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // SWAP ShrugFirstHitGuaranteed
            new LegendaryEntry(204, "Teumessos", VariantRoot.Dryas, LegendaryRegistry.FamilyLightArmor, 0, ClauseType.ReflectBoostFirstHit, 10, 0, 0, 0), // SWAP FlameProcDoubleFirstHit
            new LegendaryEntry(207, "Kyrene", VariantRoot.Melissa, LegendaryRegistry.FamilyLightArmor, 0, ClauseType.AutoCureRestoresHpPct, 5, 0, 0, 0),
            new LegendaryEntry(210, "Arethousa", VariantRoot.Panika, LegendaryRegistry.FamilyLightArmor, 0, ClauseType.DodgeGrantsCounterWindow, 0, 0, 0, 0), // SWAP FirstParaAutoFails
            new LegendaryEntry(213, "Kyllene", VariantRoot.Oreias, LegendaryRegistry.FamilyLightArmor, 0, ClauseType.StamRegenMirrorsManaHalf, 0, 0, 0, 0) // SWAP DodgeDoubleFirstAttack
        }
    };

    // Leather helm slot: orc helm here is a Leather-material item (code truth over the doc's
    // bone-shape claim), so it rolls as a Leather-helm alternate (11-armor-light.md §1).
    private static Item RollLeatherHelm() => Utility.RandomBool() ? (Item)new LeatherCap() : new OrcHelm();
}
