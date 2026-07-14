using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L5 trash. Donor: Shade.
[SerializationGenerator(0, false)]
public partial class DrownedTidewraith : BaseCreature
{
    [Constructible]
    public DrownedTidewraith() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0835;
        BaseSoundID = 0x482;

        SetStr(118, 142);
        SetDex(88, 108);
        SetInt(178, 202);

        SetHits(320, 340);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Cold, 50);

        SetResistance(ResistanceType.Physical, 35, 44);
        SetResistance(ResistanceType.Cold, 33, 43);
        SetResistance(ResistanceType.Poison, 24, 34);

        SetSkill(SkillName.EvalInt, 77.0, 89.0);
        SetSkill(SkillName.Magery, 77.0, 89.0);
        SetSkill(SkillName.MagicResist, 69.0, 81.0);
        SetSkill(SkillName.Tactics, 61.0, 73.0);
        SetSkill(SkillName.Wrestling, 57.0, 67.0);

        Fame = 1900;
        Karma = -1900;

        VirtualArmor = 41;
    }

    public override string CorpseName => "a tide-wraith's remnant";
    public override string DefaultName => "a tide-wraith";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
