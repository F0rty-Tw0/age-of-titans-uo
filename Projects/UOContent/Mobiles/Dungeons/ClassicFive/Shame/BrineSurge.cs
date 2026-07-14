using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class BrineSurge : BaseCreature
{
    [Constructible]
    public BrineSurge() : base(AIType.AI_Mage)
    {
        Name = "a brine surge";

        Body = 16;
        Hue = 0x04F2;
        BaseSoundID = 278;

        SetStr(230, 260);
        SetDex(95, 115);
        SetInt(210, 240);

        SetHits(475, 485);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.EvalInt, 78.0, 88.0);
        SetSkill(SkillName.Magery, 78.0, 88.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        CanSwim = true;
    }

    public override string CorpseName => "a brine surge's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
