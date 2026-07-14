using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Evil Mage (human
// body; T2A has no distinct huntress body).
[SerializationGenerator(0, false)]
public partial class WyldThiasosArcher : BaseCreature
{
    [Constructible]
    public WyldThiasosArcher() : base(AIType.AI_Archer)
    {
        Body = 0x190;
        Hue = 0x0844;

        SetStr(190, 220);
        SetDex(140, 170);
        SetInt(90, 110);

        SetHits(600, 650);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 52);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.Archery, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4500;
        Karma = -4500;

        VirtualArmor = 55;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
    }

    public override string CorpseName => "a Thiasos archer's corpse";
    public override string DefaultName => "a Thiasos archer";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
