using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus support. L8 trash.
// Donor: Gore Fiend.
[SerializationGenerator(0, false)]
public partial class TartarusSoulflayer : BaseCreature
{
    [Constructible]
    public TartarusSoulflayer() : base(AIType.AI_Melee)
    {
        Body = 305;
        Hue = 0x0022;
        BaseSoundID = 224;

        SetStr(555, 605);
        SetDex(222, 252);
        SetInt(105, 135);

        SetHits(855, 875);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 85);
        SetDamageType(ResistanceType.Poison, 15);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 36, 46);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 41, 51);

        SetSkill(SkillName.MagicResist, 86.0, 101.0);
        SetSkill(SkillName.Tactics, 91.0, 106.0);
        SetSkill(SkillName.Wrestling, 96.0, 111.0);

        Fame = 13400;
        Karma = -13400;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a soulflayer's corpse";
    public override string DefaultName => "a soulflayer";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
