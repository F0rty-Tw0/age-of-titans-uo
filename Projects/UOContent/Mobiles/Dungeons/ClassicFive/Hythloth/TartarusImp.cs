using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash. Donor: Imp.
[SerializationGenerator(0, false)]
public partial class TartarusImp : BaseCreature
{
    [Constructible]
    public TartarusImp() : base(AIType.AI_Mage)
    {
        Body = 74;
        Hue = 0x0021;
        BaseSoundID = 422;

        SetStr(260, 300);
        SetDex(220, 250);
        SetInt(260, 300);

        SetHits(635, 645);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Fire, 40);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a tartarus imp's corpse";
    public override string DefaultName => "a tartarus imp";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
