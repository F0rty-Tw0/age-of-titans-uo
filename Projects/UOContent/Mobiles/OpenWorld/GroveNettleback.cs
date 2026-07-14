using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves, L3. Donor: Giant Spider.
[SerializationGenerator(0, false)]
public partial class GroveNettleback : BaseCreature
{
    [Constructible]
    public GroveNettleback() : base(AIType.AI_Melee)
    {
        Body = 28;
        Hue = 0x0851;
        BaseSoundID = 0x388;

        SetStr(105, 128);
        SetDex(80, 98);
        SetInt(32, 48);

        SetHits(125, 150);

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

    public override string CorpseName => "a nettleback spider corpse";
    public override string DefaultName => "a nettleback spider";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison HitPoison => Poison.Lesser;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
