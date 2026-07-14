using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L6 trash. Donor: Shadow Fiend.
[SerializationGenerator(0, false)]
public partial class CursedLampadShade : BaseCreature
{
    [Constructible]
    public CursedLampadShade() : base(AIType.AI_Melee)
    {
        Body = 0xA8;
        Hue = 0x0455;

        SetStr(260, 290);
        SetDex(190, 215);
        SetInt(110, 135);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 46, 54);
        SetResistance(ResistanceType.Poison, 42, 50);
        SetResistance(ResistanceType.Energy, 24, 32);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a shade's flickering remains";
    public override string DefaultName => "a Lampad shade";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
