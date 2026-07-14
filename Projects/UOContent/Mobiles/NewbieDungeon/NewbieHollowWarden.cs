using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieHollowWarden : NewbieElite
{
    [Constructible]
    public NewbieHollowWarden() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(60, 80);
        SetDex(40, 55);
        SetInt(150, 180);

        SetHits(170, 200);

        SetDamage(7, 12);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 70.0, 80.0);
        SetSkill(SkillName.Magery, 70.0, 80.0);
        SetSkill(SkillName.MagicResist, 65.0, 80.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "the Warden of the Gate";

    public override bool ClickTitle => false;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Regular;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
