using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L9 core trash. Donor: Elder Gazer.
[SerializationGenerator(0, false)]
public partial class StormEye : BaseCreature
{
    [Constructible]
    public StormEye() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0491;
        BaseSoundID = 377;

        SetStr(650, 700);
        SetDex(180, 210);
        SetInt(320, 360);

        SetHits(2200, 2400);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 105.0, 115.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a tempest eye's corpse";
    public override string DefaultName => "a tempest eye";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
