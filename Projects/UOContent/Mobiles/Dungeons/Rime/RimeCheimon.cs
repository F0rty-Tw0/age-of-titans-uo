using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon mini-boss, between the
// core and Abaris (elite L8). Donor: White Wyrm.
[SerializationGenerator(0, false)]
public partial class RimeCheimon : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() };

    [Constructible]
    public RimeCheimon() : base(AIType.AI_Melee)
    {
        Name = "Cheimon";
        Title = "the Deep-Winter";

        Body = Utility.RandomBool() ? 180 : 49;
        Hue = 0x0AF3;
        BaseSoundID = 362;

        SetStr(520, 560);
        SetDex(150, 180);
        SetInt(190, 220);

        SetHits(700, 720);

        SetDamage(16, 22);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 22, 32);
        SetResistance(ResistanceType.Cold, 75, 85);
        SetResistance(ResistanceType.Poison, 38, 48);
        SetResistance(ResistanceType.Energy, 38, 48);

        SetSkill(SkillName.MagicResist, 95.0, 108.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 98.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "the Deep-Winter's corpse";

    public override bool CanFly => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Deep Freeze: 25% chance on a landed hit to sap stamina. Knockback is a flavor message
    // only - no position change (no knockback primitive in this codebase).
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Cheimon's chill knocks you back!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
