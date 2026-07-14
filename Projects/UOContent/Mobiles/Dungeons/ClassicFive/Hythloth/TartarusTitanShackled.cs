using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans sub-faction.
// L8 trash. Donor: Ogre Lord.
[SerializationGenerator(0, false)]
public partial class TartarusTitanShackled : BaseCreature
{
    [Constructible]
    public TartarusTitanShackled() : base(AIType.AI_Melee)
    {
        Body = 83;
        Hue = 0x0455;
        BaseSoundID = 427;

        SetStr(790, 880);
        SetDex(86, 105);
        SetInt(56, 80);

        SetHits(880, 910);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 36, 46);
        SetResistance(ResistanceType.Cold, 36, 46);
        SetResistance(ResistanceType.Poison, 46, 56);
        SetResistance(ResistanceType.Energy, 46, 56);

        SetSkill(SkillName.MagicResist, 130.0, 145.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 95.0, 105.0);

        Fame = 15100;
        Karma = -15100;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a shackled titan's corpse";
    public override string DefaultName => "a shackled titan";

    public override PackInstinct PackInstinct => PackInstinct.Daemon;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
