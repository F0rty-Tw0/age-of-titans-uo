using System;
using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-enhancements.md) - Shame elite. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class Thaumas : DungeonElite
{
    // Whirlpool Call adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    // Whirlpool pulse cadence; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public Thaumas() : base(AIType.AI_Mage)
    {
        Name = "Thaumas";
        Title = "the Wavebreaker";

        Body = 16;
        Hue = 0x04F8;
        BaseSoundID = 278;

        SetStr(260, 290);
        SetDex(100, 120);
        SetInt(220, 250);

        SetHits(480, 540);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 85.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 60;

        CanSwim = true;
    }

    public override string CorpseName => "a wavebreaker's corpse";

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 7;

    public override void OnThink()
    {
        base.OnThink();

        // Whirlpool pulse: cold AoE that punishes clumping.
        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 5);
    }

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Separate roll: calls up to two SeaSerpent adds.
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new SeaSerpent());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
        AddLoot(LootPack.Gems, 5);
    }
}
