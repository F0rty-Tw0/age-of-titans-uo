using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L4 trash. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class DrownedRower : BaseCreature
{
    [Constructible]
    public DrownedRower() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0841;
        BaseSoundID = 0x48D;

        SetStr(140, 165);
        SetDex(70, 90);
        SetInt(20, 30);

        SetHits(180, 200);

        SetDamage(10, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 34);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 22, 30);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1350;
        Karma = -1350;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a verdigris skeletal corpse";
    public override string DefaultName => "a drowned rower";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
