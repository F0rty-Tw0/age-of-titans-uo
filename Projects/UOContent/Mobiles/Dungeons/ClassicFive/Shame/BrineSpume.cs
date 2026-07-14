using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L5. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class BrineSpume : BaseCreature
{
    [Constructible]
    public BrineSpume() : base(AIType.AI_Mage)
    {
        Name = "a brine spume";

        Body = 16;
        Hue = 0x04F8;
        BaseSoundID = 278;

        SetStr(180, 210);
        SetDex(80, 100);
        SetInt(170, 200);

        SetHits(295, 305);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.EvalInt, 70.0, 80.0);
        SetSkill(SkillName.Magery, 70.0, 80.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 60.0, 75.0);
        SetSkill(SkillName.Wrestling, 60.0, 75.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 50;

        CanSwim = true;
    }

    public override string CorpseName => "a brine spume's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
