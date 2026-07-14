using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Reaper.
// PoisonBreath ("Pollen Breath") reuses the local reskin defined in WyldSentinel.cs.
[SerializationGenerator(0, false)]
public partial class WyldTreant : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new PoisonBreath() };

    [Constructible]
    public WyldTreant() : base(AIType.AI_Mage)
    {
        Body = 47;
        Hue = 0x0851;
        BaseSoundID = 442;

        SetStr(200, 230);
        SetDex(70, 90);
        SetInt(130, 160);

        SetHits(640, 710);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 80);
        SetDamageType(ResistanceType.Poison, 20);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 65, 75);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 80.0, 95.0);
        SetSkill(SkillName.Magery, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 4900;
        Karma = -4900;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a grove treant's corpse";
    public override string DefaultName => "a grove treant";

    public override Poison PoisonImmune => Poison.Greater;
    public override bool DisallowAllMoves => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
