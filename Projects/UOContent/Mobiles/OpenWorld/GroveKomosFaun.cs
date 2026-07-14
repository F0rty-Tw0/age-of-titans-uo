using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, Komos revel, L2. Donor: Satyr.
[SerializationGenerator(0, false)]
public partial class GroveKomosFaun : BaseCreature
{
    [Constructible]
    public GroveKomosFaun() : base(AIType.AI_Melee)
    {
        Body = 271;
        Hue = 0x0491;
        BaseSoundID = 0x586;

        SetStr(72, 92);
        SetDex(62, 82);
        SetInt(22, 35);

        SetHits(85, 100);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 25);
        SetResistance(ResistanceType.Fire, 5, 10);
        SetResistance(ResistanceType.Cold, 5, 10);
        SetResistance(ResistanceType.Poison, 10, 15);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 550;
        Karma = -550;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a kōmos faun corpse";
    public override string DefaultName => "a kōmos faun";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
