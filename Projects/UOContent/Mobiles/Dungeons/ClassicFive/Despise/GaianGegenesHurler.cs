using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes sub-faction. L5. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class GaianGegenesHurler : BaseCreature
{
    [Constructible]
    public GaianGegenesHurler() : base(AIType.AI_Archer)
    {
        Body = 75;
        Hue = 0x0455;
        BaseSoundID = 604;

        SetStr(230, 260);
        SetDex(70, 90);
        SetInt(35, 55);

        SetHits(320, 350);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 22, 28);
        SetResistance(ResistanceType.Cold, 22, 28);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.Archery, 60.0, 72.0);
        SetSkill(SkillName.MagicResist, 52.0, 62.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 3850;
        Karma = -3850;

        VirtualArmor = 46;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(30, 45)));
    }

    public override string CorpseName => "a gegenes hurler's corpse";
    public override string DefaultName => "a gegenes hurler";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
