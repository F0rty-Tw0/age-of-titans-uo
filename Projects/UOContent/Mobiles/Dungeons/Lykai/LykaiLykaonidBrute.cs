using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L5 trash. Donor: Orc Brute.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidBrute : BaseCreature
{
    [Constructible]
    public LykaiLykaonidBrute() : base(AIType.AI_Melee)
    {
        Body = 189;
        Hue = 0x0483;
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

    public override string CorpseName => "a Lykaonid brute-prince's corpse";
    public override string DefaultName => "a Lykaonid brute-prince";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
