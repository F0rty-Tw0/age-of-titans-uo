using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes sub-faction. L5. Donor: StoneGargoyle.
[SerializationGenerator(0, false)]
public partial class GaianGegenesWarden : BaseCreature
{
    [Constructible]
    public GaianGegenesWarden() : base(AIType.AI_Melee)
    {
        Body = 67;
        Hue = 0x0455;
        BaseSoundID = 372;

        SetStr(235, 265);
        SetDex(55, 75);
        SetInt(40, 60);

        SetHits(340, 365);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 4000;
        Karma = -4000;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a gegenes warden's corpse";
    public override string DefaultName => "a gegenes warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

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
        AddLoot(LootPack.Average);
    }
}
