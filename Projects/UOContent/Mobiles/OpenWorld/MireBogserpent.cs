using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L5. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class MireBogserpent : BaseCreature
{
    [Constructible]
    public MireBogserpent() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0851;
        BaseSoundID = 219;

        SetStr(225, 265);
        SetDex(92, 114);
        SetInt(50, 70);

        SetHits(310, 360);

        SetDamage(13, 18);

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

    public override string CorpseName => "a serpent corpse";
    public override string DefaultName => "a bog serpent";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Greater;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
