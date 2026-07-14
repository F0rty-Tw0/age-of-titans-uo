using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L7 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class PyreKaminosForgemaster : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyreKaminosForgemaster() : base(AIType.AI_Melee)
    {
        Body = 9;
        Hue = 0x0669;
        BaseSoundID = 357;

        SetStr(350, 390);
        SetDex(155, 180);
        SetInt(190, 220);

        SetHits(640, 700);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 62);
        SetResistance(ResistanceType.Fire, 58, 68);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 34, 44);
        SetResistance(ResistanceType.Energy, 34, 44);

        SetSkill(SkillName.EvalInt, 70.0, 80.0);
        SetSkill(SkillName.Magery, 70.0, 80.0);
        SetSkill(SkillName.MagicResist, 82.0, 92.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Kaminoi forgemaster's corpse";
    public override string DefaultName => "the Kaminoi forgemaster";

    public override bool CanFly => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Anvil-Fall: 25% chance on a landed hit to slam the defender back and sap their stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0669, true, "The forgemaster's anvil falls!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
