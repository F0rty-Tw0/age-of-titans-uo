using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame ambient, L5. Donor: Eagle.
[SerializationGenerator(0, false)]
public partial class BrinePetrel : BaseCreature
{
    [Constructible]
    public BrinePetrel() : base(AIType.AI_Melee)
    {
        Name = "a storm-petrel";

        Body = 5;
        Hue = 0x0481;
        BaseSoundID = 0x2EE;

        SetStr(170, 200);
        SetDex(160, 180);
        SetInt(40, 60);

        SetHits(290, 310);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 10, 20);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.MagicResist, 40.0, 55.0);
        SetSkill(SkillName.Tactics, 60.0, 75.0);
        SetSkill(SkillName.Wrestling, 55.0, 70.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a storm-petrel's corpse";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
