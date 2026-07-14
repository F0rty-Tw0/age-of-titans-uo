using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class TartarusGargoyleLord : BaseCreature
{
    [Constructible]
    public TartarusGargoyleLord() : base(AIType.AI_Mage)
    {
        Body = 4;
        Hue = 0x0455;
        BaseSoundID = 372;

        SetStr(345, 385);
        SetDex(165, 195);
        SetInt(225, 255);

        SetHits(645, 665);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 49, 59);
        SetResistance(ResistanceType.Fire, 41, 51);
        SetResistance(ResistanceType.Cold, 21, 29);
        SetResistance(ResistanceType.Poison, 31, 39);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 81.0, 96.0);
        SetSkill(SkillName.Magery, 81.0, 96.0);
        SetSkill(SkillName.MagicResist, 86.0, 101.0);
        SetSkill(SkillName.Tactics, 76.0, 91.0);
        SetSkill(SkillName.Wrestling, 71.0, 91.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 51;
    }

    public override string CorpseName => "a tartarus gargoyle-lord's corpse";
    public override string DefaultName => "a tartarus gargoyle-lord";

    public override bool CanFly => true;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
