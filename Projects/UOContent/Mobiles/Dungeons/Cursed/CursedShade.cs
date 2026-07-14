using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L5 trash. Donor: Shadow Fiend.
[SerializationGenerator(0, false)]
public partial class CursedShade : BaseCreature
{
    [Constructible]
    public CursedShade() : base(AIType.AI_Melee)
    {
        Body = 0xA8;
        Hue = 0x0455;

        SetStr(190, 220);
        SetDex(160, 190);
        SetInt(90, 115);

        SetHits(300, 350);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 42, 50);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a barrow shade's remains";
    public override string DefaultName => "a barrow shade";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
