using Server.Engines.Rarity;
using Xunit;

namespace UOContent.Tests;

// 20-jewelry.md §2 (jewelry, own Legendary column — a step above Epic, framework §4) and
// 21-clothing.md §2 (clothing, Uncommon..Epic only — no distinct Legendary tier; the table
// duplicates Epic into the Legendary slot the same way WeaponEffectTable does, so the 5 bound
// relics still carry "full Epic effects" per 21-clothing.md §3).
public class AccessoryEffectTableTests
{
    [Fact]
    public void OlympianLegendary_MatchesDocMagnitudes()
    {
        var row = AccessoryEffectTable.Get(VariantRoot.Olympian, ItemRarity.Legendary, isClothing: false);

        Assert.Equal(9, row.StatBonus);
        Assert.Equal(5, row.LightningProcPct);
    }

    [Fact]
    public void HecateanEpic_MatchesDocMagnitudes()
    {
        var row = AccessoryEffectTable.Get(VariantRoot.Hecatean, ItemRarity.Epic, isClothing: false);

        Assert.Equal(18, row.ManaRegenPct);
        Assert.Equal(7, row.SpellDamagePct);
        Assert.Equal(3, row.ManaLeechPct);
    }

    [Fact]
    public void NyxianLegendary_GrantsNightSightAndPoisonResist()
    {
        var row = AccessoryEffectTable.Get(VariantRoot.Nyxian, ItemRarity.Legendary, isClothing: false);

        Assert.Equal(15, row.HidingBonus);
        Assert.Equal(15, row.StealthBonus);
        Assert.True(row.NightSight);
        Assert.Equal(10, row.PoisonResistPct);
    }

    [Fact]
    public void MaenadEpic_HasFrenzyChanceAndSwingRider()
    {
        var row = AccessoryEffectTable.Get(VariantRoot.Maenad, ItemRarity.Epic, isClothing: true);

        Assert.Equal(4, row.FrenzyChancePct);
        Assert.Equal(10, row.FrenzyDamagePct);
        Assert.Equal(10, row.FrenzySwingPct);
    }

    [Fact]
    public void CommonAndNone_AreEmpty()
    {
        Assert.True(AccessoryEffectTable.Get(VariantRoot.Olympian, ItemRarity.Common, isClothing: false).IsEmpty);
        Assert.True(AccessoryEffectTable.Get(VariantRoot.None, ItemRarity.Legendary, isClothing: false).IsEmpty);
        Assert.True(AccessoryEffectTable.Get(VariantRoot.Laurel, ItemRarity.Common, isClothing: true).IsEmpty);
    }

    [Theory]
    [InlineData(VariantRoot.Laurel)]
    [InlineData(VariantRoot.Charis)]
    [InlineData(VariantRoot.Maenad)]
    [InlineData(VariantRoot.Hestian)]
    [InlineData(VariantRoot.Arachne)]
    public void ClothingRoots_HaveNoDistinctLegendaryRow(VariantRoot root)
    {
        // Drops themselves never reach Legendary for clothing (§1) — the Legendary slot exists
        // only so the 5 bound relics (§3) can carry "full Epic effects"; it always equals Epic,
        // never an independently-tuned row (mirrors how WeaponEffectTable duplicates Epic too).
        var epic = AccessoryEffectTable.Get(root, ItemRarity.Epic, isClothing: true);
        var legendary = AccessoryEffectTable.Get(root, ItemRarity.Legendary, isClothing: true);

        Assert.Equal(epic, legendary);
    }

    [Fact]
    public void JewelryRoots_HaveDistinctLegendaryStepAboveEpic()
    {
        // Unlike clothing, jewelry's Legendary column is its own step above Epic (framework §4).
        var epic = AccessoryEffectTable.Get(VariantRoot.Demetrian, ItemRarity.Epic, isClothing: false);
        var legendary = AccessoryEffectTable.Get(VariantRoot.Demetrian, ItemRarity.Legendary, isClothing: false);

        Assert.NotEqual(epic, legendary);
        Assert.True(legendary.AllRegenPct > epic.AllRegenPct);
    }
}
