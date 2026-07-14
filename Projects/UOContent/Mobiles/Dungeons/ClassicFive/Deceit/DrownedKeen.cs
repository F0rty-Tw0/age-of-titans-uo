using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L5 trash. Donor: Bogle.
[SerializationGenerator(0, false)]
public partial class DrownedKeen : BaseCreature
{
    [Constructible]
    public DrownedKeen() : base(AIType.AI_Mage)
    {
        Body = 153;
        Hue = 0x0847;
        BaseSoundID = 0x482;

        SetStr(120, 145);
        SetDex(88, 108);
        SetInt(175, 200);

        SetHits(330, 350);

        SetDamage(14, 19);

        SetResistance(ResistanceType.Physical, 38, 48);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 78.0, 90.0);
        SetSkill(SkillName.Magery, 78.0, 90.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 62.0, 74.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 1950;
        Karma = -1950;

        VirtualArmor = 43;
    }

    public override string CorpseName => "a bone-green ghostly corpse";
    public override string DefaultName => "a keening dead";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
