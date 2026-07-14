using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - ambient/fodder, L8. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class StormMoth : BaseCreature
{
    [Constructible]
    public StormMoth() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0480;
        BaseSoundID = 422;

        SetStr(260, 300);
        SetDex(180, 210);
        SetInt(60, 90);

        SetHits(780, 820);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 45;
    }

    public override string CorpseName => "a static moth's corpse";
    public override string DefaultName => "a static moth";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
