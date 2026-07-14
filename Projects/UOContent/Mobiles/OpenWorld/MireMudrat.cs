using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, ambient, L5. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class MireMudrat : BaseCreature
{
    [Constructible]
    public MireMudrat() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0844;
        BaseSoundID = 0x188;

        SetStr(215, 250);
        SetDex(94, 116);
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

    public override string CorpseName => "a giant rat corpse";
    public override string DefaultName => "a mud rat";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
