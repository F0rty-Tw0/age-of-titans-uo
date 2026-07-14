using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 trash. Donor: Grey Wolf.
[SerializationGenerator(0, false)]
public partial class RimeHowler : BaseCreature
{
    [Constructible]
    public RimeHowler() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(25, 27);
        Hue = 0x047E;
        BaseSoundID = 0xE5;

        SetStr(150, 175);
        SetDex(140, 165);
        SetInt(40, 55);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a frost howler's corpse";
    public override string DefaultName => "a frost howler";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
