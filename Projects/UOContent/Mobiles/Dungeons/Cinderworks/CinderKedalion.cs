using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - mini-boss, between Brontes
// (elite) and Kelmion (boss). Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class CinderKedalion : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath }; // "Pyre"

    [Constructible]
    public CinderKedalion() : base(AIType.AI_Mage)
    {
        Name = "Kedalion";
        Title = "the Bellows-Bound";

        Body = 131;
        Hue = 0x0669;
        BaseSoundID = 768;

        SetStr(310, 340);
        SetDex(90, 110);
        SetInt(150, 180);

        SetHits(545, 550);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 70, 80);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 60;
    }

    public override string CorpseName => "the Bellows-Bound's corpse";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Bellows-Wrath: 25% chance to sear the defender's magic away.
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bellows-wrath sears your magic away!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
