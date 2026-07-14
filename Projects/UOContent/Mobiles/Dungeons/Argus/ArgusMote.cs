using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault (dev-docs/gap-families-bestiary.md §6.7) - Covetous. L3 trash.
// Donor: Gazer Larva.
[SerializationGenerator(0, false)]
public partial class ArgusMote : BaseCreature
{
    [Constructible]
    public ArgusMote() : base(AIType.AI_Mage)
    {
        Body = 778;
        Hue = 0x0486;
        BaseSoundID = 377;

        SetStr(100, 120);
        SetDex(70, 90);
        SetInt(110, 140);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 55.0, 65.0);
        SetSkill(SkillName.Magery, 55.0, 65.0);
        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 40.0, 50.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a gazing mote's remains";
    public override string DefaultName => "a gazing mote";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
