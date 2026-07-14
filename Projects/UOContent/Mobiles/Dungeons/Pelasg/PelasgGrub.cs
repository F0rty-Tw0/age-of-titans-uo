using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L2 ambient/fodder. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class PelasgGrub : BaseCreature
{
    [Constructible]
    public PelasgGrub() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0798;
        BaseSoundID = 456;

        SetStr(50, 70);
        SetDex(55, 75);
        SetInt(15, 25);

        SetHits(80, 100);

        SetDamage(4, 7);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 16, 22);
        SetResistance(ResistanceType.Fire, 8, 14);
        SetResistance(ResistanceType.Cold, 8, 14);
        SetResistance(ResistanceType.Poison, 14, 20);
        SetResistance(ResistanceType.Energy, 8, 14);

        SetSkill(SkillName.MagicResist, 26.0, 36.0);
        SetSkill(SkillName.Tactics, 30.0, 42.0);
        SetSkill(SkillName.Wrestling, 30.0, 42.0);

        Fame = 600;
        Karma = -600;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a painted grub's corpse";
    public override string DefaultName => "a painted grub";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
