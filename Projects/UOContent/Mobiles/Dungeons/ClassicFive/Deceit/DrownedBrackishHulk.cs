using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L4 trash. Donor: RottingCorpse.
[SerializationGenerator(0, false)]
public partial class DrownedBrackishHulk : BaseCreature
{
    [Constructible]
    public DrownedBrackishHulk() : base(AIType.AI_Melee)
    {
        Body = 155;
        Hue = 0x0830;
        BaseSoundID = 471;

        SetStr(160, 190);
        SetDex(50, 65);
        SetInt(30, 45);

        SetHits(215, 225);

        SetDamage(10, 15);

        SetDamageType(ResistanceType.Physical, 0);
        SetDamageType(ResistanceType.Cold, 50);
        SetDamageType(ResistanceType.Poison, 50);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 18, 26);

        SetSkill(SkillName.Poisoning, 60.0, 75.0);
        SetSkill(SkillName.MagicResist, 55.0, 68.0);
        SetSkill(SkillName.Tactics, 58.0, 68.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a silt-caked corpse";
    public override string DefaultName => "a brackish hulk";

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
