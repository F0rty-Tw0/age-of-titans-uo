using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - L5 trash; doubles as the boss-room
// custodian while TideHerald is on respawn. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class TideSentinel : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath }; // "Tidal Breath"

    [Constructible]
    public TideSentinel() : base(AIType.AI_Melee)
    {
        Body = 16;
        Hue = 0x0532;
        BaseSoundID = 278;

        SetStr(170, 200);
        SetDex(60, 80);
        SetInt(45, 60);

        SetHits(320, 380);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 60, 70);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 1600;
        Karma = -1600;

        VirtualArmor = 44;

        CanSwim = true;
    }

    public override string CorpseName => "a coral-crusted corpse";
    public override string DefaultName => "a coral sentinel";

    public override Poison PoisonImmune => Poison.Regular;

    public override int LootBagLevel => 4;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
