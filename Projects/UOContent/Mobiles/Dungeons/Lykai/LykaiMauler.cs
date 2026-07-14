using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L4 trash. Donor: Orc Brute.
[SerializationGenerator(0, false)]
public partial class LykaiMauler : BaseCreature
{
    [Constructible]
    public LykaiMauler() : base(AIType.AI_Melee)
    {
        Body = 189;
        Hue = 0x0844;
        BaseSoundID = 0x45A;

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

        Fame = 2900;
        Karma = -2900;

        VirtualArmor = 40;
    }

    public override string CorpseName => "an Arcadian mauler's corpse";
    public override string DefaultName => "an Arcadian mauler";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
