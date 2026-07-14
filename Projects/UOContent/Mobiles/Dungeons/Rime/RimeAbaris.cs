using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon elite. Donor: White Wyrm.
[SerializationGenerator(0, false)]
public partial class RimeAbaris : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() };

    [Constructible]
    public RimeAbaris() : base(AIType.AI_Melee)
    {
        Name = "Abaris";
        Title = "the Hoarfrost Herald";

        Body = Utility.RandomBool() ? 180 : 49;
        Hue = 0x047E;
        BaseSoundID = 362;

        SetStr(560, 600);
        SetDex(140, 170);
        SetInt(220, 260);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 58, 68);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 80, 90);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 105.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "the Hoarfrost Herald's corpse";

    public override bool CanFly => true;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Killing Frost: 25% chance on a landed hit to drain mana and freeze the defender's resolve.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 18;
            defender.PublicOverheadMessage(MessageType.Regular, 0x047E, true, "Abaris calls down the killing frost!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
