using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - mini-boss beside
// Nyktimos, the pack's grey elder. Donor: Orcish Lord.
[SerializationGenerator(0, false)]
public partial class LykaiMainalos : DungeonElite
{
    // Elder Howl adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public LykaiMainalos() : base(AIType.AI_Melee)
    {
        Name = "Mainalos";
        Title = "the Old Wolf";

        Body = 138;
        Hue = 0x0483;
        BaseSoundID = 0x45A;

        SetStr(340, 370);
        SetDex(140, 160);
        SetInt(50, 65);

        SetHits(540, 550);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.MagicResist, 68.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 92.0);
        SetSkill(SkillName.Wrestling, 80.0, 92.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Old Wolf's corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override PackInstinct PackInstinct => PackInstinct.Canine;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 6;

    // Elder Howl: 15% chance to call up to three Lykai Hound adds (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 3, () => new LykaiHound());
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
