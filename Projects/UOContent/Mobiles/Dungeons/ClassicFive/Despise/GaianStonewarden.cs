using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L5 core. Donor: StoneGargoyle.
[SerializationGenerator(0, false)]
public partial class GaianStonewarden : BaseCreature
{
    [Constructible]
    public GaianStonewarden() : base(AIType.AI_Melee)
    {
        Body = 67;
        Hue = 0x0455;
        BaseSoundID = 372;

        SetStr(230, 260);
        SetDex(55, 75);
        SetInt(40, 60);

        SetHits(300, 330);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3700;
        Karma = -3700;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a stone warden's corpse";
    public override string DefaultName => "a stone warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
