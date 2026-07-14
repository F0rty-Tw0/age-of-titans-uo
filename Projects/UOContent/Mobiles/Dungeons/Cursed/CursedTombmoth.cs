using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L5 fodder. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class CursedTombmoth : BaseCreature
{
    [Constructible]
    public CursedTombmoth() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0455;
        BaseSoundID = 422;

        SetStr(130, 155);
        SetDex(170, 195);
        SetInt(30, 45);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 22, 28);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 38.0, 48.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 48.0, 58.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a moth's dusty wings";
    public override string DefaultName => "a tomb moth";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
