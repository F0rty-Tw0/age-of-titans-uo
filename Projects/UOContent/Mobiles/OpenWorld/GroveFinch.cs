using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, ambient, L2. Donor: Chicken.
[SerializationGenerator(0, false)]
public partial class GroveFinch : BaseCreature
{
    [Constructible]
    public GroveFinch() : base(AIType.AI_Melee)
    {
        Body = 0xD0;
        Hue = 0x0851;
        BaseSoundID = 0x6E;

        SetStr(50, 70);
        SetDex(60, 80);
        SetInt(15, 25);

        SetHits(80, 88);

        SetDamage(4, 6);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 15, 20);
        SetResistance(ResistanceType.Fire, 5);
        SetResistance(ResistanceType.Cold, 5);
        SetResistance(ResistanceType.Poison, 5, 10);
        SetResistance(ResistanceType.Energy, 5);

        SetSkill(SkillName.MagicResist, 20.0, 25.0);
        SetSkill(SkillName.Tactics, 25.0, 35.0);
        SetSkill(SkillName.Wrestling, 25.0, 35.0);

        Fame = 300;
        Karma = 0;

        VirtualArmor = 14;
    }

    public override string CorpseName => "a grove finch corpse";
    public override string DefaultName => "a grove finch";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
