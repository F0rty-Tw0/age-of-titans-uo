using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame support, L5. Donor: Snow Elemental.
[SerializationGenerator(0, false)]
public partial class BrineSeep : BaseCreature
{
    [Constructible]
    public BrineSeep() : base(AIType.AI_Melee)
    {
        Name = "a brine seep";

        Body = 163;
        Hue = 0x0480;
        BaseSoundID = 263;

        SetStr(280, 310);
        SetDex(120, 140);
        SetInt(60, 85);

        SetHits(295, 305);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 80);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 100, 100);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 55.0, 70.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 90.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a brine seep's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
