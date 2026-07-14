using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L7 core. Donor: OphidianMage.
[SerializationGenerator(0, false)]
public partial class DrakonOphidianSeer : BaseCreature
{
    [Constructible]
    public DrakonOphidianSeer() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0501;
        BaseSoundID = 639;

        SetStr(200, 230);
        SetDex(190, 210);
        SetInt(540, 590);

        SetHits(640, 660);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 95.0, 110.0);
        SetSkill(SkillName.Magery, 95.0, 110.0);
        SetSkill(SkillName.Meditation, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 65.0, 85.0);
        SetSkill(SkillName.Wrestling, 30.0, 55.0);

        Fame = 9100;
        Karma = -9100;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a drakon seer's corpse";
    public override string DefaultName => "a drakon seer";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
