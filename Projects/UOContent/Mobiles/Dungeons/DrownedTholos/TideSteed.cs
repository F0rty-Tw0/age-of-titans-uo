// Pantheon tamables — Drowned Tholos (dev-docs/dungeon-ladder.md). Donor: Horse (T2A body).
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class TideSteed : BaseMount
{
    [Constructible]
    public TideSteed() : base(0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x0481;
        BaseSoundID = 0xA8;

        SetStr(70, 90);
        SetDex(60, 80);
        SetInt(15, 25);

        SetHits(95, 115);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 25);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 10, 15);

        SetSkill(SkillName.MagicResist, 20.0, 25.0);
        SetSkill(SkillName.Tactics, 30.0, 40.0);
        SetSkill(SkillName.Wrestling, 30.0, 40.0);

        Fame = 400;
        Karma = 300;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 47.1;
    }

    public override string CorpseName => "a tide-born steed corpse";
    public override string DefaultName => "a tide-born steed";
}
