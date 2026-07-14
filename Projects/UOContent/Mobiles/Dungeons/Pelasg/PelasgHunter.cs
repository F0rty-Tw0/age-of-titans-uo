using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9) - Painted Caves. L3 trash.
// Donor: Troglodyte (bow-equipped ranged row).
[SerializationGenerator(0, false)]
public partial class PelasgHunter : BaseCreature
{
    [Constructible]
    public PelasgHunter() : base(AIType.AI_Archer)
    {
        Body = 267;
        Hue = 0x0844;
        BaseSoundID = 0x59F;

        SetStr(125, 150);
        SetDex(90, 110);
        SetInt(35, 50);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 33);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Archery, 55.0, 70.0);
        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 48.0, 60.0);
        SetSkill(SkillName.Wrestling, 40.0, 52.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 30;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(30, 45)));
    }

    public override string CorpseName => "a Pelasgian hunter's corpse";
    public override string DefaultName => "a Pelasgian hunter";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
