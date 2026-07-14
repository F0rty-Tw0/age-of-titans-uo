using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - ambient fodder, L3 trash. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class MyrmiGrub : BaseCreature
{
    [Constructible]
    public MyrmiGrub() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0798;
        BaseSoundID = 456;

        SetStr(85, 105);
        SetDex(50, 65);
        SetInt(25, 40);

        SetHits(110, 140);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 22, 30);
        SetResistance(ResistanceType.Fire, 8, 15);
        SetResistance(ResistanceType.Cold, 8, 15);
        SetResistance(ResistanceType.Poison, 90, 100);
        SetResistance(ResistanceType.Energy, 8, 15);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 1200;
        Karma = -1200;

        VirtualArmor = 26;
    }

    public override string CorpseName => "a myrmex grub's corpse";
    public override string DefaultName => "a myrmex grub";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
