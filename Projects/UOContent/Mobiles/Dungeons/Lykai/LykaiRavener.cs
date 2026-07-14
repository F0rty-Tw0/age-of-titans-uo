using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L5 trash. Donor: Dire Wolf.
[SerializationGenerator(0, false)]
public partial class LykaiRavener : BaseCreature
{
    [Constructible]
    public LykaiRavener() : base(AIType.AI_Melee)
    {
        Body = 23;
        Hue = 0x0964;
        BaseSoundID = 0xE5;

        SetStr(220, 250);
        SetDex(100, 120);
        SetInt(45, 60);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a Lykaian ravener's corpse";
    public override string DefaultName => "a Lykaian ravener";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
