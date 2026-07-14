using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Stone Harpy.
[SerializationGenerator(0, false)]
public partial class BrineSquallHawk : BaseCreature
{
    [Constructible]
    public BrineSquallHawk() : base(AIType.AI_Melee)
    {
        Name = "a squall-hawk";

        Body = 73;
        Hue = 0x0481;
        BaseSoundID = 402;

        SetStr(290, 320);
        SetDex(195, 215);
        SetInt(75, 100);

        SetHits(500, 520);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 60.0, 75.0);
        SetSkill(SkillName.Tactics, 80.0, 95.0);
        SetSkill(SkillName.Wrestling, 78.0, 93.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a squall-hawk's corpse";

    public override bool BleedImmune => true;
    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average, 2);
    }
}
