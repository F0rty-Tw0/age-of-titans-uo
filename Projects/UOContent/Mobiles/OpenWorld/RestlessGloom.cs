using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless, L2. Donor: Spectre.
[SerializationGenerator(0, false)]
public partial class RestlessGloom : BaseCreature
{
    [Constructible]
    public RestlessGloom() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0847;
        BaseSoundID = 0x482;

        SetStr(65, 85);
        SetDex(60, 78);
        SetInt(45, 65);

        SetHits(80, 95);
        SetMana(40, 55);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 25);
        SetResistance(ResistanceType.Fire, 5, 10);
        SetResistance(ResistanceType.Cold, 5, 10);
        SetResistance(ResistanceType.Poison, 10, 15);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.EvalInt, 30.0, 40.0);
        SetSkill(SkillName.Magery, 30.0, 40.0);
        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 550;
        Karma = -550;

        VirtualArmor = 20;

        PackReg(4);
    }

    public override string CorpseName => "a ghostly corpse";
    public override string DefaultName => "a graveyard gloom";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
