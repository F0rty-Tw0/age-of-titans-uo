using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L6 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class DrownedWrecklich : BaseCreature
{
    [Constructible]
    public DrownedWrecklich() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0835;
        BaseSoundID = 0x3E9;

        SetStr(175, 200);
        SetDex(110, 130);
        SetInt(260, 290);

        SetHits(460, 490);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 10);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 42, 52);
        SetResistance(ResistanceType.Fire, 24, 34);
        SetResistance(ResistanceType.Cold, 50, 60);
        SetResistance(ResistanceType.Poison, 44, 54);
        SetResistance(ResistanceType.Energy, 38, 48);

        SetSkill(SkillName.EvalInt, 85.0, 97.0);
        SetSkill(SkillName.Magery, 85.0, 97.0);
        SetSkill(SkillName.MagicResist, 82.0, 94.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 64.0, 74.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a wreck-lich's corpse";
    public override string DefaultName => "a wreck-lich";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
