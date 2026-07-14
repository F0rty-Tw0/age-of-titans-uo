using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class WyldHarrier : BaseCreature
{
    [Constructible]
    public WyldHarrier() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0483;
        BaseSoundID = 402;

        SetStr(175, 215);
        SetDex(110, 140);
        SetInt(50, 70);

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
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a moon harrier's corpse";
    public override string DefaultName => "a moon harrier";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
