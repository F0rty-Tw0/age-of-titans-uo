// Pantheon tamables — Cinderworks (dev-docs/dungeon-ladder.md). Donor: HellHound.
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class CinderHound : BaseCreature
{
    [Constructible]
    public CinderHound() : base(AIType.AI_Melee, FightMode.Aggressor)
    {
        Body = 98;
        Hue = 0x0654;
        BaseSoundID = 229;

        SetStr(140, 170);
        SetDex(90, 110);
        SetInt(50, 70);

        SetHits(190, 230);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 40;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 71.1;
    }

    public override string CorpseName => "a forge hound corpse";
    public override string DefaultName => "a forge hound";

    public override int Meat => 1;
    public override FoodType FavoriteFood => FoodType.Meat;
}
