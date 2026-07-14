using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder.md §2) - L6 trash; doubles as the boss-room
// custodian while CinderHeart is on respawn. Donor: Fire Elemental.
[SerializationGenerator(0, false)]
public partial class CinderSentinel : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath }; // "Forge Breath"

    [Constructible]
    public CinderSentinel() : base(AIType.AI_Melee)
    {
        Body = 15;
        Hue = 0x0654;
        BaseSoundID = 838;

        SetStr(240, 280);
        SetDex(70, 90);
        SetInt(70, 90);

        SetHits(500, 550);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 70, 80);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a forge sentinel's cinders";
    public override string DefaultName => "a forge sentinel";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
