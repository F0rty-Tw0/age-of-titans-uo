using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L6 trash. Donor: Bone Knight.
[SerializationGenerator(0, false)]
public partial class CursedSentinel : BaseCreature
{
    [Constructible]
    public CursedSentinel() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x0851;
        BaseSoundID = 451;

        SetStr(300, 340);
        SetDex(140, 165);
        SetInt(70, 95);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 82.0, 92.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a bound sentinel's remains";
    public override string DefaultName => "a bound sentinel";

    public override bool BleedImmune => true;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
