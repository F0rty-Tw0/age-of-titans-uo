using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L6 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class RimeGolem : BaseCreature
{
    [Constructible]
    public RimeGolem() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x047E;

        SetStr(320, 360);
        SetDex(80, 100);
        SetInt(40, 55);

        SetHits(480, 540);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a hoarfrost golem's remains";
    public override string DefaultName => "a hoarfrost golem";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
