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

// Root → display name / body hue / stack-group helpers. Pure static tables, zero allocation on
// lookup (BuildStackGroups runs once at type init).
public static class VariantRootInfo
{
    // None + 21 original roots + 64 re-theme roots (2026-07-07).
    public const int RootCount = 86;

    // Index = (int)VariantRoot. Lowercase single-click prefix (framework §6).
    private static readonly string[] _displayNames =
    {
        "",
        "zephyr", "phobos", "agrotera", "pallas", "stygian",
        "polias", "cyclopean", "paean", "tritonian", "talarian", "aegis",
        "olympian", "hecatean", "tychean", "nyxian", "demetrian",
        "laurel", "charis", "maenad", "hestian", "arachne",

        // Swords
        "phoibos", "areia", "menis", "aristeia", "haima",
        // Maces
        "ennosigaios", "kataigis", "rhaistes", "eryma", "kamatos",
        // Polearms
        "theristes", "sarisa", "phalanx", "horme", "zophos",
        // Staves
        "empousa", "prester", "alexikakos", "manteia", "baskania",
        // Fencing
        "ios", "ephodos", "aiolos", "kentron", "ophis",
        // Archery
        "hekatos", "belos", "pede", "toxikon", "skopos",
        // Leather armor
        "naias", "dryas", "oreias", "melissa", "panika",
        // Studded armor
        "kynegis", "batos", "arkas", "elaphis", "skia",
        // Bone armor
        "melinoe", "makaria", "tymbos", "nekyia", "katachthon",
        // Ring armor
        "hoplites", "taxis", "dromos", "zoster", "alkimos",
        // Chain armor
        "phylax", "egregoros", "teichos", "halysis", "phrourion",
        // Plate armor
        "adamas", "kaminos", "kolossos", "panoplia", "akamatos",
        // Shields
        "amyntor", "probolos", "herkos", "pnoe"
    };

    // Index = (int)VariantRoot. Placeholder base body hues (framework §3), tune in-client.
    // Base = the run's Uncommon (2nd) shade; the rarity ramp adds one shade per tier
    // (Uncommon..Legendary => base+0..base+3). Common is unhued. Vague runs marked below.
    private static readonly int[] _baseHues =
    {
        0,
        89, 34, 64, 1151, 1106,   // Zephyr, Phobos, Agrotera, Pallas, Stygian
        1151, 44, 49, 99, 89, 1151, // Polias, Cyclopean, Paean, Tritonian, Talarian, Aegis
        2213, 896, 66, 1109, 51,  // Olympian, Hecatean, Tychean(tune), Nyxian(tune), Demetrian(tune)
        66, 25, 15, 43, 907,       // Laurel(tune), Charis(tune), Maenad(tune), Hestian(tune), Arachne(tune)

        // placeholders — in-client hue pass pending (re-theme 2026-07-07)
        34, 36, 38, 40, 42,            // Phoibos, Areia, Menis, Aristeia, Haima (swords — blood run)
        44, 46, 48, 50, 52,            // Ennosigaios, Kataigis, Rhaistes, Eryma, Kamatos (maces — ember run)
        64, 66, 68, 70, 72,            // Theristes, Sarisa, Phalanx, Horme, Zophos (polearms — forest run)
        896, 898, 900, 902, 904,       // Empousa, Prester, Alexikakos, Manteia, Baskania (staves — violet run)
        99, 101, 103, 105, 107,        // Ios, Ephodos, Aiolos, Kentron, Ophis (fencing — sea run)
        89, 91, 93, 95, 97,            // Hekatos, Belos, Pede, Toxikon, Skopos (archery — sky run)
        65, 67, 69, 71, 73,            // Naias, Dryas, Oreias, Melissa, Panika (leather — forest run)
        1106, 1108, 1110, 1112, 1114,  // Kynegis, Batos, Arkas, Elaphis, Skia (studded — shadow run)
        1109, 1111, 1113, 1115, 1117,  // Melinoe, Makaria, Tymbos, Nekyia, Katachthon (bone — near-black run)
        49, 51, 53, 55, 57,            // Hoplites, Taxis, Dromos, Zoster, Alkimos (ring — gold run)
        2213, 2215, 2217, 2219, 2221,  // Phylax, Egregoros, Teichos, Halysis, Phrourion (chain — storm-gold run)
        66, 68, 70, 72, 74,            // Adamas, Kaminos, Kolossos, Panoplia, Akamatos (plate — gold-green run)
        1151, 1153, 1155, 1157         // Amyntor, Probolos, Herkos, Pnoe (shields — silver run)
    };

    // Stack-group ids (framework §9.4 stacking): roots in the same non-zero group share ONE
    // stacking pool across a worn suit; None and every ungrouped root get their own unique
    // singleton id so they never accidentally pool with an unrelated root.
    private const byte GroupNone = 0;
    private const byte GroupBulwark = 1;
    private const byte GroupForge = 2;
    private const byte GroupMending = 3;
    private const byte GroupWard = 4;
    private const byte GroupStride = 5;
    private const byte GroupSorcery = 6;
    private const byte GroupNight = 7;
    private const byte GroupDurability = 8;
    private const byte FirstSingletonGroup = 9;

    // 1 (None) + 8 named groups + 40 singleton roots (framework re-theme 2026-07-07).
    public const int StackGroupCount = 49;

    private static readonly byte[] _stackGroups = BuildStackGroups();

    static VariantRootInfo()
    {
        if (_displayNames.Length != RootCount || _baseHues.Length != RootCount || _stackGroups.Length != RootCount)
        {
            throw new InvalidOperationException(
                $"VariantRootInfo table length mismatch: displayNames={_displayNames.Length}, " +
                $"baseHues={_baseHues.Length}, stackGroups={_stackGroups.Length}, expected={RootCount}"
            );
        }
    }

    public static string GetDisplayName(VariantRoot root) => _displayNames[(int)root];

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

    private static byte[] BuildStackGroups()
    {
        var groups = new byte[RootCount];

        SetGroup(
            groups, GroupBulwark,
            VariantRoot.Polias, VariantRoot.Aegis, VariantRoot.Hoplites, VariantRoot.Phylax, VariantRoot.Adamas,
            VariantRoot.Arkas, VariantRoot.Tymbos, VariantRoot.Panoplia, VariantRoot.Alkimos, VariantRoot.Kolossos,
            VariantRoot.Taxis, VariantRoot.Teichos, VariantRoot.Probolos
        );
        SetGroup(
            groups, GroupForge,
            VariantRoot.Cyclopean, VariantRoot.Dryas, VariantRoot.Batos, VariantRoot.Katachthon, VariantRoot.Kaminos,
            VariantRoot.Amyntor
        );
        SetGroup(
            groups, GroupMending,
            VariantRoot.Paean, VariantRoot.Naias, VariantRoot.Melissa, VariantRoot.Makaria, VariantRoot.Akamatos,
            VariantRoot.Pnoe
        );
        SetGroup(
            groups, GroupWard,
            VariantRoot.Tritonian, VariantRoot.Alexikakos, VariantRoot.Nekyia, VariantRoot.Phrourion,
            VariantRoot.Herkos, VariantRoot.Egregoros
        );
        SetGroup(
            groups, GroupStride,
            VariantRoot.Talarian, VariantRoot.Ophis, VariantRoot.Oreias, VariantRoot.Elaphis, VariantRoot.Kynegis,
            VariantRoot.Dromos, VariantRoot.Panika, VariantRoot.Melinoe
        );
        SetGroup(groups, GroupSorcery, VariantRoot.Hecatean, VariantRoot.Manteia);
        SetGroup(groups, GroupNight, VariantRoot.Nyxian, VariantRoot.Skia);
        SetGroup(groups, GroupDurability, VariantRoot.Zoster, VariantRoot.Halysis);

        // Every root not claimed by a named group above (None included, pinned at group 0)
        // gets its own unique singleton id, in enum declaration order.
        byte nextSingleton = FirstSingletonGroup;
        for (var i = 1; i < groups.Length; i++)
        {
            if (groups[i] == GroupNone)
            {
                groups[i] = nextSingleton++;
            }
        }

        return groups;
    }

    private static void SetGroup(byte[] groups, byte group, params VariantRoot[] roots)
    {
        foreach (var root in roots)
        {
            groups[(int)root] = group;
        }
    }
}
