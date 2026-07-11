using Server.Items;

namespace Server.Engines.Rarity;

public static partial class RarityEffects
{
    // ---- Runtime damage/speed anchors (framework §2/§7; D directive 2026-07-11) --------------

    // A variant weapon's combat + display min/max damage is derived at read time from its base's
    // §7 ratio and its rarity's D anchor — NOTHING is serialized, so a live rebalance retro-tunes
    // every dropped item. Plain (non-variant) weapons and subclassed/unmapped types return false and
    // keep their stock MinDamage/MaxDamage. The theme DamagePct bonus stays multiplicative on top
    // (applied later in BeginWeaponHit), so this only sets the base range.
    public static bool TryGetAnchorDamage(BaseWeapon weapon, out int min, out int max)
    {
        min = 0;
        max = 0;

        if (weapon is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return false;
        }

        if (!FamilyRegistry.TryGetWeaponBaseStats(weapon.GetType(), out var ratio, out _))
        {
            return false;
        }

        var (_, rarity) = ResolveRootRarity(variant, weapon.Rarity);
        RarityDamageAnchors.DamageRange(ratio, rarity, out min, out max);
        return true;
    }

    // A variant weapon's Speed stat, anchored so the live pre-AOS swing delay at the reference
    // stamina yields the base's §7 swing-seconds. Rarity-independent (the ladder fixes swing speed;
    // rarity owns damage), so it applies to any variant regardless of tier. Same non-serialized,
    // read-time mechanism as the damage anchor.
    public static bool TryGetAnchorSpeed(BaseWeapon weapon, out float speed)
    {
        speed = 0;

        if (weapon is not IVariantItem variant || variant.VariantRoot == VariantRoot.None && variant.LegendaryId == 0)
        {
            return false;
        }

        if (!FamilyRegistry.TryGetWeaponBaseStats(weapon.GetType(), out _, out var swingSeconds))
        {
            return false;
        }

        speed = RarityDamageAnchors.SpeedFromSwingSeconds(swingSeconds);
        return true;
    }
}
