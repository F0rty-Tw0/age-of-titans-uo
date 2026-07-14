using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion mini-boss, L6. Donor: Giant Serpent.
// Location: the southern fen [2963,3617]. Respawn 30-60 min.
[SerializationGenerator(0, false)]
public partial class MireFenmother : DungeonElite
{
    // Sever & Split adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public MireFenmother() : base(AIType.AI_Melee)
    {
        Body = 0x15;
        Hue = 0x0491;
        BaseSoundID = 219;

        SetStr(340, 385);
        SetDex(114, 138);
        SetInt(62, 86);

        SetHits(540, 550);

        SetDamage(16, 20);

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

    public override string CorpseName => "a serpent corpse";
    public override string DefaultName => "the Fen-Mother";

    public override bool ClickTitle => false;

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison PoisonImmune => Poison.Deadly;
    public override Poison HitPoison => Poison.Deadly;

    public override int EliteBagLevel => 6;

    // Sever & Split: 15% chance to call a Snake add (capped at 3, dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 3, () => new Snake());
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
