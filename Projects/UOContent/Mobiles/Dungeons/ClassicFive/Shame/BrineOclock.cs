using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Shame (dev-docs/classic-five-bestiary.md). Donor: ForestOstard (T2A body).
[SerializationGenerator(0, false)]
public partial class BrineOclock : BaseMount
{
    public override string DefaultName => "a brine oclock";

    [Constructible]
    public BrineOclock() : base(0xDB, 0x3EA5, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x4F8;
        BaseSoundID = 0x270;

        SetStr(130, 155);
        SetDex(70, 90);
        SetInt(6, 15);

        SetHits(160, 190);
        SetMana(0);

        SetDamage(9, 12);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 25, 32);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 10, 18);

        SetSkill(SkillName.MagicResist, 35.1, 42.0);
        SetSkill(SkillName.Tactics, 40.1, 50.0);
        SetSkill(SkillName.Wrestling, 45.1, 55.0);

        Fame = 1600;
        Karma = 0;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 67.1;
    }

    public override string CorpseName => "a brine oclock's corpse";

    public override int Meat => 3;
    public override FoodType FavoriteFood => FoodType.FruitsAndVeggies | FoodType.GrainsAndHay | FoodType.Fish;
    public override PackInstinct PackInstinct => PackInstinct.Ostard;
}
