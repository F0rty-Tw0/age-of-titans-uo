using System;
using ModernUO.Serialization;
using Server.Engines.Rarity;

namespace Server.Items;

[SerializationGenerator(0)]
public partial class LootBag : BaseContainer
{
    // Index = level 0..10. Placeholder hues, tune later.
    private static readonly int[] _hues =
    {
        0, 0x2C, 0x2C, 0x59, 0x59, 0x4F2, 0x4F2, 0x9C4, 0x9C4, 0x501, 0x501
    };

    [SerializableField(0)]
    private int _level;

    [Constructible]
    public LootBag(int level = 0) : base(0xE76)
    {
        _level = Math.Clamp(level, 0, 10);
        Name = "a loot bag";
        Hue = _hues[_level];
    }

    public override double DefaultWeight => 2.0;

    // One-shot reward: no container gump ever shows. Double-clicking dumps every item into
    // the finder's backpack (falling to the ground only if the pack is full) and the bag
    // vanishes. Overriding OnDoubleClick (not Open) guarantees interception at the true
    // double-click entry point, before any DisplayTo gump is sent.
    public override void OnDoubleClick(Mobile from)
    {
        if (from.AccessLevel <= AccessLevel.Player && !from.InRange(GetWorldLocation(), 2))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
            return;
        }

        var items = Items;

        // Iterate backwards: AddToBackpack reparents each item, mutating this list.
        for (var i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            // Rarity variants/legendaries set a custom Name; fall back to the base cliloc for anything plain.
            var name = item.Name ?? Localization.GetText(item.LabelNumber);

            if (item is IRarity rarityItem)
            {
                name = RarityConfig.WithSuffix(name, rarityItem.Rarity);
            }

            from.AddToBackpack(item);
            from.SendMessage($"You received: {name}");

            // World broadcast for top-tier finds happens here — at claim time, when the finder
            // actually sees the item — never at mob-death roll time. Gates on the min tier itself.
            RaritySystem.Announce(from, item);
        }

        Delete();
    }

    public override void OnSingleClick(Mobile from)
    {
        base.OnSingleClick(from);
        LabelTo(from, $"[level {_level}]"); // T2A clients single-click
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);
        list.Add(1060658, $"{"level"}\t{_level}"); // ~1_val~: ~2_val~ — literal must be a hole
    }
}
