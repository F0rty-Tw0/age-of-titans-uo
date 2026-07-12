using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Chainmail — metal armor (family 7), material ladder 1. Only 3 live slots (helm/chest/legs), so
// its set capstone threshold is 3, not 4. "The watchful wall."
public static class ChainmailFamily
{
    public static readonly ArmorFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyMetalArmor,
        Material = ArmorMaterialType.Chainmail,
        LadderIndex = 1,
        CapstoneThreshold = 3,
        CapstoneName = "Bulwark",
        CapstoneIcon = BuffIcon.Toughness,
        SlotFactories = new Func<Item>[]
        {
            () => new ChainChest(), () => new ChainLegs(), () => new ChainCoif()
        },
        SetPieces = new Func<Item>[]
        {
            () => new ChainCoif(), () => new ChainChest(), () => new ChainLegs()
        },
        SlotSignatures = new[]
        {
            new SlotSignature(ArmorBodyType.Helmet, ClauseType.ParaResistBoostsSpellDr, 15, 5, 0),
            new SlotSignature(ArmorBodyType.Chest, ClauseType.EmergencyRegenTick, 10, 0, 0),
            new SlotSignature(ArmorBodyType.Legs, ClauseType.HpRegenBurstOnCritTaken, 5, 0, 0)
        },
        Lanes = new[]
        {
            // Phylax — guard (AR + shrug)
            new LaneDefinition
            {
                Root = VariantRoot.Phylax, DisplayName = "phylax", MythTag = "the guard", BaseHue = 2213, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2, ShrugPct = 3 },
                    new ArmorEffectRow { BonusAr = 3, ShrugPct = 5 },
                    new ArmorEffectRow { BonusAr = 4, ShrugPct = 8 }
                }
            },
            // Egregoros — unsleeping (para resist + shrug)
            new LaneDefinition
            {
                Root = VariantRoot.Egregoros, DisplayName = "egregoros", MythTag = "the wakeful", BaseHue = 2215, StackGroup = StackGroup.Ward,
                Armor = new[]
                {
                    new ArmorEffectRow { ParaResistPct = 10 },
                    new ArmorEffectRow { ParaResistPct = 15, ShrugPct = 3 },
                    new ArmorEffectRow { ParaResistPct = 20, ShrugPct = 5 },
                    new ArmorEffectRow { ParaResistPct = 30, ShrugPct = 8 }
                }
            },
            // Teichos — wall (DR)
            new LaneDefinition
            {
                Root = VariantRoot.Teichos, DisplayName = "teichos", MythTag = "the wall", BaseHue = 2217, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { DrPct = 1 },
                    new ArmorEffectRow { DrPct = 2 },
                    new ArmorEffectRow { DrPct = 3 },
                    new ArmorEffectRow { DrPct = 3, BonusAr = 1 }
                }
            },
            // Halysis — the chain (bonus AR). Was the durability/self-repair lane; self-repair was
            // removed with the durability overhaul (Part B2), so this lane now carries a plain AR
            // ladder. Flagged for a proper re-theme in a later balance pass — it currently mirrors
            // the Phylax AR identity.
            new LaneDefinition
            {
                Root = VariantRoot.Halysis, DisplayName = "halysis", MythTag = "the chain", BaseHue = 2219, StackGroup = StackGroup.Durability,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2 },
                    new ArmorEffectRow { BonusAr = 3 },
                    new ArmorEffectRow { BonusAr = 4 }
                }
            },
            // Phrourion — fortress (spell DR)
            new LaneDefinition
            {
                Root = VariantRoot.Phrourion, DisplayName = "phrourion", MythTag = "stronghold", BaseHue = 2221, StackGroup = StackGroup.Ward,
                Armor = new[]
                {
                    new ArmorEffectRow { SpellDrPct = 2 },
                    new ArmorEffectRow { SpellDrPct = 3 },
                    new ArmorEffectRow { SpellDrPct = 5 },
                    new ArmorEffectRow { SpellDrPct = 6 }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(187, "Erechtheus", VariantRoot.Phylax, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.ShrugReflect, 15, 0, 0, 0),
            new LegendaryEntry(190, "Erichthonios", VariantRoot.Halysis, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.BlockRestoresHp, 5, 0, 0, 0), // SWAP FlameProcHealBlock
            new LegendaryEntry(193, "Podaleirios", VariantRoot.Phrourion, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.ResistSkillBoostLowHp, 5, 50, 0, 0), // de-overlap 2026-07-12 (arming-group split): was RerollFirstResist (SPELL_DR == Chainmail/Helmet sig); RESIST_SKILL clears both SPELL_DR (Helmet) and REGEN_BURST (Chest/Legs) chainmail lanes
            new LegendaryEntry(196, "Proteus", VariantRoot.Egregoros, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.ParaResistBoostsResistSkill, 10, 5, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ParaResistStunsAttacker (SPELL_DR == Chainmail/Helmet sig); RESIST_SKILL keeps the Egregoros para-resist identity
            new LegendaryEntry(199, "Patroklos", VariantRoot.Teichos, LegendaryRegistry.FamilyMetalArmor, 1, ClauseType.DeflectSecondaryFirstHit, 0, 0, 0, 0) // SWAP OnKillDodgeDoubleDuration
        }
    };
}
