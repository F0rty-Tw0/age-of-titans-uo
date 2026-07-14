using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - mini-boss, between Kymopoleia
// (elite) and Aigaion (boss). Donor: Deep Sea Serpent.
[SerializationGenerator(0, false)]
public partial class TideNavarch : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath }; // "Undertow"

    [Constructible]
    public TideNavarch() : base(AIType.AI_Melee)
    {
        Name = "Nauplios";
        Title = "the Wreckwarden";

        Body = 150;
        Hue = 0x0850;
        BaseSoundID = 447;

        SetStr(220, 250);
        SetDex(80, 100);
        SetInt(60, 80);

        SetHits(375, 380);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 46, 56);
        SetResistance(ResistanceType.Cold, 38, 48);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 22, 32);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 82.0, 92.0);
        SetSkill(SkillName.Wrestling, 82.0, 92.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 54;

        CanSwim = true;
    }

    public override string CorpseName => "the Wreckwarden's corpse";

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Wreckwarden's Pull: 25% chance to drag the defender under and sap stamina.
        // Knockback is a flavor message only - no position change (no knockback primitive in
        // this codebase), same idiom as TideHerald.OnGaveMeleeAttack.
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Nauplios drags you into the wreckage!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
