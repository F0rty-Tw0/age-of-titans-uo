using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// L2 ranged: the bone bowman's meaner sibling in the deeper halls. Faster draw, real
// pressure — teaches sustained kiting under fire before the elite depth.
[SerializationGenerator(0, false)]
public partial class NewbieGraveArcher : BaseCreature
{
    [Constructible]
    public NewbieGraveArcher() : base(AIType.AI_Archer)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0385;
        BaseSoundID = 0x48D;

        SetStr(40, 55);
        SetDex(70, 85);
        SetInt(15, 25);

        SetHits(66, 82);

        SetDamage(3, 6);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 12, 18);
        SetResistance(ResistanceType.Cold, 12, 20);

        SetSkill(SkillName.Archery, 40.0, 50.0);
        SetSkill(SkillName.MagicResist, 20.0, 30.0);
        SetSkill(SkillName.Tactics, 30.0, 40.0);

        Fame = 250;
        Karma = -250;

        VirtualArmor = 16;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(15, 25)));
    }

    public override string CorpseName => "a grave archer's corpse";
    public override string DefaultName => "a grave archer";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
