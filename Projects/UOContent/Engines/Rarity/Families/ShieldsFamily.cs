using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Shields — family 9. Ladder (§1): buckler 0, wooden shield 1, wooden kite 2, metal shield 3,
// metal kite 4, heater 5. Namespace: 12-shields.md §3-4. Root-keyed lanes (shields are NOT in
// ArmorSlotSignatureTable). Athena root here is Aegis; the other four are shield-only (re-theme).
public static class ShieldsFamily
{
    public static readonly ShieldFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyShields,
        ShieldFactories = new Func<Item>[]
        {
            () => new Buckler(), () => new WoodenShield(), () => new WoodenKiteShield(),
            () => new MetalShield(), () => new MetalKiteShield(), () => new HeaterShield()
        },
        Lanes = new[]
        {
            // Aegis — Athena (shield bulwark)
            new LaneDefinition
            {
                Root = VariantRoot.Aegis, DisplayName = "aegis", MythTag = "the aegis", BaseHue = 1151, StackGroup = StackGroup.Bulwark,
                Shield = new[]
                {
                    new ArmorEffectRow { ParryPct = 3 },
                    new ArmorEffectRow { ParryPct = 5, ParryDrPct = 5 },
                    new ArmorEffectRow { ParryPct = 7, ParryDrPct = 10, ParryThorns = true },
                    new ArmorEffectRow { ParryPct = 8, ParryDrPct = 12, ParryThorns = true }
                }
            },
            // Amyntor — counter (reflect + self-repair)
            new LaneDefinition
            {
                Root = VariantRoot.Amyntor, DisplayName = "amyntor", MythTag = "the defender", BaseHue = 1151, StackGroup = StackGroup.Forge,
                Shield = new[]
                {
                    new ArmorEffectRow { ReflectPct = 4 },
                    new ArmorEffectRow { ReflectPct = 6 },
                    new ArmorEffectRow { ReflectPct = 8, Signature = ClauseType.ParryExtraReflect, S1 = 6 },
                    // Legendary reflect 10 -> 12 (2026-07-11 sim pass): the DEDICATED reflect lane
                    // measured 8x below Probolos's block-elemental rider; nudged up while that
                    // rider's flat proc damage came down (RunBlockClause). Suit-wide cap 25 is law.
                    new ArmorEffectRow { ReflectPct = 12, Signature = ClauseType.ParryExtraReflect, S1 = 6 }
                }
            },
            // Probolos — breakwater (DR on block + stam regen)
            new LaneDefinition
            {
                Root = VariantRoot.Probolos, DisplayName = "probolos", MythTag = "breakwater", BaseHue = 1153, StackGroup = StackGroup.Bulwark,
                Shield = new[]
                {
                    new ArmorEffectRow { ParryDrPct = 5 },
                    new ArmorEffectRow { ParryDrPct = 8, StamRegenPct = 8 },
                    new ArmorEffectRow { ParryDrPct = 10, StamRegenPct = 10, Signature = ClauseType.BlockElemental, S1 = 0 },
                    new ArmorEffectRow { ParryDrPct = 12, StamRegenPct = 12, Signature = ClauseType.BlockElemental, S1 = 0 }
                }
            },
            // Herkos — barrier (spell DR + para resist)
            new LaneDefinition
            {
                Root = VariantRoot.Herkos, DisplayName = "herkos", MythTag = "the fence", BaseHue = 1155, StackGroup = StackGroup.Ward,
                Shield = new[]
                {
                    new ArmorEffectRow { SpellDrPct = 3 },
                    new ArmorEffectRow { SpellDrPct = 5, ParaResistPct = 15 },
                    new ArmorEffectRow { SpellDrPct = 7, ParaResistPct = 25, ResistSkillBonus = 5, Signature = ClauseType.ParaResistBoostsSpellDr, S1 = 15, S2 = 5 },
                    new ArmorEffectRow { SpellDrPct = 8, ParaResistPct = 30, ResistSkillBonus = 5, Signature = ClauseType.ParaResistBoostsSpellDr, S1 = 15, S2 = 5 }
                }
            },
            // Pnoe — second wind (HP regen + heals received, parry-stam signature)
            new LaneDefinition
            {
                Root = VariantRoot.Pnoe, DisplayName = "pnoe", MythTag = "breath", BaseHue = 1157, StackGroup = StackGroup.Mending,
                Shield = new[]
                {
                    new ArmorEffectRow { HpRegenPct = 10 },
                    new ArmorEffectRow { HpRegenPct = 15, HealsReceivedPct = 5 },
                    new ArmorEffectRow { HpRegenPct = 20, HealsReceivedPct = 8, Signature = ClauseType.ParryRestoresStam, S1 = 10 },
                    new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, Signature = ClauseType.ParryRestoresStam, S1 = 10 }
                }
            }
        },
        Legendaries = new[]
        {
            // Aegis line (Athena)
            new LegendaryEntry(216, "Ankyle", VariantRoot.Aegis, LegendaryRegistry.FamilyShields, 0, ClauseType.ParryFirstHitGuaranteed, 0, 0, 0, 0),
            new LegendaryEntry(217, "Oiliades", VariantRoot.Aegis, LegendaryRegistry.FamilyShields, 1, ClauseType.ParryCritStun, 0, 0, 0, 0),
            new LegendaryEntry(218, "Salamis", VariantRoot.Aegis, LegendaryRegistry.FamilyShields, 2, ClauseType.ParryExtraReflect, 6, 0, 0, 0),
            new LegendaryEntry(219, "Telamon", VariantRoot.Aegis, LegendaryRegistry.FamilyShields, 3, ClauseType.ParryForcesMissEveryN, 5, 0, 0, 0),
            new LegendaryEntry(220, "Sakos", VariantRoot.Aegis, LegendaryRegistry.FamilyShields, 4, ClauseType.LowHpGuaranteedParry, 30, 0, 0, 0),
            new LegendaryEntry(221, "Aias", VariantRoot.Aegis, LegendaryRegistry.FamilyShields, 5, ClauseType.ParryFirstHitGuaranteedStun, 0, 0, 0, 0),

            // was Cyclopean line → Amyntor (the counter-blow)
            new LegendaryEntry(222, "Amphion", VariantRoot.Amyntor, LegendaryRegistry.FamilyShields, 0, ClauseType.OnKillRestoreHpPct, 25, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectFirstHit (BLOCK_PARRY == ParryExtraReflect sig)
            new LegendaryEntry(223, "Zethos", VariantRoot.Amyntor, LegendaryRegistry.FamilyShields, 1, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // de-overlap 2026-07-12: was ReflectBurstOnCritBlock (shared ParryExtraReflect shield sig lane, reflect-boost)
            new LegendaryEntry(224, "Proitos", VariantRoot.Amyntor, LegendaryRegistry.FamilyShields, 2, ClauseType.SpellDrBurstOnCritTaken, 3, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectBoostFirstHit (BLOCK_PARRY == ParryExtraReflect sig)
            new LegendaryEntry(225, "Tiryns", VariantRoot.Amyntor, LegendaryRegistry.FamilyShields, 3, ClauseType.EmergencyRegenTick, 10, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectHealBlock (BLOCK_PARRY == ParryExtraReflect sig)
            new LegendaryEntry(226, "Danaos", VariantRoot.Amyntor, LegendaryRegistry.FamilyShields, 4, ClauseType.OnKillTriggerHeldPotion, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockRestoresHp (BLOCK_PARRY == ParryExtraReflect sig)
            new LegendaryEntry(227, "Akrisios", VariantRoot.Amyntor, LegendaryRegistry.FamilyShields, 5, ClauseType.StationaryRegenFaster, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectCritStun (BLOCK_PARRY == ParryExtraReflect sig)

            // was Paean line → Pnoe (second wind)
            new LegendaryEntry(228, "Phylakos", VariantRoot.Pnoe, LegendaryRegistry.FamilyShields, 0, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0),
            new LegendaryEntry(229, "Autonoos", VariantRoot.Pnoe, LegendaryRegistry.FamilyShields, 1, ClauseType.OnKillRestoreExtraHp, 15, 0, 0, 0),
            new LegendaryEntry(230, "Aiakos", VariantRoot.Pnoe, LegendaryRegistry.FamilyShields, 2, ClauseType.OnKillFullManaRestore, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockManaLeech (BLOCK_PARRY == ParryRestoresStam sig); keeps the mana identity off-lane
            new LegendaryEntry(231, "Aristaios", VariantRoot.Pnoe, LegendaryRegistry.FamilyShields, 3, ClauseType.ManaRegenMirrorsHp, 0, 0, 0, 0),
            new LegendaryEntry(232, "Boutes", VariantRoot.Pnoe, LegendaryRegistry.FamilyShields, 4, ClauseType.StamRegenMirrorsHp, 0, 0, 0, 0),
            new LegendaryEntry(233, "Asklepios", VariantRoot.Pnoe, LegendaryRegistry.FamilyShields, 5, ClauseType.OnKillRestoreHpPct, 25, 0, 0, 0),

            // was Tritonian line → Herkos (the fence of war)
            new LegendaryEntry(234, "Laomedon", VariantRoot.Herkos, LegendaryRegistry.FamilyShields, 0, ClauseType.ResistSkillDoubleLowHp, 10, 50, 0, 0), // de-overlap 2026-07-12 (arming-group split): was FirstParaAutoFails (SPELL_DR == ParaResistBoostsSpellDr sig); RESIST_SKILL keeps the ward identity
            new LegendaryEntry(235, "Ilion", VariantRoot.Herkos, LegendaryRegistry.FamilyShields, 1, ClauseType.ResistSkillBoostLowHp, 5, 50, 0, 0),
            new LegendaryEntry(236, "Palaimon", VariantRoot.Herkos, LegendaryRegistry.FamilyShields, 2, ClauseType.ReflectBurstOnCritBlock, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ParaResistStunsAttacker (SPELL_DR == ParaResistBoostsSpellDr sig); the fence throws crits back
            new LegendaryEntry(237, "Alkathous", VariantRoot.Herkos, LegendaryRegistry.FamilyShields, 3, ClauseType.ParaResistBoostsResistSkill, 10, 5, 0, 0),
            new LegendaryEntry(238, "Megareus", VariantRoot.Herkos, LegendaryRegistry.FamilyShields, 4, ClauseType.BlockRestoresHp, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was RerollFirstResist (SPELL_DR == ParaResistBoostsSpellDr sig); the barrier mends on a block
            new LegendaryEntry(239, "Hyperbios", VariantRoot.Herkos, LegendaryRegistry.FamilyShields, 5, ClauseType.DeflectSecondaryFirstHit, 0, 0, 0, 0),

            // was Talarian line → Probolos (the breakwater)
            new LegendaryEntry(240, "Panoptes", VariantRoot.Probolos, LegendaryRegistry.FamilyShields, 0, ClauseType.SpellDrBurstOnCritTaken, 3, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockRestoreStam (BLOCK_PARRY == BlockElemental sig)
            new LegendaryEntry(241, "Abderos", VariantRoot.Probolos, LegendaryRegistry.FamilyShields, 1, ClauseType.HpRegenBurstOnCritTaken, 5, 3, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ParryFirstHitGuaranteed (BLOCK_PARRY == BlockElemental sig)
            new LegendaryEntry(242, "Myrtilos", VariantRoot.Probolos, LegendaryRegistry.FamilyShields, 2, ClauseType.OnKillRestoreHpPct, 25, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockDrainStam (BLOCK_PARRY == BlockElemental sig)
            new LegendaryEntry(243, "Kerberos", VariantRoot.Probolos, LegendaryRegistry.FamilyShields, 3, ClauseType.EmergencyRegenTick, 10, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ParryFirstHitGuaranteed (BLOCK_PARRY == BlockElemental sig)
            new LegendaryEntry(244, "Lasthenes", VariantRoot.Probolos, LegendaryRegistry.FamilyShields, 4, ClauseType.OnKillFullManaRestore, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockManaLeech (BLOCK_PARRY == BlockElemental sig)
            new LegendaryEntry(245, "Melanippos", VariantRoot.Probolos, LegendaryRegistry.FamilyShields, 5, ClauseType.StationaryRegenFaster, 5, 0, 0, 0) // de-overlap 2026-07-12 (arming-group split): was BlockNextShotCrit (BLOCK_PARRY == BlockElemental sig)
        }
    };
}
