using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong. L4 trash. Donor: Juka Warrior.
[SerializationGenerator(0, false)]
public partial class WaymanThug : BaseCreature
{
    [Constructible]
    public WaymanThug() : base(AIType.AI_Melee)
    {
        Body = 764;
        Hue = 0x0964;

        SetStr(200, 230);
        SetDex(110, 135);
        SetInt(60, 80);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 55.0, 68.0);
        SetSkill(SkillName.Tactics, 65.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 78.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a Wayman thug's corpse";
    public override string DefaultName => "a Wayman thug";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
