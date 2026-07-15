using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Hythloth (dev-docs/classic-five-bestiary.md). Donor: Panther.
[SerializationGenerator(0, false)]
public partial class TartarusHellcat : BaseCreature
{
    [Constructible]
    public TartarusHellcat() : base(AIType.AI_Melee, FightMode.Aggressor)
    {
        Body = 0xD6;
        Hue = 0x21;
        BaseSoundID = 0x462;

        SetStr(230, 265);
        SetDex(140, 165);
        SetInt(26, 50);

        SetHits(350, 400);
        SetMana(0);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Fire, 40);

        SetResistance(ResistanceType.Physical, 45, 52);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 25, 35);

        SetSkill(SkillName.MagicResist, 58.1, 65.0);
        SetSkill(SkillName.Tactics, 85.1, 95.0);
        SetSkill(SkillName.Wrestling, 85.1, 95.0);

        Fame = 5000;
        Karma = -5000;

        VirtualArmor = 52;

        Tamable = true;
        ControlSlots = 2;
        MinTameSkill = 92.1;
    }

    public override string CorpseName => "a tartarus hellcat's corpse";
    public override string DefaultName => "a tartarus hellcat";

    public override int Meat => 1;
    public override int Hides => 10;
    public override FoodType FavoriteFood => FoodType.Meat;
    public override PackInstinct PackInstinct => PackInstinct.Feline;
}
