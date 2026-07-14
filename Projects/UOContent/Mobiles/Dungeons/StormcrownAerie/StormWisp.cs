using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder.md §4) - L8 trash. Donor: Wisp.
[SerializationGenerator(0, false)]
public partial class StormWisp : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Static Breath"

    [Constructible]
    public StormWisp() : base(AIType.AI_Mage)
    {
        Body = 58;
        Hue = 0x0480;
        BaseSoundID = 466;

        SetStr(260, 300);
        SetDex(200, 230);
        SetInt(220, 250);

        SetHits(780, 880);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a charged wisp's corpse";
    public override string DefaultName => "a charged wisp";

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}

// MonsterAbilities has no energy breath variant; scoped locally rather than touching the
// shared Abilities folder (out of this dungeon's file ownership) - mirrors the Nemean
// Wildwood's local PoisonBreath reskin on WyldSentinel for the same reason.
public class EnergyBreath : FireBreath
{
    public override int EnergyDamage => 100;
    public override int FireDamage => 0;
    public override int BreathEffectHue => 0x489;
}
