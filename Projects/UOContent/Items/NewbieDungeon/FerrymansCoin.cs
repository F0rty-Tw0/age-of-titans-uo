using System;
using ModernUO.Serialization;

namespace Server.Items;

// Graduation keepsake handed once per character at level 4 (Newbie Dungeon, dev-docs/newbie-dungeon.md
// §5). Pure trinket: no stats, no trade value. LootType.Blessed only — Blessed and Newbied are
// mutually exclusive (Item.cs:80), so this is the sole "can't be lost" flag it carries.
[SerializationGenerator(0, false)]
public partial class FerrymansCoin : Item
{
    [SerializableField(0)]
    private string _ownerName;

    [SerializableField(1)]
    private DateTime _graduationDate;

    [Constructible]
    public FerrymansCoin() : base(0xEF0)
    {
        Hue = 0x0482;
        LootType = LootType.Blessed;
        Movable = true;
    }

    public FerrymansCoin(string ownerName) : this()
    {
        _ownerName = ownerName;
        _graduationDate = Core.Now;
    }

    public override double DefaultWeight => 0.1;

    public override string DefaultName => "the Ferryman's Coin";

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        list.Add(1060658, $"{"Carried by"}\t{_ownerName}");         // ~1_val~: ~2_val~
        list.Add(1060658, $"{"Graduated"}\t{_graduationDate:d}");   // ~1_val~: ~2_val~
    }
}
