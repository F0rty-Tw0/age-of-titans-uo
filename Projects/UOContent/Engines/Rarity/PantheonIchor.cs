using ModernUO.Serialization;

namespace Server.Engines.Rarity;

// The single salvage material: salvaging an Uncommon/Rare/Epic variant item at the Pantheon
// altar yields RarityConfig.SalvageMultiplier of these; upgrading a themed item consumes
// RarityConfig.UpgradeCost of them. Plain stackable, no state beyond the amount.
[SerializationGenerator(0, false)]
public partial class PantheonIchor : Item
{
    [Constructible]
    public PantheonIchor(int amount = 1) : base(0x1EA7) // PLACEHOLDER item id, tune later
    {
        Stackable = true;
        Amount = amount;
        Name = "ichor";
        Hue = 0x501; // PLACEHOLDER hue (legendary hue family), tune later
    }

    public override double DefaultWeight => 0.1;
}
