using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - coalwalk host. L6 trash. Donor: Fire Elemental.
[SerializationGenerator(0, false)]
public partial class PyreMagmaton : BaseCreature
{
    [Constructible]
    public PyreMagmaton() : base(AIType.AI_Melee)
    {
        Body = 15;
        Hue = 0x0669;
        BaseSoundID = 838;

        SetStr(240, 280);
        SetDex(100, 120);
        SetInt(90, 115);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Fire, 75);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a magma elemental's slag";
    public override string DefaultName => "a magma elemental";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
