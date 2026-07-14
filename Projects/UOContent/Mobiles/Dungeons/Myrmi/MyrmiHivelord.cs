using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L6 trash. Donor: Black Solen Queen.
[SerializationGenerator(0, false)]
public partial class MyrmiHivelord : BaseCreature
{
    [Constructible]
    public MyrmiHivelord() : base(AIType.AI_Melee)
    {
        Body = 807;
        Hue = 0x0966;
        BaseSoundID = 959;

        SetStr(320, 350);
        SetDex(110, 130);
        SetInt(50, 65);

        SetHits(480, 530);

        SetDamage(13, 17);

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

    public override string CorpseName => "a myrmex hive-lord's corpse";
    public override string DefaultName => "a myrmex hive-lord";

    public override Poison PoisonImmune => Poison.Deadly;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
