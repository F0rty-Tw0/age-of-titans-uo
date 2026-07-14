using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L4 trash. Donor: Black Solen Warrior.
[SerializationGenerator(0, false)]
public partial class MyrmiChitinguard : BaseCreature
{
    [Constructible]
    public MyrmiChitinguard() : base(AIType.AI_Melee)
    {
        Body = 806;
        Hue = 0x0966;
        BaseSoundID = 959;

        SetStr(160, 185);
        SetDex(100, 120);
        SetInt(35, 50);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2900;
        Karma = -2900;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a myrmex chitin-guard's corpse";
    public override string DefaultName => "a myrmex chitin-guard";

    public override PackInstinct PackInstinct => PackInstinct.Arachnid;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
