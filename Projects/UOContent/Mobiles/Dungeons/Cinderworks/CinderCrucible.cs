using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L6 trash. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class CinderCrucible : BaseCreature
{
    // Radiant Heat aura pulse timing; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public CinderCrucible() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0654;
        BaseSoundID = 456;

        SetStr(260, 300);
        SetDex(40, 55);
        SetInt(50, 70);

        SetHits(500, 550);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a crucible construct's shell";
    public override string DefaultName => "a crucible construct";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void OnThink()
    {
        base.OnThink();

        // Radiant Heat: always-on aura (unlike CinderAutomaton's below-30% Overheat gate).
        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 3);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
