using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 core family. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class TideConscript : BaseCreature
{
    [Constructible]
    public TideConscript() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0847;
        BaseSoundID = 471;

        SetStr(75, 95);
        SetDex(35, 50);
        SetInt(15, 25);

        SetHits(110, 150);

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

        VirtualArmor = 22;
    }

    public override string CorpseName => "a drowned conscript's corpse";
    public override string DefaultName => "a drowned conscript";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
