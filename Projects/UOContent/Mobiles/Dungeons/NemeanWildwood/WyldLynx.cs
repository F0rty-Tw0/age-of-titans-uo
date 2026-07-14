using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Cougar.
[SerializationGenerator(0, false)]
public partial class WyldLynx : BaseCreature
{
    [Constructible]
    public WyldLynx() : base(AIType.AI_Melee)
    {
        Body = 63;
        Hue = 0x0483;
        BaseSoundID = 0x73;

        SetStr(180, 220);
        SetDex(110, 140);
        SetInt(30, 45);

        SetHits(440, 490);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a silver lynx corpse";
    public override string DefaultName => "a silver lynx";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
