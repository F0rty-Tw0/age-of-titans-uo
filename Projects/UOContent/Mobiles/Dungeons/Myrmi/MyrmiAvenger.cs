using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest (dev-docs/gap-families-bestiary.md §6.4) - Solen Hive + Terathan swarm, L6 trash. Donor: Terathan Avenger.
[SerializationGenerator(0, false)]
public partial class MyrmiAvenger : BaseCreature
{
    [Constructible]
    public MyrmiAvenger() : base(AIType.AI_Melee)
    {
        Body = 152;
        Hue = 0x0021;
        BaseSoundID = 0x24D;

        SetStr(300, 330);
        SetDex(120, 140);
        SetInt(50, 65);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a myrmex avenger's corpse";
    public override string DefaultName => "a myrmex avenger";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override Poison PoisonImmune => Poison.Greater;
    public override PackInstinct PackInstinct => PackInstinct.Arachnid;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
