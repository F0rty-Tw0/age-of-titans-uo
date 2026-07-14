using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// L1 ranged trash: the barrow's first archer. Teaches "someone is shooting me — use
// cover / close in" at pillow-fight damage. AI_Archer needs a real bow + arrows
// (RatmanArcher/TidePelagosHarpooner pattern).
[SerializationGenerator(0, false)]
public partial class NewbieBoneBowman : BaseCreature
{
    [Constructible]
    public NewbieBoneBowman() : base(AIType.AI_Archer)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0482;
        BaseSoundID = 0x48D;

        SetStr(30, 40);
        SetDex(50, 65);
        SetInt(10, 20);

        SetHits(30, 45);

        SetDamage(2, 4);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 8, 14);
        SetResistance(ResistanceType.Cold, 10, 18);

        SetSkill(SkillName.Archery, 25.0, 35.0);
        SetSkill(SkillName.MagicResist, 10.0, 20.0);
        SetSkill(SkillName.Tactics, 20.0, 30.0);

        Fame = 150;
        Karma = -150;

        VirtualArmor = 12;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(10, 20)));
    }

    public override string CorpseName => "a bowman's corpse";
    public override string DefaultName => "a bone bowman";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lesser;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 0;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
