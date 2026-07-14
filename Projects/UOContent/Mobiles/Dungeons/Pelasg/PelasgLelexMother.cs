using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Painted Deep (dev-docs/gap-families-bestiary.md §6.9e) - Painted Caves, Leleges cave-clan. L4 trash. Donor: Troglodyte.
[SerializationGenerator(0, false)]
public partial class PelasgLelexMother : BaseCreature
{
    // Rouse the Clan adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public PelasgLelexMother() : base(AIType.AI_Mage)
    {
        Body = 267;
        Hue = 0x0967;
        BaseSoundID = 0x59F;

        SetStr(150, 175);
        SetDex(80, 100);
        SetInt(110, 135);

        SetHits(200, 240);

        SetDamage(9, 12);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.EvalInt, 55.0, 68.0);
        SetSkill(SkillName.Magery, 55.0, 68.0);
        SetSkill(SkillName.MagicResist, 52.0, 65.0);
        SetSkill(SkillName.Tactics, 50.0, 62.0);
        SetSkill(SkillName.Wrestling, 45.0, 58.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 36;
    }

    public override string CorpseName => "the Leleges clan-mother's corpse";
    public override string DefaultName => "the Leleges clan-mother";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool CanHeal => true;

    public override int LootBagLevel => 3;

    // Rouse the Clan: 15% chance to call a Leleges forager add, capped at 2 (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new PelasgLelexForager());
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
