using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless, L3. Donor: Wraith.
[SerializationGenerator(0, false)]
public partial class RestlessWailer : BaseCreature
{
    [Constructible]
    public RestlessWailer() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(90, 118);
        SetDex(78, 102);
        SetInt(60, 88);

        SetHits(130, 155);
        SetMana(60, 85);

        SetDamage(7, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.EvalInt, 45.0, 55.0);
        SetSkill(SkillName.Magery, 45.0, 55.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;

        PackReg(6);
    }

    public override string CorpseName => "a ghostly corpse";
    public override string DefaultName => "a wailing wraith";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
