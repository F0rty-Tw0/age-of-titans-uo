using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L5 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineSmith : BaseCreature
{
    [Constructible]
    public ArgusTelchineSmith() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0798;
        BaseSoundID = 456;

        SetStr(290, 320);
        SetDex(90, 115);
        SetInt(40, 55);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 25, 33);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 65.0, 78.0);

        Fame = 4400;
        Karma = -4400;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a Telchine gild-smith's wreckage";
    public override string DefaultName => "a Telchine gild-smith";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
