using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus ambient. L7 trash.
// Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class TartarusAshhound : BaseCreature
{
    [Constructible]
    public TartarusAshhound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0455;
        BaseSoundID = 229;

        SetStr(295, 335);
        SetDex(215, 245);
        SetInt(85, 115);

        SetHits(640, 655);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 44, 54);
        SetResistance(ResistanceType.Fire, 54, 64);
        SetResistance(ResistanceType.Cold, 19, 29);
        SetResistance(ResistanceType.Poison, 19, 29);
        SetResistance(ResistanceType.Energy, 19, 29);

        SetSkill(SkillName.MagicResist, 79.0, 94.0);
        SetSkill(SkillName.Tactics, 79.0, 89.0);
        SetSkill(SkillName.Wrestling, 84.0, 99.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 41;
    }

    public override string CorpseName => "an ash-hound's corpse";
    public override string DefaultName => "an ash-hound";

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
