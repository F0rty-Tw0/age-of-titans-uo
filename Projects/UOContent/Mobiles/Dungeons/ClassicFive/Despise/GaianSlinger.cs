using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian expansion. L4 core. Donor: Ratman.
[SerializationGenerator(0, false)]
public partial class GaianSlinger : BaseCreature
{
    [Constructible]
    public GaianSlinger() : base(AIType.AI_Archer)
    {
        Body = 42;
        Hue = 0x09C4;
        BaseSoundID = 437;

        SetStr(140, 165);
        SetDex(85, 105);
        SetInt(35, 50);

        SetHits(190, 210);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);

        SetSkill(SkillName.Archery, 55.0, 68.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 36;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(30, 45)));
    }

    public override string CorpseName => "a slinger's corpse";
    public override string DefaultName => "a gaian slinger";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
