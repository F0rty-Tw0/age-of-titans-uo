using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L6 trash. Donor: Spectral Armour.
[SerializationGenerator(0, false)]
public partial class CursedArmour : BaseCreature
{
    [Constructible]
    public CursedArmour() : base(AIType.AI_Melee)
    {
        Body = 637;
        Hue = 0x0AA8;

        SetStr(280, 310);
        SetDex(120, 145);
        SetInt(70, 95);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Cold, 25);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 38, 46);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 78.0, 88.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 52;
    }

    public override string CorpseName => "an inanimate suit of armour";
    public override string DefaultName => "an animate spectral armour";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
