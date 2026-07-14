using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L6 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class RimeSeer : BaseCreature
{
    [Constructible]
    public RimeSeer() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0AF3;
        BaseSoundID = 0x3E9;

        SetStr(260, 300);
        SetDex(140, 165);
        SetInt(270, 300);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 76.0, 86.0);
        SetSkill(SkillName.Magery, 76.0, 86.0);
        SetSkill(SkillName.MagicResist, 66.0, 76.0);
        SetSkill(SkillName.Tactics, 74.0, 84.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a hoarfrost seer's corpse";
    public override string DefaultName => "a hoarfrost seer";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
