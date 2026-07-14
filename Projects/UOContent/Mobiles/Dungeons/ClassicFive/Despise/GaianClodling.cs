using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L3 core. Donor: Ratman.
[SerializationGenerator(0, false)]
public partial class GaianClodling : BaseCreature
{
    [Constructible]
    public GaianClodling() : base(AIType.AI_Melee)
    {
        Body = 42;
        Hue = 0x0972;
        BaseSoundID = 437;

        SetStr(100, 120);
        SetDex(75, 95);
        SetInt(25, 40);

        SetHits(130, 145);

        SetDamage(8, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 8, 14);
        SetResistance(ResistanceType.Cold, 8, 14);
        SetResistance(ResistanceType.Poison, 12, 18);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1100;
        Karma = -1100;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a clodling's corpse";
    public override string DefaultName => "a gaian clodling";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
