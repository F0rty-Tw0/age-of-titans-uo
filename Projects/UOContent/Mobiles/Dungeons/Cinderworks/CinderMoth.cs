using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L5 trash. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class CinderMoth : BaseCreature
{
    [Constructible]
    public CinderMoth() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0655;
        BaseSoundID = 422;

        SetStr(150, 180);
        SetDex(85, 105);
        SetInt(25, 40);

        SetHits(260, 300);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a molten mongrel's corpse";
    public override string DefaultName => "a molten mongrel";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
