using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - coalwalk host. L6 trash. Donor: Wraith.
[SerializationGenerator(0, false)]
public partial class PyreAshwraith : BaseCreature
{
    [Constructible]
    public PyreAshwraith() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0453;
        BaseSoundID = 0x482;

        SetStr(200, 230);
        SetDex(140, 165);
        SetInt(220, 250);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 28, 38);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 46;
    }

    public override string CorpseName => "an ash wraith's remains";
    public override string DefaultName => "an ash wraith";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
