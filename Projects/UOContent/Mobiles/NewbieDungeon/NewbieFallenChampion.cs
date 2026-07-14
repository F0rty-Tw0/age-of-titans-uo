using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieFallenChampion : NewbieElite
{
    [Constructible]
    public NewbieFallenChampion() : base(AIType.AI_Melee)
    {
        Name = "Anax";
        Title = "the Unyielded";

        Body = 57;
        Hue = 0x0021;
        BaseSoundID = 451;

        SetStr(220, 260);
        SetDex(70, 90);
        SetInt(30, 45);

        SetHits(200, 240);

        SetDamage(10, 16);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 70.0, 85.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a skeletal corpse";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
