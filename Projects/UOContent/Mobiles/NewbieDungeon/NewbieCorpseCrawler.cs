using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieCorpseCrawler : BaseCreature
{
    [Constructible]
    public NewbieCorpseCrawler() : base(AIType.AI_Melee)
    {
        Body = 3;
        BaseSoundID = 471;

        SetStr(25, 35);
        SetDex(15, 25);
        SetInt(10, 20);

        SetHits(30, 45);

        SetDamage(2, 5);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 8, 12);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 5, 10);

        SetSkill(SkillName.MagicResist, 10.0, 20.0);
        SetSkill(SkillName.Tactics, 15.0, 25.0);
        SetSkill(SkillName.Wrestling, 15.0, 25.0);

        Fame = 60;
        Karma = -60;

        VirtualArmor = 10;

        SetSpeed(0.3, 0.6);
    }

    public override string CorpseName => "a rotting corpse";
    public override string DefaultName => "a shambling corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 0;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
