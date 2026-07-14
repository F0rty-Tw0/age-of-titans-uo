using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L5 trash. Donor: SkeletalMage.
[SerializationGenerator(0, false)]
public partial class DrownedNecromage : BaseCreature
{
    [Constructible]
    public DrownedNecromage() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0835;
        BaseSoundID = 451;

        SetStr(115, 140);
        SetDex(85, 105);
        SetInt(170, 195);

        SetHits(320, 340);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 46, 56);
        SetResistance(ResistanceType.Poison, 24, 34);
        SetResistance(ResistanceType.Energy, 28, 38);

        SetSkill(SkillName.EvalInt, 75.0, 87.0);
        SetSkill(SkillName.Magery, 75.0, 87.0);
        SetSkill(SkillName.MagicResist, 68.0, 80.0);
        SetSkill(SkillName.Tactics, 60.0, 72.0);
        SetSkill(SkillName.Wrestling, 56.0, 66.0);

        Fame = 1850;
        Karma = -1850;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "a drowned necromage";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
