using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L6 trash. Donor: Bone Magi.
[SerializationGenerator(0, false)]
public partial class CursedBonemagi : BaseCreature
{
    [Constructible]
    public CursedBonemagi() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0842;
        BaseSoundID = 451;

        SetStr(230, 260);
        SetDex(130, 155);
        SetInt(280, 310);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "a bone hierophant";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
