using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L5 trash. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class CursedWight : BaseCreature
{
    [Constructible]
    public CursedWight() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0851;
        BaseSoundID = 0x48D;

        SetStr(210, 240);
        SetDex(110, 135);
        SetInt(50, 70);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 32, 40);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a dig-wight's bones";
    public override string DefaultName => "a dig-wight";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
