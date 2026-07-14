using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4, the Wreckers. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class ShoreWreckLord : BaseCreature
{
    [Constructible]
    public ShoreWreckLord() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0491;
        BaseSoundID = 0x48D;

        SetStr(158, 186);
        SetDex(86, 106);
        SetInt(42, 60);

        SetHits(230, 238);

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

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "the wreck-captain";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    // Wrecker's Hook: 15% chance to drag and sap stamina when struck.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Stam -= 10;
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The wreck-captain's hook drags you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
