namespace Server.Engines.Rarity;

// The five retired shared armor roots (re-theme 2026-07-07). Decode-only for old saves — remapped
// to lane-specific roots on load (RethemeMigration). They keep their live table rows so old items
// still resolve correctly until migrated: all five have armor rows; four also have shield rows
// (Polias never legitimately sat on a shield). No legendaries, factories, or capstone — not a drop
// source. Aegis is NOT here — it is the active shield bulwark root (see ShieldsFamily).
public static class LegacyRoots
{
    public static readonly LegacyRootsDefinition Definition = new()
    {
        Lanes = new[]
        {
            // Polias — Athena (bulwark). Armor only.
            new LaneDefinition
            {
                Root = VariantRoot.Polias, DisplayName = "polias", MythTag = "Athena", BaseHue = 1151, StackGroup = StackGroup.Bulwark,
                Armor = new[]
                {
                    new ArmorEffectRow { BonusAr = 1 },
                    new ArmorEffectRow { BonusAr = 2, DrPct = 1 },
                    new ArmorEffectRow { BonusAr = 3, DrPct = 2, ShrugPct = 5 },
                    new ArmorEffectRow { BonusAr = 4, DrPct = 3, ShrugPct = 8 }
                }
            },
            // Cyclopean — Hephaistos (forge). Armor + shield.
            new LaneDefinition
            {
                Root = VariantRoot.Cyclopean, DisplayName = "cyclopean", MythTag = "Hephaestus", BaseHue = 44, StackGroup = StackGroup.Forge,
                Armor = new[]
                {
                    new ArmorEffectRow { ReflectPct = 2 },
                    new ArmorEffectRow { ReflectPct = 3, SelfRepair = true },
                    new ArmorEffectRow { ReflectPct = 5, SelfRepair = true, FlameProcPct = 4 },
                    new ArmorEffectRow { ReflectPct = 6, SelfRepair = true, FlameProcPct = 8 }
                },
                Shield = new[]
                {
                    new ArmorEffectRow { ReflectPct = 4 },
                    new ArmorEffectRow { ReflectPct = 6, SelfRepair = true },
                    new ArmorEffectRow { ReflectPct = 8, SelfRepair = true, FlameProcPct = 6 },
                    new ArmorEffectRow { ReflectPct = 10, SelfRepair = true, FlameProcPct = 10 }
                }
            },
            // Paean — Apollo (mending). Armor + shield.
            new LaneDefinition
            {
                Root = VariantRoot.Paean, DisplayName = "paean", MythTag = "Apollo", BaseHue = 49, StackGroup = StackGroup.Mending,
                Armor = new[]
                {
                    new ArmorEffectRow { HpRegenPct = 8 },
                    new ArmorEffectRow { HpRegenPct = 12, HealsReceivedPct = 5 },
                    new ArmorEffectRow { HpRegenPct = 18, HealsReceivedPct = 8, AutoCure = true },
                    new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, AutoCure = true }
                },
                Shield = new[]
                {
                    new ArmorEffectRow { HpRegenPct = 10 },
                    new ArmorEffectRow { HpRegenPct = 15, HealsReceivedPct = 5 },
                    new ArmorEffectRow { HpRegenPct = 20, HealsReceivedPct = 8, AutoCure = true },
                    new ArmorEffectRow { HpRegenPct = 25, HealsReceivedPct = 10, AutoCure = true }
                }
            },
            // Tritonian — Poseidon (ward). Armor + shield.
            new LaneDefinition
            {
                Root = VariantRoot.Tritonian, DisplayName = "tritonian", MythTag = "Poseidon", BaseHue = 99, StackGroup = StackGroup.Ward,
                Armor = new[]
                {
                    new ArmorEffectRow { SpellDrPct = 2 },
                    new ArmorEffectRow { SpellDrPct = 3, ParaResistPct = 10 },
                    new ArmorEffectRow { SpellDrPct = 5, ParaResistPct = 20, ResistSkillBonus = 5 },
                    new ArmorEffectRow { SpellDrPct = 6, ParaResistPct = 30, ResistSkillBonus = 5 }
                },
                Shield = new[]
                {
                    new ArmorEffectRow { SpellDrPct = 3 },
                    new ArmorEffectRow { SpellDrPct = 5, ParaResistPct = 15 },
                    new ArmorEffectRow { SpellDrPct = 7, ParaResistPct = 25, ResistSkillBonus = 5 },
                    new ArmorEffectRow { SpellDrPct = 8, ParaResistPct = 30, ResistSkillBonus = 5 }
                }
            },
            // Talarian — Hermes (stride). Armor + shield.
            new LaneDefinition
            {
                Root = VariantRoot.Talarian, DisplayName = "talarian", MythTag = "Hermes", BaseHue = 89, StackGroup = StackGroup.Stride,
                Armor = new[]
                {
                    new ArmorEffectRow { WeightReductionPct = 10, StamRegenPct = 4 },
                    new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 6 },
                    new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8, DodgePct = 3 },
                    new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10, DodgePct = 5 }
                },
                Shield = new[]
                {
                    new ArmorEffectRow { WeightReductionPct = 20, StamRegenPct = 5 },
                    new ArmorEffectRow { WeightReductionPct = 30, StamRegenPct = 8 },
                    new ArmorEffectRow { WeightReductionPct = 40, StamRegenPct = 10, DodgePct = 4 },
                    new ArmorEffectRow { WeightReductionPct = 50, StamRegenPct = 12, DodgePct = 6 }
                }
            }
        }
    };
}
