using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless (ambient), L2. Donor: Eagle.
[SerializationGenerator(0, false)]
public partial class RestlessCrow : BaseCreature
{
    [Constructible]
    public RestlessCrow() : base(AIType.AI_Melee)
    {
        Body = 5;
        Hue = 0x0455;
        BaseSoundID = 0x2EE;

        SetStr(58, 75);
        SetDex(72, 92);
        SetInt(18, 28);

        SetHits(80, 90);

        SetDamage(4, 7);

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

    public override string CorpseName => "a crow's corpse";
    public override string DefaultName => "a graveyard crow";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
