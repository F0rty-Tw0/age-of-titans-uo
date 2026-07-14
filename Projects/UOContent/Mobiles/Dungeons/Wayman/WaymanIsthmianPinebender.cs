using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Isthmian wreckers. L6 trash. Donor: Juka Lord.
[SerializationGenerator(0, false)]
public partial class WaymanIsthmianPinebender : BaseCreature
{
    [Constructible]
    public WaymanIsthmianPinebender() : base(AIType.AI_Melee)
    {
        Body = 766;
        Hue = 0x0844;

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

    public override string CorpseName => "an Isthmian pine-bender's corpse";
    public override string DefaultName => "an Isthmian pine-bender";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    // Pine-Snap: 20% chance to snap a bent pine-bow back on the defender, knocking them down and sapping stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The pine-bender's snapped bow sends you sprawling!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
