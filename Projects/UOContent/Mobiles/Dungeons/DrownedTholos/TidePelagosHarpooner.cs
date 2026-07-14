using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor:
// Skeleton. DEVIATION: no stock BoneArcher/SkeletalArcher exists in this codebase (verified by
// search), so "Archer" AI follows the real established ranged pattern from RatmanArcher.cs -
// AI_Archer with a bow equipped via AddItem and arrows packed via PackItem.
[SerializationGenerator(0, false)]
public partial class TidePelagosHarpooner : BaseCreature
{
    [Constructible]
    public TidePelagosHarpooner() : base(AIType.AI_Archer)
    {
        Body = 50;
        Hue = 0x0481;
        BaseSoundID = 0x48D;

        SetStr(150, 180);
        SetDex(65, 85);
        SetInt(20, 35);

        SetHits(280, 320);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 38);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.Archery, 65.0, 80.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 63.0, 73.0);

        Fame = 1350;
        Karma = -1350;

        VirtualArmor = 36;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
    }

    public override string CorpseName => "a harpooner's corpse";
    public override string DefaultName => "a Pelagos harpooner";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
