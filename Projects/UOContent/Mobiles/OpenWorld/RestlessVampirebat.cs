using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless, L3. Donor: Vampire Bat.
[SerializationGenerator(0, false)]
public partial class RestlessVampirebat : BaseCreature
{
    [Constructible]
    public RestlessVampirebat() : base(AIType.AI_Melee)
    {
        Body = 317;
        Hue = 0x0454;
        BaseSoundID = 0x270;

        SetStr(108, 132);
        SetDex(78, 98);
        SetInt(30, 45);

        SetHits(130, 150);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a bat's corpse";
    public override string DefaultName => "a crypt vampire bat";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
