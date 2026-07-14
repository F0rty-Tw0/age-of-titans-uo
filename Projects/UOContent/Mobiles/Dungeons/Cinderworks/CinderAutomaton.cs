using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder.md §2) - L6 trash. Donor: Golem (body only; the
// donor's summoned/scalar constructor doesn't fit this fixed stat block).
[SerializationGenerator(0, false)]
public partial class CinderAutomaton : BaseCreature
{
    // Overheat aura pulse timing; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public CinderAutomaton() : base(AIType.AI_Melee)
    {
        Body = 752;
        Hue = 0x0798;
        BaseSoundID = 456;

        SetStr(260, 300);
        SetDex(40, 55);
        SetInt(60, 80);

        SetHits(500, 550);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 80, 90);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 55;

        // Bronze construct, deliberately sluggish: slower AI think-cycle than default.
        ActiveSpeed = 0.4;
        PassiveSpeed = 0.8;
    }

    public override string CorpseName => "a bronze automaton's shell";
    public override string DefaultName => "a bronze automaton";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void OnThink()
    {
        base.OnThink();

        // Overheat: below 30% HP, pulses a fire aura on adjacent players/pets as a
        // comeback mechanic - punishes players who chip it down slowly.
        if (Hits < HitsMax * 30 / 100)
        {
            DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 3);
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
