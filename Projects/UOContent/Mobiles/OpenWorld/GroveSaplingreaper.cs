using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L3. Donor: Reaper.
[SerializationGenerator(0, false)]
public partial class GroveSaplingreaper : BaseCreature
{
    [Constructible]
    public GroveSaplingreaper() : base(AIType.AI_Mage)
    {
        Body = 47;
        Hue = 0x0851;
        BaseSoundID = 442;

        SetStr(90, 112);
        SetDex(78, 98);
        SetInt(60, 85);

        SetHits(135, 160);
        SetMana(60, 80);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.EvalInt, 45.0, 55.0);
        SetSkill(SkillName.Magery, 45.0, 55.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a sapling reaper corpse";
    public override string DefaultName => "a sapling reaper";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
