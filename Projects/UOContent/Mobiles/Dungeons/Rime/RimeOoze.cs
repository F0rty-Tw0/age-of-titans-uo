using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L4 trash. Donor: Frost Ooze.
[SerializationGenerator(0, false)]
public partial class RimeOoze : BaseCreature
{
    [Constructible]
    public RimeOoze() : base(AIType.AI_Melee)
    {
        Body = 94;
        Hue = 0x0B0F;
        BaseSoundID = 456;

        SetStr(130, 155);
        SetDex(60, 80);
        SetInt(40, 55);

        SetHits(190, 230);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 12, 18);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a frost ooze's remains";
    public override string DefaultName => "a frost ooze";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
