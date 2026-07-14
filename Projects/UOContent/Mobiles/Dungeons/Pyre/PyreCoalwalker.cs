using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - coalwalk host. L5 trash. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class PyreCoalwalker : BaseCreature
{
    [Constructible]
    public PyreCoalwalker() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0453;
        BaseSoundID = 0x48D;

        SetStr(210, 240);
        SetDex(100, 120);
        SetInt(45, 65);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 28, 36);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 20, 27);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 66.0, 76.0);
        SetSkill(SkillName.Wrestling, 66.0, 76.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a coalwalking shade's ashes";
    public override string DefaultName => "a coalwalking shade";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
