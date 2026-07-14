using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L5, Bronze Watch. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class PeakBronzeArcher : BaseCreature
{
    [Constructible]
    public PeakBronzeArcher() : base(AIType.AI_Archer)
    {
        Body = 4;
        Hue = 0x0798;
        BaseSoundID = 372;

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

        SetSkill(SkillName.Archery, 78.0, 90.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 60.0, 72.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(30, 45)));
    }

    public override string CorpseName => "a gargoyle corpse";
    public override string DefaultName => "a bronze archer";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
