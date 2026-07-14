using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L6
// trash. Donor: Fire Elemental.
[SerializationGenerator(0, false)]
public partial class CinderKeryxBellows : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public CinderKeryxBellows() : base(AIType.AI_Melee)
    {
        Body = 15;
        Hue = 0x0654;
        BaseSoundID = 838;

        SetStr(230, 270);
        SetDex(50, 65);
        SetInt(70, 90);

        SetHits(480, 530);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 2300;
        Karma = -2300;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a keryx bellows-tender's cinders";
    public override string DefaultName => "a keryx bellows-tender";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
