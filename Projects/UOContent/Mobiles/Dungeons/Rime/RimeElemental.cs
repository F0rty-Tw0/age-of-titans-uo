using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L5 trash. Donor: Snow Elemental.
[SerializationGenerator(0, false)]
public partial class RimeElemental : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() };

    [Constructible]
    public RimeElemental() : base(AIType.AI_Melee)
    {
        Body = 163;
        Hue = 0x0485;
        BaseSoundID = 263;

        SetStr(210, 240);
        SetDex(130, 155);
        SetInt(80, 100);

        SetHits(320, 370);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 80);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 78.0, 88.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a snowbound elemental's remains";
    public override string DefaultName => "a snowbound elemental";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
