using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4, the Wreckers. Donor: Evil Mage
// (human body; T2A has no distinct witch body).
[SerializationGenerator(0, false)]
public partial class ShoreWreckWitch : BaseCreature
{
    [Constructible]
    public ShoreWreckWitch() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0491;

        SetStr(135, 158);
        SetDex(86, 106);
        SetInt(78, 100);

        SetHits(205, 238);
        SetMana(78, 100);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.EvalInt, 58.0, 68.0);
        SetSkill(SkillName.Magery, 58.0, 68.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;

        PackReg(8);
    }

    public override string DefaultName => "a wreck-witch";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    // False Beacon: 20% chance to sap stamina on a landed swing.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 10;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The wreck-witch's false beacon leaves you disoriented!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
