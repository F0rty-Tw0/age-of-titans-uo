using System;

namespace Server.Engines.Rarity;

// The per-rarity weapon damage anchors D (framework §2) and the runtime formulas that turn a base's
// ratio / swing-seconds (framework §7) into live min/max damage and a Speed stat. Runtime-only by
// design: nothing here is serialized, so retuning D or a ratio retro-adjusts every dropped variant
// weapon on the next read — combat and tooltip alike (RarityEffects.TryGetAnchorDamage/Speed).
public static class RarityDamageAnchors
{
    // D[rarity], indexed ItemRarity.Common..Legendary. USER DIRECTIVE 2026-07-11: the "softened
    // ~3x curve" (10/13/17/22/30) supersedes the original ~11x anchors (10/15.6/27.2/46.2/111).
    // Base ratios and the ladder structure (framework §7) are unchanged.
    private static readonly double[] D = { 10, 13, 17, 22, 30 };

    // Damage range = anchor +-10% (2026-07-11 directive): the average sits on the anchor, min ~0.9x,
    // max ~1.1x, each rounded to the nearest integer (ties away from zero).
    private const double MinSpread = 0.9;
    private const double MaxSpread = 1.1;

    // Pre-AOS (T2A) swing formula reference: BaseWeapon.GetDelay's else branch is
    // delay = 15000 / ((stam + 100) * speed). Anchoring at reference stamina 100 (matches
    // RarityEffects.Tooltips.ReferenceStamina) gives speed = 15000 / ((100 + 100) * swingSeconds).
    // The live in-fight delay still scales with the wielder's actual stamina around this anchor.
    private const double SwingDelayNumerator = 15000.0;
    private const int ReferenceStamina = 100;

    // ratio x D[rarity] — the base's on-spec average damage before the +-10% spread (framework §2).
    public static double Anchor(double ratio, ItemRarity rarity) => ratio * D[(int)rarity];

    public static void DamageRange(double ratio, ItemRarity rarity, out int min, out int max)
    {
        var anchor = Anchor(ratio, rarity);
        min = (int)Math.Round(anchor * MinSpread, MidpointRounding.AwayFromZero);
        max = (int)Math.Round(anchor * MaxSpread, MidpointRounding.AwayFromZero);
    }

    // The Speed stat whose live pre-AOS delay at reference stamina equals swingSeconds. The stat is
    // int-truncated inside GetDelay, so the live delay lands within one stat-int of the design value;
    // the tooltip (no truncation) shows the exact design seconds.
    public static float SpeedFromSwingSeconds(double swingSeconds) =>
        (float)(SwingDelayNumerator / ((ReferenceStamina + 100) * swingSeconds));
}
