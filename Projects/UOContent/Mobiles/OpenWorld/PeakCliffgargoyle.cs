using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L5. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class PeakCliffgargoyle : BaseCreature
{
    [Constructible]
    public PeakCliffgargoyle() : base(AIType.AI_Mage)
    {
        Body = 4;
        Hue = 0x0492;
        BaseSoundID = 372;

        SetStr(200, 235);
        SetDex(95, 115);
        SetInt(110, 140);

        SetHits(320, 360);
        SetMana(90, 120);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 24, 32);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 68.0, 78.0);
        SetSkill(SkillName.Magery, 68.0, 78.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 82.0, 96.0);

        Fame = 3800;
        Karma = -3800;

        VirtualArmor = 44;

        PackReg(6);
    }

    public override string CorpseName => "a gargoyle corpse";
    public override string DefaultName => "a cliff gargoyle";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
