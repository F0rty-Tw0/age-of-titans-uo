using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class TidePelagosDeckhand : BaseCreature
{
    [Constructible]
    public TidePelagosDeckhand() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0847;
        BaseSoundID = 471;

        SetStr(170, 200);
        SetDex(40, 55);
        SetInt(20, 35);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 42);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a deckhand's corpse";
    public override string DefaultName => "a Pelagos deckhand";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
