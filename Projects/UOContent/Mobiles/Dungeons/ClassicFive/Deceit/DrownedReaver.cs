using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L5 trash. Donor: BoneKnight.
[SerializationGenerator(0, false)]
public partial class DrownedReaver : BaseCreature
{
    [Constructible]
    public DrownedReaver() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x0847;
        BaseSoundID = 451;

        SetStr(225, 255);
        SetDex(73, 93);
        SetInt(37, 53);

        SetHits(350, 370);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 24, 34);
        SetResistance(ResistanceType.Cold, 51, 61);
        SetResistance(ResistanceType.Poison, 27, 37);
        SetResistance(ResistanceType.Energy, 34, 44);

        SetSkill(SkillName.MagicResist, 64.0, 74.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 2050;
        Karma = -2050;

        VirtualArmor = 49;
    }

    public override string CorpseName => "a bone-green skeletal corpse";
    public override string DefaultName => "a drowned reaver";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
