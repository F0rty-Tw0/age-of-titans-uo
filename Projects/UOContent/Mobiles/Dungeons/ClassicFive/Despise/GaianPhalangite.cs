using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian family. L4 trash. Donor: Ettin.
[SerializationGenerator(0, false)]
public partial class GaianPhalangite : BaseCreature
{
    [Constructible]
    public GaianPhalangite() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x0455;
        BaseSoundID = 367;

        SetStr(185, 210);
        SetDex(60, 80);
        SetInt(40, 60);

        SetHits(208, 218);

        SetDamage(13, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 47);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 74.0);

        Fame = 2900;
        Karma = -2900;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a phalangite's corpse";
    public override string DefaultName => "a gaian phalangite";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
