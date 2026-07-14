using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, ambient, L5. Donor: Bullfrog.
[SerializationGenerator(0, false)]
public partial class MireBullfrog : BaseCreature
{
    [Constructible]
    public MireBullfrog() : base(AIType.AI_Melee)
    {
        Body = 81;
        Hue = 0x0851;
        BaseSoundID = 0x266;

        SetStr(215, 250);
        SetDex(90, 110);
        SetInt(44, 62);

        SetHits(290, 320);

        SetDamage(10, 14);

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

    public override string CorpseName => "a bull frog corpse";
    public override string DefaultName => "a mire bullfrog";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
