using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L7. Donor: Ice Elemental.
[SerializationGenerator(0, false)]
public partial class BrineHailspite : BaseCreature
{
    [Constructible]
    public BrineHailspite() : base(AIType.AI_Mage)
    {
        Name = "a hailspite";

        Body = 161;
        Hue = 0x0480;
        BaseSoundID = 268;

        SetStr(260, 290);
        SetDex(105, 125);
        SetInt(250, 280);

        SetHits(620, 655);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Cold, 75);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 100, 100);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 82.0, 97.0);
        SetSkill(SkillName.Magery, 82.0, 97.0);
        SetSkill(SkillName.MagicResist, 97.0, 112.0);
        SetSkill(SkillName.Tactics, 77.0, 92.0);
        SetSkill(SkillName.Wrestling, 72.0, 87.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 64;
    }

    public override string CorpseName => "a hailspite's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
