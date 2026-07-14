using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Peaks Expansion, L4. Donor: Gazer Larva.
[SerializationGenerator(0, false)]
public partial class PeakGazerling : BaseCreature
{
    [Constructible]
    public PeakGazerling() : base(AIType.AI_Mage)
    {
        Body = 778;
        Hue = 0x0455;
        BaseSoundID = 377;

        SetStr(140, 165);
        SetDex(85, 105);
        SetInt(90, 115);

        SetHits(190, 225);
        SetMana(70, 95);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.EvalInt, 58.0, 68.0);
        SetSkill(SkillName.Magery, 58.0, 68.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;

        PackReg(6);
    }

    public override string CorpseName => "a gazer corpse";
    public override string DefaultName => "a crag gazer larva";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
