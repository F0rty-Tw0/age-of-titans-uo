using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - hoard-wardens, L3 trash. Donor: Gazer Larva.
[SerializationGenerator(0, false)]
public partial class ArgusScryeye : BaseCreature
{
    [Constructible]
    public ArgusScryeye() : base(AIType.AI_Mage)
    {
        Body = 778;
        Hue = 0x0486;
        BaseSoundID = 377;

        SetStr(105, 125);
        SetDex(72, 92);
        SetInt(112, 142);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 56.0, 66.0);
        SetSkill(SkillName.Magery, 56.0, 66.0);
        SetSkill(SkillName.MagicResist, 52.0, 62.0);
        SetSkill(SkillName.Tactics, 46.0, 56.0);
        SetSkill(SkillName.Wrestling, 42.0, 52.0);

        Fame = 1520;
        Karma = -1520;

        VirtualArmor = 33;
    }

    public override string CorpseName => "a scrying eye's remains";
    public override string DefaultName => "a scrying eye";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
