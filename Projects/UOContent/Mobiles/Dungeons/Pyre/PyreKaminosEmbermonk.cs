using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L6 trash. Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class PyreKaminosEmbermonk : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyreKaminosEmbermonk() : base(AIType.AI_Mage)
    {
        Body = 131;
        Hue = 0x0655;
        BaseSoundID = 768;

        SetStr(200, 230);
        SetDex(140, 165);
        SetInt(190, 220);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 60);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 58, 68);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 28, 36);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.EvalInt, 74.0, 84.0);
        SetSkill(SkillName.Magery, 74.0, 84.0);
        SetSkill(SkillName.MagicResist, 64.0, 74.0);
        SetSkill(SkillName.Tactics, 64.0, 74.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a Kaminoi ember-monk's corpse";
    public override string DefaultName => "a Kaminoi ember-monk";

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
