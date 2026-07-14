using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - mini-boss, between Enceladus
// (elite) and Typhon (boss). Donor: Titan.
[SerializationGenerator(0, false)]
public partial class StormEphialtes : DungeonElite
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Storm-Chain Breath"

    [Constructible]
    public StormEphialtes() : base(AIType.AI_Mage)
    {
        Name = "Ephialtes";
        Title = "the Storm-Chained";

        Body = 76;
        Hue = 0x0492;
        BaseSoundID = 609;

        SetStr(800, 860);
        SetDex(170, 200);
        SetInt(350, 400);

        SetHits(2380, 2400);

        SetDamage(22, 28);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 62, 72);
        SetResistance(ResistanceType.Fire, 42, 52);
        SetResistance(ResistanceType.Cold, 42, 52);
        SetResistance(ResistanceType.Energy, 68, 78);

        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 110.0);
        SetSkill(SkillName.MagicResist, 100.0, 110.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 75;
    }

    public override string CorpseName => "the Storm-Chained's corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 9;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Chain-Lash: 15% chance on being hit to reflect a share of the blow back as lightning.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Damage(damage * 3 / 10, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The chains lash back with storm-fire!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
