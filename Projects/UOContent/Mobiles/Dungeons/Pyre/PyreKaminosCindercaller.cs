using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L7 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class PyreKaminosCindercaller : BaseCreature
{
    // Kindle the Unburnt adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public PyreKaminosCindercaller() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0655;
        BaseSoundID = 0x3E9;

        SetStr(270, 310);
        SetDex(150, 175);
        SetInt(340, 380);

        SetHits(610, 670);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 60);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 48, 58);
        SetResistance(ResistanceType.Fire, 58, 68);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 32, 42);
        SetResistance(ResistanceType.Energy, 32, 42);

        SetSkill(SkillName.EvalInt, 88.0, 98.0);
        SetSkill(SkillName.Magery, 88.0, 98.0);
        SetSkill(SkillName.MagicResist, 82.0, 92.0);
        SetSkill(SkillName.Tactics, 74.0, 84.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a Kaminoi cinder-caller's corpse";
    public override string DefaultName => "a Kaminoi cinder-caller";

    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 6;

    // Kindle the Unburnt: 15% chance on being hit to call a living-ember add, capped at 2 (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new PyreEmberling());
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
    }
}
