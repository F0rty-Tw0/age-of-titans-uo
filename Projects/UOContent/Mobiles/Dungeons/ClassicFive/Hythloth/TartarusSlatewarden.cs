using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash.
// Donor: Stone Gargoyle.
[SerializationGenerator(0, false)]
public partial class TartarusSlatewarden : BaseCreature
{
    [Constructible]
    public TartarusSlatewarden() : base(AIType.AI_Melee)
    {
        Body = 67;
        Hue = 0x0455;
        BaseSoundID = 0x174;

        SetStr(555, 605);
        SetDex(162, 192);
        SetInt(182, 212);

        SetHits(875, 895);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 61, 71);
        SetResistance(ResistanceType.Fire, 31, 41);
        SetResistance(ResistanceType.Cold, 21, 31);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.MagicResist, 96.0, 111.0);
        SetSkill(SkillName.Tactics, 91.0, 106.0);
        SetSkill(SkillName.Wrestling, 86.0, 101.0);

        Fame = 13600;
        Karma = -13600;

        VirtualArmor = 66;
    }

    public override string CorpseName => "a slate warden's corpse";
    public override string DefaultName => "a slate warden";

    public override Poison PoisonImmune => Poison.Lethal;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
