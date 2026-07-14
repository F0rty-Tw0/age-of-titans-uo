using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L3 trash. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class PelasgCaveviper : BaseCreature
{
    [Constructible]
    public PelasgCaveviper() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0844;
        BaseSoundID = 0xDB;

        SetStr(130, 155);
        SetDex(75, 95);
        SetInt(35, 50);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 18, 26);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 52.0, 65.0);
        SetSkill(SkillName.Wrestling, 52.0, 65.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a painted viper's corpse";
    public override string DefaultName => "a painted viper";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
