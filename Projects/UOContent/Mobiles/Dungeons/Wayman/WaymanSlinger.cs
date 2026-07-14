using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L3 trash. Donor: Brigand.
[SerializationGenerator(0, false)]
public partial class WaymanSlinger : BaseCreature
{
    [Constructible]
    public WaymanSlinger() : base(AIType.AI_Archer)
    {
        Hue = 0x0964;

        if (Female = Utility.RandomBool())
        {
            Body = 0x191;
            Name = NameList.RandomName("female");
        }
        else
        {
            Body = 0x190;
            Name = NameList.RandomName("male");
        }

        SetStr(110, 135);
        SetDex(100, 125);
        SetInt(55, 75);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 36);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Archery, 58.0, 72.0);
        SetSkill(SkillName.MagicResist, 38.0, 48.0);
        SetSkill(SkillName.Tactics, 52.0, 65.0);
        SetSkill(SkillName.Wrestling, 35.0, 48.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 32;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(40, 60)));
        AddItem(new Boots(Utility.RandomNeutralHue()));
        AddItem(new FancyShirt());
        AddItem(new Bandana());

        Utility.AssignRandomHair(this);
    }

    public override string CorpseName => "a Wayman slinger's corpse";
    public override string DefaultName => "a Wayman slinger";

    public override bool ClickTitle => false;
    public override bool AlwaysMurderer => true;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
        AddLoot(LootPack.Meager);
    }
}
