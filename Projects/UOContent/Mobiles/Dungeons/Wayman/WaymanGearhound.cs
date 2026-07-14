using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L5 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class WaymanGearhound : BaseCreature
{
    [Constructible]
    public WaymanGearhound() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0798;

        SetStr(300, 330);
        SetDex(120, 145);
        SetInt(70, 90);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 35, 43);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 75.0, 88.0);
        SetSkill(SkillName.Tactics, 62.0, 75.0);
        SetSkill(SkillName.Wrestling, 62.0, 75.0);

        Fame = 4300;
        Karma = -4300;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a shattered clockwork hound";
    public override string DefaultName => "a clockwork hound";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
