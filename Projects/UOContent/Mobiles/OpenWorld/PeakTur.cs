using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks, L4. Donor: Grizzly Bear.
[SerializationGenerator(0, false)]
public partial class PeakTur : BaseCreature
{
    [Constructible]
    public PeakTur() : base(AIType.AI_Melee)
    {
        Body = 212;
        Hue = 0x0455;
        BaseSoundID = 0xA3;

        SetStr(155, 182);
        SetDex(88, 105);
        SetInt(40, 58);

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

    public override string CorpseName => "a tur corpse";
    public override string DefaultName => "a wild tur";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
