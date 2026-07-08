using Server;
using Server.Engines.Rarity;
using Server.Items;
using Xunit;

namespace UOContent.Tests;

// Guards the BaseWeapon v12->v13 root remap (RethemeMigration) and the shared WeaponFamilyMap.
// Item construction needs the world fixture, so this runs under the sequential collection.
[Collection("Sequential UOContent Tests")]
public class RethemeMigrationTests
{
    [Fact]
    public void WeaponFamilyMap_MapsOneRepresentativePerFamily()
    {
        AssertFamily(new Hatchet(), LegendaryRegistry.FamilyAxes);
        AssertFamily(new Katana(), LegendaryRegistry.FamilySwords);
        AssertFamily(new Bardiche(), LegendaryRegistry.FamilyPolearms);
        AssertFamily(new WarAxe(), LegendaryRegistry.FamilyMaces); // war axe is a mace mechanically
        AssertFamily(new QuarterStaff(), LegendaryRegistry.FamilyStaves);
        AssertFamily(new Dagger(), LegendaryRegistry.FamilyFencing);
        AssertFamily(new Bow(), LegendaryRegistry.FamilyArchery);
    }

    [Fact]
    public void RemapWeaponRoot_RepointsLegacyRoots_ByFamily()
    {
        // Non-axe weapons carrying a legacy shared root move to their family's bespoke root.
        AssertRemap(new Katana(), VariantRoot.Zephyr, VariantRoot.Menis);
        AssertRemap(new Katana(), VariantRoot.Phobos, VariantRoot.Phoibos);
        AssertRemap(new Katana(), VariantRoot.Agrotera, VariantRoot.Haima);
        AssertRemap(new Katana(), VariantRoot.Pallas, VariantRoot.Areia);
        AssertRemap(new Katana(), VariantRoot.Stygian, VariantRoot.Aristeia);

        AssertRemap(new WarAxe(), VariantRoot.Zephyr, VariantRoot.Ennosigaios); // mace family
        AssertRemap(new Bow(), VariantRoot.Stygian, VariantRoot.Pede);          // archery family
        AssertRemap(new Dagger(), VariantRoot.Pallas, VariantRoot.Ophis);       // fencing family
    }

    [Fact]
    public void RemapWeaponRoot_LeavesAxesAndAlreadyThemedAndNone_Unchanged()
    {
        AssertRemap(new Hatchet(), VariantRoot.Zephyr, VariantRoot.Zephyr);   // axes keep the legacy five
        AssertRemap(new OrnateAxe(), VariantRoot.Stygian, VariantRoot.Stygian);
        AssertRemap(new Katana(), VariantRoot.Phoibos, VariantRoot.Phoibos);  // already per-family (idempotent)
        AssertRemap(new Katana(), VariantRoot.None, VariantRoot.None);        // no variant
    }

    private static void AssertFamily(Item item, byte expected)
    {
        try
        {
            Assert.True(WeaponFamilyMap.TryGetFamily(item, out var family));
            Assert.Equal(expected, family);
        }
        finally
        {
            item.Delete();
        }
    }

    private static void AssertRemap(Item item, VariantRoot from, VariantRoot expected)
    {
        try
        {
            Assert.Equal(expected, RethemeMigration.RemapWeaponRoot(from, item));
        }
        finally
        {
            item.Delete();
        }
    }

    // ---- Armor side (BaseArmor v11->v12, Phase 4) — pure function, no fixture needed --------

    public static readonly TheoryData<VariantRoot, ArmorMaterialType, VariantRoot> ArmorRemaps = new()
    {
        // Ring
        { VariantRoot.Polias, ArmorMaterialType.Ringmail, VariantRoot.Hoplites },
        { VariantRoot.Cyclopean, ArmorMaterialType.Ringmail, VariantRoot.Zoster },
        { VariantRoot.Paean, ArmorMaterialType.Ringmail, VariantRoot.Alkimos },
        { VariantRoot.Tritonian, ArmorMaterialType.Ringmail, VariantRoot.Taxis },
        { VariantRoot.Talarian, ArmorMaterialType.Ringmail, VariantRoot.Dromos },
        // Chain
        { VariantRoot.Polias, ArmorMaterialType.Chainmail, VariantRoot.Phylax },
        { VariantRoot.Cyclopean, ArmorMaterialType.Chainmail, VariantRoot.Halysis },
        { VariantRoot.Paean, ArmorMaterialType.Chainmail, VariantRoot.Phrourion },
        { VariantRoot.Tritonian, ArmorMaterialType.Chainmail, VariantRoot.Egregoros },
        { VariantRoot.Talarian, ArmorMaterialType.Chainmail, VariantRoot.Teichos },
        // Plate
        { VariantRoot.Polias, ArmorMaterialType.Plate, VariantRoot.Adamas },
        { VariantRoot.Cyclopean, ArmorMaterialType.Plate, VariantRoot.Kaminos },
        { VariantRoot.Paean, ArmorMaterialType.Plate, VariantRoot.Akamatos },
        { VariantRoot.Tritonian, ArmorMaterialType.Plate, VariantRoot.Kolossos },
        { VariantRoot.Talarian, ArmorMaterialType.Plate, VariantRoot.Panoplia },
        // Leather
        { VariantRoot.Polias, ArmorMaterialType.Leather, VariantRoot.Naias },
        { VariantRoot.Cyclopean, ArmorMaterialType.Leather, VariantRoot.Dryas },
        { VariantRoot.Paean, ArmorMaterialType.Leather, VariantRoot.Melissa },
        { VariantRoot.Tritonian, ArmorMaterialType.Leather, VariantRoot.Panika },
        { VariantRoot.Talarian, ArmorMaterialType.Leather, VariantRoot.Oreias },
        // Studded
        { VariantRoot.Polias, ArmorMaterialType.Studded, VariantRoot.Arkas },
        { VariantRoot.Cyclopean, ArmorMaterialType.Studded, VariantRoot.Batos },
        { VariantRoot.Paean, ArmorMaterialType.Studded, VariantRoot.Elaphis },
        { VariantRoot.Tritonian, ArmorMaterialType.Studded, VariantRoot.Skia },
        { VariantRoot.Talarian, ArmorMaterialType.Studded, VariantRoot.Kynegis },
        // Bone
        { VariantRoot.Polias, ArmorMaterialType.Bone, VariantRoot.Tymbos },
        { VariantRoot.Cyclopean, ArmorMaterialType.Bone, VariantRoot.Katachthon },
        { VariantRoot.Paean, ArmorMaterialType.Bone, VariantRoot.Makaria },
        { VariantRoot.Tritonian, ArmorMaterialType.Bone, VariantRoot.Nekyia },
        { VariantRoot.Talarian, ArmorMaterialType.Bone, VariantRoot.Melinoe }
    };

    [Theory]
    [MemberData(nameof(ArmorRemaps))]
    public void RemapArmorRoot_MapsEveryLegacyRootPerMaterial(
        VariantRoot legacy, ArmorMaterialType material, VariantRoot expected
    ) =>
        Assert.Equal(expected, RethemeMigration.RemapArmorRoot(legacy, material, isShield: false));

    [Theory]
    [InlineData(VariantRoot.Cyclopean, VariantRoot.Amyntor)]
    [InlineData(VariantRoot.Paean, VariantRoot.Pnoe)]
    [InlineData(VariantRoot.Tritonian, VariantRoot.Herkos)]
    [InlineData(VariantRoot.Talarian, VariantRoot.Probolos)]
    [InlineData(VariantRoot.Polias, VariantRoot.Aegis)] // never legitimate on a shield — fail safe
    public void RemapArmorRoot_ShieldsMapToTheShieldSet(VariantRoot legacy, VariantRoot expected) =>
        Assert.Equal(expected, RethemeMigration.RemapArmorRoot(legacy, ArmorMaterialType.Plate, isShield: true));

    [Fact]
    public void RemapArmorRoot_IsIdempotentForNonLegacyRoots()
    {
        Assert.Equal(VariantRoot.None, RethemeMigration.RemapArmorRoot(VariantRoot.None, ArmorMaterialType.Plate, false));
        Assert.Equal(VariantRoot.Aegis, RethemeMigration.RemapArmorRoot(VariantRoot.Aegis, ArmorMaterialType.Plate, true));
        Assert.Equal(VariantRoot.Adamas, RethemeMigration.RemapArmorRoot(VariantRoot.Adamas, ArmorMaterialType.Plate, false));
        Assert.Equal(VariantRoot.Naias, RethemeMigration.RemapArmorRoot(VariantRoot.Naias, ArmorMaterialType.Leather, false));
        // Materials outside the six variant-bearing ones never carried variants — legacy roots
        // pass through untouched there (out-of-range value stands in for any exotic material).
        Assert.Equal(VariantRoot.Paean, RethemeMigration.RemapArmorRoot(VariantRoot.Paean, (ArmorMaterialType)255, false));
    }
}
