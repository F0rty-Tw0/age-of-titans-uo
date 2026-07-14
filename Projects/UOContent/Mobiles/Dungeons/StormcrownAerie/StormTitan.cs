using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder.md §4) - L9 trash. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class StormTitan : BaseCreature
{
    // Thunderclap aura pulse timing; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public StormTitan() : base(AIType.AI_Melee)
    {
        Body = 75;
        Hue = 0x0492;
        BaseSoundID = 604;

        SetStr(700, 780);
        SetDex(120, 150);
        SetInt(60, 90);

        SetHits(2200, 2400);

        SetDamage(20, 26);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.MagicResist, 95.0, 100.0);
        SetSkill(SkillName.Tactics, 95.0, 100.0);
        SetSkill(SkillName.Wrestling, 95.0, 100.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 68;

        // Chained titan, deliberately sluggish: slower AI think-cycle than default.
        ActiveSpeed = 0.4;
        PassiveSpeed = 0.8;
    }

    public override string CorpseName => "a chained titan's corpse";
    public override string DefaultName => "a chained titan";

    public override bool BleedImmune => true;

    public override int LootBagLevel => 8;

    public override void OnThink()
    {
        base.OnThink();

        // Thunderclap: 4 energy damage/tick to adjacent players and pets.
        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 4);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
