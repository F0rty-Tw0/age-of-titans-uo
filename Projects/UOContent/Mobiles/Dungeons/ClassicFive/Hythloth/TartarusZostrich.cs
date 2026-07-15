using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Hythloth (dev-docs/classic-five-bestiary.md). Donor: FrenziedOstard (T2A body).
[SerializationGenerator(0, false)]
public partial class TartarusZostrich : BaseMount
{
    public override string DefaultName => "a tartarus zostrich";

    [Constructible]
    public TartarusZostrich() : base(0xDA, 0x3EA4, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x21;
        BaseSoundID = 0x275;

        SetStr(160, 185);
        SetDex(101, 115);
        SetInt(46, 65);

        SetHits(240, 290);

        SetDamage(12, 15);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Fire, 75);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Poison, 30, 40);

        SetSkill(SkillName.MagicResist, 55.1, 62.0);
        SetSkill(SkillName.Tactics, 50.0);
        SetSkill(SkillName.Wrestling, 55.1, 65.0);

        Fame = 3600;
        Karma = -3600;

        Tamable = true;
        ControlSlots = 2;
        MinTameSkill = 85.1;
    }

    public override string CorpseName => "a tartarus zostrich's corpse";

    public override Poison PoisonImmune => Poison.Regular;
}
