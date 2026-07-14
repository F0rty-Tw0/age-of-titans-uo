using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian ambient. L3. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class GaianRubblecrawler : BaseCreature
{
    [Constructible]
    public GaianRubblecrawler() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0972;
        BaseSoundID = 456;

        SetStr(90, 110);
        SetDex(30, 45);
        SetInt(20, 35);

        SetHits(120, 140);

        SetDamage(7, 12);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 20, 28);

        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 40.0, 50.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 950;
        Karma = -950;

        VirtualArmor = 28;
    }

    public override string CorpseName => "a rubble-crawler's remains";
    public override string DefaultName => "a rubble-crawler";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
