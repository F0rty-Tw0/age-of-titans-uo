using System;

namespace Server.Engines.Rarity;

// Rarity tuning tables. Pure table so it is unit testable.
public static class RarityConfig
{
    public const ItemRarity MaxTier = ItemRarity.Legendary;

    public const int MaxBagLevel = 10;

    // Index = ItemRarity. Title-case tier tokens for labels/OPL (item names stay lowercase per
    // UO convention; only the rarity tier is capitalized, e.g. "a dagger [Rare]" / "rarity: Rare").
    private static readonly string[] _names =
    {
        "Common", "Uncommon", "Rare", "Epic", "Legendary"
    };

    // Index = ItemRarity. Cached name suffix appended inline to single-click names
    // (e.g. "a dagger [Rare]"). Common is empty = zero visible change. Cached = zero alloc.
    private static readonly string[] _suffixes =
    {
        "", " [Uncommon]", " [Rare]", " [Epic]", " [Legendary]"
    };

    // Index = ItemRarity. Placeholder hues, tune later. Same family as LootBag.cs.
    private static readonly int[] _hues =
    {
        0, 0x4F8, 0x4F1, 0x4FB, 0x501
    };

    // Index = bag level 0..10. Absolute rarity ceiling per bag level. L0/L1/L2 are
    // spec-fixed; the rest are placeholders ramping to Legendary, to tune.
    private static readonly ItemRarity[] _maxRarityByBagLevel =
    {
        ItemRarity.Uncommon, // 0
        ItemRarity.Uncommon, // 1
        ItemRarity.Rare,     // 2
        ItemRarity.Rare,     // 3
        ItemRarity.Epic,     // 4
        ItemRarity.Epic,     // 5
        ItemRarity.Epic,     // 6
        ItemRarity.Legendary, // 7
        ItemRarity.Legendary, // 8
        ItemRarity.Legendary, // 9
        ItemRarity.Legendary  // 10
    };

    // Index = ItemRarity. Placeholder multipliers, tune later.
    private static readonly int[] _salvageMultiplier =
    {
        1, 2, 4, 8, 16
    };

    // Index = CURRENT ItemRarity. Ichor cost to upgrade to the next tier; 0 = not upgradeable
    // (Common has no theme to carry up, Epic is the upgrade ceiling, Legendary never upgrades).
    // Placeholder costs, tune later.
    private static readonly int[] _upgradeCost =
    {
        0, 20, 80, 0, 0
    };

    public static string GetName(ItemRarity rarity) => _names[Math.Clamp((int)rarity, 0, _names.Length - 1)];

    public static string GetSuffix(ItemRarity rarity) => _suffixes[Math.Clamp((int)rarity, 0, _suffixes.Length - 1)];

    // Appends the tier suffix to a display name for OPL/tooltip name lines, e.g.
    // "Klytios" -> "Klytios [Legendary]". Null or Common passes through unchanged (so the
    // caller's name==null branch still fires and Common items look identical).
    public static string WithSuffix(string name, ItemRarity rarity) =>
        name != null && rarity != ItemRarity.Common ? $"{name}{GetSuffix(rarity)}" : name;

    public static int GetHue(ItemRarity rarity) => _hues[Math.Clamp((int)rarity, 0, _hues.Length - 1)];

    public static ItemRarity MaxRarityForBagLevel(int bagLevel) =>
        _maxRarityByBagLevel[Math.Clamp(bagLevel, 0, MaxBagLevel)];

    public static int SalvageMultiplier(ItemRarity rarity) =>
        _salvageMultiplier[Math.Clamp((int)rarity, 0, _salvageMultiplier.Length - 1)];

    public static int UpgradeCost(ItemRarity rarity) =>
        _upgradeCost[Math.Clamp((int)rarity, 0, _upgradeCost.Length - 1)];
}
