using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - coalwalk host. L6 trash. Donor: Skeletal Mage.
[SerializationGenerator(0, false)]
public partial class PyreCoalgeist : BaseCreature
{
    [Constructible]
    public PyreCoalgeist() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0026;
        BaseSoundID = 451;

        SetStr(190, 220);
        SetDex(110, 130);
        SetInt(240, 270);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Fire, 60);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a coal geist's ashes";
    public override string DefaultName => "a coal geist";

    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
