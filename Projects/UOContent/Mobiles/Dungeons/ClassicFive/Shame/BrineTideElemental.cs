using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L6. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class BrineTideElemental : BaseCreature
{
    [Constructible]
    public BrineTideElemental() : base(AIType.AI_Melee)
    {
        Name = "a tide elemental";

        Body = 16;
        Hue = 0x04F2;
        BaseSoundID = 278;

        SetStr(240, 270);
        SetDex(100, 120);
        SetInt(100, 130);

        SetHits(510, 530);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 80.0, 95.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 58;

        CanSwim = true;
    }

    public override string CorpseName => "a tide elemental's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
