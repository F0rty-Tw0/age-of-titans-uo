using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus support. L7 trash.
// Donor: Chaos Daemon.
[SerializationGenerator(0, false)]
public partial class TartarusHellbrand : BaseCreature
{
    [Constructible]
    public TartarusHellbrand() : base(AIType.AI_Melee)
    {
        Body = 792;
        Hue = 0x0022;
        BaseSoundID = 0x3E9;

        SetStr(320, 360);
        SetDex(220, 250);
        SetInt(100, 130);

        SetHits(635, 645);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 85);
        SetDamageType(ResistanceType.Fire, 15);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 95.0, 105.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a hellbrand's corpse";
    public override string DefaultName => "a hellbrand";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
