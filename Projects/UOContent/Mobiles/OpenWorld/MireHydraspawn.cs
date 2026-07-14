using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire, L6. Donor: Giant Serpent.
[SerializationGenerator(0, false)]
public partial class MireHydraspawn : BaseCreature
{
    // Sever & Split adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public MireHydraspawn() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0491;
        BaseSoundID = 219;

        SetStr(330, 380);
        SetDex(112, 136);
        SetInt(60, 84);

        SetHits(480, 540);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 92.0, 102.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a hydra-spawn corpse";
    public override string DefaultName => "a hissing hydra-spawn";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison PoisonImmune => Poison.Deadly;
    public override Poison HitPoison => Poison.Deadly;

    // Sever & Split: 12% chance to call a Snake add (capped at 2, dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.12)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Snake());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
