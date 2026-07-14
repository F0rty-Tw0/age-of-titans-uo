using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong. L6 trash. Donor: Juka Lord.
[SerializationGenerator(0, false)]
public partial class WaymanCaptain : BaseCreature
{
    [Constructible]
    public WaymanCaptain() : base(AIType.AI_Melee)
    {
        Body = 766;
        Hue = 0x0964;

        SetStr(380, 420);
        SetDex(120, 145);
        SetInt(90, 115);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 62);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.MagicResist, 82.0, 95.0);
        SetSkill(SkillName.Tactics, 82.0, 95.0);
        SetSkill(SkillName.Wrestling, 78.0, 92.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 56;
    }

    public override string CorpseName => "the Wayman captain's corpse";
    public override string DefaultName => "the Wayman captain";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    // Cutthroat's Order: 15% chance to bark a command that saps the attacker's stamina.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            attacker.Stam -= 12;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "The captain snarls an order at the Wayman line!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
