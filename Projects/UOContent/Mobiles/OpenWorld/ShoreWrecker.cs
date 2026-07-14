using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4, the Wreckers. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class ShoreWrecker : BaseCreature
{
    [Constructible]
    public ShoreWrecker() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0530;
        BaseSoundID = 471;

        SetStr(158, 186);
        SetDex(84, 102);
        SetInt(42, 60);

        SetHits(205, 238);

        SetDamage(12, 16);

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

    public override string CorpseName => "a rotting corpse";
    public override string DefaultName => "a drowned wrecker";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
