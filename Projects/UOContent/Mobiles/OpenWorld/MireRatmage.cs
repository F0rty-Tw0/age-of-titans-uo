using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L5. Donor: Ratman Mage.
[SerializationGenerator(0, false)]
public partial class MireRatmage : BaseCreature
{
    [Constructible]
    public MireRatmage() : base(AIType.AI_Mage)
    {
        Body = 0x8F;
        Hue = 0x0851;
        BaseSoundID = 437;

        SetStr(195, 230);
        SetDex(82, 100);
        SetInt(90, 115);
        SetMana(90, 115);

        SetHits(290, 330);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 26, 34);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 80.0, 95.0);
        SetSkill(SkillName.Magery, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 60.0, 72.0);
        SetSkill(SkillName.Wrestling, 55.0, 68.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a ratman corpse";
    public override string DefaultName => "a fen ratman shaman";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
