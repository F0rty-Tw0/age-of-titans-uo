using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - ambient/fodder. L3. Donor: Dire Wolf.
[SerializationGenerator(0, false)]
public partial class WaymanCurhound : BaseCreature
{
    [Constructible]
    public WaymanCurhound() : base(AIType.AI_Melee)
    {
        Body = 23;
        Hue = 0x0844;
        BaseSoundID = 0xE5;

        SetStr(105, 130);
        SetDex(105, 130);
        SetInt(25, 40);

        SetHits(130, 155);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 36);
        SetResistance(ResistanceType.Fire, 14, 22);
        SetResistance(ResistanceType.Cold, 12, 20);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 12, 20);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 62.0);
        SetSkill(SkillName.Wrestling, 50.0, 62.0);

        Fame = 1300;
        Karma = -1300;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a mangy cur's corpse";
    public override string DefaultName => "a mangy cur";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
