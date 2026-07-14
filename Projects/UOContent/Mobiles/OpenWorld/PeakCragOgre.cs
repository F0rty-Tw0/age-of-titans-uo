using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks, L5. Donor: Ogre.
[SerializationGenerator(0, false)]
public partial class PeakCragOgre : BaseCreature
{
    [Constructible]
    public PeakCragOgre() : base(AIType.AI_Melee)
    {
        Body = 1;
        Hue = 0x0455;
        BaseSoundID = 427;

        SetStr(225, 265);
        SetDex(92, 112);
        SetInt(48, 68);

        SetHits(320, 370);

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

    public override string CorpseName => "an ogre corpse";
    public override string DefaultName => "a crag ogre";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
