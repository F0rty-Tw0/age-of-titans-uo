using System.Collections.Generic;
using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

public class LegendaryRegistryTests
{
    [Fact]
    public void Registry_ContainsAllWeaponArmorJewelryAndClothingFamilyEntries()
    {
        // 270 through the jewelry + 5 body-clothing relics, plus the 5 hat-bound clothing relics
        // (271-275, 21-clothing.md §3).
        Assert.Equal(275, LegendaryRegistry.Entries.Count);
    }

    [Fact]
    public void Ids_OneThroughTwoSeventyFive_AllPresent()
    {
        for (ushort id = 1; id <= 275; id++)
        {
            Assert.True(LegendaryRegistry.TryGet(id, out _), $"Missing legendary id {id}");
        }
    }

    [Fact]
    public void Ids_AreUnique()
    {
        var seen = new HashSet<ushort>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            Assert.True(seen.Add(entry.Id), $"Duplicate legendary id {entry.Id}");
        }
    }

    [Fact]
    public void FamilyCounts_MatchDesignDocs()
    {
        var counts = new Dictionary<byte, int>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            counts.TryGetValue(entry.Family, out var n);
            counts[entry.Family] = n + 1;
        }

        Assert.Equal(40, counts[LegendaryRegistry.FamilyAxes]);
        Assert.Equal(40, counts[LegendaryRegistry.FamilySwords]);
        Assert.Equal(10, counts[LegendaryRegistry.FamilyPolearms]);
        Assert.Equal(35, counts[LegendaryRegistry.FamilyMaces]);
        Assert.Equal(15, counts[LegendaryRegistry.FamilyStaves]);
        Assert.Equal(30, counts[LegendaryRegistry.FamilyFencing]);
        Assert.Equal(15, counts[LegendaryRegistry.FamilyArchery]);
        Assert.Equal(15, counts[LegendaryRegistry.FamilyMetalArmor]);
        Assert.Equal(15, counts[LegendaryRegistry.FamilyLightArmor]);
        Assert.Equal(30, counts[LegendaryRegistry.FamilyShields]);
        Assert.Equal(20, counts[LegendaryRegistry.FamilyJewelry]);
        Assert.Equal(10, counts[LegendaryRegistry.FamilyClothing]); // 5 body relics + 5 hat relics
    }

    [Fact]
    public void Names_AreGloballyUnique()
    {
        var names = new HashSet<string>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            Assert.False(string.IsNullOrWhiteSpace(entry.Name));
            Assert.True(names.Add(entry.Name), $"Duplicate legendary name {entry.Name}");
        }
    }

    [Fact]
    public void EveryEntry_HasRootMatchingItsFamilyAndRealClause()
    {
        foreach (var entry in LegendaryRegistry.Entries)
        {
            Assert.InRange(entry.Family, LegendaryRegistry.FamilyAxes, LegendaryRegistry.FamilyClothing);
            Assert.NotEqual(ClauseType.None, entry.Clause);

            if (entry.Family <= LegendaryRegistry.FamilyArchery)
            {
                Assert.InRange(entry.BaseIndex, (byte)0, (byte)7);
                // Post-re-theme: each weapon root belongs to exactly one family (WeaponFamilyMap).
                Assert.True(
                    WeaponFamilyMap.TryGetRootFamily(entry.Root, out var rootFamily) && rootFamily == entry.Family,
                    $"{entry.Name} root {entry.Root} does not belong to weapon family {entry.Family}"
                );
            }
            else if (entry.Family == LegendaryRegistry.FamilyJewelry)
            {
                Assert.InRange(entry.BaseIndex, (byte)0, (byte)3); // ring..earrings
                Assert.True(
                    entry.Root is VariantRoot.Olympian or VariantRoot.Hecatean or VariantRoot.Tychean
                        or VariantRoot.Nyxian or VariantRoot.Demetrian,
                    $"{entry.Name} has non-jewelry root {entry.Root}"
                );
            }
            else if (entry.Family == LegendaryRegistry.FamilyClothing)
            {
                Assert.InRange(entry.BaseIndex, (byte)0, (byte)11); // body pieces 0-4, hats 8-11
                Assert.True(
                    entry.Root is VariantRoot.Laurel or VariantRoot.Charis or VariantRoot.Maenad
                        or VariantRoot.Hestian or VariantRoot.Arachne,
                    $"{entry.Name} has non-clothing root {entry.Root}"
                );
            }
            else if (entry.Family == LegendaryRegistry.FamilyShields)
            {
                Assert.InRange(entry.BaseIndex, (byte)0, (byte)5); // buckler..heater
                Assert.True(
                    entry.Root is VariantRoot.Aegis or VariantRoot.Amyntor or VariantRoot.Probolos
                        or VariantRoot.Herkos or VariantRoot.Pnoe,
                    $"{entry.Name} has non-shield root {entry.Root}"
                );
            }
            else
            {
                // Re-theme 2026-07-07: armor roots are material-locked — the entry's BaseIndex
                // (material ladder position) must match its root's material set.
                Assert.InRange(entry.BaseIndex, (byte)0, (byte)2);

                var expected = entry.Family == LegendaryRegistry.FamilyMetalArmor
                    ? entry.BaseIndex switch
                    {
                        0 => new[] { VariantRoot.Hoplites, VariantRoot.Taxis, VariantRoot.Dromos, VariantRoot.Zoster, VariantRoot.Alkimos },
                        1 => new[] { VariantRoot.Phylax, VariantRoot.Egregoros, VariantRoot.Teichos, VariantRoot.Halysis, VariantRoot.Phrourion },
                        _ => new[] { VariantRoot.Adamas, VariantRoot.Kaminos, VariantRoot.Kolossos, VariantRoot.Panoplia, VariantRoot.Akamatos }
                    }
                    : entry.BaseIndex switch
                    {
                        0 => new[] { VariantRoot.Naias, VariantRoot.Dryas, VariantRoot.Oreias, VariantRoot.Melissa, VariantRoot.Panika },
                        1 => new[] { VariantRoot.Kynegis, VariantRoot.Batos, VariantRoot.Arkas, VariantRoot.Elaphis, VariantRoot.Skia },
                        _ => new[] { VariantRoot.Melinoe, VariantRoot.Makaria, VariantRoot.Tymbos, VariantRoot.Nekyia, VariantRoot.Katachthon }
                    };

                Assert.Contains(entry.Root, expected);
            }
        }
    }

    [Fact]
    public void Labrys_IsPhobosDoubleAxeCritSplash()
    {
        Assert.True(LegendaryRegistry.TryGet(12, out var labrys));
        Assert.Equal("Labrys", labrys.Name);
        Assert.Equal(VariantRoot.Phobos, labrys.Root);
        Assert.Equal(3, labrys.BaseIndex); // double axe
        Assert.Equal(ClauseType.CritSplash, labrys.Clause);
        Assert.Equal(15, labrys.P2); // 15% splash
    }

    [Fact]
    public void EachRoot_HasOneLegendaryPerFamilyBase()
    {
        // Post-re-theme (2026-07-07): each weapon family carries its own five roots, one legendary
        // per base. Counts = that family's base-ladder length. Axes keep the original five roots
        // (8 bases). Armor: Polias is metal(3) + light(3) only (shields use Aegis instead) = 6.
        // Cyclopean/Paean/Tritonian/Talarian: metal(3) + light(3) + shields(6) = 12 each.
        // Aegis: shields only (6).
        var counts = new Dictionary<VariantRoot, int>();

        foreach (var entry in LegendaryRegistry.Entries)
        {
            counts.TryGetValue(entry.Root, out var n);
            counts[entry.Root] = n + 1;
        }

        // Axes (8 bases) — the original five roots, now axe-exclusive.
        Assert.Equal(8, counts[VariantRoot.Zephyr]);
        Assert.Equal(8, counts[VariantRoot.Phobos]);
        Assert.Equal(8, counts[VariantRoot.Agrotera]);
        Assert.Equal(8, counts[VariantRoot.Pallas]);
        Assert.Equal(8, counts[VariantRoot.Stygian]);

        // Swords (8 bases).
        Assert.Equal(8, counts[VariantRoot.Phoibos]);
        Assert.Equal(8, counts[VariantRoot.Areia]);
        Assert.Equal(8, counts[VariantRoot.Menis]);
        Assert.Equal(8, counts[VariantRoot.Aristeia]);
        Assert.Equal(8, counts[VariantRoot.Haima]);

        // Polearms (2 bases).
        Assert.Equal(2, counts[VariantRoot.Theristes]);
        Assert.Equal(2, counts[VariantRoot.Sarisa]);
        Assert.Equal(2, counts[VariantRoot.Phalanx]);
        Assert.Equal(2, counts[VariantRoot.Horme]);
        Assert.Equal(2, counts[VariantRoot.Zophos]);

        // Maces (7 bases).
        Assert.Equal(7, counts[VariantRoot.Ennosigaios]);
        Assert.Equal(7, counts[VariantRoot.Kataigis]);
        Assert.Equal(7, counts[VariantRoot.Rhaistes]);
        Assert.Equal(7, counts[VariantRoot.Eryma]);
        Assert.Equal(7, counts[VariantRoot.Kamatos]);

        // Staves (3 bases).
        Assert.Equal(3, counts[VariantRoot.Empousa]);
        Assert.Equal(3, counts[VariantRoot.Prester]);
        Assert.Equal(3, counts[VariantRoot.Alexikakos]);
        Assert.Equal(3, counts[VariantRoot.Manteia]);
        Assert.Equal(3, counts[VariantRoot.Baskania]);

        // Fencing (6 bases).
        Assert.Equal(6, counts[VariantRoot.Ios]);
        Assert.Equal(6, counts[VariantRoot.Ephodos]);
        Assert.Equal(6, counts[VariantRoot.Aiolos]);
        Assert.Equal(6, counts[VariantRoot.Kentron]);
        Assert.Equal(6, counts[VariantRoot.Ophis]);

        // Archery (3 bases).
        Assert.Equal(3, counts[VariantRoot.Hekatos]);
        Assert.Equal(3, counts[VariantRoot.Belos]);
        Assert.Equal(3, counts[VariantRoot.Pede]);
        Assert.Equal(3, counts[VariantRoot.Toxikon]);
        Assert.Equal(3, counts[VariantRoot.Skopos]);

        // Re-theme 2026-07-07: retired shared armor roots hold NO legendaries anymore.
        Assert.False(counts.ContainsKey(VariantRoot.Polias));
        Assert.False(counts.ContainsKey(VariantRoot.Cyclopean));
        Assert.False(counts.ContainsKey(VariantRoot.Paean));
        Assert.False(counts.ContainsKey(VariantRoot.Tritonian));
        Assert.False(counts.ContainsKey(VariantRoot.Talarian));

        // Armor: one legendary per material root (30 roots × 1). Material index is asserted
        // root-by-root in EveryEntry_HasRootMatchingItsFamilyAndRealClause.
        foreach (var root in new[]
                 {
                     VariantRoot.Hoplites, VariantRoot.Taxis, VariantRoot.Dromos, VariantRoot.Zoster, VariantRoot.Alkimos,
                     VariantRoot.Phylax, VariantRoot.Egregoros, VariantRoot.Teichos, VariantRoot.Halysis, VariantRoot.Phrourion,
                     VariantRoot.Adamas, VariantRoot.Kaminos, VariantRoot.Kolossos, VariantRoot.Panoplia, VariantRoot.Akamatos,
                     VariantRoot.Naias, VariantRoot.Dryas, VariantRoot.Oreias, VariantRoot.Melissa, VariantRoot.Panika,
                     VariantRoot.Kynegis, VariantRoot.Batos, VariantRoot.Arkas, VariantRoot.Elaphis, VariantRoot.Skia,
                     VariantRoot.Melinoe, VariantRoot.Makaria, VariantRoot.Tymbos, VariantRoot.Nekyia, VariantRoot.Katachthon
                 })
        {
            Assert.Equal(1, counts[root]);
        }

        // Shields: five roots × six shapes.
        Assert.Equal(6, counts[VariantRoot.Aegis]);
        Assert.Equal(6, counts[VariantRoot.Amyntor]);
        Assert.Equal(6, counts[VariantRoot.Pnoe]);
        Assert.Equal(6, counts[VariantRoot.Herkos]);
        Assert.Equal(6, counts[VariantRoot.Probolos]);

        // Jewelry: 4 slots (ring/bracelet/necklace/earrings) per root, one legendary each.
        Assert.Equal(4, counts[VariantRoot.Olympian]);
        Assert.Equal(4, counts[VariantRoot.Hecatean]);
        Assert.Equal(4, counts[VariantRoot.Tychean]);
        Assert.Equal(4, counts[VariantRoot.Nyxian]);
        Assert.Equal(4, counts[VariantRoot.Demetrian]);

        // Clothing: two relics per root — a body piece + a hat (21-clothing.md §3).
        Assert.Equal(2, counts[VariantRoot.Laurel]);
        Assert.Equal(2, counts[VariantRoot.Charis]);
        Assert.Equal(2, counts[VariantRoot.Maenad]);
        Assert.Equal(2, counts[VariantRoot.Hestian]);
        Assert.Equal(2, counts[VariantRoot.Arachne]);
    }

    [Fact]
    public void Exists_RejectsUnknownAndZero()
    {
        Assert.False(LegendaryRegistry.Exists(0));
        Assert.False(LegendaryRegistry.Exists(999));
        Assert.True(LegendaryRegistry.Exists(1));
    }

    [Fact]
    public void WeaponLegendary_UniqueClause_NeverEqualsItsLaneSignature()
    {
        // Audit invariant (framework D1 / punch list): a legendary's unique clause type must differ
        // from its root's Epic-tier lane signature type — same type at a different cadence still
        // double-fires the same proc (the Pede precedent). Type-level check, within-lane only.
        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (entry.Family > LegendaryRegistry.FamilyArchery)
            {
                continue; // weapon families only
            }

            var signature = WeaponEffectTable.Get(entry.Root, ItemRarity.Epic).Signature;

            if (signature == ClauseType.None)
            {
                continue; // axes and the signatureless lanes have nothing to collide with
            }

            Assert.False(
                entry.Clause == signature,
                $"{entry.Name} ({entry.Root}) unique clause {entry.Clause} duplicates its lane signature"
            );
        }
    }
}
