using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L2 trash. Donor: Dire Wolf.
[SerializationGenerator(0, false)]
public partial class LykaiCur : BaseCreature
{
    [Constructible]
    public LykaiCur() : base(AIType.AI_Melee)
    {
        Body = 23;
        Hue = 0x0964;
        BaseSoundID = 0xE5;

        SetStr(55, 75);
        SetDex(55, 70);
        SetInt(20, 30);

        SetHits(80, 110);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 18, 25);
        SetResistance(ResistanceType.Fire, 8, 15);
        SetResistance(ResistanceType.Cold, 8, 15);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 8, 15);

        SetSkill(SkillName.MagicResist, 32.0, 42.0);
        SetSkill(SkillName.Tactics, 42.0, 52.0);
        SetSkill(SkillName.Wrestling, 42.0, 52.0);

        Fame = 900;
        Karma = -900;

        VirtualArmor = 22;
    }

    public override string CorpseName => "a caves cur's corpse";
    public override string DefaultName => "a caves cur";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
