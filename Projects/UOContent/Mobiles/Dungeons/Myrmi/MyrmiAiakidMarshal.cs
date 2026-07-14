using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L7 trash. Donor: Terathan Avenger.
[SerializationGenerator(0, false)]
public partial class MyrmiAiakidMarshal : BaseCreature
{
    [Constructible]
    public MyrmiAiakidMarshal() : base(AIType.AI_Melee)
    {
        Name = "the Aiakid war-marshal";

        Body = 152;
        Hue = 0x0021;
        BaseSoundID = 589;

        SetStr(400, 430);
        SetDex(150, 170);
        SetInt(60, 75);

        SetHits(700, 720);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 60);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 40, 48);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.MagicResist, 75.0, 88.0);
        SetSkill(SkillName.Tactics, 88.0, 98.0);
        SetSkill(SkillName.Wrestling, 88.0, 98.0);

        Fame = 12000;
        Karma = -12000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "the Aiakid war-marshal's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Arachnid;
    public override Poison PoisonImmune => Poison.Deadly;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 6;

    // Chitin Wall: 15% chance on being hit to reflect a share of the blow back as poison.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15 && CanBeHarmful(attacker))
        {
            DoHarmful(attacker);
            AOS.Damage(attacker, this, damage / 4, 0, 0, 0, 100, 0);
            attacker.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The chitin wall lashes back with venom!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
