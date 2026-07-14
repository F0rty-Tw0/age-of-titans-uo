using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L6 trash. Donor: Golem Controller.
[SerializationGenerator(0, false)]
public partial class WaymanCogwright : BaseCreature
{
    [Constructible]
    public WaymanCogwright() : base(AIType.AI_Mage)
    {
        Body = 400;
        Hue = 0x0798;

        SetStr(280, 310);
        SetDex(110, 135);
        SetInt(260, 300);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 28, 36);
        SetResistance(ResistanceType.Cold, 28, 36);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.EvalInt, 88.0, 100.0);
        SetSkill(SkillName.Magery, 88.0, 100.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 70.0, 82.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 6100;
        Karma = -6100;

        VirtualArmor = 53;
    }

    public override string CorpseName => "a Talos cogwright's corpse";
    public override string DefaultName => "a Talos cogwright";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
