using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L4 trash. Donor: RottingCorpse.
[SerializationGenerator(0, false)]
public partial class DrownedSilt : BaseCreature
{
    [Constructible]
    public DrownedSilt() : base(AIType.AI_Melee)
    {
        Body = 155;
        Hue = 0x0830;
        BaseSoundID = 471;

        SetStr(165, 195);
        SetDex(48, 63);
        SetInt(32, 47);

        SetHits(210, 230);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 0);
        SetDamageType(ResistanceType.Cold, 50);
        SetDamageType(ResistanceType.Poison, 50);

        SetResistance(ResistanceType.Physical, 33, 41);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 36, 46);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.Poisoning, 60.0, 75.0);
        SetSkill(SkillName.MagicResist, 55.0, 68.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1550;
        Karma = -1550;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a silt-choked corpse";
    public override string DefaultName => "a silt-choked corpse";

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
