using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans sub-faction.
// L8 trash. Donor: Gargoyle Enforcer.
[SerializationGenerator(0, false)]
public partial class TartarusTitanJailer : BaseCreature
{
    [Constructible]
    public TartarusTitanJailer() : base(AIType.AI_Mage)
    {
        Body = 0x2F2;
        Hue = 0x0022;
        BaseSoundID = 0x174;

        SetStr(790, 880);
        SetDex(168, 208);
        SetInt(208, 248);

        SetHits(830, 855);

        SetDamage(19, 24);

        SetResistance(ResistanceType.Physical, 58, 68);
        SetResistance(ResistanceType.Fire, 58, 68);
        SetResistance(ResistanceType.Cold, 33, 43);
        SetResistance(ResistanceType.Poison, 38, 48);
        SetResistance(ResistanceType.Energy, 28, 38);

        SetSkill(SkillName.MagicResist, 122.0, 132.0);
        SetSkill(SkillName.Tactics, 82.0, 92.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);
        SetSkill(SkillName.Swords, 92.0, 102.0);
        SetSkill(SkillName.Anatomy, 82.0, 92.0);
        SetSkill(SkillName.Magery, 92.0, 102.0);
        SetSkill(SkillName.EvalInt, 82.0, 102.0);
        SetSkill(SkillName.Meditation, 82.0, 102.0);

        Fame = 12300;
        Karma = -12300;

        VirtualArmor = 57;
    }

    public override string CorpseName => "a titan-jailer's corpse";
    public override string DefaultName => "a titan-jailer";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    // Warding flame: 15% chance to reflect ~25% of the incoming blow back at the attacker as fire.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            attacker.Damage(damage * 25 / 100, this);
            attacker.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x22, false, "The warding flame throws your blow back at you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
