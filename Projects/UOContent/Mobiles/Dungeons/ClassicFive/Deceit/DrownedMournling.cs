using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L4 trash. Donor: Ghoul.
[SerializationGenerator(0, false)]
public partial class DrownedMournling : BaseCreature
{
    [Constructible]
    public DrownedMournling() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0835;
        BaseSoundID = 0x482;

        SetStr(110, 130);
        SetDex(95, 115);
        SetInt(40, 55);

        SetHits(185, 195);

        SetDamage(9, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 34);
        SetResistance(ResistanceType.Cold, 26, 34);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 14, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1300;
        Karma = -1300;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a mournling's husk";
    public override string DefaultName => "a mournling";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
