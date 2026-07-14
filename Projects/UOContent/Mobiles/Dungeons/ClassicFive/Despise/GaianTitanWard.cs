using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian family. L4 trash. Donor: Ogre.
[SerializationGenerator(0, false)]
public partial class GaianTitanWard : BaseCreature
{
    [Constructible]
    public GaianTitanWard() : base(AIType.AI_Melee)
    {
        Body = 1;
        Hue = 0x0455;
        BaseSoundID = 427;

        SetStr(190, 215);
        SetDex(50, 65);
        SetInt(45, 65);

        SetHits(226, 236);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 18, 25);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3100;
        Karma = -3100;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a titan-ward's corpse";
    public override string DefaultName => "a gaian titan-ward";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 3;

    // Stone skin: 15% chance to reflect ~20% of the incoming blow back at the attacker, no debuff.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage * 20 / 100, this);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The stone skin throws your blow back at you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
