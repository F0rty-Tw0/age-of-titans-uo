using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 core family. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class TideRower : BaseCreature
{
    [Constructible]
    public TideRower() : base(AIType.AI_Melee)
    {
        Body = 50;
        Hue = 0x08A5;
        BaseSoundID = 0x48D;

        SetStr(70, 90);
        SetDex(45, 60);
        SetInt(15, 25);

        SetHits(100, 140);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 18, 26);
        SetResistance(ResistanceType.Cold, 22, 32);
        SetResistance(ResistanceType.Poison, 10, 15);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 800;
        Karma = -800;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a barnacled skeletal corpse";
    public override string DefaultName => "a barnacled rower";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
