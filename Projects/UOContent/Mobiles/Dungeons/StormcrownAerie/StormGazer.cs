using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L8 core trash. Donor: Gazer.
[SerializationGenerator(0, false)]
public partial class StormGazer : BaseCreature
{
    [Constructible]
    public StormGazer() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0491;
        BaseSoundID = 377;

        SetStr(260, 300);
        SetDex(150, 180);
        SetInt(260, 300);

        SetHits(800, 880);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a storm gazer's corpse";
    public override string DefaultName => "a storm gazer";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
