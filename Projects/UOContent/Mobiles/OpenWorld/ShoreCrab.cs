using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L3. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class ShoreCrab : BaseCreature
{
    [Constructible]
    public ShoreCrab() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0530;
        BaseSoundID = 397;

        SetStr(108, 132);
        SetDex(76, 96);
        SetInt(30, 45);

        SetHits(130, 155);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a crab corpse";
    public override string DefaultName => "a giant shore crab";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
