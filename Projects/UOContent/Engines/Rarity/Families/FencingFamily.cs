using System;
using Server.Items;

namespace Server.Engines.Rarity;

// Fencing — family 5. Ladder: dagger 0, kryss 1, war fork 2, pitchfork 3, short spear 4, spear 5.
// Namespace: spear-heroes and mythic serpents. Roots per 06-fencing.md §3. "Serpent's tempo."
public static class FencingFamily
{
    public static readonly WeaponFamilyDefinition Definition = new()
    {
        Family = LegendaryRegistry.FamilyFencing,
        LadderTypes = new[]
        {
            typeof(Dagger), typeof(Kryss), typeof(WarFork), typeof(Pitchfork), typeof(ShortSpear), typeof(Spear)
        },
        Ratios = new[] { 0.50, 0.55, 0.60, 0.65, 0.70, 0.75 },
        SwingSeconds = new[] { 2.05, 2.15, 2.25, 2.35, 2.45, 2.55 },
        Factories = new Func<Item>[]
        {
            () => new Dagger(), () => new Kryss(), () => new WarFork(), () => new Pitchfork(),
            () => new ShortSpear(), () => new Spear()
        },
        Lanes = new[]
        {
            // Ios (venom): poison apply chance; signature = None (poison tier-up is numeric PoisonTier).
            new LaneDefinition
            {
                Root = VariantRoot.Ios, DisplayName = "ios", MythTag = "venom", BaseHue = 99,
                Weapon = new[]
                {
                    new WeaponEffectRow { PoisonApplyPct = 8 },
                    new WeaponEffectRow { PoisonApplyPct = 10, HitChancePct = 6 },
                    new WeaponEffectRow { PoisonApplyPct = 12, HitChancePct = 8, PoisonTier = 1 },
                    new WeaponEffectRow { PoisonApplyPct = 12, HitChancePct = 8, PoisonTier = 1 }
                }
            },
            // Ephodos (lunge, P14): first-hit-of-fight bonus dmg + crit; signature = guaranteed first-hit
            // crit that refunds its swing stamina.
            new LaneDefinition
            {
                Root = VariantRoot.Ephodos, DisplayName = "ephodos", MythTag = "the assault", BaseHue = 101,
                Weapon = new[]
                {
                    new WeaponEffectRow { FirstHitBonusPct = 15 },
                    new WeaponEffectRow { FirstHitBonusPct = 20, CritChancePct = 8 },
                    new WeaponEffectRow { FirstHitBonusPct = 25, CritChancePct = 10, Signature = ClauseType.CritFirstHitStamRefund },
                    new WeaponEffectRow { FirstHitBonusPct = 25, CritChancePct = 10, Signature = ClauseType.CritFirstHitStamRefund }
                }
            },
            // Aiolos (flurry): swing speed + extra-swing proc; signature = an extra swing may chain.
            new LaneDefinition
            {
                Root = VariantRoot.Aiolos, DisplayName = "aiolos", MythTag = "Aiolos", BaseHue = 103,
                Weapon = new[]
                {
                    new WeaponEffectRow { SwingSpeedPct = 8 },
                    new WeaponEffectRow { SwingSpeedPct = 8, HitChancePct = 6 },
                    // Chain cadence 5 -> 7 (2026-07-11 sim pass): the guaranteed every-5th extra
                    // swing + chain measured ~+60% DPS over the family pack; every-7th keeps the
                    // lane clearly fastest without lapping the damage lanes.
                    new WeaponEffectRow { SwingSpeedPct = 10, HitChancePct = 8, ExtraSwingPct = 10, Signature = ClauseType.ExtraSwingChain, S1 = 7 },
                    new WeaponEffectRow { SwingSpeedPct = 10, HitChancePct = 8, ExtraSwingPct = 10, Signature = ClauseType.ExtraSwingChain, S1 = 7 }
                }
            },
            // Kentron (puncture): armor pen + hit%; signature = every 3rd hit fully ignores armor.
            new LaneDefinition
            {
                Root = VariantRoot.Kentron, DisplayName = "kentron", MythTag = "the sting", BaseHue = 105,
                Weapon = new[]
                {
                    new WeaponEffectRow { ArmorPenPct = 10 },
                    new WeaponEffectRow { ArmorPenPct = 15, HitChancePct = 6 },
                    new WeaponEffectRow { ArmorPenPct = 20, HitChancePct = 8, Signature = ClauseType.NthHitFullArmorPen, S1 = 3 },
                    new WeaponEffectRow { ArmorPenPct = 20, HitChancePct = 8, Signature = ClauseType.NthHitFullArmorPen, S1 = 3 }
                }
            },
            // Ophis (evasion): worn-side dodge/parry; signature = a successful dodge opens a counter window.
            new LaneDefinition
            {
                Root = VariantRoot.Ophis, DisplayName = "ophis", MythTag = "the serpent", BaseHue = 107, StackGroup = StackGroup.Stride,
                Weapon = new[]
                {
                    new WeaponEffectRow { DodgePct = 6 },
                    new WeaponEffectRow { DodgePct = 6 },
                    new WeaponEffectRow { DodgePct = 8, Signature = ClauseType.DodgeGrantsCounterWindow },
                    new WeaponEffectRow { DodgePct = 8, Signature = ClauseType.DodgeGrantsCounterWindow }
                }
            }
        },
        Legendaries = new[]
        {
            // Aiolos line (was Zephyr — the wind-lord's flurry). All FIT.
            new LegendaryEntry(141, "Balios", VariantRoot.Aiolos, LegendaryRegistry.FamilyFencing, 0, ClauseType.ExtraSwingEveryN, 4, 0, 0, 0),
            new LegendaryEntry(142, "Kyknos", VariantRoot.Aiolos, LegendaryRegistry.FamilyFencing, 1, ClauseType.ExtraSwingStackingHit, 5, 20, 0, 0),
            new LegendaryEntry(143, "Asteropaios", VariantRoot.Aiolos, LegendaryRegistry.FamilyFencing, 2, ClauseType.ExtraSwingEveryN, 4, 10, 0, 0), // P2=armor pen %
            new LegendaryEntry(144, "Protesilaos", VariantRoot.Aiolos, LegendaryRegistry.FamilyFencing, 3, ClauseType.ExtraSwingFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(145, "Akamas", VariantRoot.Aiolos, LegendaryRegistry.FamilyFencing, 4, ClauseType.ExtraSwingEveryN, 3, 0, 5, 0), // P3=stam leech %
            new LegendaryEntry(146, "Peleus", VariantRoot.Aiolos, LegendaryRegistry.FamilyFencing, 5, ClauseType.ExtraSwingGuaranteedHit, 4, 0, 0, 0),

            // Ephodos line (was Phobos — the opening lunge). Signature = CritFirstHitStamRefund.
            new LegendaryEntry(147, "Parthenopaios", VariantRoot.Ephodos, LegendaryRegistry.FamilyFencing, 0, ClauseType.CritFullHpDouble, 0, 0, 0, 0), // SWAP CritFirstHit (== signature restated); P3 stays 0
            new LegendaryEntry(148, "Kapaneus", VariantRoot.Ephodos, LegendaryRegistry.FamilyFencing, 1, ClauseType.CritSplash, 4, 10, 3, 0),
            new LegendaryEntry(149, "Tydeus", VariantRoot.Ephodos, LegendaryRegistry.FamilyFencing, 2, ClauseType.CritExecuteUnder15, 3, 25, 0, 0), // P2=25% threshold
            new LegendaryEntry(150, "Asios", VariantRoot.Ephodos, LegendaryRegistry.FamilyFencing, 3, ClauseType.CritSplash, 4, 15, 3, 0),
            new LegendaryEntry(151, "Meleagros", VariantRoot.Ephodos, LegendaryRegistry.FamilyFencing, 4, ClauseType.CritStagger, 4, 0, 0, 0), // SWAP CritFirstHit (== signature restated)
            new LegendaryEntry(152, "Pelion", VariantRoot.Ephodos, LegendaryRegistry.FamilyFencing, 5, ClauseType.CritEveryN, 3, 0, 0, 0), // SWAP CritArmorPen (pen is Kentron's)

            // Ios line (was Agrotera — venomous serpents, mark re-read as venom). All FIT.
            new LegendaryEntry(153, "Amphisbaena", VariantRoot.Ios, LegendaryRegistry.FamilyFencing, 0, ClauseType.MarkFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(154, "Delphyne", VariantRoot.Ios, LegendaryRegistry.FamilyFencing, 1, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(155, "Python", VariantRoot.Ios, LegendaryRegistry.FamilyFencing, 2, ClauseType.MarkOnCrit, 0, 0, 0, 0),
            new LegendaryEntry(156, "Lamia", VariantRoot.Ios, LegendaryRegistry.FamilyFencing, 3, ClauseType.MarkSpreadOnDeath, 3, 0, 0, 0),
            new LegendaryEntry(157, "Typhon", VariantRoot.Ios, LegendaryRegistry.FamilyFencing, 4, ClauseType.MarkAllSources25, 25, 0, 0, 0),
            new LegendaryEntry(158, "Hydra", VariantRoot.Ios, LegendaryRegistry.FamilyFencing, 5, ClauseType.MarkFirstHit, 0, 0, 0, 0),

            // Ophis line (was Pallas — guardian serpents, block re-read as sway). All FIT.
            new LegendaryEntry(159, "Aspis", VariantRoot.Ophis, LegendaryRegistry.FamilyFencing, 0, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(160, "Ladon", VariantRoot.Ophis, LegendaryRegistry.FamilyFencing, 1, ClauseType.ReflectFirstHit, 20, 0, 0, 0),
            new LegendaryEntry(161, "Ekhion", VariantRoot.Ophis, LegendaryRegistry.FamilyFencing, 2, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(162, "Kaineus", VariantRoot.Ophis, LegendaryRegistry.FamilyFencing, 3, ClauseType.BlockFirstHit, 0, 0, 0, 0),
            new LegendaryEntry(163, "Kolchis", VariantRoot.Ophis, LegendaryRegistry.FamilyFencing, 4, ClauseType.ReflectFirstHit, 25, 0, 0, 0),
            new LegendaryEntry(164, "Bellerophon", VariantRoot.Ophis, LegendaryRegistry.FamilyFencing, 5, ClauseType.BlockFirstHit, 0, 0, 0, 0),

            // Kentron line (was Stygian — the sting bites through). Signature = NthHitFullArmorPen.
            new LegendaryEntry(165, "Ketos", VariantRoot.Kentron, LegendaryRegistry.FamilyFencing, 0, ClauseType.CritArmorPen, 4, 20, 0, 0), // SWAP StamDrainOnCrit
            new LegendaryEntry(166, "Sybaris", VariantRoot.Kentron, LegendaryRegistry.FamilyFencing, 1, ClauseType.CritArmorPen, 5, 20, 0, 0), // SWAP LifestealOnCrit (doc's NthHitFullArmorPen == signature)
            new LegendaryEntry(167, "Drakaina", VariantRoot.Kentron, LegendaryRegistry.FamilyFencing, 2, ClauseType.CritArmorPen, 6, 25, 0, 0), // SWAP OnKillRestore (doc's NthHitFullArmorPen == signature)
            new LegendaryEntry(168, "Ismenios", VariantRoot.Kentron, LegendaryRegistry.FamilyFencing, 3, ClauseType.CritArmorPen, 6, 25, 0, 0), // SWAP LifestealOnCrit
            new LegendaryEntry(169, "Ophion", VariantRoot.Kentron, LegendaryRegistry.FamilyFencing, 4, ClauseType.CritArmorPen, 4, 15, 0, 0), // SWAP OnKillRestore (doc's NthHitFullArmorPen == signature)
            new LegendaryEntry(170, "Kampe", VariantRoot.Kentron, LegendaryRegistry.FamilyFencing, 5, ClauseType.CritArmorPen, 6, 20, 0, 0) // SWAP CritManaLeech
        }
    };
}
