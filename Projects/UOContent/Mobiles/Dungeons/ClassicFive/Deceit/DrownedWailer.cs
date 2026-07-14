using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L5 trash. Donor: Wraith.
[SerializationGenerator(0, false)]
public partial class DrownedWailer : BaseCreature
{
    [Constructible]
    public DrownedWailer() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0841;
        BaseSoundID = 0x482;

        SetStr(120, 145);
        SetDex(90, 110);
        SetInt(175, 200);

        SetHits(325, 335);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Cold, 50);

        SetResistance(ResistanceType.Physical, 36, 46);
        SetResistance(ResistanceType.Cold, 34, 44);
        SetResistance(ResistanceType.Poison, 22, 32);

        SetSkill(SkillName.EvalInt, 78.0, 90.0);
        SetSkill(SkillName.Magery, 78.0, 90.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 62.0, 74.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 1900;
        Karma = -1900;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a keening ghostly corpse";
    public override string DefaultName => "a drowned wailer";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
