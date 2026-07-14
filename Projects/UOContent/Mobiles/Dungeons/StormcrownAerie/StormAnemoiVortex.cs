using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L9. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class StormAnemoiVortex : BaseCreature
{
    // Static Field aura pulse timing; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public StormAnemoiVortex() : base(AIType.AI_Melee)
    {
        Body = 13;
        Hue = 0x0480;
        BaseSoundID = 655;

        SetStr(600, 680);
        SetDex(200, 230);
        SetInt(150, 180);

        SetHits(2150, 2380);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Energy, 60, 70);

        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a howling vortex's corpse";
    public override string DefaultName => "a howling vortex";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    public override void OnThink()
    {
        base.OnThink();

        // Static Field: 4 energy damage/tick to adjacent players and pets.
        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 4);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
