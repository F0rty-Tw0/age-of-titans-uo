using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L4 trash. Donor: Grizzly Bear.
[SerializationGenerator(0, false)]
public partial class PelasgCavebear : BaseCreature
{
    [Constructible]
    public PelasgCavebear() : base(AIType.AI_Melee)
    {
        Body = 212;
        Hue = 0x0967;
        BaseSoundID = 0xA3;

        SetStr(160, 185);
        SetDex(100, 120);
        SetInt(35, 50);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 42);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 15, 22);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a painted cave-bear's corpse";
    public override string DefaultName => "a painted cave-bear";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
