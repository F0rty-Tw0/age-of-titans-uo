using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Shame (dev-docs/classic-five-bestiary.md). Donor: Panther.
[SerializationGenerator(0, false)]
public partial class BrineLynx : BaseCreature
{
    [Constructible]
    public BrineLynx() : base(AIType.AI_Animal, FightMode.Aggressor)
    {
        Body = 0xD6;
        Hue = 0x480;
        BaseSoundID = 0x462;

        SetStr(180, 210);
        SetDex(120, 145);
        SetInt(26, 50);

        SetHits(220, 260);
        SetMana(0);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 15, 22);

        SetSkill(SkillName.MagicResist, 45.1, 50.0);
        SetSkill(SkillName.Tactics, 70.1, 80.0);
        SetSkill(SkillName.Wrestling, 70.1, 80.0);

        Fame = 3200;
        Karma = 0;

        VirtualArmor = 38;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 75.1;
    }

    public override string CorpseName => "a brine lynx's corpse";
    public override string DefaultName => "a brine lynx";

    public override int Meat => 1;
    public override int Hides => 10;
    public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
    public override PackInstinct PackInstinct => PackInstinct.Feline;
}
