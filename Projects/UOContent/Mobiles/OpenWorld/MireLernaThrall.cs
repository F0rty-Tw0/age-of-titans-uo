using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, Cult of Lerna, L5. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class MireLernaThrall : BaseCreature
{
    [Constructible]
    public MireLernaThrall() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0851;
        BaseSoundID = 471;

        SetStr(222, 262);
        SetDex(94, 116);
        SetInt(48, 68);

        SetHits(300, 350);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 82.0, 96.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a rotting corpse";
    public override string DefaultName => "a Lerna thrall";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Regular;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
