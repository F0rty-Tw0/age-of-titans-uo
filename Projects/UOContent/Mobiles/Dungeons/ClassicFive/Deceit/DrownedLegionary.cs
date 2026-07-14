using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L5 trash. Donor: SkeletalKnight.
[SerializationGenerator(0, false)]
public partial class DrownedLegionary : BaseCreature
{
    [Constructible]
    public DrownedLegionary() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x0841;
        BaseSoundID = 451;

        SetStr(210, 240);
        SetDex(70, 90);
        SetInt(35, 50);

        SetHits(335, 345);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 38, 48);
        SetResistance(ResistanceType.Fire, 22, 32);
        SetResistance(ResistanceType.Cold, 50, 60);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 32, 42);

        SetSkill(SkillName.MagicResist, 62.0, 72.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 78.0, 88.0);

        Fame = 1900;
        Karma = -1900;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a verdigris skeletal corpse";
    public override string DefaultName => "a drowned legionary";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
