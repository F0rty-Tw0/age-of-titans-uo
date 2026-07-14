using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor:
// Skeleton.
[SerializationGenerator(0, false)]
public partial class TidePelagosDrummer : BaseCreature
{
    [Constructible]
    public TidePelagosDrummer() : base(AIType.AI_Melee)
    {
        Body = 50;
        Hue = 0x08A5;
        BaseSoundID = 0x48D;

        SetStr(150, 180);
        SetDex(55, 70);
        SetInt(20, 35);

        SetHits(280, 320);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 38);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 63.0, 73.0);
        SetSkill(SkillName.Wrestling, 63.0, 73.0);

        Fame = 1350;
        Karma = -1350;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a drummer's corpse";
    public override string DefaultName => "a Pelagos drummer";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
