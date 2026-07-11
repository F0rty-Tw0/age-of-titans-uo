using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Staves — family 4. Ladder: quarter staff 0, gnarled staff 1, black staff 2. Namespace:
// seers/sorcerers. Roots per 05-staves.md §3. "The seer's rod." Every staff variant also carries
// the universal mana-regen rider (§1: +6/+8/+12 by tier), folded into each lane row.
public static class StavesFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyStaves,
        LadderTypes = new[] { typeof(QuarterStaff), typeof(GnarledStaff), typeof(BlackStaff) },
        Ratios = new[] { 0.65, 0.72, 0.80 },
        SwingSeconds = new[] { 2.70, 2.85, 3.00 },
        Factories = new Func<Item>[] { () => new QuarterStaff(), () => new GnarledStaff(), () => new BlackStaff() },
        Lanes = new[]
        {
            // Empousa (siphon): mana leech; signature = crits burst-leech double the normal %.
            new LaneDefinition
            {
                Root = VariantRoot.Empousa, DisplayName = "empousa", MythTag = "Empousa", BaseHue = 896,
                Weapon = new[]
                {
                    new WeaponEffectRow { ManaLeechPct = 6, ManaRegenPct = 6 },
                    new WeaponEffectRow { ManaLeechPct = 8, CritChancePct = 8, ManaRegenPct = 8 },
                    new WeaponEffectRow { ManaLeechPct = 10, CritChancePct = 10, ManaRegenPct = 12, Signature = ClauseType.CritManaLeech, S2 = 20 },
                    new WeaponEffectRow { ManaLeechPct = 10, CritChancePct = 10, ManaRegenPct = 12, Signature = ClauseType.CritManaLeech, S2 = 20 }
                }
            },
            // Prester (storm): elemental proc; signature = None (fixed lightning).
            new LaneDefinition
            {
                Root = VariantRoot.Prester, DisplayName = "prester", MythTag = "fire-wind", BaseHue = 898,
                Weapon = new[]
                {
                    new WeaponEffectRow { ElementalProcPct = 6, ManaRegenPct = 6 },
                    new WeaponEffectRow { ElementalProcPct = 8, CritChancePct = 8, ManaRegenPct = 8 },
                    new WeaponEffectRow { ElementalProcPct = 10, CritChancePct = 10, ManaRegenPct = 12 },
                    new WeaponEffectRow { ElementalProcPct = 10, CritChancePct = 10, ManaRegenPct = 12 }
                }
            },
            // Alexikakos (ward): spell DR + Resisting Spells (worn-side); signature = first spell
            // each fight is heavily DR'd (SpellDrBoostFirstHit S1 = boosted DR %).
            new LaneDefinition
            {
                Root = VariantRoot.Alexikakos, DisplayName = "alexikakos", MythTag = "Apollo", BaseHue = 900, StackGroup = StackGroup.Ward,
                Weapon = new[]
                {
                    new WeaponEffectRow { SpellDrPct = 2, ManaRegenPct = 6 },
                    new WeaponEffectRow { SpellDrPct = 3, HitChancePct = 6, ManaRegenPct = 8 },
                    new WeaponEffectRow { SpellDrPct = 5, HitChancePct = 8, ResistSkillBonus = 5, ManaRegenPct = 12, Signature = ClauseType.SpellDrBoostFirstHit, S1 = 15 },
                    new WeaponEffectRow { SpellDrPct = 5, HitChancePct = 8, ResistSkillBonus = 5, ManaRegenPct = 12, Signature = ClauseType.SpellDrBoostFirstHit, S1 = 15 }
                }
            },
            // Manteia (oracle): mana regen (lane + universal) + heals-received + auto-cure; sig = None.
            new LaneDefinition
            {
                Root = VariantRoot.Manteia, DisplayName = "manteia", MythTag = "prophecy", BaseHue = 902, StackGroup = StackGroup.Sorcery,
                Weapon = new[]
                {
                    new WeaponEffectRow { ManaRegenPct = 14 },
                    new WeaponEffectRow { ManaRegenPct = 20, HitChancePct = 6 },
                    new WeaponEffectRow { ManaRegenPct = 30, HitChancePct = 8, HealsReceivedPct = 15, AutoCure = true },
                    new WeaponEffectRow { ManaRegenPct = 30, HitChancePct = 8, HealsReceivedPct = 15, AutoCure = true }
                }
            },
            // Baskania (curse): lifesteal + heal-block procs; signature = crits heal-block.
            new LaneDefinition
            {
                Root = VariantRoot.Baskania, DisplayName = "baskania", MythTag = "the evil eye", BaseHue = 904,
                Weapon = new[]
                {
                    new WeaponEffectRow { LifestealPct = 6, ManaRegenPct = 6 },
                    new WeaponEffectRow { LifestealPct = 6, HealBlockProcPct = 6, ManaRegenPct = 8 },
                    new WeaponEffectRow { LifestealPct = 8, HealBlockProcPct = 8, ManaRegenPct = 12, Signature = ClauseType.CritHealBlock, S2 = 3 },
                    new WeaponEffectRow { LifestealPct = 8, HealBlockProcPct = 8, ManaRegenPct = 12, Signature = ClauseType.CritHealBlock, S2 = 3 }
                }
            }
        },
        Legendaries = new[]
        {
            // Empousa line (was Zephyr — mana drawn from a shade's grasp)
            new LegendaryEntry(126, "Teiresias", VariantRoot.Empousa, LegendaryRegistry.FamilyStaves, 0, ClauseType.ExtraSwingManaLeech, 5, 5, 0, 0),
            new LegendaryEntry(127, "Kalchas", VariantRoot.Empousa, LegendaryRegistry.FamilyStaves, 1, ClauseType.ExtraSwingManaLeech, 5, 8, 0, 0), // SWAP ExtraSwingElemental (elemental is Prester's); P1 5-hit cadence
            new LegendaryEntry(128, "Amphiaraos", VariantRoot.Empousa, LegendaryRegistry.FamilyStaves, 2, ClauseType.ExtraSwingManaLeech, 5, 10, 0, 0), // SWAP ExtraSwingHealBlock (heal-block is Baskania's)

            // Prester line (was Phobos — the storm's circle). Signature = None.
            new LegendaryEntry(129, "Kassandra", VariantRoot.Prester, LegendaryRegistry.FamilyStaves, 0, ClauseType.CritElemental, 0, 0, 1, 0), // SWAP CritManaLeech (mana leech is Empousa's); P3=1 first hit
            new LegendaryEntry(130, "Mopsos", VariantRoot.Prester, LegendaryRegistry.FamilyStaves, 1, ClauseType.CritElemental, 5, 1, 0, 0), // P2=1 fire
            new LegendaryEntry(131, "Melampos", VariantRoot.Prester, LegendaryRegistry.FamilyStaves, 2, ClauseType.CritElemental, 6, 0, 0, 0), // SWAP CritHealBlock (heal-block is Baskania's); lightning

            // Manteia line (was Agrotera — the oracle's mercy). All swap to the auto-cure family.
            new LegendaryEntry(132, "Medeia", VariantRoot.Manteia, LegendaryRegistry.FamilyStaves, 0, ClauseType.AutoCureRestoresStamMana, 10, 0, 0, 0), // SWAP MarkManaLeech
            new LegendaryEntry(133, "Kirke", VariantRoot.Manteia, LegendaryRegistry.FamilyStaves, 1, ClauseType.AutoCureRestoresHpPct, 8, 0, 0, 0), // SWAP MarkElemental
            new LegendaryEntry(134, "Phineus", VariantRoot.Manteia, LegendaryRegistry.FamilyStaves, 2, ClauseType.LowHpEmergencyCure, 30, 15, 0, 0), // SWAP MarkHealBlockFirstHit

            // Alexikakos line (was Pallas — the ward that turns evil aside). Signature = SpellDrBoostFirstHit.
            new LegendaryEntry(135, "Orpheus", VariantRoot.Alexikakos, LegendaryRegistry.FamilyStaves, 0, ClauseType.ParaResistBoostsSpellDr, 10, 5, 0, 0), // SWAP BlockManaLeech
            new LegendaryEntry(136, "Polyeidos", VariantRoot.Alexikakos, LegendaryRegistry.FamilyStaves, 1, ClauseType.SpellDrBurstOnCritTaken, 6, 0, 0, 0), // SWAP BlockElemental
            new LegendaryEntry(137, "Helenos", VariantRoot.Alexikakos, LegendaryRegistry.FamilyStaves, 2, ClauseType.FirstParaAutoFails, 0, 0, 0, 0), // SWAP ReflectHealBlock

            // Baskania line (was Stygian — the evil eye lingers). Signature = CritHealBlock; uniques avoid it.
            new LegendaryEntry(138, "Manto", VariantRoot.Baskania, LegendaryRegistry.FamilyStaves, 0, ClauseType.MarkHealBlock, 3, 0, 0, 0), // SWAP CritManaLeech
            new LegendaryEntry(139, "Idmon", VariantRoot.Baskania, LegendaryRegistry.FamilyStaves, 1, ClauseType.MarkHealBlockFirstHit, 3, 0, 0, 0), // SWAP CritElemental
            new LegendaryEntry(140, "Theoklymenos", VariantRoot.Baskania, LegendaryRegistry.FamilyStaves, 2, ClauseType.HealBlockOnFirstHitLanded, 3, 0, 0, 0) // SWAP CritHealBlock (== signature)
        }
    };
}
