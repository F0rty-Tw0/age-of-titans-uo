using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L6
// trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class CinderKeryxSmith : BaseCreature
{
    [Constructible]
    public CinderKeryxSmith() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0967;
        BaseSoundID = 456;

        SetStr(260, 300);
        SetDex(40, 55);
        SetInt(50, 70);

        SetHits(500, 550);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a keryx smith's shell";
    public override string DefaultName => "a keryx smith";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Emberbrand: 20% chance to sear the defender's magic away.
        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The ember-brand sears your magic away!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
