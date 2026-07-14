using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class TartarusDaemonspawn : BaseCreature
{
    [Constructible]
    public TartarusDaemonspawn() : base(AIType.AI_Mage)
    {
        Body = 9;
        Hue = 0x0021;
        BaseSoundID = 357;

        SetStr(555, 605);
        SetDex(142, 172);
        SetInt(355, 385);

        SetHits(840, 860);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 61, 71);
        SetResistance(ResistanceType.Cold, 36, 46);
        SetResistance(ResistanceType.Poison, 31, 41);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.EvalInt, 86.0, 96.0);
        SetSkill(SkillName.Magery, 86.0, 96.0);
        SetSkill(SkillName.MagicResist, 96.0, 111.0);
        SetSkill(SkillName.Tactics, 86.0, 96.0);
        SetSkill(SkillName.Wrestling, 81.0, 96.0);

        Fame = 13100;
        Karma = -13100;

        VirtualArmor = 61;
    }

    public override string CorpseName => "a tartarus daemonspawn's corpse";
    public override string DefaultName => "a tartarus daemonspawn";

    public override bool CanFly => true;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
