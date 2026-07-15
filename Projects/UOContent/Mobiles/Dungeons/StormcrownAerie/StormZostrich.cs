// Pantheon tamables — Stormcrown Aerie (dev-docs/dungeon-ladder.md). Donor: FrenziedOstard (T2A body).
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class StormZostrich : BaseMount
{
    [Constructible]
    public StormZostrich() : base(0xDA, 0x3EA4, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x0481;
        BaseSoundID = 0x275;

        SetStr(240, 280);
        SetDex(110, 130);
        SetInt(90, 120);

        SetHits(280, 330);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 55, 63);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 50, 60);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 6500;
        Karma = 6500;

        VirtualArmor = 55;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 90.1;
    }

    public override string CorpseName => "a storm zostrich corpse";
    public override string DefaultName => "a storm zostrich";
}
