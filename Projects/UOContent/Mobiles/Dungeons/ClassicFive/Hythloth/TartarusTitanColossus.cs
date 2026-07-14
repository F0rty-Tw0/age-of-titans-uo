using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans sub-faction.
// L8 trash. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class TartarusTitanColossus : BaseCreature
{
    [Constructible]
    public TartarusTitanColossus() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0455;
        BaseSoundID = 604;

        SetStr(720, 790);
        SetDex(120, 150);
        SetInt(45, 70);

        SetHits(900, 930);
        SetMana(0);

        SetDamage(21, 26);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 66, 76);
        SetResistance(ResistanceType.Fire, 38, 48);
        SetResistance(ResistanceType.Cold, 33, 43);
        SetResistance(ResistanceType.Poison, 38, 48);
        SetResistance(ResistanceType.Energy, 38, 48);

        SetSkill(SkillName.MagicResist, 75.0, 110.0);
        SetSkill(SkillName.Tactics, 90.0, 110.0);
        SetSkill(SkillName.Wrestling, 90.0, 105.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 64;
    }

    public override string CorpseName => "a titan colossus's corpse";
    public override string DefaultName => "a titan colossus";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
