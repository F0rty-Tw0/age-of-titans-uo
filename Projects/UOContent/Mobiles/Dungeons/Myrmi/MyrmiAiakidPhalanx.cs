using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L5 trash. Donor: Terathan Avenger.
[SerializationGenerator(0, false)]
public partial class MyrmiAiakidPhalanx : BaseCreature
{
    [Constructible]
    public MyrmiAiakidPhalanx() : base(AIType.AI_Melee)
    {
        Body = 152;
        Hue = 0x0966;
        BaseSoundID = 589;

        SetStr(220, 250);
        SetDex(115, 135);
        SetInt(40, 55);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 48;
    }

    public override string CorpseName => "an Aiakid phalangite's corpse";
    public override string DefaultName => "an Aiakid phalangite";

    public override PackInstinct PackInstinct => PackInstinct.Arachnid;
    public override Poison PoisonImmune => Poison.Deadly;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
