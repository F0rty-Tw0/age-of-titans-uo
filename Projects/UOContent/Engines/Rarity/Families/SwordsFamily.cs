using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Swords — family 1. Ladder: butcher knife 0, cleaver 1, cutlass 2, scimitar 3, katana 4,
// broadsword 5, longsword 6, viking sword 7. Namespace: Trojan War / Perseid cycle. Roots per
// 02-swords.md §3 (re-theme 2026-07-07). "The hero's duel."
public static class SwordsFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilySwords,
        LadderTypes = new[]
        {
            typeof(ButcherKnife), typeof(Cleaver), typeof(Cutlass), typeof(Scimitar),
            typeof(Katana), typeof(Broadsword), typeof(Longsword), typeof(VikingSword)
        },
        Ratios = new[] { 0.60, 0.65, 0.70, 0.75, 0.80, 0.85, 0.90, 0.95 },
        SwingSeconds = new[] { 2.55, 2.65, 2.75, 2.85, 2.95, 3.05, 3.15, 3.25 },
        Factories = new Func<Item>[]
        {
            () => new ButcherKnife(), () => new Cleaver(), () => new Cutlass(), () => new Scimitar(),
            () => new Katana(), () => new Broadsword(), () => new Longsword(), () => new VikingSword()
        },
        Lanes = new[]
        {
            // Phoibos (precision): hit% + crit%; signature = first hit of each fight always crits.
            new LaneDefinition
            {
                Root = VariantRoot.Phoibos, DisplayName = "phoibos", MythTag = "Apollo", BaseHue = 34,
                Weapon = new[]
                {
                    new WeaponEffectRow { HitChancePct = 6 },
                    new WeaponEffectRow { HitChancePct = 6, CritChancePct = 8 },
                    new WeaponEffectRow { HitChancePct = 8, CritChancePct = 10, Signature = ClauseType.CritFirstHit },
                    new WeaponEffectRow { HitChancePct = 8, CritChancePct = 10, Signature = ClauseType.CritFirstHit }
                }
            },
            // Areia (riposte): block + DR-on-block; signature = a block guarantees the next hit crits.
            new LaneDefinition
            {
                Root = VariantRoot.Areia, DisplayName = "areia", MythTag = "Athena", BaseHue = 36,
                Weapon = new[]
                {
                    new WeaponEffectRow { BlockPct = 6 },
                    new WeaponEffectRow { BlockPct = 6, BlockDrPct = 8 },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, Signature = ClauseType.BlockNextShotCrit },
                    new WeaponEffectRow { BlockPct = 8, BlockDrPct = 12, Signature = ClauseType.BlockNextShotCrit }
                }
            },
            // Menis (wrath-ramp, P28): stacking dmg per consecutive same-target hit; signature = burst
            // splash on reaching max stacks (RampMaxStacksSplash S1 = splash %, S2 = target cap).
            new LaneDefinition
            {
                Root = VariantRoot.Menis, DisplayName = "menis", MythTag = "Achilles", BaseHue = 38,
                Weapon = new[]
                {
                    new WeaponEffectRow { RampPerStackPct = 2, RampMaxStacks = 5 },
                    new WeaponEffectRow { RampPerStackPct = 3, RampMaxStacks = 5, DamagePct = 8 },
                    // DamagePct 10 -> 8 (2026-07-11 sim pass): at full single-target ramp uptime
                    // Menis measured ~+34% over the Phobos-tier lanes; the ramp is the identity
                    // (real fights reset it), so only the flat rider comes down.
                    new WeaponEffectRow { RampPerStackPct = 3, RampMaxStacks = 6, DamagePct = 8, Signature = ClauseType.RampMaxStacksSplash, S1 = 12, S2 = 3 },
                    new WeaponEffectRow { RampPerStackPct = 3, RampMaxStacks = 6, DamagePct = 8, Signature = ClauseType.RampMaxStacksSplash, S1 = 12, S2 = 3 }
                }
            },
            // Aristeia (glory, P23): on-kill stamina restore; signature = on-kill full stam + the next
            // swing within S1 seconds crits.
            new LaneDefinition
            {
                Root = VariantRoot.Aristeia, DisplayName = "aristeia", MythTag = "glory", BaseHue = 40,
                Weapon = new[]
                {
                    new WeaponEffectRow { OnKillStamPct = 10 },
                    new WeaponEffectRow { OnKillStamPct = 15, DamagePct = 8 },
                    new WeaponEffectRow { OnKillStamPct = 20, DamagePct = 10, Signature = ClauseType.OnKillFullStamNextHitCrit, S1 = 5 },
                    new WeaponEffectRow { OnKillStamPct = 20, DamagePct = 10, Signature = ClauseType.OnKillFullStamNextHitCrit, S1 = 5 }
                }
            },
            // Haima (bleed): "gash" poison tick + lifesteal; signature = poisoned targets take +S1% dmg.
            new LaneDefinition
            {
                Root = VariantRoot.Haima, DisplayName = "haima", MythTag = "blood", BaseHue = 42,
                Weapon = new[]
                {
                    new WeaponEffectRow { PoisonApplyPct = 8 },
                    new WeaponEffectRow { PoisonApplyPct = 10, LifestealPct = 6 },
                    new WeaponEffectRow { PoisonApplyPct = 12, LifestealPct = 8, Signature = ClauseType.PoisonedTakeBonusDamage, S1 = 10 },
                    new WeaponEffectRow { PoisonApplyPct = 12, LifestealPct = 8, Signature = ClauseType.PoisonedTakeBonusDamage, S1 = 10 }
                }
            }
        },
        Legendaries = new[]
        {
            // Menis line (was Zephyr — the relentless onslaught)
            new LegendaryEntry(41, "Podarkes", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 0, ClauseType.DoubleStrikeEveryN, 5, 0, 0, 0), // SWAP ExtraSwingOnParry (parry belongs to Areia)
            new LegendaryEntry(42, "Antilochos", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 1, ClauseType.ExtraSwingGuaranteedHit, 5, 0, 0, 0),
            new LegendaryEntry(43, "Xanthos", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 2, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
            new LegendaryEntry(44, "Eumelos", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 3, ClauseType.ExtraSwingEveryN, 5, 0, 0, 0),
            new LegendaryEntry(45, "Idaios", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 4, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(46, "Thoas", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 5, ClauseType.ExtraSwingEveryN, 5, 1, 0, 0), // P2=1 stagger
            new LegendaryEntry(47, "Rhesos", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 6, ClauseType.ExtraSwingEveryN, 5, 10, 0, 0), // P2=armor pen %
            new LegendaryEntry(48, "Meriones", VariantRoot.Menis, LegendaryRegistry.FamilySwords, 7, ClauseType.ExtraSwingEveryN, 5, 0, 5, 0), // P3=stam leech %

            // Phoibos line (was Phobos — the unerring strike). Signature = CritFirstHit; uniques avoid it.
            new LegendaryEntry(49, "Diomedes", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 0, ClauseType.MarkOnCrit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritFullHpDouble (CRIT == CritFirstHit sig)
            new LegendaryEntry(50, "Hektor", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 1, ClauseType.MarkAllSources25, 25, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritSplash (CRIT == CritFirstHit sig)
            new LegendaryEntry(51, "Sarpedon", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 2, ClauseType.MarkHealBlock, 3, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritExecuteUnder15 (CRIT == CritFirstHit sig)
            new LegendaryEntry(52, "Aineias", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 3, ClauseType.MarkFirstHit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritArmorPen (CRIT == CritFirstHit sig)
            new LegendaryEntry(53, "Idomeneus", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 4, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritSplash (CRIT == CritFirstHit sig)
            new LegendaryEntry(54, "Neoptolemos", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 5, ClauseType.MarkNearbyAllies, 3, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritSplash (CRIT == CritFirstHit sig)
            new LegendaryEntry(55, "Agenor", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 6, ClauseType.MarkElemental, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritEveryN (CRIT == CritFirstHit sig)
            new LegendaryEntry(56, "Chrysaor", VariantRoot.Phoibos, LegendaryRegistry.FamilySwords, 7, ClauseType.MarkManaLeech, 10, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was CritArmorPen (CRIT == CritFirstHit sig)

            // Haima line (was Agrotera — the opened vein; "mark" reads as a bleeding gash). All FIT.
            new LegendaryEntry(57, "Kephalos", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 0, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(58, "Peirithoos", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 1, ClauseType.PoisonTickDoubled, 0, 0, 0, 0),
            new LegendaryEntry(59, "Paris", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 2, ClauseType.MarkHealBlock, 3, 0, 0, 0),
            new LegendaryEntry(60, "Melanion", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 3, ClauseType.MarkFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(61, "Dolon", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 4, ClauseType.MarkNearbyAllies, 3, 0, 0, 0),
            new LegendaryEntry(62, "Odysseus", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 5, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0),
            new LegendaryEntry(63, "Perseus", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 6, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(64, "Harpe", VariantRoot.Haima, LegendaryRegistry.FamilySwords, 7, ClauseType.MarkOnCrit, 0, 0, 0, 0),

            // Areia line (was Pallas — the perfect riposte). Signature = BlockNextShotCrit; all FIT.
            new LegendaryEntry(65, "Nestor", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 0, ClauseType.CritFirstHit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockFirstHit (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(66, "Polydamas", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 1, ClauseType.CritEveryN, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockFirstHit (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(67, "Antenor", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 2, ClauseType.CritExecuteUnder15, 6, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectFirstHit (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(68, "Menestheus", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 3, ClauseType.CritArmorPen, 5, 15, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockRestoreStam (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(69, "Eurypylos", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 4, ClauseType.CritFullHpDouble, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockFirstHit (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(70, "Sthenelos", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 5, ClauseType.CritSplash, 5, 10, 3, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectFirstHit (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(71, "Amphitryon", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 6, ClauseType.MarkFirstHit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was ReflectFirstHit (BLOCK_PARRY == BlockNextShotCrit sig)
            new LegendaryEntry(72, "Deiphobos", VariantRoot.Areia, LegendaryRegistry.FamilySwords, 7, ClauseType.CritStagger, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was BlockRestoreStam (BLOCK_PARRY == BlockNextShotCrit sig)

            // Aristeia line (was Stygian — glory drinks deep). Signature = OnKillFullStamNextHitCrit.
            new LegendaryEntry(73, "Iphitos", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 0, ClauseType.StamDrainOnCrit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was OnKillRestore (ON_KILL == OnKillFullStamNextHitCrit sig)
            new LegendaryEntry(74, "Memnon", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 1, ClauseType.CritExecuteUnder15, 6, 25, 0, 0), // de-overlap 2026-07-12 (arming-group split): was OnKillRestore (ON_KILL == OnKillFullStamNextHitCrit sig); P2=25 execute threshold suits the glory finisher and re-homes the ClauseText threshold example off Tydeus
            new LegendaryEntry(75, "Euphorbos", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 2, ClauseType.MarkFirstHit, 0, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was OnKillRestore (ON_KILL == OnKillFullStamNextHitCrit sig)
            new LegendaryEntry(76, "Palamedes", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 3, ClauseType.LifestealOnCrit, 8, 0, 0, 0),
            new LegendaryEntry(77, "Agamemnon", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 4, ClauseType.CritEveryN, 5, 0, 0, 0), // de-overlap 2026-07-12 (arming-group split): was OnKillRestore (ON_KILL == OnKillFullStamNextHitCrit sig)
            new LegendaryEntry(78, "Aigisthos", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 5, ClauseType.LifestealOnCrit, 8, 0, 0, 0),
            new LegendaryEntry(79, "Elektryon", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 6, ClauseType.LifestealOnCrit, 8, 0, 0, 0),
            new LegendaryEntry(80, "Achilles", VariantRoot.Aristeia, LegendaryRegistry.FamilySwords, 7, ClauseType.StamDrainOnCrit, 0, 0, 0, 0)
        }
    };
}
