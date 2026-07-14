using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family (Telchines), L6. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class BrineTelchinGaler : BaseCreature
{
    [Constructible]
    public BrineTelchinGaler() : base(AIType.AI_Mage)
    {
        Name = "a telchine galer";

        Body = 13;
        Hue = 0x0481;
        BaseSoundID = 655;

        SetStr(200, 230);
        SetDex(170, 190);
        SetInt(200, 230);

        SetHits(475, 495);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.MagicResist, 75.0, 90.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 90.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a telchine galer's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
