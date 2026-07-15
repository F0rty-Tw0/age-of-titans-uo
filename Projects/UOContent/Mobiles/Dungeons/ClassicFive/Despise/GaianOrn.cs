using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Despise (dev-docs/classic-five-bestiary.md). Donor: DesertOstard (T2A body).
[SerializationGenerator(0, false)]
public partial class GaianOrn : BaseMount
{
    public override string DefaultName => "a gaian orn";

    [Constructible]
    public GaianOrn() : base(0xD2, 0x3EA3, AIType.AI_Animal, FightMode.Aggressor)
    {
        Hue = 0x972;
        BaseSoundID = 0x270;

        SetStr(96, 115);
        SetDex(56, 75);
        SetInt(26, 45);

        SetHits(85, 105);
        SetMana(0);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 25, 32);
        SetResistance(ResistanceType.Fire, 10, 18);
        SetResistance(ResistanceType.Cold, 10, 18);
        SetResistance(ResistanceType.Poison, 10, 18);
        SetResistance(ResistanceType.Energy, 10, 18);

        SetSkill(SkillName.MagicResist, 20.1, 25.0);
        SetSkill(SkillName.Tactics, 40.1, 50.0);
        SetSkill(SkillName.Wrestling, 40.1, 50.0);

        Fame = 900;
        Karma = 0;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 45.1;
    }

    public override string CorpseName => "a gaian orn's corpse";

    public override FoodType FavoriteFood => FoodType.Meat;
}
