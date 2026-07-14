using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon family. L7 trash. Donor: OphidianKnight.
[SerializationGenerator(0, false)]
public partial class DrakonZealot : BaseCreature
{
    [Constructible]
    public DrakonZealot() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x066D;
        BaseSoundID = 634;

        SetStr(580, 620);
        SetDex(170, 190);
        SetInt(50, 75);

        SetHits(655, 665);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.Swords, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 11000;
        Karma = -11000;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a drakon zealot's corpse";
    public override string DefaultName => "a drakon zealot";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
