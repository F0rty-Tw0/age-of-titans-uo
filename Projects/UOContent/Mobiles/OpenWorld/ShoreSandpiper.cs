using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L3, ambient. Donor: Eagle.
[SerializationGenerator(0, false)]
public partial class ShoreSandpiper : BaseCreature
{
    [Constructible]
    public ShoreSandpiper() : base(AIType.AI_Melee)
    {
        Body = 5;
        Hue = 0x0481;
        BaseSoundID = 0x2EE;

        SetStr(108, 132);
        SetDex(90, 112);
        SetInt(30, 45);

        SetHits(125, 145);

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

    public override string CorpseName => "an eagle corpse";
    public override string DefaultName => "a sandpiper";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override bool CanFly => true;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
