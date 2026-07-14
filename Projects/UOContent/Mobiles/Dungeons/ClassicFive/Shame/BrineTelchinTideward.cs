using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family (Telchines), L6. Donor: Kraken.
[SerializationGenerator(0, false)]
public partial class BrineTelchinTideward : BaseCreature
{
    [Constructible]
    public BrineTelchinTideward() : base(AIType.AI_Melee)
    {
        Name = "a telchine tideward";

        Body = 77;
        Hue = 0x04F2;
        BaseSoundID = 353;

        SetStr(700, 740);
        SetDex(200, 220);
        SetInt(40, 60);

        SetHits(510, 530);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 30.0, 45.0);
        SetSkill(SkillName.Tactics, 60.0, 75.0);
        SetSkill(SkillName.Wrestling, 60.0, 75.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        // Kraken donor is water-locked (CantWalk); this family shares land spawners,
        // so it swims AND walks (same fix as BrineLeechEel).
        CanSwim = true;
    }

    public override string CorpseName => "a telchine tideward's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    // Undertow grip: 20% chance to sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The undertow grip saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
