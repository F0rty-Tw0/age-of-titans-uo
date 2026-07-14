using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L6 trash. Donor: Spectre.
[SerializationGenerator(0, false)]
public partial class CursedGheist : BaseCreature
{
    [Constructible]
    public CursedGheist() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0AA8;
        BaseSoundID = 0x482;

        SetStr(220, 250);
        SetDex(170, 195);
        SetInt(280, 310);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a spectral husk";
    public override string DefaultName => "a cairn geist";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
