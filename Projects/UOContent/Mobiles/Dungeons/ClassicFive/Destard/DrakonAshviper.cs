using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L6 ambient. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class DrakonAshviper : BaseCreature
{
    [Constructible]
    public DrakonAshviper() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x066D;
        BaseSoundID = 0xDB;

        SetStr(410, 440);
        SetDex(130, 150);
        SetInt(55, 75);

        SetHits(465, 485);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Poison, 60);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 80, 95);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.Poisoning, 65.0, 80.0);
        SetSkill(SkillName.MagicResist, 45.0, 60.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 4300;
        Karma = -4300;

        VirtualArmor = 42;
    }

    public override string CorpseName => "an ash viper's corpse";
    public override string DefaultName => "an ash viper";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison HitPoison => Poison.Regular;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
