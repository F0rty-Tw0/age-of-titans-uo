using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L5 trash. Donor: Skeletal Mage.
[SerializationGenerator(0, false)]
public partial class CursedGravecaller : BaseCreature
{
    [Constructible]
    public CursedGravecaller() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0486;
        BaseSoundID = 451;

        SetStr(190, 220);
        SetDex(120, 145);
        SetInt(230, 260);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "a grave-caller";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
