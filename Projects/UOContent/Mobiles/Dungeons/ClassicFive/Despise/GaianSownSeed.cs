using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian support. L3 trash. Donor: GiantSerpent.
// Unhatched and inert: no venom attack, just poison-immune ambient filler.
[SerializationGenerator(0, false)]
public partial class GaianSownSeed : BaseCreature
{
    [Constructible]
    public GaianSownSeed() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x09C2;
        BaseSoundID = 219;

        SetStr(90, 110);
        SetDex(40, 55);
        SetInt(30, 45);

        SetHits(115, 125);

        SetDamage(6, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 8, 12);
        SetResistance(ResistanceType.Cold, 8, 12);
        SetResistance(ResistanceType.Poison, 60, 75);
        SetResistance(ResistanceType.Energy, 8, 12);

        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 900;
        Karma = -900;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a sown seed's husk";
    public override string DefaultName => "a sown seed";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
