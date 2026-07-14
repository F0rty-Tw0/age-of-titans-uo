using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder.md §2) - L6 trash. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class CinderGargoyle : BaseCreature
{
    [Constructible]
    public CinderGargoyle() : base(AIType.AI_Melee)
    {
        Body = 4;
        Hue = 0x0966;
        BaseSoundID = 372;

        SetStr(220, 260);
        SetDex(80, 100);
        SetInt(60, 80);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a slag gargoyle's corpse";
    public override string DefaultName => "a slag gargoyle";

    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Molten Riposte: 20% chance to reflect a quarter of the hit back as fire.
        if (Utility.RandomDouble() < 0.20)
        {
            attacker.Damage(damage / 4, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Molten stone lashes back at you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
