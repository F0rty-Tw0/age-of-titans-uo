using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L5 trash. Donor: Skeletal Mage.
[SerializationGenerator(0, false)]
public partial class PyreKaminosAcolyte : BaseCreature
{
    [Constructible]
    public PyreKaminosAcolyte() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0655;
        BaseSoundID = 451;

        SetStr(170, 200);
        SetDex(100, 120);
        SetInt(180, 210);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Fire, 60);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 24, 32);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 62.0, 72.0);
        SetSkill(SkillName.Magery, 62.0, 72.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 58.0, 68.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a Kaminoi acolyte's ashes";
    public override string DefaultName => "a Kaminoi acolyte";

    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
