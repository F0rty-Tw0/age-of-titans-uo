using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L5. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class MireBlacksnake : BaseCreature
{
    [Constructible]
    public MireBlacksnake() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0844;
        BaseSoundID = 0xDB;

        SetStr(218, 258);
        SetDex(100, 122);
        SetInt(48, 68);

        SetHits(290, 330);

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

    public override string CorpseName => "a snake corpse";
    public override string DefaultName => "a black fen snake";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override Poison HitPoison => Poison.Greater;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
