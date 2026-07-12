using System;
using Server.Engines.BuffIcons;
using Server.Items;

namespace Server.Engines.Rarity;

// Ringmail — metal armor (family 7), material ladder 0. Namespace: 10-armor-metal.md §3-4.
// "The hoplite's kit."
public static class RingmailFamily
{
    public static readonly ArmorFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyMetalArmor,
        Material = ArmorMaterialType.Ringmail,
        LadderIndex = 0,
        CapstoneThreshold = 4,
        CapstoneName = "Phalanx-Thorns",
        CapstoneIcon = BuffIcon.Block,
        SlotFactories = new Func<Item>[]
        {
            () => new RingmailChest(), () => new RingmailLegs(), () => new RingmailArms(), () => new RingmailGloves()
        },
        SetPieces = new Func<Item>[]
        {
            () => new RingmailChest(), () => new RingmailArms(), () => new RingmailGloves(), () => new RingmailLegs()
        },
        SlotSignatures = new[]
        {
            new SlotSignature(ArmorBodyType.Chest, ClauseType.ShrugFirstHitDrBurst, 8, 3, 0),
            new SlotSignature(ArmorBodyType.Arms, ClauseType.ShrugStunAttacker, 0, 0, 0),
            new SlotSignature(ArmorBodyType.Gloves, ClauseType.OnKillStamRestoreExtendImmunity, 5, 0, 0),
            new SlotSignature(ArmorBodyType.Legs, ClauseType.StamRegenMirrorsManaHalf, 0, 0, 0)
        },
        Lanes = new[]
        {
            // Hoplites — line-shield (AR + block-as-shrug)
            new LaneDefinition
            {
                Root = VariantRoot.Hoplites, DisplayName = "hoplites", MythTag = "the hoplites", BaseHue = 49, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2, ShrugPct = 3 },
                    new ArmorEffectRow { BonusAr = 3, ShrugPct = 5 },
                    new ArmorEffectRow { BonusAr = 4, ShrugPct = 8 }
                }
            },
            // Taxis — formation (DR + para resist)
            new LaneDefinition
            {
                Root = VariantRoot.Taxis, DisplayName = "taxis", MythTag = "battle order", BaseHue = 51, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { DrPct = 1, ParaResistPct = 10 },
                    new ArmorEffectRow { DrPct = 2, ParaResistPct = 15 },
                    new ArmorEffectRow { DrPct = 2, ParaResistPct = 20 },
                    new ArmorEffectRow { DrPct = 3, ParaResistPct = 30 }
                }
            },
            // Dromos — march (stam regen + weight)
            new LaneDefinition
            {
                Root = VariantRoot.Dromos, DisplayName = "dromos", MythTag = "the march", BaseHue = 53, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { WeightReductionPct = 10, StamRegenPct = 4 },
                    new ArmorEffectRow { WeightReductionPct = 15, StamRegenPct = 6 },
                    new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 8 },
                    new ArmorEffectRow { WeightReductionPct = 25, StamRegenPct = 10 }
                }
            },
            // Zoster — war-belt (bonus AR). Was the durability/self-repair lane; self-repair was
            // removed with the durability overhaul (Part B2), so this lane now carries a plain AR
            // ladder. Flagged for a proper re-theme in a later balance pass — it currently mirrors
            // the Hoplites AR identity.
            new LaneDefinition
            {
                Root = VariantRoot.Zoster, DisplayName = "zoster", MythTag = "the girdle", BaseHue = 55, StackGroup = StackGroup.Durability,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2 },
                    new ArmorEffectRow { BonusAr = 3 },
                    new ArmorEffectRow { BonusAr = 4 }
                }
            },
            // Alkimos — valiant (shrug)
            new LaneDefinition
            {
                Root = VariantRoot.Alkimos, DisplayName = "alkimos", MythTag = "the valiant", BaseHue = 57, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { ShrugPct = 3 },
                    new ArmorEffectRow { ShrugPct = 4 },
                    new ArmorEffectRow { ShrugPct = 5 },
                    new ArmorEffectRow { ShrugPct = 8 }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(186, "Kekrops", VariantRoot.Hoplites, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ShrugFirstHitDrainStam (SHRUG == Ringmail Chest/Arms slot sigs)
            new LegendaryEntry(189, "Perdix", VariantRoot.Zoster, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.ReflectBurstOnCritBlock, 5, 0, 0, 0), // SWAP FlameProcDoubleFirstHit
            new LegendaryEntry(192, "Machaon", VariantRoot.Alkimos, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.SpellDrVsPoisonDot, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ShrugReflect (SHRUG == Ringmail Chest/Arms slot sigs); Machaon the healer wards poison DoT (SPELL_DR is clear on Ringmail), and re-homes the enabler the split removed from Skylla
            new LegendaryEntry(195, "Nereus", VariantRoot.Taxis, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.ParaResistBoostsSpellDr, 10, 5, 0, 0),
            new LegendaryEntry(198, "Automedon", VariantRoot.Dromos, LegendaryRegistry.FamilyMetalArmor, 0, ClauseType.DodgeRefundStam, 10, 0, 0, 0) // de-overlap 2026-07-12: was OnKillStamRestoreExtendImmunity (== Ringmail/Gloves slot signature); restores pre-SWAP identity
        }
    };
}
