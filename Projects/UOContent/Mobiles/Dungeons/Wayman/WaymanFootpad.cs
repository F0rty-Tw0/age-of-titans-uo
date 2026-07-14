using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L3 trash. Donor: Brigand.
[SerializationGenerator(0, false)]
public partial class WaymanFootpad : BaseCreature
{
    [Constructible]
    public WaymanFootpad() : base(AIType.AI_Melee)
    {
        Hue = 0x0844;

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

        SetStr(120, 145);
        SetDex(90, 110);
        SetInt(55, 75);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 38);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Swords, 55.0, 70.0);
        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 40.0, 52.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 34;

        AddItem(
            Utility.Random(4) switch
            {
                0 => new Longsword(),
                1 => new Broadsword(),
                2 => new Axe(),
                _ => new Club()
            }
        );
        AddItem(new Boots(Utility.RandomNeutralHue()));
        AddItem(new FancyShirt());
        AddItem(new Bandana());

        Utility.AssignRandomHair(this);
    }

    public override string CorpseName => "a road footpad's corpse";
    public override string DefaultName => "a road footpad";

    public override bool ClickTitle => false;
    public override bool AlwaysMurderer => true;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
        AddLoot(LootPack.Meager);
    }
}
