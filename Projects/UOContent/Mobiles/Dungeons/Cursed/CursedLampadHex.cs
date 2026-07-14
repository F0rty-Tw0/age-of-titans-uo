using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L7 trash. Donor: Evil Mage.
[SerializationGenerator(0, false)]
public partial class CursedLampadHex : BaseCreature
{
    [Constructible]
    public CursedLampadHex() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0AA8;

        SetStr(260, 300);
        SetDex(170, 200);
        SetInt(300, 340);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 38, 46);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 72.0, 82.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 9600;
        Karma = -9600;

        VirtualArmor = 52;
    }

    public override string CorpseName => "an evil mage's corpse";
    public override string DefaultName => "a Lampad hexer";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
