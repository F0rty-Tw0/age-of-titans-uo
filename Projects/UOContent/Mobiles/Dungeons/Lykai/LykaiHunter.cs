using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L3 trash. Donor: Orc.
[SerializationGenerator(0, false)]
public partial class LykaiHunter : BaseCreature
{
    [Constructible]
    public LykaiHunter() : base(AIType.AI_Archer)
    {
        Body = 17;
        Hue = 0x0964;
        BaseSoundID = 0x45A;

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

    public override string CorpseName => "an Arcadian hunter's corpse";
    public override string DefaultName => "an Arcadian hunter";

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
