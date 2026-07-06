namespace Server.Items;

public static class DoubleClickEquip
{
    public static bool TryEquip(Mobile from, Item item)
    {
        if (item.Parent == from || !from.Alive)
        {
            return false;
        }

        if (!item.IsChildOf(from.Backpack))
        {
            from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            return false;
        }

        return DisplaceConflicts(from, item) && from.EquipItem(item);
    }

    // Clears whatever occupies the item's layer (and, for weapons, the hand-conflict layer) so
    // EquipItem's own CheckEquip/CheckConflictingLayer pass instead of bouncing. Shared by TryEquip
    // (double-click) and PlayerMobile.EquipItem (paperdoll drag), since both need the same swap rules.
    public static bool DisplaceConflicts(Mobile from, Item item)
    {
        if (!Displace(from, item.Layer))
        {
            return false;
        }

        if (item is BaseWeapon)
        {
            if (item.Layer == Layer.TwoHanded && !Displace(from, Layer.OneHanded))
            {
                return false;
            }

            if (item.Layer == Layer.OneHanded && from.FindItemOnLayer(Layer.TwoHanded) is BaseWeapon &&
                !Displace(from, Layer.TwoHanded))
            {
                return false;
            }
        }

        return true;
    }

    private static bool Displace(Mobile from, Layer layer)
    {
        var existing = from.FindItemOnLayer(layer);

        if (existing == null)
        {
            return true;
        }

        return from.PlaceInBackpack(existing);
    }
}
