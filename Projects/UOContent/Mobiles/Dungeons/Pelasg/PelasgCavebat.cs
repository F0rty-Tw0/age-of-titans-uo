using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L2 ambient/fodder. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class PelasgCavebat : BaseCreature
{
    [Constructible]
    public PelasgCavebat() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0964;
        BaseSoundID = 422;

        SetStr(50, 70);
        SetDex(55, 75);
        SetInt(15, 25);

        SetHits(80, 100);

        SetDamage(4, 7);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 16, 22);
        SetResistance(ResistanceType.Fire, 8, 14);
        SetResistance(ResistanceType.Cold, 8, 14);
        SetResistance(ResistanceType.Poison, 14, 20);
        SetResistance(ResistanceType.Energy, 8, 14);

        SetSkill(SkillName.MagicResist, 26.0, 36.0);
        SetSkill(SkillName.Tactics, 30.0, 42.0);
        SetSkill(SkillName.Wrestling, 30.0, 42.0);

        Fame = 600;
        Karma = -600;

        VirtualArmor = 20;
    }

    public override string CorpseName => "a cave bat's corpse";
    public override string DefaultName => "a cave bat";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
