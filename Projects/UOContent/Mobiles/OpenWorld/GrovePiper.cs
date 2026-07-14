using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves, L3. Donor: Satyr.
[SerializationGenerator(0, false)]
public partial class GrovePiper : BaseCreature
{
    [Constructible]
    public GrovePiper() : base(AIType.AI_Melee)
    {
        Body = 271;
        Hue = 0x0491;
        BaseSoundID = 0x586;

        SetStr(112, 135);
        SetDex(82, 100);
        SetInt(34, 50);

        SetHits(135, 160);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a satyr's corpse";
    public override string DefaultName => "a satyr piper";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    // Beguiling Pipes: 20% chance to sap stamina on a landed swing.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 8;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The piper's tune saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
