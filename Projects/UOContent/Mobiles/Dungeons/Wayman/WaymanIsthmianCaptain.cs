using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Isthmian wreckers. L7 trash. Donor: Juka Lord.
[SerializationGenerator(0, false)]
public partial class WaymanIsthmianCaptain : BaseCreature
{
    // Toll Collected adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public WaymanIsthmianCaptain() : base(AIType.AI_Melee)
    {
        Body = 766;
        Hue = 0x0798;

        SetStr(430, 470);
        SetDex(130, 155);
        SetInt(100, 125);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 58, 68);
        SetResistance(ResistanceType.Fire, 33, 42);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.MagicResist, 92.0, 106.0);
        SetSkill(SkillName.Tactics, 92.0, 106.0);
        SetSkill(SkillName.Wrestling, 88.0, 102.0);

        Fame = 7200;
        Karma = -7200;

        VirtualArmor = 60;
    }

    public override string CorpseName => "the Isthmian toll-captain's corpse";
    public override string DefaultName => "the Isthmian toll-captain";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override int LootBagLevel => 6;

    // Toll Collected: 15% chance to call up a cutpurse from the ditch, capped at 2 (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new WaymanCutpurse());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
        AddLoot(LootPack.Gems, 2);
    }
}
