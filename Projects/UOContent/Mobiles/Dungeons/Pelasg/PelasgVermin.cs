using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9) - Painted Caves. L2 trash.
// Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class PelasgVermin : BaseCreature
{
    [Constructible]
    public PelasgVermin() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0844;
        BaseSoundID = 0x188;

        SetStr(60, 80);
        SetDex(60, 80);
        SetInt(20, 30);

        SetHits(80, 110);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 26);
        SetResistance(ResistanceType.Fire, 10, 16);
        SetResistance(ResistanceType.Cold, 10, 16);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 10, 16);

        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 35.0, 48.0);
        SetSkill(SkillName.Wrestling, 35.0, 48.0);

        Fame = 800;
        Karma = -800;

        VirtualArmor = 24;
    }

    public override string CorpseName => "a cave vermin's corpse";
    public override string DefaultName => "a cave vermin";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
