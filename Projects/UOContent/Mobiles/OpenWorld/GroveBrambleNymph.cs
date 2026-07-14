using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, Bramble Court, L3. Donor: Pixie.
[SerializationGenerator(0, false)]
public partial class GroveBrambleNymph : BaseCreature
{
    [Constructible]
    public GroveBrambleNymph() : base(AIType.AI_Mage)
    {
        Body = 128;
        Hue = 0x0491;
        BaseSoundID = 0x467;

        SetStr(90, 112);
        SetDex(78, 98);
        SetInt(60, 85);

        SetHits(125, 150);
        SetMana(60, 80);

        SetDamage(7, 10);

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
    }

    public override string CorpseName => "a bramble nymph corpse";
    public override string DefaultName => "a bramble nymph";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
