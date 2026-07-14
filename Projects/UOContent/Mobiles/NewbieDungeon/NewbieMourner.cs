using ModernUO.Serialization;

namespace Server.Mobiles;

// The barrow's 10th resident and its only trash caster: teaches "close the distance on a
// mage" before the player meets the Hollow Warden elite. Deliberately frail — a weak
// Magery kit on paper-thin HP so the lesson is cheap.
[SerializationGenerator(0, false)]
public partial class NewbieMourner : BaseCreature
{
    [Constructible]
    public NewbieMourner() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(30, 40);
        SetDex(35, 45);
        SetInt(60, 75);

        SetHits(60, 78);

        SetDamage(2, 5);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 8, 14);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 10, 20);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.EvalInt, 35.0, 45.0);
        SetSkill(SkillName.Magery, 35.0, 45.0);
        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 20.0, 30.0);
        SetSkill(SkillName.Wrestling, 20.0, 30.0);

        Fame = 200;
        Karma = -200;

        VirtualArmor = 15;
    }

    public override string CorpseName => "a mourner's fading corpse";
    public override string DefaultName => "a forgotten mourner";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
