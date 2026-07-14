using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L6. Donor: Rotting Corpse.
[SerializationGenerator(0, false)]
public partial class MireDrowned : BaseCreature
{
    [Constructible]
    public MireDrowned() : base(AIType.AI_Melee)
    {
        Body = 155;
        Hue = 0x0844;
        BaseSoundID = 471;

        SetStr(322, 372);
        SetDex(108, 132);
        SetInt(58, 82);

        SetHits(460, 510);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a rotting corpse";
    public override string DefaultName => "a fen-drowned corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;
    public override Poison HitPoison => Poison.Greater;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
