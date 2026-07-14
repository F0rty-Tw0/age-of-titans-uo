using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L4 trash. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class DrownedSailor : BaseCreature
{
    [Constructible]
    public DrownedSailor() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0835;
        BaseSoundID = 471;

        SetStr(145, 175);
        SetDex(53, 68);
        SetInt(24, 34);

        SetHits(190, 210);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 20, 28);

        SetSkill(SkillName.MagicResist, 44.0, 54.0);
        SetSkill(SkillName.Tactics, 57.0, 67.0);
        SetSkill(SkillName.Wrestling, 57.0, 67.0);

        Fame = 1450;
        Karma = -1450;

        VirtualArmor = 33;
    }

    public override string CorpseName => "a drowned corpse";
    public override string DefaultName => "a drowned sailor";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
