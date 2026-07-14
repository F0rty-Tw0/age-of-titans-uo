using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Isthmian wreckers. L7 trash. Donor: Golem Controller.
[SerializationGenerator(0, false)]
public partial class WaymanIsthmianBedwright : BaseCreature
{
    [Constructible]
    public WaymanIsthmianBedwright() : base(AIType.AI_Mage)
    {
        Body = 400;
        Hue = 0x0964;

        SetStr(320, 355);
        SetDex(125, 150);
        SetInt(300, 340);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 32, 40);
        SetResistance(ResistanceType.Poison, 23, 31);
        SetResistance(ResistanceType.Energy, 36, 44);

        SetSkill(SkillName.EvalInt, 98.0, 112.0);
        SetSkill(SkillName.Magery, 98.0, 112.0);
        SetSkill(SkillName.MagicResist, 97.0, 112.0);
        SetSkill(SkillName.Tactics, 80.0, 92.0);
        SetSkill(SkillName.Wrestling, 76.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "an Isthmian bed-wright's corpse";
    public override string DefaultName => "an Isthmian bed-wright";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 6;

    // Rack and Ruin: 20% chance to wrench the defender's frame against the iron bed's rack.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 14;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bed-wright's rack wrenches at your limbs!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
        AddLoot(LootPack.Gems, 2);
    }
}
