using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: "Skeletal
// Knight" per the design doc is body 57 (BoneKnight.cs) - same naming quirk established by
// TideHoplite.cs; the SkeletalKnight.cs class (body 147) is the doc's "Bone Knight" donor.
[SerializationGenerator(0, false)]
public partial class TideMarine : BaseCreature
{
    [Constructible]
    public TideMarine() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(170, 200);
        SetDex(55, 70);
        SetInt(25, 40);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 42);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a drowned marine's corpse";
    public override string DefaultName => "a drowned marine";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
