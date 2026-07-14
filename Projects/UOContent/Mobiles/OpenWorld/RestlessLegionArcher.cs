using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless (Barrow Legion), L3. Donor: Skeleton.
[SerializationGenerator(0, false)]
public partial class RestlessLegionArcher : BaseCreature
{
    [Constructible]
    public RestlessLegionArcher() : base(AIType.AI_Archer)
    {
        Body = Utility.RandomList(50, 56);
        Hue = 0x0847;
        BaseSoundID = 0x48D;

        SetStr(108, 132);
        SetDex(95, 115);
        SetInt(30, 45);

        SetHits(130, 155);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.Archery, 55.0, 65.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(15, 25)));
    }

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "a barrow archer";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
