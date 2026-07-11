using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Studded — light armor (family 8), material ladder 1. "The hunter's brand."
public static class StuddedFamily
{
    public static readonly ArmorFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyLightArmor,
        Material = ArmorMaterialType.Studded,
        LadderIndex = 1,
        CapstoneThreshold = 4,
        CapstoneName = "Venom",
        CapstoneIcon = BuffIcon.InjectedStrike,
        SlotFactories = new Func<Item>[]
        {
            () => new StuddedChest(), () => new StuddedLegs(), () => new StuddedArms(),
            () => new StuddedGorget(), () => new StuddedGloves()
        },
        SetPieces = new Func<Item>[]
        {
            () => new StuddedGorget(), () => new StuddedChest(), () => new StuddedArms(),
            () => new StuddedGloves(), () => new StuddedLegs()
        },
        SlotSignatures = new[]
        {
            new SlotSignature(ArmorBodyType.Helmet, ClauseType.SpellDrVsPoisonDot, 0, 0, 0), // StuddedMempo only
            new SlotSignature(ArmorBodyType.Gorget, ClauseType.ParaResistStunsAttacker, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Chest, ClauseType.ShrugFirstHitPoisonAttacker, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Arms, ClauseType.ShrugReflect, 10, 0, 0),
            new SlotSignature(ArmorBodyType.Gloves, ClauseType.OnKillDodgeDoubleDuration, 5, 0, 0),
            new SlotSignature(ArmorBodyType.Legs, ClauseType.StamRegenMirrorsHp, 0, 0, 0)
        },
        Lanes = new[]
        {
            // Kynegis — chase (dodge + stam regen)
            new LaneDefinition
            {
                Root = VariantRoot.Kynegis, DisplayName = "kynegis", MythTag = "the huntress", BaseHue = 1106, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { DodgePct = 3, StamRegenPct = 4 },
                    new ArmorEffectRow { DodgePct = 4, StamRegenPct = 6 },
                    new ArmorEffectRow { DodgePct = 5, StamRegenPct = 8 },
                    new ArmorEffectRow { DodgePct = 8, StamRegenPct = 10 }
                }
            },
            // Batos — bramble (thorns)
            new LaneDefinition
            {
                Root = VariantRoot.Batos, DisplayName = "batos", MythTag = "the briar", BaseHue = 1108, StackGroup = StackGroup.Forge,
                Armor = new[]
                {
                    new ArmorEffectRow { ReflectPct = 2 },
                    new ArmorEffectRow { ReflectPct = 3 },
                    new ArmorEffectRow { ReflectPct = 5 },
                    new ArmorEffectRow { ReflectPct = 6 }
                }
            },
            // Arkas — bear (AR + shrug)
            new LaneDefinition
            {
                Root = VariantRoot.Arkas, DisplayName = "arkas", MythTag = "Arkas", BaseHue = 1110, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2, ShrugPct = 3 },
                    new ArmorEffectRow { BonusAr = 3, ShrugPct = 5 },
                    new ArmorEffectRow { BonusAr = 4, ShrugPct = 8 }
                }
            },
            // Elaphis — deer (weight + dodge)
            new LaneDefinition
            {
                Root = VariantRoot.Elaphis, DisplayName = "elaphis", MythTag = "the hind", BaseHue = 1112, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { WeightReductionPct = 10, DodgePct = 3 },
                    new ArmorEffectRow { WeightReductionPct = 20, DodgePct = 4 },
                    new ArmorEffectRow { WeightReductionPct = 30, DodgePct = 5 },
                    new ArmorEffectRow { WeightReductionPct = 40, DodgePct = 8 }
                }
            },
            // Skia — shadow (poison resist + Hiding)
            new LaneDefinition
            {
                Root = VariantRoot.Skia, DisplayName = "skia", MythTag = "shade", BaseHue = 1114, StackGroup = StackGroup.Night,
                Armor = new[]
                {
                    new ArmorEffectRow { PoisonResistPct = 5, HidingBonus = 2 },
                    new ArmorEffectRow { PoisonResistPct = 8, HidingBonus = 3 },
                    new ArmorEffectRow { PoisonResistPct = 10, HidingBonus = 5 },
                    new ArmorEffectRow { PoisonResistPct = 12, HidingBonus = 5 }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(202, "Kithairon", VariantRoot.Arkas, LegendaryRegistry.FamilyLightArmor, 1, ClauseType.ShrugStunAttacker, 0, 0, 0, 0),
            new LegendaryEntry(205, "Khimaira", VariantRoot.Batos, LegendaryRegistry.FamilyLightArmor, 1, ClauseType.ReflectCritStun, 0, 0, 0, 0), // SWAP FlameProcPoison
            new LegendaryEntry(208, "Daphne", VariantRoot.Elaphis, LegendaryRegistry.FamilyLightArmor, 1, ClauseType.WeightReductionSuiteBurstOnDodge, 5, 0, 0, 0), // SWAP LowHpEmergencyCure
            new LegendaryEntry(211, "Skylla", VariantRoot.Skia, LegendaryRegistry.FamilyLightArmor, 1, ClauseType.SpellDrVsPoisonDot, 0, 0, 0, 0),
            new LegendaryEntry(214, "Melanippe", VariantRoot.Kynegis, LegendaryRegistry.FamilyLightArmor, 1, ClauseType.DodgeRefundStam, 15, 0, 0, 0)
        }
    };
}
