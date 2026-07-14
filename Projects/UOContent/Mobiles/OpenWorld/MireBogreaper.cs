using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, L6. Donor: Reaper.
// HasBreath Poison ("Pollen Breath") reuses the local PoisonBreath reskin defined in WyldSentinel.cs
// (dev-docs/open-world-bestiary.md notes this as the established free/uncounted FireBreath reskin).
[SerializationGenerator(0, false)]
public partial class MireBogreaper : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new PoisonBreath() };

    [Constructible]
    public MireBogreaper() : base(AIType.AI_Mage)
    {
        Body = 47;
        Hue = 0x0851;
        BaseSoundID = 442;

        SetStr(330, 380);
        SetDex(112, 136);
        SetInt(115, 140);
        SetMana(115, 140);

        SetHits(480, 540);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 34, 42);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 34, 42);

        SetSkill(SkillName.EvalInt, 90.0, 105.0);
        SetSkill(SkillName.Magery, 90.0, 105.0);
        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 75.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a reapers corpse";
    public override string DefaultName => "a bog reaper";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override Poison PoisonImmune => Poison.Lethal;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
