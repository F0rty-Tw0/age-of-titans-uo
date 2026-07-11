using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Archery — family 6. Ladder: bow 0, crossbow 1, heavy crossbow 2. Namespace: archer-myths. Roots
// per 07-archery.md §3. "The far mark." Pallas legends here are pure block (no reflect); the Epic
// Skopos thorns rider is melee-only (gated in engine).
public static class ArcheryFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyArchery,
        LadderTypes = new[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow) },
        Ratios = new[] { 0.70, 0.80, 0.90 },
        SwingSeconds = new[] { 3.10, 3.40, 3.70 },
        Factories = new Func<Item>[] { () => new Bow(), () => new Crossbow(), () => new HeavyCrossbow() },
        Lanes = new[]
        {
            // Hekatos (deadeye): hit% + crit% + crit dmg; signature = first shot of each fight crits.
            new LaneDefinition
            {
                Root = VariantRoot.Hekatos, DisplayName = "hekatos", MythTag = "Apollo", BaseHue = 89,
                Weapon = new[]
                {
                    new WeaponEffectRow { HitChancePct = 6 },
                    new WeaponEffectRow { HitChancePct = 6, CritChancePct = 8 },
                    new WeaponEffectRow { HitChancePct = 8, CritChancePct = 10, CritDamagePct = 20, Signature = ClauseType.CritFirstHit },
                    new WeaponEffectRow { HitChancePct = 8, CritChancePct = 10, CritDamagePct = 20, Signature = ClauseType.CritFirstHit }
                }
            },
            // Belos (volley): splash; signature = every 4th shot fires a full-power splash burst.
            new LaneDefinition
            {
                Root = VariantRoot.Belos, DisplayName = "belos", MythTag = "the missile", BaseHue = 91,
                Weapon = new[]
                {
                    new WeaponEffectRow { SplashPct = 8 },
                    new WeaponEffectRow { SplashPct = 10, DamagePct = 8 },
                    new WeaponEffectRow { SplashPct = 12, DamagePct = 10, Signature = ClauseType.NthHitSplash, S1 = 4, S2 = 12, S3 = 3 },
                    new WeaponEffectRow { SplashPct = 12, DamagePct = 10, Signature = ClauseType.NthHitSplash, S1 = 4, S2 = 12, S3 = 3 }
                }
            },
            // Pede (pin): stagger procs; signature = crits stagger (same shape as Kataigis).
            new LaneDefinition
            {
                Root = VariantRoot.Pede, DisplayName = "pede", MythTag = "the fetter", BaseHue = 93,
                Weapon = new[]
                {
                    new WeaponEffectRow { StaggerProcPct = 4 },
                    new WeaponEffectRow { StaggerProcPct = 6, DamagePct = 8 },
                    new WeaponEffectRow { StaggerProcPct = 8, DamagePct = 10, Signature = ClauseType.CritStagger },
                    new WeaponEffectRow { StaggerProcPct = 8, DamagePct = 10, Signature = ClauseType.CritStagger }
                }
            },
            // Toxikon (toxin): poison apply chance + hit%; signature = poisoned targets are marked.
            new LaneDefinition
            {
                Root = VariantRoot.Toxikon, DisplayName = "toxikon", MythTag = "arrow-poison", BaseHue = 95,
                Weapon = new[]
                {
                    new WeaponEffectRow { PoisonApplyPct = 8 },
                    new WeaponEffectRow { PoisonApplyPct = 10, HitChancePct = 6 },
                    new WeaponEffectRow { PoisonApplyPct = 12, HitChancePct = 8, Signature = ClauseType.PoisonedTargetsMarked, S1 = 14 },
                    new WeaponEffectRow { PoisonApplyPct = 12, HitChancePct = 8, Signature = ClauseType.PoisonedTargetsMarked, S1 = 14 }
                }
            },
            // Skopos (warden): hit% + defensive block/DR (holds at range); signature = a block guarantees
            // the next shot crits.
            new LaneDefinition
            {
                Root = VariantRoot.Skopos, DisplayName = "skopos", MythTag = "the watcher", BaseHue = 97,
                Weapon = new[]
                {
                    new WeaponEffectRow { HitChancePct = 6, BlockPct = 6 },
                    new WeaponEffectRow { HitChancePct = 8, BlockPct = 6, BlockDrPct = 8 },
                    new WeaponEffectRow { HitChancePct = 8, BlockPct = 8, BlockDrPct = 12, Signature = ClauseType.BlockNextShotCrit },
                    new WeaponEffectRow { HitChancePct = 8, BlockPct = 8, BlockDrPct = 12, Signature = ClauseType.BlockNextShotCrit }
                }
            }
        },
        Legendaries = new[]
        {
            // Belos line (was Zephyr — swift arrows). Signature = NthHitSplash; all FIT.
            new LegendaryEntry(171, "Skythes", VariantRoot.Belos, LegendaryRegistry.FamilyArchery, 0, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(172, "Molpadia", VariantRoot.Belos, LegendaryRegistry.FamilyArchery, 1, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
            new LegendaryEntry(173, "Stymphalia", VariantRoot.Belos, LegendaryRegistry.FamilyArchery, 2, ClauseType.ExtraSwingSplash, 5, 10, 3, 0),

            // Hekatos line (was Phobos — killing shots). Signature = CritFirstHit.
            new LegendaryEntry(174, "Teukros", VariantRoot.Hekatos, LegendaryRegistry.FamilyArchery, 0, ClauseType.CritFirstHitStamRefund, 0, 0, 0, 0), // SWAP CritFirstHit (== signature)
            new LegendaryEntry(175, "Pandaros", VariantRoot.Hekatos, LegendaryRegistry.FamilyArchery, 1, ClauseType.CritFullHpDouble, 0, 0, 0, 0), // SWAP drop P3=1 (would force a first-hit crit == signature)
            new LegendaryEntry(176, "Alkon", VariantRoot.Hekatos, LegendaryRegistry.FamilyArchery, 2, ClauseType.CritArmorPen, 5, 10, 0, 0),

            // Toxikon line (was Agrotera — the hunt made mark). Signature = PoisonedTargetsMarked; all FIT.
            new LegendaryEntry(177, "Skamandrios", VariantRoot.Toxikon, LegendaryRegistry.FamilyArchery, 0, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(178, "Nessos", VariantRoot.Toxikon, LegendaryRegistry.FamilyArchery, 1, ClauseType.PoisonTickDoubled, 0, 0, 0, 0),
            new LegendaryEntry(179, "Penthesileia", VariantRoot.Toxikon, LegendaryRegistry.FamilyArchery, 2, ClauseType.MarkNearbyAllies, 3, 0, 0, 0),

            // Skopos line (was Pallas — deflection at range). Signature = BlockNextShotCrit.
            new LegendaryEntry(180, "Philoktetes", VariantRoot.Skopos, LegendaryRegistry.FamilyArchery, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(181, "Kheiron", VariantRoot.Skopos, LegendaryRegistry.FamilyArchery, 1, ClauseType.BlockRestoreStam, 10, 0, 0, 0),
            new LegendaryEntry(182, "Kydon", VariantRoot.Skopos, LegendaryRegistry.FamilyArchery, 2, ClauseType.BlockDrainStam, 3, 0, 0, 0), // SWAP BlockNextShotCrit (== signature)

            // Pede line (was Stygian — the draining arrow). Signature = CritStagger.
            new LegendaryEntry(183, "Toxeus", VariantRoot.Pede, LegendaryRegistry.FamilyArchery, 0, ClauseType.CritEveryN, 6, 0, 0, 0), // SWAP OnKillRestore
            new LegendaryEntry(184, "Lerna", VariantRoot.Pede, LegendaryRegistry.FamilyArchery, 1, ClauseType.CritEveryN, 5, 0, 0, 0), // SWAP LifestealOnCrit
            new LegendaryEntry(185, "Krotos", VariantRoot.Pede, LegendaryRegistry.FamilyArchery, 2, ClauseType.CritEveryN, 4, 0, 0, 0) // SWAP LifestealOnCrit
        }
    };
}
