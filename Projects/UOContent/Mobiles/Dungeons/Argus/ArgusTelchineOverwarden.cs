using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L8 trash. Donor: Elder Gazer.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineOverwarden : BaseCreature
{
    [Constructible]
    public ArgusTelchineOverwarden() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0479;
        BaseSoundID = 377;

        SetStr(300, 330);
        SetDex(180, 210);
        SetInt(340, 380);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 52, 60);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 42, 50);
        SetResistance(ResistanceType.Poison, 36, 44);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 12000;
        Karma = -12000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Telchine over-warden's remains";
    public override string DefaultName => "the Telchine over-warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    // Hundred-Eyed Ward: 15% chance to reflect 25% of the incoming blow back as energy damage.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            AOS.Damage(attacker, this, damage * 25 / 100, 0, 0, 0, 0, 100);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "A hundred eyes turn your blow into a bolt of energy!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
