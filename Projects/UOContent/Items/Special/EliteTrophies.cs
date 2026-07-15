using ModernUO.Serialization;

namespace Server.Items;

// Signature deco trophies dropped only by the five Stygian/Stormcrown apex bosses
// (EliteDecoDrops.cs pool is replaced entirely — see DungeonElite.CreateDecoDrop overrides).

[SerializationGenerator(0)]
public partial class BidentOfHades : Item
{
    [Constructible]
    public BidentOfHades() : base(0xE87)
    {
        Name = "the Bident of Hades";
        Hue = 0x497; // dark/shadow
    }
}

[SerializationGenerator(0)]
public partial class CharonsFerryLantern : Item
{
    [Constructible]
    public CharonsFerryLantern() : base(0xA25)
    {
        Name = "Charon's Ferry Lantern";
        Hue = 0x4001; // pale ghost
    }
}

[SerializationGenerator(0)]
public partial class StatueOfCerberus : Item
{
    [Constructible]
    public StatueOfCerberus() : base(0x139A)
    {
        Name = "Statue of Cerberus";
        Hue = 0x1; // black
    }
}

[SerializationGenerator(0)]
public partial class ScalesOfTheJudgeKing : Item
{
    [Constructible]
    public ScalesOfTheJudgeKing() : base(0x1852)
    {
        Name = "Scales of the Judge-King";
        Hue = 0x8A5; // gold
    }
}

[SerializationGenerator(0)]
public partial class TyphonsStormStandard : Item
{
    [Constructible]
    public TyphonsStormStandard() : base(0x428)
    {
        Name = "Typhon's Storm-Standard";
        Hue = 0x482; // storm-blue
    }
}
