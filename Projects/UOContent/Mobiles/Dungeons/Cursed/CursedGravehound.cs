using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L5 trash. Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class CursedGravehound : BaseCreature
{
    [Constructible]
    public CursedGravehound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0455;
        BaseSoundID = 229;

        SetStr(200, 230);
        SetDex(150, 175);
        SetInt(40, 60);

        SetHits(300, 350);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a barrow hound's carcass";
    public override string DefaultName => "a barrow hound";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
