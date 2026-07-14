using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Ice Elemental.
[SerializationGenerator(0, false)]
public partial class BrineGlacier : BaseCreature
{
    [Constructible]
    public BrineGlacier() : base(AIType.AI_Mage)
    {
        Name = "a brine glacier";

        Body = 161;
        Hue = 0x0480;
        BaseSoundID = 268;

        SetStr(220, 250);
        SetDex(90, 110);
        SetInt(215, 245);

        SetHits(490, 510);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Cold, 75);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 10, 20);
        SetResistance(ResistanceType.Cold, 100, 100);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.MagicResist, 90.0, 105.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a brine glacier's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
