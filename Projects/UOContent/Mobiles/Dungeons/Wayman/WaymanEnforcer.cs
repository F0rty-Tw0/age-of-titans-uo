using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong. L5 trash. Donor: Juka Lord.
[SerializationGenerator(0, false)]
public partial class WaymanEnforcer : BaseCreature
{
    [Constructible]
    public WaymanEnforcer() : base(AIType.AI_Melee)
    {
        Body = 766;
        Hue = 0x0964;

        SetStr(300, 330);
        SetDex(110, 135);
        SetInt(75, 95);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 24, 32);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 68.0, 80.0);
        SetSkill(SkillName.Tactics, 72.0, 85.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a Wayman enforcer's corpse";
    public override string DefaultName => "a Wayman enforcer";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
