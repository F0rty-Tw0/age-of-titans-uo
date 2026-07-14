using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L5 core. Donor: OgreLord.
[SerializationGenerator(0, false)]
public partial class GaianMountainborn : BaseCreature
{
    [Constructible]
    public GaianMountainborn() : base(AIType.AI_Melee)
    {
        Body = 83;
        Hue = 0x0455;
        BaseSoundID = 427;

        SetStr(260, 290);
        SetDex(55, 75);
        SetInt(45, 65);

        SetHits(330, 360);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 72.0, 84.0);
        SetSkill(SkillName.Wrestling, 70.0, 82.0);

        Fame = 4100;
        Karma = -4100;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a mountain-born's corpse";
    public override string DefaultName => "a mountain-born";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
