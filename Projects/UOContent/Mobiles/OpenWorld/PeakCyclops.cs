using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks, L5. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class PeakCyclops : BaseCreature
{
    [Constructible]
    public PeakCyclops() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0798;
        BaseSoundID = 604;

        SetStr(232, 272);
        SetDex(95, 115);
        SetInt(50, 70);

        SetHits(330, 380);

        SetDamage(14, 19);

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

    public override string CorpseName => "a cyclopean corpse";
    public override string DefaultName => "a bronze cyclops";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override bool BleedImmune => true;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
