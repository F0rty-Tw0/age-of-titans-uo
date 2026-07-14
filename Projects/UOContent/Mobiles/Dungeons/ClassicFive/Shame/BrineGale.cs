using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family, L5. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class BrineGale : BaseCreature
{
    [Constructible]
    public BrineGale() : base(AIType.AI_Mage)
    {
        Name = "a brine gale";

        Body = 13;
        Hue = 0x04F8;
        BaseSoundID = 655;

        SetStr(170, 200);
        SetDex(140, 160);
        SetInt(160, 190);

        SetHits(305, 315);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.EvalInt, 65.0, 80.0);
        SetSkill(SkillName.Magery, 65.0, 80.0);
        SetSkill(SkillName.MagicResist, 65.0, 80.0);
        SetSkill(SkillName.Tactics, 65.0, 80.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a brine gale's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
