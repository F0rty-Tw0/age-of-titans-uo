using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Isthmian wreckers. L4 trash. Donor: Brigand.
[SerializationGenerator(0, false)]
public partial class WaymanIsthmianCutthroat : BaseCreature
{
    [Constructible]
    public WaymanIsthmianCutthroat() : base(AIType.AI_Melee)
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

        SetStr(200, 230);
        SetDex(110, 135);
        SetInt(60, 80);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 18, 26);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.Swords, 58.0, 72.0);
        SetSkill(SkillName.MagicResist, 55.0, 68.0);
        SetSkill(SkillName.Tactics, 65.0, 80.0);
        SetSkill(SkillName.Wrestling, 58.0, 72.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 44;

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

    public override string CorpseName => "an Isthmian cutthroat's corpse";
    public override string DefaultName => "an Isthmian cutthroat";

    public override bool ClickTitle => false;
    public override bool AlwaysMurderer => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
