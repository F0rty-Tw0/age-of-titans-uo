using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 ambient/fodder. Donor:
// Mongbat.
[SerializationGenerator(0, false)]
public partial class TideGull : BaseCreature
{
    [Constructible]
    public TideGull() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0481;
        BaseSoundID = 422;

        SetStr(50, 65);
        SetDex(50, 65);
        SetInt(10, 20);

        SetHits(80, 110);

        SetDamage(4, 7);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 12, 18);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 8, 14);

        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 30.0, 40.0);
        SetSkill(SkillName.Wrestling, 30.0, 40.0);

        Fame = 500;
        Karma = -500;

        VirtualArmor = 14;
    }

    public override string CorpseName => "a gull's carcass";
    public override string DefaultName => "a brine gull";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
