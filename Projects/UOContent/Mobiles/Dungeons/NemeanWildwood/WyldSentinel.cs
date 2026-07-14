using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash; doubles as the boss-room
// custodian pattern used by other dungeon sentinels. Donor: Earth Elemental.
[SerializationGenerator(0, false)]
public partial class WyldSentinel : BaseCreature
{
    // MonsterAbilities has no poison breath variant; scoped locally rather than touching the
    // shared Abilities folder (out of this dungeon's file ownership).
    private static readonly MonsterAbility[] _abilities = { new PoisonBreath() }; // "Pollen Breath"

    [Constructible]
    public WyldSentinel() : base(AIType.AI_Melee)
    {
        Body = 14;
        Hue = 0x0851;
        BaseSoundID = 268;

        SetStr(240, 270);
        SetDex(80, 100);
        SetInt(90, 110);

        SetHits(640, 720);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 70, 80);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 5000;
        Karma = -5000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a mossbound corpse";
    public override string DefaultName => "a grove sentinel";

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}

// Poison-damage reskin of FireBreath, local to this dungeon (see comment on _abilities above).
public class PoisonBreath : FireBreath
{
    public override int PoisonDamage => 100;
    public override int FireDamage => 0;
    public override int BreathEffectHue => 0x496;
}
