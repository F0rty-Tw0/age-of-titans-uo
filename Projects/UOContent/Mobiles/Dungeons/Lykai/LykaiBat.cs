using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - ambient, L2 trash. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class LykaiBat : BaseCreature
{
    [Constructible]
    public LykaiBat() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0483;
        BaseSoundID = 422;

        SetStr(55, 75);
        SetDex(55, 70);
        SetInt(20, 30);

        SetHits(80, 110);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 18, 25);
        SetResistance(ResistanceType.Fire, 8, 15);
        SetResistance(ResistanceType.Cold, 8, 15);
        SetResistance(ResistanceType.Poison, 12, 20);
        SetResistance(ResistanceType.Energy, 8, 15);

        SetSkill(SkillName.MagicResist, 32.0, 42.0);
        SetSkill(SkillName.Tactics, 42.0, 52.0);
        SetSkill(SkillName.Wrestling, 42.0, 52.0);

        Fame = 900;
        Karma = -900;

        VirtualArmor = 22;
    }

    public override string CorpseName => "a cave bat's corpse";
    public override string DefaultName => "a warren bat";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
