using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L4 trash. Donor: Dire Wolf.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidSkin : BaseCreature
{
    [Constructible]
    public LykaiLykaonidSkin() : base(AIType.AI_Melee)
    {
        Body = 23;
        Hue = 0x0964;
        BaseSoundID = 0xE5;

        SetStr(160, 185);
        SetDex(100, 120);
        SetInt(35, 50);

        SetHits(200, 235);

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

    public override string CorpseName => "a Lykaonid skin-changer's corpse";
    public override string DefaultName => "a Lykaonid skin-changer";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
