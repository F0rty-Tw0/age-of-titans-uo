using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless (Mourner's Cortège), L3. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class RestlessSexton : BaseCreature
{
    [Constructible]
    public RestlessSexton() : base(AIType.AI_Mage)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0454;
        BaseSoundID = 0x48D;

        SetStr(90, 118);
        SetDex(78, 102);
        SetInt(60, 88);

        SetHits(150, 160);
        SetMana(60, 85);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.EvalInt, 45.0, 55.0);
        SetSkill(SkillName.Magery, 45.0, 55.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;

        PackReg(6);
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "the cortège sexton";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    // Grave Chill: 20% chance to drain mana on a landed swing.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 8;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The sexton's chill sinks into your bones!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
