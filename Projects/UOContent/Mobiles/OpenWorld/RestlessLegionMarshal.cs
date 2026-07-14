using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Restless (Barrow Legion), L3. Donor: Bone Knight.
[SerializationGenerator(0, false)]
public partial class RestlessLegionMarshal : BaseCreature
{
    // Rally the Fallen adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public RestlessLegionMarshal() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x0454;
        BaseSoundID = 451;

        SetStr(112, 136);
        SetDex(80, 100);
        SetInt(32, 48);

        SetHits(155, 160);

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

    public override string CorpseName => "a skeletal corpse";
    public override string DefaultName => "the barrow marshal";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    // Rally the Fallen: 15% chance to call a barrow legionary (capped at 2, dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new RestlessLegionSoldier());
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
