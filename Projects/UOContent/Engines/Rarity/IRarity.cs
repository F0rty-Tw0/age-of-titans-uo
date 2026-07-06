namespace Server.Engines.Rarity;

public interface IRarity
{
    ItemRarity Rarity { get; set; }
    ItemRarity MaxRarity { get; }
}
