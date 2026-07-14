using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L4 trash. Donor: Orc.
[SerializationGenerator(0, false)]
public partial class PelasgWarhunter : BaseCreature
{
    [Constructible]
    public PelasgWarhunter() : base(AIType.AI_Archer)
    {
        Body = 17;
        Hue = 0x0798;
        BaseSoundID = 0x45A;

        SetStr(145, 170);
        SetDex(115, 135);
        SetInt(35, 50);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 33, 40);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Archery, 60.0, 75.0);
        SetSkill(SkillName.MagicResist, 46.0, 56.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 45.0, 58.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 36;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(40, 60)));
    }

    public override string CorpseName => "a Pelasgian war-hunter's corpse";
    public override string DefaultName => "a Pelasgian war-hunter";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
