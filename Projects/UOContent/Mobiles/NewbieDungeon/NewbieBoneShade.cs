using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieBoneShade : BaseCreature
{
    [Constructible]
    public NewbieBoneShade() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x03B2;
        BaseSoundID = 0x48D;

        SetStr(20, 30);
        SetDex(20, 30);
        SetInt(6, 10);

        SetHits(25, 35);

        SetDamage(2, 4);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 5, 10);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 10, 15);

        SetSkill(SkillName.MagicResist, 10.0, 15.0);
        SetSkill(SkillName.Tactics, 15.0, 20.0);
        SetSkill(SkillName.Wrestling, 25.0, 35.0);

        Fame = 50;
        Karma = -50;

        VirtualArmor = 8;

        SetSpeed(0.3, 0.6);
    }

    public override string CorpseName => "a frail skeletal corpse";
    public override string DefaultName => "a frail skeleton";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 0;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
