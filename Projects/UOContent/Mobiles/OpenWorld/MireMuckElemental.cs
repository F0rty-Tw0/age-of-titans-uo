using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L5. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class MireMuckElemental : BaseCreature
{
    [Constructible]
    public MireMuckElemental() : base(AIType.AI_Melee)
    {
        Body = 16;
        Hue = 0x0844;
        BaseSoundID = 278;

        SetStr(228, 268);
        SetDex(90, 112);
        SetInt(48, 68);

        SetHits(320, 370);

        SetDamage(12, 17);

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

    public override string CorpseName => "a water elemental corpse";
    public override string DefaultName => "a muck elemental";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
