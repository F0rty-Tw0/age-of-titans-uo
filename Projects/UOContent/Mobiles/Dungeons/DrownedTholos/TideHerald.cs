using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - boss. Donor: Kraken.
[SerializationGenerator(0, false)]
public partial class TideHerald : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath }; // "Deluge"

    [Constructible]
    public TideHerald() : base(AIType.AI_Melee)
    {
        Name = "Aigaion";
        Title = "the Drowned Herald";

        Body = 77;
        Hue = 0x0850;
        BaseSoundID = 353;

        SetStr(260, 290);
        SetDex(90, 110);
        SetInt(60, 80);

        SetHits(370, 380);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 70, 80);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 12000;
        Karma = -12000;

        VirtualArmor = 60;

        // Kraken donor is water-locked (CantWalk); this boss must work in a land-based
        // boss room, so it swims AND walks.
        CanSwim = true;
    }

    public override string CorpseName => "a drowned herald's corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 5;
    public override int EliteBagCount => 2;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Sovereign's Undertow: 30% chance to drag under and sap stamina. Knockback is a
        // flavor message only - no position change (no knockback primitive in this codebase).
        if (Utility.RandomDouble() < 0.30)
        {
            defender.Stam -= 20;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The sovereign tide drags you under!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
