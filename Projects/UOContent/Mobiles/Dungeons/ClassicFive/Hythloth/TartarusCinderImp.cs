using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus ambient. L7 trash.
// Donor: Imp.
[SerializationGenerator(0, false)]
public partial class TartarusCinderImp : BaseCreature
{
    [Constructible]
    public TartarusCinderImp() : base(AIType.AI_Mage)
    {
        Body = 74;
        Hue = 0x0022;
        BaseSoundID = 422;

        SetStr(258, 298);
        SetDex(218, 248);
        SetInt(258, 298);

        SetHits(635, 650);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Fire, 40);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 44, 54);
        SetResistance(ResistanceType.Fire, 54, 64);
        SetResistance(ResistanceType.Cold, 24, 34);
        SetResistance(ResistanceType.Poison, 44, 54);
        SetResistance(ResistanceType.Energy, 34, 44);

        SetSkill(SkillName.EvalInt, 79.0, 89.0);
        SetSkill(SkillName.Magery, 89.0, 99.0);
        SetSkill(SkillName.MagicResist, 89.0, 99.0);
        SetSkill(SkillName.Tactics, 74.0, 84.0);
        SetSkill(SkillName.Wrestling, 69.0, 79.0);

        Fame = 8900;
        Karma = -8900;

        VirtualArmor = 47;
    }

    public override string CorpseName => "a cinder imp's corpse";
    public override string DefaultName => "a cinder imp";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
