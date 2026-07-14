using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L8 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class CursedHierophant : BaseCreature
{
    [Constructible]
    public CursedHierophant() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0486;
        BaseSoundID = 0x3E9;

        SetStr(300, 330);
        SetDex(180, 210);
        SetInt(340, 380);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 35, 42);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 12000;
        Karma = -12000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a lich's ancient corpse";
    public override string DefaultName => "a Hecate hierophant";

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
