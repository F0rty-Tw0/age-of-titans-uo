using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L7 core. Donor: OphidianKnight.
[SerializationGenerator(0, false)]
public partial class DrakonOphidianAvenger : BaseCreature
{
    [Constructible]
    public DrakonOphidianAvenger() : base(AIType.AI_Melee)
    {
        Body = 86;
        Hue = 0x066D;
        BaseSoundID = 634;

        SetStr(580, 620);
        SetDex(160, 180);
        SetInt(60, 85);

        SetHits(650, 670);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.Swords, 85.0, 100.0);
        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9200;
        Karma = -9200;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a drakon avenger's corpse";
    public override string DefaultName => "a drakon avenger";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
