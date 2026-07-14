using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Isthmian wreckers. L5 trash. Donor: Juka Warrior.
[SerializationGenerator(0, false)]
public partial class WaymanIsthmianReaver : BaseCreature
{
    [Constructible]
    public WaymanIsthmianReaver() : base(AIType.AI_Melee)
    {
        Body = 764;
        Hue = 0x0844;

        SetStr(270, 300);
        SetDex(130, 155);
        SetInt(50, 70);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 60.0, 72.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 3900;
        Karma = -3900;

        VirtualArmor = 49;
    }

    public override string CorpseName => "an Isthmian reaver's corpse";
    public override string DefaultName => "an Isthmian reaver";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
