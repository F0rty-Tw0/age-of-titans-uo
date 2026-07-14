using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong. L6 trash. Donor: Golem Controller.
[SerializationGenerator(0, false)]
public partial class WaymanArtificer : BaseCreature
{
    [Constructible]
    public WaymanArtificer() : base(AIType.AI_Mage)
    {
        Body = 400;
        Hue = 0x0798;

        SetStr(280, 310);
        SetDex(110, 135);
        SetInt(260, 300);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 28, 36);
        SetResistance(ResistanceType.Cold, 28, 36);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.EvalInt, 88.0, 100.0);
        SetSkill(SkillName.Magery, 88.0, 100.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 70.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 6100;
        Karma = -6100;

        VirtualArmor = 53;
    }

    public override string CorpseName => "a Talos artificer's corpse";
    public override string DefaultName => "a Talos artificer";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    // Reforge: 15% chance to reflect ~25% of the incoming blow back as physical damage.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage * 25 / 100, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The artificer's reforging spell throws your blow back at you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
