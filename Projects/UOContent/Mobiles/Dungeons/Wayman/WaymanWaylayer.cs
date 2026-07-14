using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong. L4 trash. Donor: Juka Warrior
// (bow-equipped ranged row; body/idiom shared with Thug).
[SerializationGenerator(0, false)]
public partial class WaymanWaylayer : BaseCreature
{
    [Constructible]
    public WaymanWaylayer() : base(AIType.AI_Archer)
    {
        Body = 764;
        Hue = 0x0844;

        SetStr(190, 220);
        SetDex(120, 150);
        SetInt(60, 80);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 18, 26);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Archery, 70.0, 85.0);
        SetSkill(SkillName.MagicResist, 52.0, 65.0);
        SetSkill(SkillName.Tactics, 62.0, 75.0);
        SetSkill(SkillName.Wrestling, 45.0, 58.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 42;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(40, 60)));
    }

    public override string CorpseName => "a Wayman waylayer's corpse";
    public override string DefaultName => "a Wayman waylayer";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
