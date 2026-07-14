using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core. Donor: Gargoyle.
[SerializationGenerator(0, false)]
public partial class GaianStoneshaper : BaseCreature
{
    [Constructible]
    public GaianStoneshaper() : base(AIType.AI_Mage)
    {
        Body = 4;
        Hue = 0x0455;
        BaseSoundID = 372;

        SetStr(130, 155);
        SetDex(60, 80);
        SetInt(110, 135);

        SetHits(190, 210);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 34, 42);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 18, 25);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.EvalInt, 55.0, 68.0);
        SetSkill(SkillName.Magery, 55.0, 68.0);
        SetSkill(SkillName.MagicResist, 50.0, 62.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a stoneshaper's corpse";
    public override string DefaultName => "a gaian stoneshaper";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
