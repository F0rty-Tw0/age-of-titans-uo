using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieRestlessArcher : BaseCreature
{
    [Constructible]
    public NewbieRestlessArcher() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0385;
        BaseSoundID = 0x48D;

        SetStr(50, 65);
        SetDex(80, 95);
        SetInt(15, 25);

        SetHits(66, 85);

        SetDamage(3, 6);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 15, 20);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 200;
        Karma = -200;

        VirtualArmor = 22;
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "a restless dead";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
