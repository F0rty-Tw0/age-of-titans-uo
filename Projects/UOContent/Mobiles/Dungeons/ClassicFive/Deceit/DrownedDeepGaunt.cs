using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L6 trash. Donor: BoneKnight.
[SerializationGenerator(0, false)]
public partial class DrownedDeepGaunt : BaseCreature
{
    [Constructible]
    public DrownedDeepGaunt() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x0847;
        BaseSoundID = 451;

        SetStr(320, 350);
        SetDex(110, 130);
        SetInt(55, 75);

        SetHits(460, 490);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 58, 68);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 75.0, 88.0);
        SetSkill(SkillName.Tactics, 85.0, 97.0);
        SetSkill(SkillName.Wrestling, 80.0, 92.0);

        Fame = 3100;
        Karma = -3100;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a bone-green skeletal corpse";
    public override string DefaultName => "a deep gaunt";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
