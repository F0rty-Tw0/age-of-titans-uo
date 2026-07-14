using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, Scaled Host, L5. Donor: Lizardman.
[SerializationGenerator(0, false)]
public partial class MireScaledArcher : BaseCreature
{
    [Constructible]
    public MireScaledArcher() : base(AIType.AI_Archer)
    {
        Body = Utility.RandomList(35, 36);
        Hue = 0x0851;
        BaseSoundID = 417;

        SetStr(205, 240);
        SetDex(135, 165);
        SetInt(48, 68);

        SetHits(290, 330);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.Archery, 90.0, 104.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 60.0, 72.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(15, 25)));
    }

    public override string CorpseName => "a lizardman corpse";
    public override string DefaultName => "a scaled archer";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
