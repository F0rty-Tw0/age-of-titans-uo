using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class GaianGraniteAdder : BaseCreature
{
    [Constructible]
    public GaianGraniteAdder() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0455;
        BaseSoundID = 397;

        SetStr(160, 185);
        SetDex(50, 68);
        SetInt(30, 45);

        SetHits(200, 220);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 60, 75);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2100;
        Karma = -2100;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a granite adder's corpse";
    public override string DefaultName => "a granite adder";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;
    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
