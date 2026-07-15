// Pantheon tamables — Nemean Wildwood (dev-docs/dungeon-ladder.md). Donor: Horse (T2A body).
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class WyldCourser : BaseMount
{
    [Constructible]
    public WyldCourser() : base(0xE4, 0x3EA1, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x0486;
        BaseSoundID = 0xA8;

        SetStr(180, 210);
        SetDex(90, 110);
        SetInt(60, 90);

        SetHits(190, 220);

        SetDamage(10, 13);

        SetDamageType(ResistanceType.Physical, 80);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 4500;
        Karma = 4500;

        VirtualArmor = 45;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 77.1;
    }

    public override string CorpseName => "a moon-marked courser corpse";
    public override string DefaultName => "a moon-marked courser";
}
