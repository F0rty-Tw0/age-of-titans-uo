using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Themed sub-faction - the Nostoi. L5 trash. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class DrownedNostosDeckhand : BaseCreature
{
    [Constructible]
    public DrownedNostosDeckhand() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0835;
        BaseSoundID = 471;

        SetStr(195, 225);
        SetDex(68, 85);
        SetInt(40, 55);

        SetHits(300, 330);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 33, 41);
        SetResistance(ResistanceType.Cold, 35, 43);
        SetResistance(ResistanceType.Poison, 25, 33);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 72.0, 82.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 1750;
        Karma = -1750;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a drowned corpse";
    public override string DefaultName => "a Nostoi deckhand";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
