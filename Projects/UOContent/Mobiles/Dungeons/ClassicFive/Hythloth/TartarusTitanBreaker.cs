using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans sub-faction.
// L8 trash. Donor: Titan.
[SerializationGenerator(0, false)]
public partial class TartarusTitanBreaker : BaseCreature
{
    // MonsterAbilities has no energy breath variant; reuses the EnergyBreath reskin defined
    // in StormcrownAerie/StormWisp.cs (same Server.Mobiles namespace) rather than adding one
    // to the shared Abilities folder, out of this dungeon's file ownership.
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Levin Breath"

    [Constructible]
    public TartarusTitanBreaker() : base(AIType.AI_Mage)
    {
        Body = 76;
        Hue = 0x0022;
        BaseSoundID = 609;

        SetStr(565, 625);
        SetDex(186, 215);
        SetInt(345, 375);

        SetHits(900, 930);

        SetDamage(21, 26);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Energy, 30);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 36, 46);
        SetResistance(ResistanceType.Cold, 31, 41);
        SetResistance(ResistanceType.Poison, 36, 46);
        SetResistance(ResistanceType.Energy, 61, 71);

        SetSkill(SkillName.EvalInt, 91.0, 106.0);
        SetSkill(SkillName.Magery, 91.0, 106.0);
        SetSkill(SkillName.MagicResist, 86.0, 116.0);
        SetSkill(SkillName.Tactics, 66.0, 86.0);
        SetSkill(SkillName.Wrestling, 46.0, 56.0);

        Fame = 13800;
        Karma = -13800;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a titan-breaker's corpse";
    public override string DefaultName => "a titan-breaker";

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Thunder-fist: 20% chance to jolt the defender - stamina drain plus a knockback flavor
    // message (no knockback primitive in this codebase, mirrors TideMaw's Riptide).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The titan-breaker's fist sends a jolt through your bones!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
