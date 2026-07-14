using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L6 core. Donor: GiantSerpent.
[SerializationGenerator(0, false)]
public partial class DrakonBroodviper : BaseCreature
{
    [Constructible]
    public DrakonBroodviper() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x066D;
        BaseSoundID = 219;

        SetStr(415, 445);
        SetDex(95, 115);
        SetInt(60, 80);

        SetHits(465, 485);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 45);
        SetDamageType(ResistanceType.Poison, 55);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 80, 95);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.Poisoning, 70.0, 85.0);
        SetSkill(SkillName.MagicResist, 45.0, 60.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 90.0);

        Fame = 4300;
        Karma = -4300;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a drakon brood-viper's corpse";
    public override string DefaultName => "a drakon brood-viper";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Regular;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
