using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame mini-boss, L7. Donor: Kraken.
[SerializationGenerator(0, false)]
public partial class BrineOrmenos : DungeonElite
{
    [Constructible]
    public BrineOrmenos() : base(AIType.AI_Mage)
    {
        Name = "Ormenos";
        Title = "the Tide-Smith";

        Body = 77;
        Hue = 0x04F8;
        BaseSoundID = 353;

        SetStr(650, 690);
        SetDex(210, 230);
        SetInt(260, 290);

        SetHits(700, 720);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.MagicResist, 40.0, 55.0);
        SetSkill(SkillName.Tactics, 65.0, 80.0);
        SetSkill(SkillName.Wrestling, 55.0, 70.0);

        Fame = 7800;
        Karma = -7800;

        VirtualArmor = 66;

        // Kraken donor is water-locked (CantWalk); this family shares land spawners,
        // so it swims AND walks (same fix as BrineLeechEel).
        CanSwim = true;
    }

    public override string CorpseName => "the Tide-Smith's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int EliteBagLevel => 6;

    // Maelstrom: a cold breath.
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Break the deep: 25% chance to sap mana.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Ormenos shatters your focus!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
