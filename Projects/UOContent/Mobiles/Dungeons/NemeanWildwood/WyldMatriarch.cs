using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - elite. Donor: Ophidian Matriarch.
[SerializationGenerator(0, false)]
public partial class WyldMatriarch : DungeonElite
{
    // Coordinated Volley adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public WyldMatriarch() : base(AIType.AI_Mage)
    {
        Name = "Atalanta";
        Title = "the Unbowed";

        Body = 87;
        Hue = 0x0486;
        BaseSoundID = 644;

        SetStr(300, 330);
        SetDex(110, 130);
        SetInt(300, 330);

        SetHits(700, 720);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a silver-marked ophidian corpse";

    public override int EliteBagLevel => 7;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Coordinated Volley: 15% chance to summon a Hound add, capped at 2 (dispel-vulnerable).
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new WyldHound());
        }
    }

    public override void OnAfterDelete()
    {
        _adds.Clear();

        base.OnAfterDelete();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
