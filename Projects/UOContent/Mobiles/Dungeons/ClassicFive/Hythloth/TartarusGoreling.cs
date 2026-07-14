using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus support. L7 trash.
// Donor: Gore Fiend.
[SerializationGenerator(0, false)]
public partial class TartarusGoreling : BaseCreature
{
    [Constructible]
    public TartarusGoreling() : base(AIType.AI_Melee)
    {
        Body = 305;
        Hue = 0x0021;
        BaseSoundID = 224;

        SetStr(345, 385);
        SetDex(145, 175);
        SetInt(105, 135);

        SetHits(650, 670);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 85);
        SetDamageType(ResistanceType.Poison, 15);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 32, 42);
        SetResistance(ResistanceType.Cold, 22, 32);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 37, 47);

        SetSkill(SkillName.MagicResist, 78.0, 92.0);
        SetSkill(SkillName.Tactics, 82.0, 96.0);
        SetSkill(SkillName.Wrestling, 86.0, 100.0);

        Fame = 9400;
        Karma = -9400;

        VirtualArmor = 47;
    }

    public override string CorpseName => "a goreling's corpse";
    public override string DefaultName => "a goreling";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
