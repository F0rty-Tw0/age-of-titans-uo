using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L2. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class GroveViper : BaseCreature
{
    [Constructible]
    public GroveViper() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0851;
        BaseSoundID = 0xDB;

        SetStr(72, 92);
        SetDex(62, 82);
        SetInt(22, 35);

        SetHits(80, 95);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 25);
        SetResistance(ResistanceType.Fire, 5, 10);
        SetResistance(ResistanceType.Cold, 5, 10);
        SetResistance(ResistanceType.Poison, 10, 15);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 550;
        Karma = -550;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a bracken viper corpse";
    public override string DefaultName => "a bracken viper";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override Poison HitPoison => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
