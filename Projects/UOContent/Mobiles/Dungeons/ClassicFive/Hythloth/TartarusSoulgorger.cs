using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus support. L7 trash.
// Donor: Gore Fiend.
[SerializationGenerator(0, false)]
public partial class TartarusSoulgorger : BaseCreature
{
    [Constructible]
    public TartarusSoulgorger() : base(AIType.AI_Melee)
    {
        Body = 305;
        Hue = 0x0021;
        BaseSoundID = 224;

        SetStr(340, 380);
        SetDex(140, 170);
        SetInt(100, 130);

        SetHits(650, 660);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 85);
        SetDamageType(ResistanceType.Poison, 15);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 75.0, 90.0);
        SetSkill(SkillName.Tactics, 80.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a soulgorger's corpse";
    public override string DefaultName => "a soulgorger";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
