using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L6 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class WaymanBronzeguard : BaseCreature
{
    [Constructible]
    public WaymanBronzeguard() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0798;

        SetStr(360, 400);
        SetDex(90, 115);
        SetInt(90, 115);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 90.0, 105.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 90.0);

        Fame = 5800;
        Karma = -5800;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a shattered sentinel";
    public override string DefaultName => "a bronze sentinel";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
