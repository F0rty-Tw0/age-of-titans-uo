using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L4. Donor: Mountain Goat.
[SerializationGenerator(0, false)]
public partial class PeakRam : BaseCreature
{
    [Constructible]
    public PeakRam() : base(AIType.AI_Melee)
    {
        Body = 88;
        Hue = 0x0455;
        BaseSoundID = 0x99;

        SetStr(162, 188);
        SetDex(85, 102);
        SetInt(42, 60);

        SetHits(190, 225);

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

    public override string CorpseName => "a goat corpse";
    public override string DefaultName => "a horned ram";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
