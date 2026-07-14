using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum (dev-docs/gap-families-bestiary.md §6.5) - Terathan Keep serpents, L6 trash. Donor: Ophidian Knight.
[SerializationGenerator(0, false)]
public partial class OphianScaledbrute : BaseCreature
{
    [Constructible]
    public OphianScaledbrute() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x0847;
        BaseSoundID = 634;

        SetStr(320, 350);
        SetDex(100, 120);
        SetInt(45, 60);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a scaled brute's corpse";
    public override string DefaultName => "a scaled brute";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
