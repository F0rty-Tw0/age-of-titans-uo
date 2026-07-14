using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L5. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class BrineStormHarpy : BaseCreature
{
    [Constructible]
    public BrineStormHarpy() : base(AIType.AI_Melee)
    {
        Name = "a storm-harpy";

        Body = 30;
        Hue = 0x0481;
        BaseSoundID = 402;

        SetStr(190, 215);
        SetDex(170, 190);
        SetInt(70, 90);

        SetHits(305, 325);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 70.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 70.0, 85.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a storm-harpy's corpse";

    public override bool BleedImmune => true;
    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
