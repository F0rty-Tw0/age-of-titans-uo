using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash. Donor: Imp.
[SerializationGenerator(0, false)]
public partial class TartarusImpling : BaseCreature
{
    [Constructible]
    public TartarusImpling() : base(AIType.AI_Mage)
    {
        Body = 74;
        Hue = 0x0021;
        BaseSoundID = 422;

        SetStr(265, 305);
        SetDex(225, 255);
        SetInt(265, 305);

        SetHits(635, 655);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Fire, 40);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 46, 56);
        SetResistance(ResistanceType.Fire, 56, 66);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 46, 56);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.EvalInt, 81.0, 91.0);
        SetSkill(SkillName.Magery, 91.0, 100.0);
        SetSkill(SkillName.MagicResist, 91.0, 100.0);
        SetSkill(SkillName.Tactics, 76.0, 86.0);
        SetSkill(SkillName.Wrestling, 71.0, 81.0);

        Fame = 9100;
        Karma = -9100;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a tartarus impling's corpse";
    public override string DefaultName => "a tartarus impling";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
