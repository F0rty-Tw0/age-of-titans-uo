using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L3. Donor: Panther.
[SerializationGenerator(0, false)]
public partial class GrovePanther : BaseCreature
{
    [Constructible]
    public GrovePanther() : base(AIType.AI_Melee)
    {
        Body = 0xD6;
        Hue = 0x0851;
        BaseSoundID = 0x462;

        SetStr(108, 135);
        SetDex(78, 100);
        SetInt(30, 50);

        SetHits(130, 155);

        SetDamage(7, 11);

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

    public override string CorpseName => "a grove panther corpse";
    public override string DefaultName => "a grove panther";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
