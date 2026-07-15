// Pantheon tamables — Stygian Deep (dev-docs/dungeon-ladder.md). Donor: Horse (T2A body).
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class StygianNightmare : BaseMount
{
    [Constructible]
    public StygianNightmare() : base(0xE4, 0x3EA1, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x0455;
        BaseSoundID = 0xA8;

        SetStr(280, 320);
        SetDex(90, 110);
        SetInt(70, 100);

        SetHits(320, 380);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Fire, 30);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 55, 63);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 58;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 95.1;
    }

    public override string CorpseName => "a stygian nightmare corpse";
    public override string DefaultName => "a stygian nightmare";
}
