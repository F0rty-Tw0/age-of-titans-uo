using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L4 trash. Donor: Ghoul.
[SerializationGenerator(0, false)]
public partial class DrownedFloater : BaseCreature
{
    [Constructible]
    public DrownedFloater() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0830;
        BaseSoundID = 0x482;

        SetStr(112, 132);
        SetDex(96, 116);
        SetInt(40, 55);

        SetHits(185, 205);

        SetDamage(10, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 34);
        SetResistance(ResistanceType.Cold, 26, 34);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 14, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1350;
        Karma = -1350;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a floater's husk";
    public override string DefaultName => "a drowned floater";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
