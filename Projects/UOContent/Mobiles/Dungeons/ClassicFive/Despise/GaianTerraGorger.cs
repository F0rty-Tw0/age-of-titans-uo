using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L5 core. Donor: EarthElemental.
[SerializationGenerator(0, false)]
public partial class GaianTerraGorger : BaseCreature
{
    [Constructible]
    public GaianTerraGorger() : base(AIType.AI_Melee)
    {
        Body = 14;
        Hue = 0x09C2;
        BaseSoundID = 268;

        SetStr(240, 270);
        SetDex(40, 58);
        SetInt(35, 55);

        SetHits(310, 340);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a terra-gorger's corpse";
    public override string DefaultName => "a terra-gorger";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
