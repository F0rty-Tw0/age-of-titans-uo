using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Deceit (dev-docs/classic-five-bestiary.md). Donor: DireWolf.
[SerializationGenerator(0, false)]
public partial class DrownedHound : BaseCreature
{
    [Constructible]
    public DrownedHound() : base(AIType.AI_Melee, FightMode.Aggressor)
    {
        Body = 23;
        Hue = 0x841;
        BaseSoundID = 0xE5;

        SetStr(150, 175);
        SetDex(81, 105);
        SetInt(36, 60);

        SetHits(150, 180);
        SetMana(0);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 10, 18);
        SetResistance(ResistanceType.Poison, 10, 18);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 30.1, 40.0);
        SetSkill(SkillName.Tactics, 60.1, 70.0);
        SetSkill(SkillName.Wrestling, 60.1, 70.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 30;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 65.1;
    }

    public override string CorpseName => "a barrow hound's corpse";
    public override string DefaultName => "a barrow hound";

    public override int Meat => 1;
    public override FoodType FavoriteFood => FoodType.Meat;
    public override PackInstinct PackInstinct => PackInstinct.Canine;
}
