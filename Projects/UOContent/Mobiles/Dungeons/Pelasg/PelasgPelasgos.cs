using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves mini-boss, the clans' earth-born
// forefather. Donor: Troglodyte.
[SerializationGenerator(0, false)]
public partial class PelasgPelasgos : DungeonElite
{
    // Sons of the Soil adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public PelasgPelasgos() : base(AIType.AI_Melee)
    {
        Name = "Pelasgos";
        Title = "the Earth-Born";

        Body = 267;
        Hue = 0x0967;
        BaseSoundID = 0x59F;

        SetStr(200, 225);
        SetDex(120, 145);
        SetInt(60, 80);

        SetHits(235, 240);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 62.0, 75.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 44;
    }

    public override string CorpseName => "Pelasgos's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool BleedImmune => true;

    public override int EliteBagLevel => 4;

    // Sons of the Soil: 15% chance to call a Pelasgian whelp add, capped at 2 (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new PelasgWhelp());
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
