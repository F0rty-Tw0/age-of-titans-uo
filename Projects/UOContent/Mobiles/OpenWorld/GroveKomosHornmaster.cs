using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Groves Expansion, Komos revel, L3. Donor: Satyr.
[SerializationGenerator(0, false)]
public partial class GroveKomosHornmaster : BaseCreature
{
    // Reveler's Horn adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public GroveKomosHornmaster() : base(AIType.AI_Melee)
    {
        Body = 271;
        Hue = 0x0491;
        BaseSoundID = 0x586;

        SetStr(108, 135);
        SetDex(78, 100);
        SetInt(30, 50);

        SetHits(150, 160);

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

    public override string CorpseName => "the kōmos hornmaster's corpse";
    public override string DefaultName => "the kōmos hornmaster";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    // Reveler's Horn: 15% chance to call a GroveKomosFaun add (capped at 2, dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new GroveKomosFaun());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
