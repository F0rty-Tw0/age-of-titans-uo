using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L5. Donor: Grizzly Bear.
[SerializationGenerator(0, false)]
public partial class PeakBoulderbear : BaseCreature
{
    [Constructible]
    public PeakBoulderbear() : base(AIType.AI_Melee)
    {
        Body = 212;
        Hue = 0x0455;
        BaseSoundID = 0xA3;

        SetStr(228, 268);
        SetDex(95, 115);
        SetInt(48, 70);

        SetHits(320, 360);

        SetDamage(13, 18);

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

    public override string CorpseName => "a bear corpse";
    public override string DefaultName => "a boulder bear";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
