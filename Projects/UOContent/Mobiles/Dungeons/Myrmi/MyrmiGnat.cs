using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - ambient fodder, L3 trash. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class MyrmiGnat : BaseCreature
{
    [Constructible]
    public MyrmiGnat() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0798;
        BaseSoundID = 422;

        SetStr(85, 105);
        SetDex(95, 115);
        SetInt(25, 40);

        SetHits(110, 140);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 24, 31);
        SetResistance(ResistanceType.Fire, 8, 15);
        SetResistance(ResistanceType.Cold, 8, 15);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 8, 15);

        SetSkill(SkillName.MagicResist, 36.0, 46.0);
        SetSkill(SkillName.Tactics, 46.0, 56.0);
        SetSkill(SkillName.Wrestling, 46.0, 56.0);

        Fame = 1300;
        Karma = -1300;

        VirtualArmor = 27;
    }

    public override string CorpseName => "a nest gnat's corpse";
    public override string DefaultName => "a nest gnat";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
