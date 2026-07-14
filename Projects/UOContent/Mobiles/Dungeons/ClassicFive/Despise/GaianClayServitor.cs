using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian support. L3 trash.
// Donor: Golem (body only; the donor's summoned/scalar constructor doesn't fit a fixed stat block).
[SerializationGenerator(0, false)]
public partial class GaianClayServitor : BaseCreature
{
    [Constructible]
    public GaianClayServitor() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0972;
        BaseSoundID = 456;

        SetStr(130, 150);
        SetDex(35, 50);
        SetInt(45, 65);

        SetHits(145, 155);

        SetDamage(8, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1300;
        Karma = -1300;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a clay servitor's shell";
    public override string DefaultName => "a clay servitor";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
