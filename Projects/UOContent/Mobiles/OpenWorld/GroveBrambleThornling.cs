using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, Bramble Court, L2. Donor: Pixie.
[SerializationGenerator(0, false)]
public partial class GroveBrambleThornling : BaseCreature
{
    [Constructible]
    public GroveBrambleThornling() : base(AIType.AI_Mage)
    {
        Body = 128;
        Hue = 0x0851;
        BaseSoundID = 0x467;

        SetStr(65, 85);
        SetDex(62, 82);
        SetInt(45, 65);

        SetHits(80, 95);
        SetMana(40, 55);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 25);
        SetResistance(ResistanceType.Fire, 5, 10);
        SetResistance(ResistanceType.Cold, 5, 10);
        SetResistance(ResistanceType.Poison, 10, 15);
        SetResistance(ResistanceType.Energy, 5, 10);

        SetSkill(SkillName.EvalInt, 30.0, 40.0);
        SetSkill(SkillName.Magery, 30.0, 40.0);
        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 550;
        Karma = -550;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a thornling corpse";
    public override string DefaultName => "a thornling";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
