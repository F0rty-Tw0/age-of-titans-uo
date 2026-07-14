using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Terathan Keep serpents, L7 trash. Donor: Wyvern.
[SerializationGenerator(0, false)]
public partial class OphianDrakon : BaseCreature
{
    // MonsterAbilities has no poison breath variant; scoped locally rather than touching the
    // shared Abilities folder (out of this dungeon's file ownership).
    private static readonly MonsterAbility[] _abilities = { new OphianVenomBreath() }; // "Venom Breath"

    [Constructible]
    public OphianDrakon() : base(AIType.AI_Melee)
    {
        Body = 62;
        Hue = 0x0847;
        BaseSoundID = 362;

        SetStr(420, 460);
        SetDex(130, 155);
        SetInt(55, 70);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 54, 62);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 32, 40);
        SetResistance(ResistanceType.Poison, 48, 56);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.Poisoning, 70.0, 88.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 82.0, 94.0);
        SetSkill(SkillName.Wrestling, 82.0, 94.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 60;
    }

    public override string CorpseName => "an ophian drakon's corpse";
    public override string DefaultName => "an ophian drakon";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}

// Poison-damage reskin of FireBreath, local to the Ophian expansion (see comment on _abilities above).
public class OphianVenomBreath : FireBreath
{
    public override int PoisonDamage => 100;
    public override int FireDamage => 0;
    public override int BreathEffectHue => 0x0851;
}
