using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder.md §4) - L9 trash; doubles as the boss-room
// custodian while StormFather is on respawn. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class StormSentinel : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Zeus's Breath"

    [Constructible]
    public StormSentinel() : base(AIType.AI_Melee)
    {
        Body = 13;
        Hue = 0x0481;
        BaseSoundID = 655;

        SetStr(600, 680);
        SetDex(200, 230);
        SetInt(150, 180);

        SetHits(2200, 2400);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Energy, 60, 70);

        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a storm sentinel's corpse";
    public override string DefaultName => "a storm sentinel";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
