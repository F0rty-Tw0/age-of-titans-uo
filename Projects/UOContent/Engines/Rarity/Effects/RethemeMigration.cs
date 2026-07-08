using Server.Items;

namespace Server.Engines.Rarity;

// 2026-07-07 per-family re-theme: the five legacy shared weapon roots (Zephyr/Phobos/Agrotera/
// Pallas/Stygian) used to sit on every weapon family. On load, a non-axe weapon carrying one of
// them is remapped to its family's bespoke root (framework §3). Axes keep the legacy five (they
// are the exemplar). Anything already per-family, a non-weapon root, None, or an unknown weapon
// type is returned unchanged — the switch is idempotent and safe to run on any weapon.
public static class RethemeMigration
{
    public static VariantRoot RemapWeaponRoot(VariantRoot root, Item item)
    {
        if (root is not (VariantRoot.Zephyr or VariantRoot.Phobos or VariantRoot.Agrotera
            or VariantRoot.Pallas or VariantRoot.Stygian))
        {
            return root; // already per-family / non-weapon / None
        }

        if (!WeaponFamilyMap.TryGetFamily(item, out var family) || family == LegendaryRegistry.FamilyAxes)
        {
            return root; // unknown weapon type or the axe family -> keep the legacy root
        }

        return family switch
        {
            LegendaryRegistry.FamilySwords => root switch
            {
                VariantRoot.Zephyr => VariantRoot.Menis,
                VariantRoot.Phobos => VariantRoot.Phoibos,
                VariantRoot.Agrotera => VariantRoot.Haima,
                VariantRoot.Pallas => VariantRoot.Areia,
                _ => VariantRoot.Aristeia // Stygian
            },
            LegendaryRegistry.FamilyPolearms => root switch
            {
                VariantRoot.Zephyr => VariantRoot.Theristes,
                VariantRoot.Phobos => VariantRoot.Sarisa,
                VariantRoot.Agrotera => VariantRoot.Horme,
                VariantRoot.Pallas => VariantRoot.Phalanx,
                _ => VariantRoot.Zophos // Stygian
            },
            LegendaryRegistry.FamilyMaces => root switch
            {
                VariantRoot.Zephyr => VariantRoot.Ennosigaios,
                VariantRoot.Phobos => VariantRoot.Rhaistes,
                VariantRoot.Agrotera => VariantRoot.Kataigis,
                VariantRoot.Pallas => VariantRoot.Eryma,
                _ => VariantRoot.Kamatos // Stygian
            },
            LegendaryRegistry.FamilyStaves => root switch
            {
                VariantRoot.Zephyr => VariantRoot.Empousa,
                VariantRoot.Phobos => VariantRoot.Prester,
                VariantRoot.Agrotera => VariantRoot.Manteia,
                VariantRoot.Pallas => VariantRoot.Alexikakos,
                _ => VariantRoot.Baskania // Stygian
            },
            LegendaryRegistry.FamilyFencing => root switch
            {
                VariantRoot.Zephyr => VariantRoot.Aiolos,
                VariantRoot.Phobos => VariantRoot.Ephodos,
                VariantRoot.Agrotera => VariantRoot.Ios,
                VariantRoot.Pallas => VariantRoot.Ophis,
                _ => VariantRoot.Kentron // Stygian
            },
            LegendaryRegistry.FamilyArchery => root switch
            {
                VariantRoot.Zephyr => VariantRoot.Belos,
                VariantRoot.Phobos => VariantRoot.Hekatos,
                VariantRoot.Agrotera => VariantRoot.Toxikon,
                VariantRoot.Pallas => VariantRoot.Skopos,
                _ => VariantRoot.Pede // Stygian
            },
            _ => root
        };
    }

    // Same idea for the retired shared armor roots (Polias/Cyclopean/Paean/Tritonian/Talarian):
    // on load, body armor remaps to its material's lane-equivalent root and a shield to the
    // shield set (framework §3 re-theme, per-material bijections from docs 10/11/12 §3).
    // Idempotent: anything else passes through unchanged.
    public static VariantRoot RemapArmorRoot(VariantRoot root, ArmorMaterialType material, bool isShield)
    {
        if (root is not (VariantRoot.Polias or VariantRoot.Cyclopean or VariantRoot.Paean
            or VariantRoot.Tritonian or VariantRoot.Talarian))
        {
            return root; // already per-material / Aegis / non-armor / None
        }

        if (isShield)
        {
            return root switch
            {
                VariantRoot.Cyclopean => VariantRoot.Amyntor,
                VariantRoot.Paean => VariantRoot.Pnoe,
                VariantRoot.Tritonian => VariantRoot.Herkos,
                VariantRoot.Talarian => VariantRoot.Probolos,
                _ => VariantRoot.Aegis // Polias never legitimately sat on a shield — fail safe
            };
        }

        return material switch
        {
            ArmorMaterialType.Ringmail => root switch
            {
                VariantRoot.Polias => VariantRoot.Hoplites,
                VariantRoot.Cyclopean => VariantRoot.Zoster,
                VariantRoot.Paean => VariantRoot.Alkimos,
                VariantRoot.Tritonian => VariantRoot.Taxis,
                _ => VariantRoot.Dromos // Talarian
            },
            ArmorMaterialType.Chainmail => root switch
            {
                VariantRoot.Polias => VariantRoot.Phylax,
                VariantRoot.Cyclopean => VariantRoot.Halysis,
                VariantRoot.Paean => VariantRoot.Phrourion,
                VariantRoot.Tritonian => VariantRoot.Egregoros,
                _ => VariantRoot.Teichos // Talarian
            },
            ArmorMaterialType.Plate => root switch
            {
                VariantRoot.Polias => VariantRoot.Adamas,
                VariantRoot.Cyclopean => VariantRoot.Kaminos,
                VariantRoot.Paean => VariantRoot.Akamatos,
                VariantRoot.Tritonian => VariantRoot.Kolossos,
                _ => VariantRoot.Panoplia // Talarian
            },
            ArmorMaterialType.Leather => root switch
            {
                VariantRoot.Polias => VariantRoot.Naias,
                VariantRoot.Cyclopean => VariantRoot.Dryas,
                VariantRoot.Paean => VariantRoot.Melissa,
                VariantRoot.Tritonian => VariantRoot.Panika,
                _ => VariantRoot.Oreias // Talarian
            },
            ArmorMaterialType.Studded => root switch
            {
                VariantRoot.Polias => VariantRoot.Arkas,
                VariantRoot.Cyclopean => VariantRoot.Batos,
                VariantRoot.Paean => VariantRoot.Elaphis,
                VariantRoot.Tritonian => VariantRoot.Skia,
                _ => VariantRoot.Kynegis // Talarian
            },
            ArmorMaterialType.Bone => root switch
            {
                VariantRoot.Polias => VariantRoot.Tymbos,
                VariantRoot.Cyclopean => VariantRoot.Katachthon,
                VariantRoot.Paean => VariantRoot.Makaria,
                VariantRoot.Tritonian => VariantRoot.Nekyia,
                _ => VariantRoot.Melinoe // Talarian
            },
            _ => root // exotic/wooden materials never carried variants — leave untouched
        };
    }
}
