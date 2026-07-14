using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, Scaled Host, L5. Donor: Lizardman.
[SerializationGenerator(0, false)]
public partial class MireScaledSpear : BaseCreature
{
    [Constructible]
    public MireScaledSpear() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(35, 36);
        Hue = 0x0844;
        BaseSoundID = 417;

        SetStr(222, 262);
        SetDex(94, 116);
        SetInt(48, 68);

        SetHits(300, 350);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 82.0, 96.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a lizardman corpse";
    public override string DefaultName => "a scaled spearman";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
