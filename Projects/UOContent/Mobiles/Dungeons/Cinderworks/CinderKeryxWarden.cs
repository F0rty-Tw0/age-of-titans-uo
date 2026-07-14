using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - Kerykes bronze-servant line, L6
// trash. Donor: Stone Gargoyle.
[SerializationGenerator(0, false)]
public partial class CinderKeryxWarden : BaseCreature
{
    [Constructible]
    public CinderKeryxWarden() : base(AIType.AI_Melee)
    {
        Body = 67;
        Hue = 0x0798;
        BaseSoundID = 0x174;

        SetStr(250, 290);
        SetDex(60, 80);
        SetInt(40, 60);

        SetHits(490, 540);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a keryx warden's corpse";
    public override string DefaultName => "a keryx warden";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Molten Riposte: 20% chance to reflect a quarter of the hit back as fire.
        if (Utility.RandomDouble() < 0.20)
        {
            attacker.Damage(damage / 4, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bronze plating scorches you in return!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
