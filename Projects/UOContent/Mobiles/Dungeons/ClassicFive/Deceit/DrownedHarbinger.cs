using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L6 trash. Donor: SkeletalKnight.
[SerializationGenerator(0, false)]
public partial class DrownedHarbinger : BaseCreature
{
    [Constructible]
    public DrownedHarbinger() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x0830;
        BaseSoundID = 451;

        SetStr(310, 340);
        SetDex(115, 135);
        SetInt(50, 70);

        SetHits(470, 500);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 28, 38);
        SetResistance(ResistanceType.Cold, 56, 66);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 38, 48);

        SetSkill(SkillName.MagicResist, 73.0, 86.0);
        SetSkill(SkillName.Tactics, 83.0, 95.0);
        SetSkill(SkillName.Wrestling, 78.0, 90.0);

        Fame = 3050;
        Karma = -3050;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a verdigris skeletal corpse";
    public override string DefaultName => "a drowned harbinger";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
