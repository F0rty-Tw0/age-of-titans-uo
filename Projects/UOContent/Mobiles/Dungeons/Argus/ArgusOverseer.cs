using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L6 trash.
// Donor: Elder Gazer.
[SerializationGenerator(0, false)]
public partial class ArgusOverseer : BaseCreature
{
    [Constructible]
    public ArgusOverseer() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0479;
        BaseSoundID = 377;

        SetStr(330, 360);
        SetDex(120, 150);
        SetInt(360, 400);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 52, 62);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 88.0, 98.0);
        SetSkill(SkillName.Magery, 88.0, 98.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 78.0, 92.0);
        SetSkill(SkillName.Wrestling, 75.0, 90.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 56;
    }

    public override string CorpseName => "an all-seeing overseer's corpse";
    public override string DefaultName => "an all-seeing overseer";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    // Hundred Eyes: 15% chance to reflect ~20% of the incoming blow back as energy damage.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            AOS.Damage(attacker, this, damage * 20 / 100, 0, 0, 0, 0, 100);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "A hundred eyes turn your blow into a bolt of energy!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
