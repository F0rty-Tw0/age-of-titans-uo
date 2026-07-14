using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong. L3 trash. Donor: Brigand.
[SerializationGenerator(0, false)]
public partial class WaymanBrigand : BaseCreature
{
    [Constructible]
    public WaymanBrigand() : base(AIType.AI_Melee)
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

        SetStr(140, 165);
        SetDex(85, 105);
        SetInt(55, 75);

        SetHits(150, 190);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Swords, 55.0, 70.0);
        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 58.0, 72.0);
        SetSkill(SkillName.Wrestling, 42.0, 55.0);

        Fame = 1600;
        Karma = -1600;

        VirtualArmor = 36;

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

    public override string CorpseName => "a Wayman brigand's corpse";
    public override string DefaultName => "a Wayman brigand";

    public override bool ClickTitle => false;
    public override bool AlwaysMurderer => true;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
        AddLoot(LootPack.Meager);
    }
}
