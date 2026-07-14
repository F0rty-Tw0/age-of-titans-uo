using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core.
// Donor: Golem (body only; the donor's summoned/scalar constructor doesn't fit a fixed stat block).
[SerializationGenerator(0, false)]
public partial class GaianClaymason : BaseCreature
{
    [Constructible]
    public GaianClaymason() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0972;
        BaseSoundID = 456;

        SetStr(190, 215);
        SetDex(40, 55);
        SetInt(55, 75);

        SetHits(205, 225);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 18, 25);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a clay mason's shell";
    public override string DefaultName => "a clay mason";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
