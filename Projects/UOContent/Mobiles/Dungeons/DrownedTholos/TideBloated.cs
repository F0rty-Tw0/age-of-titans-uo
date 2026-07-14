using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: Rotting
// Corpse.
[SerializationGenerator(0, false)]
public partial class TideBloated : BaseCreature
{
    [Constructible]
    public TideBloated() : base(AIType.AI_Melee)
    {
        Body = 155;
        Hue = 0x0847;
        BaseSoundID = 471;

        SetStr(195, 225);
        SetDex(40, 55);
        SetInt(25, 40);

        SetHits(340, 380);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 46);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 45, 55);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 1600;
        Karma = -1600;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a bloated corpse";
    public override string DefaultName => "a bloated drowned";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Regular;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
