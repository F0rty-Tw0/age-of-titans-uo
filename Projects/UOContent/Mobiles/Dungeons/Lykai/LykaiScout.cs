using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - Orc Caves, L2 trash. Donor: Ratman.
[SerializationGenerator(0, false)]
public partial class LykaiScout : BaseCreature
{
    [Constructible]
    public LykaiScout() : base(AIType.AI_Melee)
    {
        Body = 42;
        Hue = 0x0483;
        BaseSoundID = 437;

        SetStr(60, 80);
        SetDex(70, 90);
        SetInt(25, 35);

        SetHits(90, 120);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 20, 28);
        SetResistance(ResistanceType.Fire, 8, 15);
        SetResistance(ResistanceType.Cold, 8, 15);
        SetResistance(ResistanceType.Poison, 10, 18);
        SetResistance(ResistanceType.Energy, 8, 15);

        SetSkill(SkillName.MagicResist, 35.0, 45.0);
        SetSkill(SkillName.Tactics, 45.0, 55.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 1000;
        Karma = -1000;

        VirtualArmor = 25;
    }

    public override string CorpseName => "an Arcadian scout's corpse";
    public override string DefaultName => "an Arcadian scout";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
