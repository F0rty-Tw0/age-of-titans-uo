using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans sub-faction.
// L7 trash. Donor: Ettin.
[SerializationGenerator(0, false)]
public partial class TartarusTitanWarden : BaseCreature
{
    [Constructible]
    public TartarusTitanWarden() : base(AIType.AI_Melee)
    {
        Body = 18;
        Hue = 0x0022;
        BaseSoundID = 367;

        SetStr(350, 390);
        SetDex(96, 120);
        SetInt(55, 80);

        SetHits(650, 680);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 56);
        SetResistance(ResistanceType.Fire, 26, 36);
        SetResistance(ResistanceType.Cold, 51, 61);
        SetResistance(ResistanceType.Poison, 26, 36);
        SetResistance(ResistanceType.Energy, 26, 36);

        SetSkill(SkillName.MagicResist, 70.0, 90.0);
        SetSkill(SkillName.Tactics, 80.0, 95.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a titan-warden's corpse";
    public override string DefaultName => "a titan-warden";

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
