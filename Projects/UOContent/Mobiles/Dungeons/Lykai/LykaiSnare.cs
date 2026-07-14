using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - Orc Caves, L3 trash. Donor: Corpser
// (body/sound only - a mobile cave snare, not the donor's rooted plant behavior).
[SerializationGenerator(0, false)]
public partial class LykaiSnare : BaseCreature
{
    [Constructible]
    public LykaiSnare() : base(AIType.AI_Melee)
    {
        Body = 8;
        Hue = 0x0964;
        BaseSoundID = 684;

        SetStr(100, 125);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 12, 20);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 12, 20);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 52.0, 62.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a cave snare's corpse";
    public override string DefaultName => "a cave snare";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
