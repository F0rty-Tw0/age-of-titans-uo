using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Destard (dev-docs/classic-five-bestiary.md). Donor: FrenziedOstard (T2A body).
[SerializationGenerator(0, false)]
public partial class DrakonZostrich : BaseMount
{
    public override string DefaultName => "a drakon zostrich";

    [Constructible]
    public DrakonZostrich() : base(0xDA, 0x3EA4, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x501;
        BaseSoundID = 0x275;

        SetStr(210, 240);
        SetDex(90, 110);
        SetInt(61, 100);

        SetHits(210, 250);

        SetDamage(11, 14);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Fire, 25);

        SetResistance(ResistanceType.Physical, 38, 45);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 50.1, 58.0);
        SetSkill(SkillName.Tactics, 55.1, 65.0);
        SetSkill(SkillName.Wrestling, 55.1, 65.0);

        Fame = 3000;
        Karma = -3000;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 80.1;
    }

    public override string CorpseName => "a drakon zostrich's corpse";

    public override FoodType FavoriteFood => FoodType.Meat;
    public override int Meat => 19;
    public override int Hides => 20;
    public override int Scales => 5;
    public override ScaleType ScaleType => ScaleType.Green;
}
