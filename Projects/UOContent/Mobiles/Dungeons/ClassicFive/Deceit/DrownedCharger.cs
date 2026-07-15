using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Deceit (dev-docs/classic-five-bestiary.md). Donor: Horse (T2A body).
[SerializationGenerator(0, false)]
public partial class DrownedCharger : BaseMount
{
    public override string DefaultName => "a drowned charger";

    [Constructible]
    public DrownedCharger() : base(0xCC, 0x3EA2, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x835;
        BaseSoundID = 0xA8;

        SetStr(110, 130);
        SetDex(46, 60);
        SetInt(46, 65);

        SetHits(110, 140);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 30.1, 40.0);
        SetSkill(SkillName.Tactics, 40.1, 50.0);
        SetSkill(SkillName.Wrestling, 45.1, 55.0);

        Fame = 1400;
        Karma = -1400;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 55.1;
    }

    public override string CorpseName => "a drowned charger's corpse";

    public override Poison PoisonImmune => Poison.Regular;
}
