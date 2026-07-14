using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L6 trash. Donor: Terathan Avenger.
[SerializationGenerator(0, false)]
public partial class MyrmiAiakidGoad : BaseCreature
{
    // Swarm Goad adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public MyrmiAiakidGoad() : base(AIType.AI_Melee)
    {
        Body = 152;
        Hue = 0x0798;
        BaseSoundID = 589;

        SetStr(320, 350);
        SetDex(140, 160);
        SetInt(50, 65);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7200;
        Karma = -7200;

        VirtualArmor = 55;
    }

    public override string CorpseName => "an Aiakid goad-warden's corpse";
    public override string DefaultName => "an Aiakid goad-warden";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override Poison PoisonImmune => Poison.Deadly;

    public override int LootBagLevel => 5;

    // Swarm Goad: 15% chance to summon an Aiakid Runner add, capped at 2 (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new MyrmiAiakidRunner());
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
