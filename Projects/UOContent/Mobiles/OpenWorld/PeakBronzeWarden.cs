using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L5, Bronze Watch. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class PeakBronzeWarden : BaseCreature
{
    [Constructible]
    public PeakBronzeWarden() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0798;

        SetStr(228, 268);
        SetDex(95, 115);
        SetInt(48, 70);

        SetHits(340, 380);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 82.0, 96.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a golem's corpse";
    public override string DefaultName => "a bronze warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    // Molten Riposte: 20% chance to reflect a quarter of the incoming blow back.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.20 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage * 25 / 100, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The bronze warden's molten hide sears you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
