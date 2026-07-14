using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L7 trash. Donor: Juka Lord.
[SerializationGenerator(0, false)]
public partial class WaymanWarlord : BaseCreature
{
    [Constructible]
    public WaymanWarlord() : base(AIType.AI_Melee)
    {
        Body = 766;
        Hue = 0x0798;

        SetStr(430, 470);
        SetDex(130, 155);
        SetInt(100, 125);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 58, 68);
        SetResistance(ResistanceType.Fire, 33, 42);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.MagicResist, 92.0, 106.0);
        SetSkill(SkillName.Tactics, 92.0, 106.0);
        SetSkill(SkillName.Wrestling, 88.0, 102.0);

        Fame = 7100;
        Karma = -7100;

        VirtualArmor = 62;
    }

    public override string CorpseName => "the Wayman road-warlord's corpse";
    public override string DefaultName => "the Wayman road-warlord";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
        AddLoot(LootPack.Gems, 2);
    }
}
