using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class TartarusFiend : BaseCreature
{
    [Constructible]
    public TartarusFiend() : base(AIType.AI_Mage)
    {
        Body = 9;
        Hue = 0x0021;
        BaseSoundID = 357;

        SetStr(550, 600);
        SetDex(140, 170);
        SetInt(350, 380);

        SetHits(845, 855);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 13000;
        Karma = -13000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a tartarus fiend's corpse";
    public override string DefaultName => "a tartarus fiend";

    public override bool CanFly => true;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
