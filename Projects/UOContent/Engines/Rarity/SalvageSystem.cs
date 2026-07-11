namespace Server.Engines.Rarity;

// Pure mechanics for the Pantheon altar's salvage/upgrade flows — no gump or targeting code
// here so the rules are unit-testable. Salvage: destroy an Uncommon..Epic drop-variant for
// ichor (RarityConfig.SalvageMultiplier). Upgrade: spend ichor (RarityConfig.UpgradeCost) to
// raise a themed item one tier, Epic cap; the item keeps its VariantRoot and ApplyVariant
// re-derives hue/name/weight. Legendaries and plain Commons participate in neither.
//
// The Can*/Try* split mirrors the altar's offering flow: Can* validates for the prompt/confirm
// stage, Try* re-validates at execution so a stale confirm gump (item moved, ichor spent,
// double-targeted) degrades to a clean no-op instead of a dupe or a free upgrade.
public static class SalvageSystem
{
    public static bool CanSalvage(Mobile from, Item item, out int yield, out string reason)
    {
        yield = 0;

        if (!TryResolveVariant(from, item, out var rarity, out reason))
        {
            return false;
        }

        if (rarity is < ItemRarity.Uncommon or > ItemRarity.Epic)
        {
            reason = "Only uncommon, rare, or epic items may be salvaged.";
            return false;
        }

        yield = RarityConfig.SalvageMultiplier(rarity);
        return true;
    }

    public static bool CanUpgrade(Mobile from, Item item, out ItemRarity next, out int cost, out string reason)
    {
        next = ItemRarity.Common;
        cost = 0;

        if (!TryResolveVariant(from, item, out var rarity, out reason))
        {
            return false;
        }

        if (rarity is not (ItemRarity.Uncommon or ItemRarity.Rare))
        {
            reason = rarity == ItemRarity.Epic
                ? "Epic is the highest tier the gods will forge."
                : "Only uncommon or rare items may be upgraded.";
            return false;
        }

        next = rarity + 1;

        // ApplyVariant clamps to MaxRarity silently — reject here so a capped item can never
        // eat the material and come back unchanged.
        if (next > ((IRarity)item).MaxRarity)
        {
            reason = "This item cannot hold a higher tier.";
            return false;
        }

        cost = RarityConfig.UpgradeCost(rarity);

        if ((from.Backpack?.GetAmount(typeof(PantheonIchor)) ?? 0) < cost)
        {
            reason = $"You need {cost} ichor to perform this upgrade.";
            return false;
        }

        return true;
    }

    public static bool TrySalvage(Mobile from, Item item)
    {
        if (!CanSalvage(from, item, out var yield, out var reason))
        {
            from.SendMessage(reason);
            return false;
        }

        item.Delete();

        var ichor = new PantheonIchor(yield);

        if (!from.AddToBackpack(ichor))
        {
            ichor.MoveToWorld(from.Location, from.Map);
        }

        from.SendMessage($"The altar consumes the offering and yields {yield} ichor.");
        return true;
    }

    public static bool TryUpgrade(Mobile from, Item item)
    {
        if (!CanUpgrade(from, item, out var next, out var cost, out var reason))
        {
            from.SendMessage(reason);
            return false;
        }

        // Atomic: consumes nothing unless the full cost is present (Container.ConsumeTotal).
        if (from.Backpack?.ConsumeTotal(typeof(PantheonIchor), cost) != true)
        {
            from.SendMessage($"You need {cost} ichor to perform this upgrade.");
            return false;
        }

        RarityEffects.ApplyVariant(item, ((IVariantItem)item).VariantRoot, next);
        from.SendMessage($"The gods reforge your item to {RarityConfig.GetName(next)}.");
        return true;
    }

    // Shared identity gate: a non-legendary themed equipment piece in the invoker's backpack.
    private static bool TryResolveVariant(Mobile from, Item item, out ItemRarity rarity, out string reason)
    {
        rarity = ItemRarity.Common;

        if (item is not IVariantItem variant || item is not IRarity r)
        {
            reason = "Only weapons, armor, clothing, or jewelry bearing a theme qualify.";
            return false;
        }

        if (variant.LegendaryId != 0)
        {
            reason = "The gods will not unmake a legendary relic.";
            return false;
        }

        if (variant.VariantRoot == VariantRoot.None)
        {
            reason = "That item bears no theme.";
            return false;
        }

        if (item.Deleted || !item.IsChildOf(from.Backpack))
        {
            reason = "The item must be in your backpack.";
            return false;
        }

        rarity = r.Rarity;
        reason = null;
        return true;
    }
}
