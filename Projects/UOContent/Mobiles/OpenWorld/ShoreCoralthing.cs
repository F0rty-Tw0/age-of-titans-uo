using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class ShoreCoralthing : BaseCreature
{
    [Constructible]
    public ShoreCoralthing() : base(AIType.AI_Melee)
    {
        Body = 16;
        Hue = 0x0851;
        BaseSoundID = 278;

        SetStr(158, 186);
        SetDex(84, 102);
        SetInt(42, 60);

        SetHits(210, 238);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;

        CanSwim = true;
    }

    public override string CorpseName => "a water elemental corpse";
    public override string DefaultName => "a coral horror";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Regular;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
