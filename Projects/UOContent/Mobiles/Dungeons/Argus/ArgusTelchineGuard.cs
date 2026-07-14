using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L6 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineGuard : BaseCreature
{
    [Constructible]
    public ArgusTelchineGuard() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0798;
        BaseSoundID = 456;

        SetStr(330, 360);
        SetDex(120, 145);
        SetInt(55, 75);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 28, 36);
        SetResistance(ResistanceType.Cold, 28, 36);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 78.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a Telchine bronze-guard's wreckage";
    public override string DefaultName => "a Telchine bronze-guard";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
