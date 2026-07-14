using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Boar.
[SerializationGenerator(0, false)]
public partial class WyldBoar : BaseCreature
{
    [Constructible]
    public WyldBoar() : base(AIType.AI_Melee)
    {
        Body = 0x122;
        Hue = 0x0844;
        BaseSoundID = 0xC4;

        SetStr(190, 230);
        SetDex(90, 120);
        SetInt(25, 40);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 3100;
        Karma = -3100;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a great boar's corpse";
    public override string DefaultName => "a great boar";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
