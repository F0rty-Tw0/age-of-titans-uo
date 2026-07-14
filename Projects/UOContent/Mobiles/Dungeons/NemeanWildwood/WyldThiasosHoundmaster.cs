using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Evil Mage (human
// body; T2A has no distinct huntress body).
[SerializationGenerator(0, false)]
public partial class WyldThiasosHoundmaster : BaseCreature
{
    // Loose the Pack adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public WyldThiasosHoundmaster() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0486;

        SetStr(210, 240);
        SetDex(90, 110);
        SetInt(150, 180);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 80.0, 95.0);
        SetSkill(SkillName.Magery, 80.0, 95.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 4800;
        Karma = -4800;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a fallen houndmaster's corpse";
    public override string DefaultName => "the Thiasos houndmaster";

    public override int LootBagLevel => 6;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Loose the Pack: 15% chance to summon a Wolf add, capped at 2 (dispel-vulnerable).
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new WyldWolf());
        }
    }

    public override void OnAfterDelete()
    {
        _adds.Clear();

        base.OnAfterDelete();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
