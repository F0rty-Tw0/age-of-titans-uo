using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash.
// Donor: Gargoyle Enforcer.
[SerializationGenerator(0, false)]
public partial class TartarusEnforcer : BaseCreature
{
    [Constructible]
    public TartarusEnforcer() : base(AIType.AI_Mage)
    {
        Body = 0x2F2;
        Hue = 0x0022;
        BaseSoundID = 0x174;

        SetStr(780, 870);
        SetDex(160, 200);
        SetInt(200, 240);

        SetHits(815, 825);

        SetDamage(18, 23);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 120.0, 130.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);
        SetSkill(SkillName.Swords, 90.0, 100.0);
        SetSkill(SkillName.Anatomy, 80.0, 90.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.EvalInt, 80.0, 100.0);
        SetSkill(SkillName.Meditation, 80.0, 100.0);

        Fame = 12000;
        Karma = -12000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a tartarus enforcer's corpse";
    public override string DefaultName => "a tartarus enforcer";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
