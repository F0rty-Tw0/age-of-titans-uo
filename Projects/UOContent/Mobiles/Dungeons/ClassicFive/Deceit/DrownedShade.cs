using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L4 trash. Donor: Spectre.
[SerializationGenerator(0, false)]
public partial class DrownedShade : BaseCreature
{
    [Constructible]
    public DrownedShade() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0835;
        BaseSoundID = 0x482;

        SetStr(95, 115);
        SetDex(80, 100);
        SetInt(140, 165);

        SetHits(205, 215);

        SetDamage(10, 15);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Cold, 50);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 18, 28);

        SetSkill(SkillName.EvalInt, 68.0, 80.0);
        SetSkill(SkillName.Magery, 68.0, 80.0);
        SetSkill(SkillName.MagicResist, 60.0, 72.0);
        SetSkill(SkillName.Tactics, 52.0, 64.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 34;
    }

    public override string CorpseName => "a drowned shade's remnant";
    public override string DefaultName => "a drowned shade";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
