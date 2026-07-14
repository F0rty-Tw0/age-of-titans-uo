using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, Scaled Host, L6. Donor: Lizardman.
[SerializationGenerator(0, false)]
public partial class MireScaledShaman : BaseCreature
{
    [Constructible]
    public MireScaledShaman() : base(AIType.AI_Mage)
    {
        Body = Utility.RandomList(35, 36);
        Hue = 0x0491;

        SetStr(300, 345);
        SetDex(95, 118);
        SetInt(115, 140);
        SetMana(115, 140);

        SetHits(470, 520);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 34, 42);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 34, 42);

        SetSkill(SkillName.EvalInt, 90.0, 105.0);
        SetSkill(SkillName.Magery, 90.0, 105.0);
        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 75.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a lizardman corpse";
    public override string DefaultName => "a scaled shaman";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison PoisonImmune => Poison.Deadly;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
