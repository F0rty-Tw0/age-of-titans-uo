using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless mini-boss, L3. Donor: Skeletal Knight.
// Spawns at the Cove barrow-ground [2438,1100], 30-60 min respawn (open-world-restless.json).
[SerializationGenerator(0, false)]
public partial class RestlessBarrowking : DungeonElite
{
    // Call the Unburied adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public RestlessBarrowking() : base(AIType.AI_Melee)
    {
        Name = "the Barrow-King";

        Body = 147;
        Hue = 0x0454;
        BaseSoundID = 451;

        SetStr(112, 136);
        SetDex(80, 100);
        SetInt(32, 48);

        SetHits(155, 160);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Fire, 10, 15);
        SetResistance(ResistanceType.Cold, 10, 15);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 10, 15);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "the Barrow-King's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int EliteBagLevel => 3;

    // Call the Unburied: 15% chance to raise a heap of bones (capped at 3, dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 3, () => new RestlessBones());
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
