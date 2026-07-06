namespace Server
{
    public static class ItemInfoConfiguration
    {
        // Pre-UOTD single-click detail labels (damage, speed, durability, armor rating).
        // Either flag enables the labels on all item types — cross-flag behavior is
        // intentional and pinned by ItemPacketTests.
        public static bool SingleClickDetails =>
            ServerConfiguration.GetSetting("itemInfo.singleClickItemDetails", false) ||
            ServerConfiguration.GetSetting("itemInfo.singleClickWeaponDetails", false);
    }
}
