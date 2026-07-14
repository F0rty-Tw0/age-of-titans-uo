using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Shore, L4. Donor: Evil Mage
// (human body; T2A has no distinct hag body).
[SerializationGenerator(0, false)]
public partial class ShoreSeahag : BaseCreature
{
    [Constructible]
    public ShoreSeahag() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0481;

        SetStr(135, 158);
        SetDex(86, 106);
        SetInt(78, 100);

        SetHits(205, 235);
        SetMana(78, 100);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 43);
        SetResistance(ResistanceType.Fire, 16, 24);
        SetResistance(ResistanceType.Cold, 16, 24);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 16, 22);

        SetSkill(SkillName.EvalInt, 58.0, 68.0);
        SetSkill(SkillName.Magery, 58.0, 68.0);
        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 38;

        PackReg(8);
    }

    public override string DefaultName => "a sea hag";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
