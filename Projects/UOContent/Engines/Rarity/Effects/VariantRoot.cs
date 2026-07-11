using System;

namespace Server.Engines.Rarity;

// The drop-variant "root" — the shared enchantment name across a family (framework §3).
// Roots are globally unique across weapon/armor/shield/jewelry/clothing categories.
public enum VariantRoot : byte
{
    None = 0,

    // Weapons (framework §3)
    Zephyr,    // Hermes  — speed
    Phobos,    // Ares    — damage
    Agrotera,  // Artemis — mark
    Pallas,    // Athena  — defense
    Stygian,   // Hades   — drain

    // Armor / helmets — legacy shared armor roots (re-theme 2026-07-07): retired from new
    // drops, decode-only for old saves, remapped to lane-specific roots on load. Aegis (shield
    // bulwark) remains active — it is not part of the retired set. Do not remove or reorder;
    // this is a byte-backed serialized enum.
    Polias,    // Athena       — bulwark
    Cyclopean, // Hephaestus   — forge
    Paean,     // Apollo       — mending
    Tritonian, // Poseidon     — ward
    Talarian,  // Hermes       — stride
    Aegis,     // Athena       — shield-only bulwark root

    // Jewelry
    Olympian,  // Zeus     — might
    Hecatean,  // Hecate   — sorcery
    Tychean,   // Tyche    — fortune
    Nyxian,    // Nyx      — night
    Demetrian, // Demeter  — harvest

    // Clothing
    Laurel,    // Nike       — victory
    Charis,    // Aphrodite  — charm
    Maenad,    // Dionysus   — frenzy
    Hestian,   // Hestia     — hearth
    Arachne,   // Arachne    — web

    // Swords (re-theme 2026-07-07 — framework §3)
    Phoibos, Areia, Menis, Aristeia, Haima,
    // Maces
    Ennosigaios, Kataigis, Rhaistes, Eryma, Kamatos,
    // Polearms
    Theristes, Sarisa, Phalanx, Horme, Zophos,
    // Staves
    Empousa, Prester, Alexikakos, Manteia, Baskania,
    // Fencing
    Ios, Ephodos, Aiolos, Kentron, Ophis,
    // Archery
    Hekatos, Belos, Pede, Toxikon, Skopos,
    // Leather armor
    Naias, Dryas, Oreias, Melissa, Panika,
    // Studded armor
    Kynegis, Batos, Arkas, Elaphis, Skia,
    // Bone armor
    Melinoe, Makaria, Tymbos, Nekyia, Katachthon,
    // Ring armor
    Hoplites, Taxis, Dromos, Zoster, Alkimos,
    // Chain armor
    Phylax, Egregoros, Teichos, Halysis, Phrourion,
    // Plate armor
    Adamas, Kaminos, Kolossos, Panoplia, Akamatos,
    // Shields (Aegis already exists above)
    Amyntor, Probolos, Herkos, Pnoe
}

// Root → display name / body hue / stack-group helpers. Array-backed facade over the family
// registry (the single source of truth — names/hues/stack groups live per-lane in Families/*.cs).
// Populated once at type init; zero allocation per lookup.
public static class VariantRootInfo
{
    // None + 21 original roots + 64 re-theme roots (2026-07-07).
    public const int RootCount = 86;

    // Index = (int)VariantRoot. Backing arrays are the aggregated registry arrays.
    private static readonly string[] _displayNames = FamilyRegistry.RootDisplayNames;
    private static readonly string[] _mythTags = FamilyRegistry.RootMythTags;
    private static readonly int[] _baseHues = FamilyRegistry.RootBaseHues;
    private static readonly byte[] _stackGroups = FamilyRegistry.RootStackGroups;

    // 1 (None) + 8 named groups + however many singleton roots the registry assigned. Derived from
    // the built table so a future root addition can't silently leave this undersized — WornEffectState
    // sizes its per-hit stackalloc spans off this value.
    public static readonly int StackGroupCount = FamilyRegistry.StackGroupCount;

    static VariantRootInfo()
    {
        if (_displayNames.Length != RootCount || _mythTags.Length != RootCount ||
            _baseHues.Length != RootCount || _stackGroups.Length != RootCount)
        {
            throw new InvalidOperationException(
                $"VariantRootInfo table length mismatch: displayNames={_displayNames.Length}, " +
                $"mythTags={_mythTags.Length}, baseHues={_baseHues.Length}, " +
                $"stackGroups={_stackGroups.Length}, expected={RootCount}"
            );
        }
    }

    public static string GetDisplayName(VariantRoot root) => _displayNames[(int)root];

    // Short myth owner/concept for the root (framework §3), e.g. "Ares", "the naiads". Used as the
    // tooltip effects-line prefix. Never empty for a claimed root (registry boot validation).
    public static string GetMythTag(VariantRoot root) => _mythTags[(int)root];

    // Body hue for a variant. Common (or None) is unhued; higher rarity = more saturated shade.
    public static int GetBodyHue(VariantRoot root, ItemRarity rarity)
    {
        if (root == VariantRoot.None || rarity == ItemRarity.Common)
        {
            return 0;
        }

        return _baseHues[(int)root] + (int)rarity - 1;
    }

    public static byte GetStackGroup(VariantRoot root) => _stackGroups[(int)root];
}
