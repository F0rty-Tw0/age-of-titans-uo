using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Maces — family 3. Ladder: club 0, mace 1, maul 2, war axe 3, hammer pick 4, war mace 5,
// war hammer 6. "War axe" is mechanically a mace (DefSkill = Macing). Namespace: Cyclopes /
// Hecatoncheires / storm-forge. Roots per 04-maces.md §3. "Storm & anvil."
public static class MacesFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyMaces,
        LadderTypes = new[]
        {
            typeof(Club), typeof(Mace), typeof(Maul), typeof(WarAxe),
            typeof(HammerPick), typeof(WarMace), typeof(WarHammer)
        },
        Ratios = new[] { 0.70, 0.75, 0.80, 0.85, 0.90, 0.95, 1.00 },
        SwingSeconds = new[] { 2.90, 3.00, 3.10, 3.20, 3.30, 3.40, 3.50 },
        Factories = new Func<Item>[]
        {
            () => new Club(), () => new Mace(), () => new Maul(), () => new WarAxe(),
            () => new HammerPick(), () => new WarMace(), () => new WarHammer()
        },
        Lanes = new[]
        {
            // Ennosigaios (quake): splash; signature = every 5th hit fires a full-power splash burst.
            new LaneDefinition
            {
                Root = VariantRoot.Ennosigaios, DisplayName = "ennosigaios", MythTag = "Poseidon", BaseHue = 44,
                Weapon = new[]
                {
                    new WeaponEffectRow { SplashPct = 8 },
                    new WeaponEffectRow { SplashPct = 10, DamagePct = 8 },
                    new WeaponEffectRow { SplashPct = 12, DamagePct = 10, Signature = ClauseType.NthHitSplash, S1 = 5, S2 = 12, S3 = 3 },
                    new WeaponEffectRow { SplashPct = 12, DamagePct = 10, Signature = ClauseType.NthHitSplash, S1 = 5, S2 = 12, S3 = 3 }
                }
            },
            // Kataigis (concussion): stagger procs; signature = crits stagger the target.
            new LaneDefinition
            {
                Root = VariantRoot.Kataigis, DisplayName = "kataigis", MythTag = "the tempest", BaseHue = 46,
                Weapon = new[]
                {
                    new WeaponEffectRow { StaggerProcPct = 4 },
                    new WeaponEffectRow { StaggerProcPct = 6, DamagePct = 8 },
                    new WeaponEffectRow { StaggerProcPct = 8, DamagePct = 10, Signature = ClauseType.CritStagger },
                    new WeaponEffectRow { StaggerProcPct = 8, DamagePct = 10, Signature = ClauseType.CritStagger }
                }
            },
            // Rhaistes (sunder): armor pen; signature = every 4th hit ignores armor entirely.
            new LaneDefinition
            {
                Root = VariantRoot.Rhaistes, DisplayName = "rhaistes", MythTag = "the smasher", BaseHue = 48,
                Weapon = new[]
                {
                    new WeaponEffectRow { ArmorPenPct = 10 },
                    new WeaponEffectRow { ArmorPenPct = 15, CritChancePct = 8 },
                    new WeaponEffectRow { ArmorPenPct = 20, CritChancePct = 10, Signature = ClauseType.NthHitFullArmorPen, S1 = 4 },
                    new WeaponEffectRow { ArmorPenPct = 20, CritChancePct = 10, Signature = ClauseType.NthHitFullArmorPen, S1 = 4 }
                }
            },
            // Eryma (anvil): block + DR + thorns; signature = BlockGrantsDrBurst.
            new LaneDefinition
            {
                Root = VariantRoot.Eryma, DisplayName = "eryma", MythTag = "the bulwark", BaseHue = 50,
                Weapon = new[]
                {
                    new WeaponEffectRow { BlockPct = 6 },
                    new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true, Signature = ClauseType.BlockGrantsDrBurst, S1 = 8, S2 = 5 },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true, Signature = ClauseType.BlockGrantsDrBurst, S1 = 8, S2 = 5 }
                }
            },
            // Kamatos (exhaust): stamina lane. Signature = None.
            new LaneDefinition
            {
                Root = VariantRoot.Kamatos, DisplayName = "kamatos", MythTag = "toil", BaseHue = 52,
                Weapon = new[]
                {
                    new WeaponEffectRow { StamRegenPct = 6 },
                    new WeaponEffectRow { StamRegenPct = 8, DefenderStamDrainFlat = 2 },
                    new WeaponEffectRow { StamRegenPct = 10, DefenderStamDrainFlat = 3 },
                    new WeaponEffectRow { StamRegenPct = 10, DefenderStamDrainFlat = 3 }
                }
            }
        },
        Legendaries = new[]
        {
            // Ennosigaios line (was Zephyr — the earth-shaker's aftershocks)
            new LegendaryEntry(91, "Briareos", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 0, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
            new LegendaryEntry(92, "Kottos", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 1, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(93, "Gyges", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 2, ClauseType.ExtraSwingEveryN, 5, 10, 0, 0), // P2=armor pen %
            new LegendaryEntry(94, "Polyphemos", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 3, ClauseType.ExtraSwingSplash, 6, 10, 3, 0), // signature is NthHitSplash, so no collision
            new LegendaryEntry(95, "Pyrphoros", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 4, ClauseType.ExtraSwingEveryN, 5, 0, 5, 0), // P3=stam leech %
            new LegendaryEntry(96, "Elektor", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 5, ClauseType.ExtraSwingOnParry, 0, 0, 0, 0),
            new LegendaryEntry(97, "Selaios", VariantRoot.Ennosigaios, LegendaryRegistry.FamilyMaces, 6, ClauseType.ExtraSwingEveryN, 7, 1, 0, 0), // SWAP cadence 5→7 (5+stagger echoed the Ennosigaios signature)

            // Rhaistes line (was Phobos — the smasher's sunder). Signature = NthHitFullArmorPen.
            new LegendaryEntry(98, "Brontes", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 0, ClauseType.CritFirstHit, 0, 0, 0, 0), // SWAP drop splash (splash is Ennosigaios')
            new LegendaryEntry(99, "Steropes", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 1, ClauseType.CritArmorPen, 5, 10, 0, 0),
            new LegendaryEntry(100, "Arges", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 2, ClauseType.CritArmorPen, 6, 15, 0, 0), // SWAP CritSplash (splash is Ennosigaios')
            new LegendaryEntry(101, "Pyrakmon", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 3, ClauseType.CritExecuteUnder15, 5, 0, 0, 0),
            new LegendaryEntry(102, "Thyella", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 4, ClauseType.CritArmorPen, 7, 20, 0, 0), // SWAP CritStagger (== Kataigis signature type)
            new LegendaryEntry(103, "Sthenaros", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 5, ClauseType.CritEveryN, 5, 0, 0, 0),
            new LegendaryEntry(104, "Keraunos", VariantRoot.Rhaistes, LegendaryRegistry.FamilyMaces, 6, ClauseType.CritArmorPen, 8, 25, 0, 0), // SWAP CritElemental (no owning mace lane)

            // Kataigis line (was Agrotera — the tempest's concussion). Signature = CritStagger.
            new LegendaryEntry(105, "Kelmis", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 0, ClauseType.CritEveryN, 3, 0, 0, 0), // SWAP MarkNearbyAllies
            new LegendaryEntry(106, "Damnameneus", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 1, ClauseType.CritExecuteUnder15, 5, 0, 0, 0), // SWAP MarkAllSources25
            new LegendaryEntry(107, "Chalybos", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 2, ClauseType.CritFirstHit, 0, 0, 0, 0), // SWAP MarkSpreadOnDeath
            new LegendaryEntry(108, "Chalkeus", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 3, ClauseType.ExtraSwingEveryN, 6, 1, 0, 0), // SWAP MarkHealBlock; literal stagger via extra-swing trigger
            new LegendaryEntry(109, "Kabeiros", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 4, ClauseType.DoubleStrikeEveryN, 7, 0, 0, 0), // SWAP MarkAllSources25
            new LegendaryEntry(110, "Pyrigenes", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 5, ClauseType.CritFullHpDouble, 8, 0, 0, 0), // SWAP MarkFirstHit
            new LegendaryEntry(111, "Aitnaios", VariantRoot.Kataigis, LegendaryRegistry.FamilyMaces, 6, ClauseType.ExtraSwingEveryN, 9, 1, 0, 0), // SWAP PoisonTickDoubled; stagger via extra-swing trigger

            // Eryma line (was Pallas — the bulwark's answer). Signature = BlockGrantsDrBurst; all FIT.
            new LegendaryEntry(112, "Khalkaspis", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(113, "Chalkodamas", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 1, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(114, "Akmon", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 2, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
            new LegendaryEntry(115, "Akmonides", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 3, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(116, "Skeptron", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 4, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
            new LegendaryEntry(117, "Adamastos", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 5, ClauseType.BlockDrainStam, 2, 0, 0, 0),
            new LegendaryEntry(118, "Ombrios", VariantRoot.Eryma, LegendaryRegistry.FamilyMaces, 6, ClauseType.ReflectFirstHit, 20, 0, 0, 0),

            // Kamatos line (was Stygian — toil unto collapse). Signature = None; all FIT.
            new LegendaryEntry(119, "Chalkoteuchos", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 0, ClauseType.StamDrainOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(120, "Sphyreus", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 1, ClauseType.OnKillRestore, 2, 0, 0, 0),
            new LegendaryEntry(121, "Empyros", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 2, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(122, "Astrapios", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 3, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(123, "Brontaios", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 4, ClauseType.OnKillRestore, 2, 0, 0, 0),
            new LegendaryEntry(124, "Aitherios", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 5, ClauseType.LifestealOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(125, "Pyriphaes", VariantRoot.Kamatos, LegendaryRegistry.FamilyMaces, 6, ClauseType.OnKillRestore, 2, 0, 0, 0)
        }
    };
}
