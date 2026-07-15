// Pantheon tamables — Drowned Tholos (dev-docs/dungeon-ladder.md). Donor: Bull.
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class TideBull : BaseCreature
{
    [Constructible]
    public TideBull() : base(AIType.AI_Animal, FightMode.Aggressor)
    {
        Body = Utility.RandomList(0xE8, 0xE9);
        Hue = 0x0847;
        BaseSoundID = 0x64;

        SetStr(90, 110);
        SetDex(60, 80);
        SetInt(50, 70);

        SetHits(130, 150);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Cold, 15, 20);
        SetResistance(ResistanceType.Poison, 15, 20);

        SetSkill(SkillName.MagicResist, 27.0, 33.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 700;
        Karma = 0;

        VirtualArmor = 30;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 59.1;
    }

    public override string CorpseName => "a sea-born bull corpse";
    public override string DefaultName => "a sea-born bull";

    public override int Meat => 10;
    public override int Hides => 15;
    public override FoodType FavoriteFood => FoodType.GrainsAndHay;
}
