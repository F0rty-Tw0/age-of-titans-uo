using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 ambient/fodder. Donor:
// Scorpion.
[SerializationGenerator(0, false)]
public partial class TideCrabling : BaseCreature
{
    [Constructible]
    public TideCrabling() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0530;
        BaseSoundID = 397;

        SetStr(60, 75);
        SetDex(30, 45);
        SetInt(10, 20);

        SetHits(90, 120);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 14, 20);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 12, 18);

        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 32.0, 42.0);
        SetSkill(SkillName.Wrestling, 32.0, 42.0);

        Fame = 500;
        Karma = -500;

        VirtualArmor = 16;
    }

    public override string CorpseName => "a crab's shell";
    public override string DefaultName => "a scuttling crab";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
