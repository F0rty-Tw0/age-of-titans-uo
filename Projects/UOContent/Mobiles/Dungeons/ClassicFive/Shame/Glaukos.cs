using System;
using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame mini-boss, L6. Donor: Water Elemental.
[SerializationGenerator(0, false)]
public partial class Glaukos : DungeonElite
{
    // Call the Deep adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Glaukos() : base(AIType.AI_Mage)
    {
        Name = "Glaukos";
        Title = "the Brine-Shepherd";

        Body = 16;
        Hue = 0x04F8;
        BaseSoundID = 278;

        SetStr(240, 270);
        SetDex(100, 120);
        SetInt(225, 255);

        SetHits(535, 545);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 65, 75);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 70.0, 85.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 62;

        CanSwim = true;
    }

    public override string CorpseName => "the Brine-Shepherd's corpse";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int EliteBagLevel => 6;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Call the deep: 15% chance to summon up to two Sea Serpent adds.
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
    }
}
