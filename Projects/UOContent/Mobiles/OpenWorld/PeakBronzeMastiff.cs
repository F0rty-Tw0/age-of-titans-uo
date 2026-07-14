using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L4, Bronze Watch. Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class PeakBronzeMastiff : BaseCreature
{
    [Constructible]
    public PeakBronzeMastiff() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0798;
        BaseSoundID = 229;

        SetStr(162, 188);
        SetDex(85, 102);
        SetInt(42, 60);

        SetHits(200, 235);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a hound corpse";
    public override string DefaultName => "a bronze mastiff";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
