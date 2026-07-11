namespace Server.Engines.Rarity;

// Display + announce helpers shared by the rarity-bearing equipment bases.
public static class RaritySystem
{
    public static ItemRarity Clamp(ItemRarity value, ItemRarity max) =>
        value < ItemRarity.Common ? ItemRarity.Common : value > max ? max : value;

    public static void AddRarityProperty(IPropertyList list, ItemRarity rarity)
    {
        if (rarity == ItemRarity.Common)
        {
            return;
        }

        list.Add(1060658, $"{"rarity"}\t{RarityConfig.GetName(rarity)}"); // ~1_val~: ~2_val~ — literal must be a hole
    }

    // World broadcast for a top-tier find. Called at claim time — when a loot bag is opened
    // (LootBag.OnDoubleClick) — never at roll time, so an unlooted bag can't spoil its contents.
    public static void Announce(Mobile finder, Item item)
    {
        var minTier = ServerConfiguration.GetSetting("rarity.announceMinTier", ItemRarity.Legendary);

        if (item is not IRarity r || r.Rarity < minTier)
        {
            return;
        }

        var itemName = item.Name ?? item.ItemData.Name;
        World.Broadcast(RarityConfig.GetHue(r.Rarity), false, $"{finder.Name} has found {itemName}!");
    }
}
