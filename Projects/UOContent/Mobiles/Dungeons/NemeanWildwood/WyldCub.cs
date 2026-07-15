// Pantheon tamables — Nemean Wildwood (dev-docs/dungeon-ladder.md). Donor: Panther.
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class WyldCub : BaseCreature
{
    [Constructible]
    public WyldCub() : base(AIType.AI_Animal, FightMode.Aggressor)
    {
        Body = 0xD6;
        Hue = 0x0501;
        BaseSoundID = 0x462;

        SetStr(160, 190);
        SetDex(120, 140);
        SetInt(50, 70);

        SetHits(260, 310);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 52);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 5000;
        Karma = 0;

        VirtualArmor = 45;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 82.1;
    }

    public override string CorpseName => "a nemean cub corpse";
    public override string DefaultName => "a nemean cub";

    public override int Meat => 1;
    public override int Hides => 10;
    public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
}
