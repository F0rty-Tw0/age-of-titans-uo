using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - elite. Donor: Sea Serpent.
[SerializationGenerator(0, false)]
public partial class TideWarden : DungeonElite
{
    // Maelstrom Call adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public TideWarden() : base(AIType.AI_Mage)
    {
        Name = "Kymopoleia";
        Title = "the Reefbound";

        Body = 150;
        Hue = 0x0851;
        BaseSoundID = 447;

        SetStr(200, 220);
        SetDex(80, 100);
        SetInt(90, 110);

        SetHits(360, 380);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 6000;
        Karma = -6000;

        VirtualArmor = 50;

        CanSwim = true;
    }

    public override string CorpseName => "a reef serpent's corpse";

    public override int EliteBagLevel => 5;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Maelstrom Call: 15% chance to summon a Brinescale add, capped at 2 (dispel-vulnerable).
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new TideBrinescale());
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
