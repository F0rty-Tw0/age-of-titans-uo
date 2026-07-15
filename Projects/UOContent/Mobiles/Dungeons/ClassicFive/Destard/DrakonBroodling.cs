using ModernUO.Serialization;

namespace Server.Mobiles;

// Pantheon tamables — Destard (dev-docs/classic-five-bestiary.md). Donor: Drake.
[SerializationGenerator(0, false)]
public partial class DrakonBroodling : BaseCreature
{
    [Constructible]
    public DrakonBroodling() : base(AIType.AI_Melee, FightMode.Aggressor)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x501;
        BaseSoundID = 362;

        SetStr(280, 310);
        SetDex(133, 152);
        SetInt(101, 140);

        SetHits(300, 350);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Fire, 40);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 55.1, 60.0);
        SetSkill(SkillName.Tactics, 80.1, 90.0);
        SetSkill(SkillName.Wrestling, 80.1, 90.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 46;

        Tamable = true;
        ControlSlots = 2;
        MinTameSkill = 87.1;
    }

    public override string CorpseName => "a drakon broodling's corpse";
    public override string DefaultName => "a drakon broodling";

    public override int Meat => 10;
    public override int Hides => 20;
    public override HideType HideType => HideType.Horned;
    public override int Scales => 2;
    public override ScaleType ScaleType => Body == 60 ? ScaleType.Yellow : ScaleType.Red;
    public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
    public override bool CanFly => true;
}
