using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless (ambient), L2. Donor: Chicken.
[SerializationGenerator(0, false)]
public partial class RestlessBonefinch : BaseCreature
{
    [Constructible]
    public RestlessBonefinch() : base(AIType.AI_Melee)
    {
        Body = 0xD0;
        Hue = 0x0835;
        BaseSoundID = 0x6E;

        SetStr(55, 72);
        SetDex(70, 88);
        SetInt(16, 26);

        SetHits(80, 88);

        SetDamage(4, 6);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 16, 22);
        SetResistance(ResistanceType.Fire, 5, 8);
        SetResistance(ResistanceType.Cold, 5, 8);
        SetResistance(ResistanceType.Poison, 8, 12);
        SetResistance(ResistanceType.Energy, 5, 8);

        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 35.0, 45.0);
        SetSkill(SkillName.Wrestling, 35.0, 45.0);

        Fame = 300;
        Karma = -300;

        VirtualArmor = 16;
    }

    public override string CorpseName => "a finch's corpse";
    public override string DefaultName => "a graveyard finch";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
