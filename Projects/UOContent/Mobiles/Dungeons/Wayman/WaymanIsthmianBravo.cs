using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Isthmian wreckers. L6 trash. Donor: Juka Lord.
[SerializationGenerator(0, false)]
public partial class WaymanIsthmianBravo : BaseCreature
{
    [Constructible]
    public WaymanIsthmianBravo() : base(AIType.AI_Melee)
    {
        Body = 766;
        Hue = 0x0964;

        SetStr(360, 400);
        SetDex(115, 140);
        SetInt(85, 110);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 28, 36);
        SetResistance(ResistanceType.Cold, 23, 33);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 26, 34);

        SetSkill(SkillName.MagicResist, 78.0, 90.0);
        SetSkill(SkillName.Tactics, 78.0, 90.0);
        SetSkill(SkillName.Wrestling, 74.0, 88.0);

        Fame = 5700;
        Karma = -5700;

        VirtualArmor = 54;
    }

    public override string CorpseName => "an Isthmian bravo's corpse";
    public override string DefaultName => "an Isthmian bravo";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
