using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L6. Donor: Alligator.
[SerializationGenerator(0, false)]
public partial class MireGreatgator : BaseCreature
{
    [Constructible]
    public MireGreatgator() : base(AIType.AI_Melee)
    {
        Body = 0xCA;
        Hue = 0x0851;
        BaseSoundID = 660;

        SetStr(330, 380);
        SetDex(108, 132);
        SetInt(58, 82);

        SetHits(470, 520);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "an alligator corpse";
    public override string DefaultName => "a great mire gator";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
