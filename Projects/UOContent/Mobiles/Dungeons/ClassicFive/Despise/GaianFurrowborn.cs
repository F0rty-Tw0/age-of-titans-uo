using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L3 core. Donor: Lizardman.
[SerializationGenerator(0, false)]
public partial class GaianFurrowborn : BaseCreature
{
    [Constructible]
    public GaianFurrowborn() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(35, 36);
        Hue = 0x09C4;
        BaseSoundID = 417;

        SetStr(110, 130);
        SetDex(80, 100);
        SetInt(30, 45);

        SetHits(140, 155);

        SetDamage(9, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 20);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1300;
        Karma = -1300;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a furrow-born's corpse";
    public override string DefaultName => "a furrow-born";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
