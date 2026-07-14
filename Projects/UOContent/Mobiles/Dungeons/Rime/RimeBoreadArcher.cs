using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L5 trash. Donor: Ratman Archer.
[SerializationGenerator(0, false)]
public partial class RimeBoreadArcher : BaseCreature
{
    [Constructible]
    public RimeBoreadArcher() : base(AIType.AI_Archer)
    {
        Body = 0x8E;
        Hue = 0x047E;
        BaseSoundID = 437;

        SetStr(200, 230);
        SetDex(150, 175);
        SetInt(70, 90);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.Archery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 46;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
    }

    public override string CorpseName => "a Boread frost-archer's corpse";
    public override string DefaultName => "a Boread frost-archer";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
