using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder.md §2) - boss. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class CinderHeart : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath }; // "Pyroclasm"

    [Constructible]
    public CinderHeart() : base(AIType.AI_Mage)
    {
        Name = "Kelmion";
        Title = "the Bellows-Heart";

        Body = 9;
        Hue = 0x0669;
        BaseSoundID = 357;

        SetStr(320, 350);
        SetDex(90, 110);
        SetInt(150, 180);

        SetHits(545, 550);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Fire, 60);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 70, 80);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "the Bellows-Heart's corpse";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 6;
    public override int EliteBagCount => 2;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Reforge: 12% chance to leech 8% of max HP back into the molten core.
        if (Utility.RandomDouble() < 0.12)
        {
            Hits += HitsMax * 8 / 100;
            PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*the molten core reforges itself*");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
