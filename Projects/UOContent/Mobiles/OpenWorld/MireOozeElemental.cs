using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L6. Donor: Poison Elemental.
[SerializationGenerator(0, false)]
public partial class MireOozeElemental : BaseCreature
{
    [Constructible]
    public MireOozeElemental() : base(AIType.AI_Mage)
    {
        Body = 162;
        Hue = 0x0851;
        BaseSoundID = 263;

        SetStr(300, 345);
        SetDex(95, 118);
        SetInt(115, 140);
        SetMana(115, 140);

        SetHits(480, 540);

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

    public override string CorpseName => "a poison elementals corpse";
    public override string DefaultName => "a poison ooze";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Lethal;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
