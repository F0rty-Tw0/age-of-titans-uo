using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L7. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class BrineWaterlord : BaseCreature
{
    [Constructible]
    public BrineWaterlord() : base(AIType.AI_Mage)
    {
        Name = "a brine water-lord";

        Body = 16;
        Hue = 0x04F8;
        BaseSoundID = 278;

        SetStr(270, 300);
        SetDex(110, 130);
        SetInt(245, 275);

        SetHits(640, 680);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 65, 75);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 86.0, 96.0);
        SetSkill(SkillName.Magery, 86.0, 96.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 76.0, 88.0);
        SetSkill(SkillName.Wrestling, 76.0, 88.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 64;

        CanSwim = true;
    }

    public override string CorpseName => "a brine water-lord's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
