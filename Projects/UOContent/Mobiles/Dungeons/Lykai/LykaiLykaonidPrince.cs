using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L6 trash. Donor: Orc Brute.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidPrince : BaseCreature
{
    [Constructible]
    public LykaiLykaonidPrince() : base(AIType.AI_Melee)
    {
        Body = 189;
        Hue = 0x0483;
        BaseSoundID = 0x45A;

        SetStr(300, 330);
        SetDex(120, 140);
        SetInt(50, 65);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a Lykaonid wolf-prince's corpse";
    public override string DefaultName => "a Lykaonid wolf-prince";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
