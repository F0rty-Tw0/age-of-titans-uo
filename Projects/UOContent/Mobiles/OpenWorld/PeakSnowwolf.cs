using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L5. Donor: White Wolf.
[SerializationGenerator(0, false)]
public partial class PeakSnowwolf : BaseCreature
{
    [Constructible]
    public PeakSnowwolf() : base(AIType.AI_Melee)
    {
        Body = 34;
        Hue = 0x0492;
        BaseSoundID = 0xE5;

        SetStr(228, 268);
        SetDex(95, 115);
        SetInt(48, 70);

        SetHits(320, 355);

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

    public override string CorpseName => "a wolf corpse";
    public override string DefaultName => "a snow wolf";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
