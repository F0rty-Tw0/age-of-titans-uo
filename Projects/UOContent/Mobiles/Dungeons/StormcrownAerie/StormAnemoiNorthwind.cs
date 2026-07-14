using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L9. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class StormAnemoiNorthwind : BaseCreature
{
    // ColdBreath is the shared reskin at Mobiles/Abilities/Fire Breath/ColdBreath.cs.
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() }; // "North Wind"

    [Constructible]
    public StormAnemoiNorthwind() : base(AIType.AI_Melee)
    {
        Body = 13;
        Hue = 0x0480;
        BaseSoundID = 655;

        SetStr(600, 680);
        SetDex(200, 230);
        SetInt(150, 180);

        SetHits(2150, 2380);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 50);
        SetDamageType(ResistanceType.Energy, 30);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a northwind spirit's corpse";
    public override string DefaultName => "a northwind spirit";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
