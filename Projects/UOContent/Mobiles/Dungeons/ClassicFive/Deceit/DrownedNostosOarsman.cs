using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Themed sub-faction - the Nostoi. L5 trash. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class DrownedNostosOarsman : BaseCreature
{
    [Constructible]
    public DrownedNostosOarsman() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0841;
        BaseSoundID = 0x48D;

        SetStr(175, 205);
        SetDex(88, 108);
        SetInt(28, 40);

        SetHits(290, 320);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 28, 36);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 1700;
        Karma = -1700;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a verdigris skeletal corpse";
    public override string DefaultName => "a Nostoi oarsman";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
