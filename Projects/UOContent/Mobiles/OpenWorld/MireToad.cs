using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire, L5. Donor: Giant Toad.
[SerializationGenerator(0, false)]
public partial class MireToad : BaseCreature
{
    [Constructible]
    public MireToad() : base(AIType.AI_Melee)
    {
        Body = 80;
        Hue = 0x0851;
        BaseSoundID = 0x26B;

        SetStr(215, 255);
        SetDex(90, 112);
        SetInt(46, 66);

        SetHits(290, 340);

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

    public override string CorpseName => "a toad corpse";
    public override string DefaultName => "a bloated marsh toad";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Regular;
    public override Poison HitPoison => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
