using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Cougar.
[SerializationGenerator(0, false)]
public partial class WyldNemean : BaseCreature
{
    [Constructible]
    public WyldNemean() : base(AIType.AI_Melee)
    {
        Body = 63;
        Hue = 0x0798;
        BaseSoundID = 0x73;

        SetStr(225, 265);
        SetDex(150, 180);
        SetInt(40, 60);

        SetHits(620, 690);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a nemean cougar's corpse";
    public override string DefaultName => "a nemean cougar";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
