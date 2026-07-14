using ModernUO.Serialization;

namespace Server.Mobiles;

// Second barrow caster (paired with NewbieMourner): a droning bone-priest of the
// unremembered. Same lesson — close the mage — with a different silhouette.
[SerializationGenerator(0, false)]
public partial class NewbieChanter : BaseCreature
{
    [Constructible]
    public NewbieChanter() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x047E;
        BaseSoundID = 451;

        SetStr(35, 45);
        SetDex(35, 45);
        SetInt(65, 80);

        SetHits(66, 80);

        SetDamage(2, 5);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 10, 16);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 10, 20);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.EvalInt, 35.0, 45.0);
        SetSkill(SkillName.Magery, 35.0, 45.0);
        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 20.0, 30.0);
        SetSkill(SkillName.Wrestling, 20.0, 30.0);

        Fame = 220;
        Karma = -220;

        VirtualArmor = 16;
    }

    public override string CorpseName => "a chanter's corpse";
    public override string DefaultName => "a barrow chanter";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
