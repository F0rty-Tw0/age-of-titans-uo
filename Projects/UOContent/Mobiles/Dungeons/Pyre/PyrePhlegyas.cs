using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon elite. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class PyrePhlegyas : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyrePhlegyas() : base(AIType.AI_Mage)
    {
        Name = "Phlegyas";
        Title = "the Unquenched";

        Body = 9;
        Hue = 0x0669;
        BaseSoundID = 357;

        SetStr(420, 460);
        SetDex(180, 210);
        SetInt(400, 440);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 58, 68);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "the Unquenched's corpse";

    public override bool CanFly => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Rivers of Fire: 25% chance on a landed hit to drain mana and cast the defender back.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 18;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0026, true, "Phlegyas drowns you in rivers of fire!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
