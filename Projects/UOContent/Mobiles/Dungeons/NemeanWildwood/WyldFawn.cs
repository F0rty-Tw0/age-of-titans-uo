using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 ambient. Donor: Hind.
[SerializationGenerator(0, false)]
public partial class WyldFawn : BaseCreature
{
    [Constructible]
    public WyldFawn() : base(AIType.AI_Melee)
    {
        Body = 0xED;
        Hue = 0x0483;

        SetStr(150, 180);
        SetDex(90, 115);
        SetInt(25, 40);

        SetHits(440, 470);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 43);
        SetResistance(ResistanceType.Fire, 10, 20);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a moonlit fawn's corpse";
    public override string DefaultName => "a moonlit fawn";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override int GetAttackSound() => 0x82;

    public override int GetHurtSound() => 0x83;

    public override int GetDeathSound() => 0x84;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
