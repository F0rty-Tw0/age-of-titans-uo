using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L3 trash. Donor: Grey Wolf.
[SerializationGenerator(0, false)]
public partial class LykaiTracker : BaseCreature
{
    [Constructible]
    public LykaiTracker() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(25, 27);
        Hue = 0x0483;
        BaseSoundID = 0xE5;

        SetStr(100, 125);
        SetDex(85, 105);
        SetInt(35, 50);

        SetHits(130, 160);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 12, 20);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 15, 22);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 52.0, 62.0);
        SetSkill(SkillName.Wrestling, 52.0, 62.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 30;
    }

    public override string CorpseName => "a Lykaian tracker's corpse";
    public override string DefaultName => "a Lykaian tracker";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
