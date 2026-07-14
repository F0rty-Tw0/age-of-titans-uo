using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L8. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class StormAnemoiDrift : BaseCreature
{
    [Constructible]
    public StormAnemoiDrift() : base(AIType.AI_Melee)
    {
        Body = 13;
        Hue = 0x0481;
        BaseSoundID = 655;

        SetStr(480, 540);
        SetDex(220, 260);
        SetInt(120, 150);

        SetHits(800, 880);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "an anemoi drift's corpse";
    public override string DefaultName => "an anemoi drift";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 7;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
