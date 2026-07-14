using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong. L5 trash. Donor: Juka Mage.
[SerializationGenerator(0, false)]
public partial class WaymanHedgewizard : BaseCreature
{
    [Constructible]
    public WaymanHedgewizard() : base(AIType.AI_Mage)
    {
        Body = 765;

        SetStr(230, 260);
        SetDex(100, 125);
        SetInt(220, 250);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.EvalInt, 75.0, 85.0);
        SetSkill(SkillName.Magery, 75.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 60.0, 72.0);
        SetSkill(SkillName.Wrestling, 55.0, 68.0);

        Fame = 4000;
        Karma = -4000;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a Wayman hedge-wizard's corpse";
    public override string DefaultName => "a Wayman hedge-wizard";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
        AddLoot(LootPack.MedScrolls, 2);
    }
}
