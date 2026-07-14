using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Themed sub-faction - the Nostoi. L5 trash. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class DrownedNostosArcher : BaseCreature
{
    [Constructible]
    public DrownedNostosArcher() : base(AIType.AI_Archer)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0830;
        BaseSoundID = 0x48D;

        SetStr(160, 190);
        SetDex(110, 130);
        SetInt(30, 42);

        SetHits(290, 320);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 28, 36);

        SetSkill(SkillName.Archery, 75.0, 88.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1700;
        Karma = -1700;

        VirtualArmor = 36;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(40, 60)));
    }

    public override string CorpseName => "a verdigris skeletal corpse";
    public override string DefaultName => "a Nostoi archer";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
