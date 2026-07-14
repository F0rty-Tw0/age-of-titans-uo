using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L6. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class MireCoilserpent : BaseCreature
{
    [Constructible]
    public MireCoilserpent() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0491;
        BaseSoundID = 219;

        SetStr(330, 380);
        SetDex(112, 136);
        SetInt(60, 84);

        SetHits(480, 540);

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

    public override string CorpseName => "a serpent corpse";
    public override string DefaultName => "a coil serpent";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison PoisonImmune => Poison.Deadly;
    public override Poison HitPoison => Poison.Deadly;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
