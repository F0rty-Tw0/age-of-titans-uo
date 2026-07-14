using ModernUO.Serialization;

namespace Server.Mobiles;

// L2 slow bruiser at the top of the band: the barrow's tank-shaped lesson — a fight you
// win by managing stamina and pace, not by trading blows blindly.
[SerializationGenerator(0, false)]
public partial class NewbieWight : BaseCreature
{
    [Constructible]
    public NewbieWight() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(60, 75);
        SetDex(25, 35);
        SetInt(10, 20);

        SetHits(90, 100);

        SetDamage(4, 7);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 18, 26);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 35.0, 45.0);
        SetSkill(SkillName.Wrestling, 35.0, 45.0);

        Fame = 300;
        Karma = -300;

        VirtualArmor = 24;

        ActiveSpeed = 0.4;
        PassiveSpeed = 0.8;
    }

    public override string CorpseName => "an unremembered corpse";
    public override string DefaultName => "an unremembered wight";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
