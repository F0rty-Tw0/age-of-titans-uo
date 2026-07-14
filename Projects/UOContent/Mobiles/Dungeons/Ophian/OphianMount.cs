using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum (dev-docs/gap-families-bestiary.md §6.5) - Terathan Keep serpents, L6 trash.
// Donor: Nightmare (body/breath only - untameable coil-mount, not a BaseMount).
[SerializationGenerator(0, false)]
public partial class OphianMount : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public OphianMount() : base(AIType.AI_Melee)
    {
        Body = 116;
        Hue = 0x0453;
        BaseSoundID = 0x16A;

        SetStr(320, 350);
        SetDex(105, 125);
        SetInt(60, 80);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Fire, 40);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "an ophian coil-mount's corpse";
    public override string DefaultName => "an ophian coil-mount";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
