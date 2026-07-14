using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus support. L7 trash.
// Donor: Chaos Daemon.
[SerializationGenerator(0, false)]
public partial class TartarusChaosbrand : BaseCreature
{
    [Constructible]
    public TartarusChaosbrand() : base(AIType.AI_Melee)
    {
        Body = 792;
        Hue = 0x0022;
        BaseSoundID = 0x3E9;

        SetStr(325, 365);
        SetDex(222, 252);
        SetInt(102, 132);

        SetHits(640, 660);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 85);
        SetDamageType(ResistanceType.Fire, 15);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 61, 71);
        SetResistance(ResistanceType.Cold, 41, 51);
        SetResistance(ResistanceType.Poison, 26, 36);
        SetResistance(ResistanceType.Energy, 26, 36);

        SetSkill(SkillName.MagicResist, 91.0, 101.0);
        SetSkill(SkillName.Tactics, 81.0, 91.0);
        SetSkill(SkillName.Wrestling, 96.0, 106.0);

        Fame = 9100;
        Karma = -9100;

        VirtualArmor = 41;
    }

    public override string CorpseName => "a chaos-brand's corpse";
    public override string DefaultName => "a chaos-brand";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
