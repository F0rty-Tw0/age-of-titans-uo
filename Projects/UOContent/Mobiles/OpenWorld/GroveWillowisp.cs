using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, L2. Donor: Wisp.
[SerializationGenerator(0, false)]
public partial class GroveWillowisp : BaseCreature
{
    [Constructible]
    public GroveWillowisp() : base(AIType.AI_Mage)
    {
        Body = 58;
        Hue = 0x0491;
        BaseSoundID = 466;

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

    public override string CorpseName => "a willow wisp corpse";
    public override string DefaultName => "a willow wisp";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
