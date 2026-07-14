using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - ambient/fodder. L5 trash. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class PyreSlaggrub : BaseCreature
{
    [Constructible]
    public PyreSlaggrub() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0669;
        BaseSoundID = 456;

        SetStr(150, 180);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 25, 35);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a slag grub's remains";
    public override string DefaultName => "a slag grub";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
