// Pantheon tamables — Stormcrown Aerie (dev-docs/dungeon-ladder.md). Donor: Drake.
using ModernUO.Serialization;

namespace Server.Mobiles;

[SerializationGenerator(0, false)]
public partial class StormDrakeling : BaseCreature
{
    [Constructible]
    public StormDrakeling() : base(AIType.AI_Melee, FightMode.Aggressor)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x0480;
        BaseSoundID = 362;

        SetStr(220, 260);
        SetDex(140, 160);
        SetInt(90, 120);

        SetHits(400, 460);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 7500;
        Karma = 0;

        VirtualArmor = 55;

        Tamable = true;
        ControlSlots = 1;
        MinTameSkill = 96.1;
    }

    public override string CorpseName => "a storm drakeling corpse";
    public override string DefaultName => "a storm drakeling";

    public override int Meat => 10;
    public override int Hides => 20;
    public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
}
