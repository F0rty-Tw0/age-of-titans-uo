using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// 10-armor-metal.md §2 and 11-armor-light.md §2 publish identical magnitude tables (only the
// AR number on the concrete item class differs by material) — ArmorEffectTable has a single
// shared "armor" table for exactly that reason, so there is no separate metal-vs-light check.
public class ArmorEffectTableTests
{
    [Theory]
    [InlineData(ItemRarity.Uncommon, 1, 0, 0)]
    [InlineData(ItemRarity.Rare, 2, 1, 0)]
    [InlineData(ItemRarity.Epic, 3, 2, 5)]
    [InlineData(ItemRarity.Legendary, 4, 3, 8)]
    public void Polias_MatchesDocMagnitudes(ItemRarity rarity, int bonusAr, int drPct, int shrugPct)
    {
        var row = ArmorEffectTable.Get(VariantRoot.Polias, rarity, isShield: false);

        Assert.Equal(bonusAr, row.BonusAr);
        Assert.Equal(drPct, row.DrPct);
        Assert.Equal(shrugPct, row.ShrugPct);
    }

    [Fact]
    public void CyclopeanEpic_ReflectsAndFlameProcs()
    {
        // Self-repair was removed with the durability overhaul (Part B2); the reflect/flame lanes stay.
        var row = ArmorEffectTable.Get(VariantRoot.Cyclopean, ItemRarity.Epic, isShield: false);

        Assert.Equal(5, row.ReflectPct);
        Assert.Equal(4, row.FlameProcPct);
    }

    [Fact]
    public void PaeanLegendary_RegensHealsAndAutoCures()
    {
        var row = ArmorEffectTable.Get(VariantRoot.Paean, ItemRarity.Legendary, isShield: false);

        Assert.Equal(25, row.HpRegenPct);
        Assert.Equal(10, row.HealsReceivedPct);
        Assert.True(row.AutoCure);
    }

    [Fact]
    public void TritonianLegendary_SpellDrParaResistAndSkillBonus()
    {
        var row = ArmorEffectTable.Get(VariantRoot.Tritonian, ItemRarity.Legendary, isShield: false);

        Assert.Equal(6, row.SpellDrPct);
        Assert.Equal(30, row.ParaResistPct);
        Assert.Equal(5, row.ResistSkillBonus);
    }

    [Fact]
    public void TalarianLegendary_WeightStamAndDodge()
    {
        var row = ArmorEffectTable.Get(VariantRoot.Talarian, ItemRarity.Legendary, isShield: false);

        // WeightReductionPct is the carry-capacity lane, rescaled to a 25% Legendary ceiling
        // (2026-07-12) and suit-capped at WornEffectState.CarryWeightCap.
        Assert.Equal(25, row.WeightReductionPct);
        Assert.Equal(WornEffectState.CarryWeightCap, row.WeightReductionPct);
        Assert.Equal(10, row.StamRegenPct);
        Assert.Equal(5, row.DodgePct);
    }

    [Fact]
    public void AegisLegendaryShield_ParryAndDrMatchDoc()
    {
        var row = ArmorEffectTable.Get(VariantRoot.Aegis, ItemRarity.Legendary, isShield: true);

        Assert.Equal(8, row.ParryPct);
        Assert.Equal(12, row.ParryDrPct);
        Assert.True(row.ParryThorns);
    }

    [Fact]
    public void ShieldTable_RunsAboveArmorTable()
    {
        // Shield magnitudes run ~2x an armor piece (framework §3/§4 pattern, scaled up).
        var armor = ArmorEffectTable.Get(VariantRoot.Cyclopean, ItemRarity.Legendary, isShield: false);
        var shield = ArmorEffectTable.Get(VariantRoot.Cyclopean, ItemRarity.Legendary, isShield: true);

        Assert.True(shield.ReflectPct > armor.ReflectPct);
        Assert.True(shield.FlameProcPct > armor.FlameProcPct);
    }

    [Fact]
    public void CommonAndNone_AreEmpty()
    {
        Assert.True(ArmorEffectTable.Get(VariantRoot.Polias, ItemRarity.Common, false).IsEmpty);
        Assert.True(ArmorEffectTable.Get(VariantRoot.None, ItemRarity.Legendary, false).IsEmpty);
        Assert.True(ArmorEffectTable.Get(VariantRoot.Aegis, ItemRarity.Common, true).IsEmpty);
    }

    [Fact]
    public void AegisRoot_HasNoArmorPackage()
    {
        // Aegis is the shield-only bulwark root — it never appears on armor pieces.
        Assert.True(ArmorEffectTable.Get(VariantRoot.Aegis, ItemRarity.Legendary, isShield: false).IsEmpty);
    }

    [Fact]
    public void PoliasRoot_HasNoShieldPackage()
    {
        // Polias never appears on shields (Aegis takes its place — framework §3).
        Assert.True(ArmorEffectTable.Get(VariantRoot.Polias, ItemRarity.Legendary, isShield: true).IsEmpty);
    }

    [Fact]
    public void NewFields_DefaultZero_KeepRowEmpty()
    {
        Assert.True(default(ArmorEffectRow).IsEmpty);
    }

    [Fact]
    public void RowWithOnlySignatureOrNewField_IsNotEmpty()
    {
        Assert.False(new ArmorEffectRow { Signature = ClauseType.ParryRestoresStam }.IsEmpty);
        Assert.False(new ArmorEffectRow { PoisonResistPct = 10 }.IsEmpty);
        Assert.False(new ArmorEffectRow { HidingBonus = 5 }.IsEmpty);
        Assert.False(new ArmorEffectRow { OnKillHpPct = 15 }.IsEmpty);
    }

    private static readonly VariantRoot[] MaterialArmorRoots =
    {
        VariantRoot.Hoplites, VariantRoot.Taxis, VariantRoot.Dromos, VariantRoot.Zoster, VariantRoot.Alkimos,
        VariantRoot.Phylax, VariantRoot.Egregoros, VariantRoot.Teichos, VariantRoot.Halysis, VariantRoot.Phrourion,
        VariantRoot.Adamas, VariantRoot.Kaminos, VariantRoot.Kolossos, VariantRoot.Panoplia, VariantRoot.Akamatos,
        VariantRoot.Naias, VariantRoot.Dryas, VariantRoot.Oreias, VariantRoot.Melissa, VariantRoot.Panika,
        VariantRoot.Kynegis, VariantRoot.Batos, VariantRoot.Arkas, VariantRoot.Elaphis, VariantRoot.Skia,
        VariantRoot.Melinoe, VariantRoot.Makaria, VariantRoot.Tymbos, VariantRoot.Nekyia, VariantRoot.Katachthon
    };

    private static readonly VariantRoot[] RethemeShieldRoots =
    {
        VariantRoot.Amyntor, VariantRoot.Probolos, VariantRoot.Herkos, VariantRoot.Pnoe
    };

    // Data coverage: every material root has a full armor ladder (Uncommon+ non-empty) and NO
    // shield package. Since the Option-A milestone, Epic+ non-shield armor reads its signature from
    // the (material x slot) ArmorSlotSignatureTable, not the per-root row — so the row's own
    // Signature at Epic/Legendary is dead and MUST be None (see WornEffectState.Rebuild's fork).
    // Legacy shared roots keep their rows for old-save decode; the loot roller never deals them.
    [Fact]
    public void EveryMaterialRoot_HasFullArmorLadder_AndNoDeadRowSignature()
    {
        foreach (var root in MaterialArmorRoots)
        {
            for (var rarity = ItemRarity.Uncommon; rarity <= ItemRarity.Legendary; rarity++)
            {
                Assert.False(ArmorEffectTable.Get(root, rarity, isShield: false).IsEmpty);
            }

            var epic = ArmorEffectTable.Get(root, ItemRarity.Epic, isShield: false);
            var legendary = ArmorEffectTable.Get(root, ItemRarity.Legendary, isShield: false);

            // Epic+ signatures live in the slot table now; the row-level ones are dead — keep them None.
            Assert.Equal(ClauseType.None, epic.Signature);
            Assert.Equal(ClauseType.None, legendary.Signature);
            Assert.True(ArmorEffectTable.Get(root, ItemRarity.Epic, isShield: true).IsEmpty);
        }
    }

    [Fact]
    public void EveryRethemeShieldRoot_HasShieldLadderWithEpicSignature()
    {
        foreach (var root in RethemeShieldRoots)
        {
            for (var rarity = ItemRarity.Uncommon; rarity <= ItemRarity.Legendary; rarity++)
            {
                Assert.False(ArmorEffectTable.Get(root, rarity, isShield: true).IsEmpty);
            }

            var epic = ArmorEffectTable.Get(root, ItemRarity.Epic, isShield: true);
            var legendary = ArmorEffectTable.Get(root, ItemRarity.Legendary, isShield: true);

            Assert.NotEqual(ClauseType.None, epic.Signature);
            Assert.Equal(epic.Signature, legendary.Signature);
            Assert.True(ArmorEffectTable.Get(root, ItemRarity.Epic, isShield: false).IsEmpty);
        }
    }

    // Audit invariant (the Pede precedent, extended to armor/shields): a legendary's unique
    // clause type never equals its own root's Epic-tier lane signature type.
    [Fact]
    public void ArmorAndShieldLegendaries_UniqueClause_NeverEqualsLaneSignature()
    {
        foreach (var entry in LegendaryRegistry.Entries)
        {
            if (entry.Family is not (LegendaryRegistry.FamilyMetalArmor or LegendaryRegistry.FamilyLightArmor
                or LegendaryRegistry.FamilyShields))
            {
                continue;
            }

            var isShield = entry.Family == LegendaryRegistry.FamilyShields;
            var signature = ArmorEffectTable.Get(entry.Root, ItemRarity.Epic, isShield).Signature;

            Assert.True(
                entry.Clause != signature,
                $"{entry.Name} unique clause {entry.Clause} duplicates its {entry.Root} lane signature"
            );
        }
    }

    // Weapon tables took signatures in Phase 3; the legacy shared ARMOR roots stay signature-free
    // forever (decode-only — framework §3).
    [Fact]
    public void LegacySharedArmorRoots_KeepNoSignature()
    {
        foreach (var root in new[]
                 {
                     VariantRoot.Polias, VariantRoot.Cyclopean, VariantRoot.Paean,
                     VariantRoot.Tritonian, VariantRoot.Talarian
                 })
        {
            for (var rarity = ItemRarity.Common; rarity <= ItemRarity.Legendary; rarity++)
            {
                Assert.Equal(ClauseType.None, ArmorEffectTable.Get(root, rarity, isShield: false).Signature);
                Assert.Equal(ClauseType.None, ArmorEffectTable.Get(root, rarity, isShield: true).Signature);
            }
        }
    }
}
