using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - mini-boss, between the core and
// Phlegyas. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class PyrePhaethon : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyrePhaethon() : base(AIType.AI_Mage)
    {
        Name = "Phaethon";
        Title = "the Sky-Scorcher";

        Body = 9;
        Hue = 0x0669;
        BaseSoundID = 357;

        SetStr(380, 420);
        SetDex(170, 200);
        SetInt(360, 400);

        SetHits(700, 720);

        SetDamage(16, 22);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 54, 64);
        SetResistance(ResistanceType.Fire, 62, 72);
        SetResistance(ResistanceType.Cold, 32, 42);
        SetResistance(ResistanceType.Poison, 36, 46);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.EvalInt, 88.0, 98.0);
        SetSkill(SkillName.Magery, 88.0, 98.0);
        SetSkill(SkillName.MagicResist, 92.0, 102.0);
        SetSkill(SkillName.Tactics, 84.0, 94.0);
        SetSkill(SkillName.Wrestling, 74.0, 88.0);

        Fame = 13000;
        Karma = -13000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "the Sky-Scorcher's corpse";

    public override bool CanFly => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Chariot Fall: 25% chance on a landed hit to drain mana and cast the defender back.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 18;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0669, true, "Phaethon's chariot falls upon you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
