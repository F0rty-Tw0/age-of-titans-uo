using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian family. L4 trash. Donor: Lizardman.
[SerializationGenerator(0, false)]
public partial class GaianSpearborn : BaseCreature
{
    [Constructible]
    public GaianSpearborn() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(35, 36);
        Hue = 0x09C4;
        BaseSoundID = 417;

        SetStr(150, 170);
        SetDex(85, 105);
        SetInt(35, 55);

        SetHits(195, 205);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 12, 18);
        SetResistance(ResistanceType.Cold, 12, 18);
        SetResistance(ResistanceType.Poison, 18, 25);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a spearborn corpse";
    public override string DefaultName => "a gaian spearborn";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
