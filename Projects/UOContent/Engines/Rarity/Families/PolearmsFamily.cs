using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Polearms — family 2. Ladder: bardiche 0, halberd 1. Namespace: Gigantes / Gigantomachy. Roots
// per 03-polearms.md §3. "The reaping line."
public static class PolearmsFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyPolearms,
        LadderTypes = new[] { typeof(Bardiche), typeof(Halberd) },
        Ratios = new[] { 0.90, 0.98 },
        SwingSeconds = new[] { 3.25, 3.40 },
        Factories = new Func<Item>[] { () => new Bardiche(), () => new Halberd() },
        Lanes = new[]
        {
            // Theristes (reap): splash; signature = every crit also splashes.
            new LaneDefinition
            {
                Root = VariantRoot.Theristes, DisplayName = "theristes", MythTag = "the reaper", BaseHue = 64,
                Weapon = new[]
                {
                    new WeaponEffectRow { SplashPct = 8 },
                    new WeaponEffectRow { SplashPct = 10, DamagePct = 8 },
                    new WeaponEffectRow { SplashPct = 12, DamagePct = 10, Signature = ClauseType.CritSplash, S2 = 12, S3 = 3 },
                    new WeaponEffectRow { SplashPct = 12, DamagePct = 10, Signature = ClauseType.CritSplash, S2 = 12, S3 = 3 }
                }
            },
            // Sarisa (impale): armor pen + crit dmg; signature = guaranteed first-hit crit.
            new LaneDefinition
            {
                Root = VariantRoot.Sarisa, DisplayName = "sarisa", MythTag = "the pike", BaseHue = 66,
                Weapon = new[]
                {
                    new WeaponEffectRow { ArmorPenPct = 10 },
                    new WeaponEffectRow { ArmorPenPct = 15, CritChancePct = 8 },
                    new WeaponEffectRow { ArmorPenPct = 20, CritChancePct = 10, CritDamagePct = 20, Signature = ClauseType.CritFirstHit },
                    new WeaponEffectRow { ArmorPenPct = 20, CritChancePct = 10, CritDamagePct = 20, Signature = ClauseType.CritFirstHit }
                }
            },
            // Phalanx (hold): block + thorns; signature = BlockGrantsDrBurst.
            new LaneDefinition
            {
                Root = VariantRoot.Phalanx, DisplayName = "phalanx", MythTag = "the phalanx", BaseHue = 68,
                Weapon = new[]
                {
                    new WeaponEffectRow { BlockPct = 6 },
                    new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true, Signature = ClauseType.BlockGrantsDrBurst, S1 = 8, S2 = 5 },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, BlockThorns = true, Signature = ClauseType.BlockGrantsDrBurst, S1 = 8, S2 = 5 }
                }
            },
            // Horme (momentum, P15): every-Nth escalating dmg; signature = the same cadence hit lands
            // a guaranteed extra swing (ExtraSwingEveryN S1 = cadence).
            new LaneDefinition
            {
                Root = VariantRoot.Horme, DisplayName = "horme", MythTag = "Horme", BaseHue = 70,
                Weapon = new[]
                {
                    new WeaponEffectRow { NthHitBonusPct = 15, NthHitN = 5 },
                    new WeaponEffectRow { NthHitBonusPct = 20, NthHitN = 5, HitChancePct = 6 },
                    new WeaponEffectRow { NthHitBonusPct = 25, NthHitN = 4, HitChancePct = 8, Signature = ClauseType.ExtraSwingEveryN, S1 = 4 },
                    new WeaponEffectRow { NthHitBonusPct = 25, NthHitN = 4, HitChancePct = 8, Signature = ClauseType.ExtraSwingEveryN, S1 = 4 }
                }
            },
            // Zophos (toll): lifesteal + on-kill restore; signature = None.
            new LaneDefinition
            {
                Root = VariantRoot.Zophos, DisplayName = "zophos", MythTag = "the gloom", BaseHue = 72,
                Weapon = new[]
                {
                    new WeaponEffectRow { LifestealPct = 6 },
                    new WeaponEffectRow { LifestealPct = 6, OnKillStamPct = 15 },
                    new WeaponEffectRow { LifestealPct = 8, OnKillStamPct = 20, LifestealExecute = true },
                    new WeaponEffectRow { LifestealPct = 8, OnKillStamPct = 20, LifestealExecute = true }
                }
            }
        },
        Legendaries = new[]
        {
            new LegendaryEntry(81, "Hippolytos", VariantRoot.Theristes, LegendaryRegistry.FamilyPolearms, 0, ClauseType.ExtraSwingManaLeech, 5, 10, 0, 0), // de-overlap 2026-07-12: was ExtraSwingSplash (== CritSplash sig, double-splash)
            new LegendaryEntry(82, "Thoon", VariantRoot.Theristes, LegendaryRegistry.FamilyPolearms, 1, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(83, "Mimas", VariantRoot.Sarisa, LegendaryRegistry.FamilyPolearms, 0, ClauseType.MarkFirstHit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritArmorPen (CRIT == CritFirstHit sig)
            new LegendaryEntry(84, "Porphyrion", VariantRoot.Sarisa, LegendaryRegistry.FamilyPolearms, 1, ClauseType.MarkAllSources25, 25, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritArmorPen (CRIT == CritFirstHit sig)
            new LegendaryEntry(85, "Gration", VariantRoot.Horme, LegendaryRegistry.FamilyPolearms, 0, ClauseType.CritEveryN, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ExtraSwingChain (EXTRA_SWING == ExtraSwingEveryN sig)
            new LegendaryEntry(86, "Polybotes", VariantRoot.Horme, LegendaryRegistry.FamilyPolearms, 1, ClauseType.NthHitFullArmorPen, 4, 0, 0, 0), // SWAP MarkAllSources25 (mark foreign to momentum)
            new LegendaryEntry(87, "Enkelados", VariantRoot.Phalanx, LegendaryRegistry.FamilyPolearms, 0, ClauseType.CritStagger, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectFirstHit (BLOCK_PARRY == BlockGrantsDrBurst sig)
            new LegendaryEntry(88, "Eurytos", VariantRoot.Phalanx, LegendaryRegistry.FamilyPolearms, 1, ClauseType.CritEveryN, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockFirstHit (BLOCK_PARRY == BlockGrantsDrBurst sig)
            new LegendaryEntry(89, "Alkyoneus", VariantRoot.Zophos, LegendaryRegistry.FamilyPolearms, 0, ClauseType.LifestealOnCrit, 8, 0, 0, 0),
            new LegendaryEntry(90, "Klytios", VariantRoot.Zophos, LegendaryRegistry.FamilyPolearms, 1, ClauseType.OnKillRestore, 2, 0, 0, 0)
        }
    };
}
