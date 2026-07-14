using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class WyldThiasosHound : BaseCreature
{
    [Constructible]
    public WyldThiasosHound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0483;
        BaseSoundID = 229;

        SetStr(180, 220);
        SetDex(100, 130);
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

        VirtualArmor = 55;
    }

    public override string CorpseName => "a Thiasos hound's corpse";
    public override string DefaultName => "a Thiasos hound";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
