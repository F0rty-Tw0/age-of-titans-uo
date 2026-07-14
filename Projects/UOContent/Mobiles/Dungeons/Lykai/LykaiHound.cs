using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - Orc Caves, L3 trash. Donor: Dire Wolf.
[SerializationGenerator(0, false)]
public partial class LykaiHound : BaseCreature
{
    [Constructible]
    public LykaiHound() : base(AIType.AI_Melee)
    {
        Body = 23;
        Hue = 0x0483;
        BaseSoundID = 0xE5;

        SetStr(100, 125);
        SetDex(105, 130);
        SetInt(30, 45);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 12, 20);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 12, 20);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 52.0, 62.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a Lykaian hound's corpse";
    public override string DefaultName => "a Lykaian hound";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
