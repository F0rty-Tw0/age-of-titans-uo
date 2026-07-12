using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Bone — light armor (family 8), material ladder 2. "The grave-warden."
public static class BoneFamily
{
    public static readonly ArmorFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyLightArmor,
        Material = ArmorMaterialType.Bone,
        LadderIndex = 2,
        CapstoneThreshold = 4,
        CapstoneName = "Grave-Chill",
        CapstoneIcon = BuffIcon.DeathStrike,
        SlotFactories = new Func<Item>[]
        {
            () => new BoneChest(), () => new BoneLegs(), () => new BoneHelm(), () => new BoneArms(), () => new BoneGloves()
        },
        SetPieces = new Func<Item>[]
        {
            () => new BoneHelm(), () => new BoneChest(), () => new BoneArms(), () => new BoneGloves(), () => new BoneLegs()
        },
        SlotSignatures = new[]
        {
            new SlotSignature(ArmorBodyType.Helmet, ClauseType.SpellDrBurstOnCritTaken, 3, 0, 0),
            new SlotSignature(ArmorBodyType.Chest, ClauseType.ShrugFirstHitDrainStam, 5, 0, 0),
            new SlotSignature(ArmorBodyType.Arms, ClauseType.ReflectCritStun, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Gloves, ClauseType.OnKillRestoreMissingHpPct, 50, 0, 0),
            new SlotSignature(ArmorBodyType.Legs, ClauseType.OnKillRestoreHpPct, 10, 0, 0)
        },
        Lanes = new[]
        {
            // Melinoe — phantom (dodge + para resist)
            new LaneDefinition
            {
                Root = VariantRoot.Melinoe, DisplayName = "melinoe", MythTag = "Melinoe", BaseHue = 1109, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { DodgePct = 3, ParaResistPct = 10 },
                    new ArmorEffectRow { DodgePct = 4, ParaResistPct = 15 },
                    new ArmorEffectRow { DodgePct = 5, ParaResistPct = 20 },
                    new ArmorEffectRow { DodgePct = 8, ParaResistPct = 30 }
                }
            },
            // Makaria — blessed death (on-kill restores)
            new LaneDefinition
            {
                Root = VariantRoot.Makaria, DisplayName = "makaria", MythTag = "Makaria", BaseHue = 1111, StackGroup = StackGroup.Mending,
                Armor = new[]
                {
                    new ArmorEffectRow { OnKillStamPct = 5 },
                    new ArmorEffectRow { OnKillStamPct = 8 },
                    new ArmorEffectRow { OnKillStamPct = 10 },
                    new ArmorEffectRow { OnKillStamPct = 15 }
                }
            },
            // Tymbos — tomb (AR + DR)
            new LaneDefinition
            {
                Root = VariantRoot.Tymbos, DisplayName = "tymbos", MythTag = "the barrow", BaseHue = 1113, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1, DrPct = 1 },
                    new ArmorEffectRow { BonusAr = 2, DrPct = 2 },
                    new ArmorEffectRow { BonusAr = 3, DrPct = 2 },
                    new ArmorEffectRow { BonusAr = 4, DrPct = 3 }
                }
            },
            // Nekyia — death-ward (spell DR)
            new LaneDefinition
            {
                Root = VariantRoot.Nekyia, DisplayName = "nekyia", MythTag = "the nekyia", BaseHue = 1115, StackGroup = StackGroup.Ward,
                Armor = new[]
                {
                    new ArmorEffectRow { SpellDrPct = 2 },
                    new ArmorEffectRow { SpellDrPct = 3 },
                    new ArmorEffectRow { SpellDrPct = 5 },
                    new ArmorEffectRow { SpellDrPct = 6 }
                }
            },
            // Katachthon — grave-thorns (reflect)
            new LaneDefinition
            {
                Root = VariantRoot.Katachthon, DisplayName = "katachthon", MythTag = "underworld", BaseHue = 1117, StackGroup = StackGroup.Forge,
                Armor = new[]
                {
                    new ArmorEffectRow { ReflectPct = 2 },
                    new ArmorEffectRow { ReflectPct = 3 },
                    new ArmorEffectRow { ReflectPct = 5 },
                    new ArmorEffectRow { ReflectPct = 6 }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(203, "Erymanthos", VariantRoot.Tymbos, LegendaryRegistry.FamilyLightArmor, 2, ClauseType.DeflectSecondaryFirstHit, 0, 0, 0, 0), // SWAP ShrugReflect
            new LegendaryEntry(206, "Echidna", VariantRoot.Katachthon, LegendaryRegistry.FamilyLightArmor, 2, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // de-overlap 2026-07-12: was ReflectCritStun (== Bone/Arms slot signature)
            new LegendaryEntry(209, "Keryneia", VariantRoot.Makaria, LegendaryRegistry.FamilyLightArmor, 2, ClauseType.EmergencyRegenTick, 10, 0, 0, 0),
            new LegendaryEntry(212, "Krommyon", VariantRoot.Nekyia, LegendaryRegistry.FamilyLightArmor, 2, ClauseType.ResistSkillDoubleLowHp, 10, 50, 0, 0), // de-overlap 2026-07-12 (arming-group split): was RerollFirstResist (SPELL_DR == Bone/Helmet SpellDrBurstOnCritTaken sig); RESIST_SKILL keeps the death-ward identity
            new LegendaryEntry(215, "Kalydon", VariantRoot.Melinoe, LegendaryRegistry.FamilyLightArmor, 2, ClauseType.DodgeRefundStamPct, 10, 0, 0, 0) // arming-group split re-home 2026-07-12: was DodgeRegenBurst (still live as the Leather/Legs slot sig); Bone permits DODGE and Melinoe carries the dodge lane. Reworked 2026-07-12 from suit-weight refund to a flat 10% max-stamina refund (weight-independent, per user directive)
        }
    };
}
