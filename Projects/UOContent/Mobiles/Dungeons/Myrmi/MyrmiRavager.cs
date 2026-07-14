using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L6 trash. Donor: Terathan Avenger.
[SerializationGenerator(0, false)]
public partial class MyrmiRavager : BaseCreature
{
    [Constructible]
    public MyrmiRavager() : base(AIType.AI_Melee)
    {
        Body = 152;
        Hue = 0x0021;
        BaseSoundID = 589;

        SetStr(320, 350);
        SetDex(135, 155);
        SetInt(50, 65);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7200;
        Karma = -7200;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a myrmex ravager's corpse";
    public override string DefaultName => "a myrmex ravager";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Arachnid;
    public override Poison PoisonImmune => Poison.Deadly;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
