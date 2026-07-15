// Pantheon tamables — Cinderworks (dev-docs/dungeon-ladder.md). Donor: Horse (T2A body).
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class CinderSteed : BaseMount
{
    [Constructible]
    public CinderSteed() : base(0xC8, 0x3E9F, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x0654;
        BaseSoundID = 0xA8;

        SetStr(120, 150);
        SetDex(70, 90);
        SetInt(50, 70);

        SetHits(140, 170);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Fire, 70);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Poison, 15, 20);
        SetResistance(ResistanceType.Energy, 15, 20);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 35;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 63.1;
    }

    public override string CorpseName => "a cinder steed corpse";
    public override string DefaultName => "a cinder steed";
}
