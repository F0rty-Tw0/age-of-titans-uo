using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L7 trash. Donor: Skeletal Knight.
[SerializationGenerator(0, false)]
public partial class CursedRevenant : BaseCreature
{
    [Constructible]
    public CursedRevenant() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x0AA8;
        BaseSoundID = 451;

        SetStr(340, 380);
        SetDex(150, 175);
        SetInt(90, 115);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 42, 50);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 88.0, 98.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a warded revenant's remains";
    public override string DefaultName => "a warded revenant";

    public override bool BleedImmune => true;

    public override int LootBagLevel => 6;

    // Grave Ward: 20% chance on being hit to reflect a share of the blow back as cold.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.20 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage / 4, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The grave ward lashes back with cold!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
