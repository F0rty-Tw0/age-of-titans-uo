using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L6 trash. Donor: Orcish Lord.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidHoundmaster : BaseCreature
{
    [Constructible]
    public LykaiLykaonidHoundmaster() : base(AIType.AI_Melee)
    {
        Body = 138;
        Hue = 0x0483;
        BaseSoundID = 0x45A;

        SetStr(300, 330);
        SetDex(120, 140);
        SetInt(50, 65);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a Lykaonid hound-master's corpse";
    public override string DefaultName => "a Lykaonid hound-master";

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 5;

    // Pelt-Ward: 20% chance to reflect 20% of an incoming blow back as physical damage.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.20 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            AOS.Damage(attacker, this, damage * 20 / 100, 100, 0, 0, 0, 0);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The pelt turns your blow back upon you!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
