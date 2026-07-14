using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L3 trash.
// Donor: Gazer.
[SerializationGenerator(0, false)]
public partial class ArgusEye : BaseCreature
{
    [Constructible]
    public ArgusEye() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0486;
        BaseSoundID = 377;

        SetStr(130, 150);
        SetDex(80, 100);
        SetInt(140, 165);

        SetHits(150, 160);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 55.0, 68.0);
        SetSkill(SkillName.Magery, 55.0, 68.0);
        SetSkill(SkillName.MagicResist, 58.0, 70.0);
        SetSkill(SkillName.Tactics, 50.0, 62.0);
        SetSkill(SkillName.Wrestling, 48.0, 60.0);

        Fame = 1650;
        Karma = -1650;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a watching eye's remains";
    public override string DefaultName => "a watching eye";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
        AddLoot(LootPack.Potions);
    }
}
