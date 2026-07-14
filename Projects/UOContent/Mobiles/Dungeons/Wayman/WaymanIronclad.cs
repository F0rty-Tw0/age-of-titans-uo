using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L7 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class WaymanIronclad : BaseCreature
{
    [Constructible]
    public WaymanIronclad() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0964;

        SetStr(410, 450);
        SetDex(100, 125);
        SetInt(100, 125);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 62, 72);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 24, 34);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 34, 44);

        SetSkill(SkillName.MagicResist, 102.0, 118.0);
        SetSkill(SkillName.Tactics, 85.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 6900;
        Karma = -6900;

        VirtualArmor = 64;
    }

    public override string CorpseName => "a shattered ironclad automaton";
    public override string DefaultName => "an ironclad automaton";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
        AddLoot(LootPack.Gems, 2);
    }
}
