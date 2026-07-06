using System;
using ModernUO.Serialization;

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
