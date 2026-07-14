using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - Orc Caves, L5 trash. Donor: Orc Brute.
[SerializationGenerator(0, false)]
public partial class LykaiBrute : BaseCreature
{
    [Constructible]
    public LykaiBrute() : base(AIType.AI_Melee)
    {
        Body = 189;
        Hue = 0x0844;
        BaseSoundID = 0x45A;

        SetStr(280, 310);
        SetDex(70, 90);
        SetInt(40, 55);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 48;
    }

    public override string CorpseName => "an Arcadian brute's corpse";
    public override string DefaultName => "an Arcadian brute";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
