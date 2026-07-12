using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Plate — metal armor (family 7), material ladder 2. "The forged colossus." Note: Plate's capstone
// name/icon ("Siege-Shock"/Knockout) is also the default arm of the legacy WornEffectState switch,
// so every non-set material reports these too — the FamilyRegistry facade reproduces that default.
public static class PlateFamily
{
    public static readonly ArmorFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyMetalArmor,
        Material = ArmorMaterialType.Plate,
        LadderIndex = 2,
        CapstoneThreshold = 4,
        CapstoneName = "Siege-Shock",
        CapstoneIcon = BuffIcon.Knockout,
        SlotFactories = new Func<Item>[]
        {
            () => new PlateChest(), () => new PlateLegs(), () => new PlateArms(),
            () => new PlateGorget(), () => new PlateGloves(), RollPlateHelm
        },
        SetPieces = new Func<Item>[]
        {
            () => new PlateHelm(), () => new PlateGorget(), () => new PlateChest(),
            () => new PlateArms(), () => new PlateGloves(), () => new PlateLegs()
        },
        SlotSignatures = new[]
        {
            new SlotSignature(ArmorBodyType.Helmet, ClauseType.ParaResistBoostsResistSkill, 5, 5, 0),
            new SlotSignature(ArmorBodyType.Gorget, ClauseType.FirstParaAutoFails, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Chest, ClauseType.ShrugFirstHitGuaranteed, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Arms, ClauseType.ShrugReflectStun, 10, 0, 0),
            new SlotSignature(ArmorBodyType.Gloves, ClauseType.HealBlockOnFirstHitLanded, 3, 0, 0),
            new SlotSignature(ArmorBodyType.Legs, ClauseType.OnKillRestoreExtraHp, 15, 0, 0)
        },
        Lanes = new[]
        {
            // Adamas — adamant (top AR + DR)
            new LaneDefinition
            {
                Root = VariantRoot.Adamas, DisplayName = "adamas", MythTag = "unbreakable", BaseHue = 66, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1, DrPct = 1 },
                    new ArmorEffectRow { BonusAr = 2, DrPct = 2 },
                    new ArmorEffectRow { BonusAr = 3, DrPct = 2 },
                    new ArmorEffectRow { BonusAr = 4, DrPct = 3 }
                }
            },
            // Kaminos — kiln (flame proc + reflect)
            new LaneDefinition
            {
                Root = VariantRoot.Kaminos, DisplayName = "kaminos", MythTag = "the kiln", BaseHue = 68, StackGroup = StackGroup.Forge,
                Armor = new[]
                {
                    new ArmorEffectRow { ReflectPct = 2 },
                    new ArmorEffectRow { ReflectPct = 3, FlameProcPct = 4 },
                    new ArmorEffectRow { ReflectPct = 5, FlameProcPct = 8 },
                    new ArmorEffectRow { ReflectPct = 6, FlameProcPct = 10 }
                }
            },
            // Kolossos — colossus (shrug + para resist)
            new LaneDefinition
            {
                Root = VariantRoot.Kolossos, DisplayName = "kolossos", MythTag = "the colossus", BaseHue = 70, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { ShrugPct = 3, ParaResistPct = 10 },
                    new ArmorEffectRow { ShrugPct = 4, ParaResistPct = 15 },
                    new ArmorEffectRow { ShrugPct = 5, ParaResistPct = 20 },
                    new ArmorEffectRow { ShrugPct = 8, ParaResistPct = 30 }
                }
            },
            // Panoplia — panoply (AR). Self-repair removed with the durability overhaul (Part B2).
            new LaneDefinition
            {
                Root = VariantRoot.Panoplia, DisplayName = "panoplia", MythTag = "the panoply", BaseHue = 72, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2 },
                    new ArmorEffectRow { BonusAr = 3 },
                    new ArmorEffectRow { BonusAr = 4 }
                }
            },
            // Akamatos — unwearying (HP regen). Self-repair removed with the durability overhaul (Part B2).
            new LaneDefinition
            {
                Root = VariantRoot.Akamatos, DisplayName = "akamatos", MythTag = "tireless", BaseHue = 74, StackGroup = StackGroup.Mending,
                Armor = new[]
                {
                    new ArmorEffectRow { HpRegenPct = 8 },
                    new ArmorEffectRow { HpRegenPct = 12 },
                    new ArmorEffectRow { HpRegenPct = 18 },
                    new ArmorEffectRow { HpRegenPct = 25 }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(188, "Kadmos", VariantRoot.Adamas, LegendaryRegistry.FamilyMetalArmor, 2, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ShrugFirstHitDrBurst (SHRUG == Plate Chest/Arms slot sigs); Plate sigs span SHRUG/SPELL_DR/RESIST_SKILL/ON_KILL, so the adamant endures via REGEN_BURST
            new LegendaryEntry(191, "Talos", VariantRoot.Kaminos, LegendaryRegistry.FamilyMetalArmor, 2, ClauseType.FlameProcBoostLowHp, 12, 30, 0, 0),
            new LegendaryEntry(194, "Iapyx", VariantRoot.Akamatos, LegendaryRegistry.FamilyMetalArmor, 2, ClauseType.StamRegenMirrorsHp, 0, 0, 0, 0), // SWAP AutoCureClearsDebuffsOnce
            new LegendaryEntry(197, "Glaukos", VariantRoot.Kolossos, LegendaryRegistry.FamilyMetalArmor, 2, ClauseType.EmergencyRegenTick, 10, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was SpellDrVsPoisonDot (SPELL_DR == Plate/Gorget FirstParaAutoFails sig); ward lanes are all forbidden on Plate, colossus falls back to REGEN_BURST
            new LegendaryEntry(200, "Damastor", VariantRoot.Panoplia, LegendaryRegistry.FamilyMetalArmor, 2, ClauseType.DeflectSecondaryFirstHit, 0, 0, 0, 0) // SWAP DodgeRestoreMana
        }
    };

    // Plate helm slot: uniform among the five interchangeable metal-helm shapes (same AR row).
    private static Item RollPlateHelm() => Utility.Random(5) switch
    {
        0 => new Helmet(),
        1 => new Bascinet(),
        2 => new NorseHelm(),
        3 => new CloseHelm(),
        _ => new PlateHelm()
    };
}
