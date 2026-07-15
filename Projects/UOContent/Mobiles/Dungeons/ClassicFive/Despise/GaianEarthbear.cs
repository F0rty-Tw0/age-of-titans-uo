using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Despise (dev-docs/classic-five-bestiary.md). Donor: GrizzlyBear.
[SerializationGenerator(0, false)]
public partial class GaianEarthbear : BaseCreature
{
    [Constructible]
    public GaianEarthbear() : base(AIType.AI_Animal, FightMode.Aggressor)
    {
        Body = 212;
        Hue = 0x972;
        BaseSoundID = 0xA3;

        SetStr(150, 175);
        SetDex(81, 105);
        SetInt(16, 40);

        SetHits(110, 130);
        SetMana(0);

        SetDamage(8, 12);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 10, 15);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 20.1, 25.0);
        SetSkill(SkillName.Tactics, 50.1, 60.0);
        SetSkill(SkillName.Wrestling, 50.1, 60.0);

        Fame = 1200;
        Karma = 0;

        VirtualArmor = 28;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 55.1;
    }

    public override string CorpseName => "an earthborn bear's corpse";
    public override string DefaultName => "an earthborn bear";

    public override int Meat => 2;
    public override int Hides => 16;
    public override FoodType FavoriteFood => FoodType.Fish | FoodType.FruitsAndVeggies | FoodType.Meat;
    public override PackInstinct PackInstinct => PackInstinct.Bear;
}
