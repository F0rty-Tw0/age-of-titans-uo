using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L3 core. Donor: GiantSpider.
[SerializationGenerator(0, false)]
public partial class GaianRootcrawler : BaseCreature
{
    [Constructible]
    public GaianRootcrawler() : base(AIType.AI_Melee)
    {
        Body = 28;
        Hue = 0x09C2;
        BaseSoundID = 0x388;

        SetStr(100, 120);
        SetDex(70, 90);
        SetInt(25, 40);

        SetHits(135, 150);

        SetDamage(8, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 33);
        SetResistance(ResistanceType.Fire, 10, 16);
        SetResistance(ResistanceType.Cold, 10, 16);
        SetResistance(ResistanceType.Poison, 25, 35);

        SetSkill(SkillName.MagicResist, 38.0, 48.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 52.0, 62.0);

        Fame = 1200;
        Karma = -1200;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a root-crawler's corpse";
    public override string DefaultName => "a root-crawler";

    public override Poison HitPoison => Poison.Lesser;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
