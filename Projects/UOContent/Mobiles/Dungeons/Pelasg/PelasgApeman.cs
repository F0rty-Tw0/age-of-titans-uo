using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves. L3 trash. Donor: Gorilla.
[SerializationGenerator(0, false)]
public partial class PelasgApeman : BaseCreature
{
    [Constructible]
    public PelasgApeman() : base(AIType.AI_Melee)
    {
        Body = 0x1D;
        Hue = 0x0967;
        BaseSoundID = 0x9E;

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

    public override string CorpseName => "a painted ape-man's corpse";
    public override string DefaultName => "a painted ape-man";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Poor);
    }
}
