using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L4 trash. Donor: Ratman Archer.
[SerializationGenerator(0, false)]
public partial class RimeArcher : BaseCreature
{
    [Constructible]
    public RimeArcher() : base(AIType.AI_Archer)
    {
        Body = 0x8E;
        Hue = 0x0AF3;
        BaseSoundID = 437;

        SetStr(150, 175);
        SetDex(110, 135);
        SetInt(60, 80);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.Archery, 75.0, 88.0);
        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 42;

        AddItem(new Bow());
        PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
    }

    public override string CorpseName => "a rime-bound archer's corpse";
    public override string DefaultName => "a rime-bound archer";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
