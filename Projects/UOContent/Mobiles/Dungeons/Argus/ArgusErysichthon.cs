using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous elite. Donor: Dragon.
[SerializationGenerator(0, false)]
public partial class ArgusErysichthon : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public ArgusErysichthon() : base(AIType.AI_Melee)
    {
        Name = "Erysichthon";
        Title = "the Ever-Hungering";

        Body = Utility.RandomList(12, 59);
        Hue = 0x0479;
        BaseSoundID = 362;

        SetStr(900, 950);
        SetDex(170, 200);
        SetInt(260, 300);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 38, 48);

        SetSkill(SkillName.EvalInt, 55.0, 65.0);
        SetSkill(SkillName.Magery, 55.0, 65.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 95.0, 108.0);
        SetSkill(SkillName.Wrestling, 92.0, 105.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "Erysichthon's corpse";

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Devour the Hoard: 25% chance to sap stamina and feed on the wound.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            Hits += HitsMax * 8 / 100;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Erysichthon's hunger knocks you back and devours your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich, 2);
        AddLoot(LootPack.Gems, 5);
    }
}
