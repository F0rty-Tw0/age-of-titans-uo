using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class NewbieGraveMiasma : BaseCreature
{
    [Constructible]
    public NewbieGraveMiasma() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0851;
        BaseSoundID = 0x482;

        SetStr(40, 55);
        SetDex(40, 55);
        SetInt(15, 25);

        SetHits(70, 90);
        SetMana(0);

        SetDamage(3, 6);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 12, 18);
        SetResistance(ResistanceType.Cold, 10, 18);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.Poisoning, 40.0, 50.0);
        SetSkill(SkillName.MagicResist, 20.0, 30.0);
        SetSkill(SkillName.Tactics, 30.0, 40.0);
        SetSkill(SkillName.Wrestling, 30.0, 40.0);

        Fame = 180;
        Karma = -180;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a miasmic corpse";
    public override string DefaultName => "a grave-touched ghoul";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;
    public override Poison HitPoison => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
