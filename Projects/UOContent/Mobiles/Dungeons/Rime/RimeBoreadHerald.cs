using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L7 trash. Donor: Arctic Ogre Lord.
[SerializationGenerator(0, false)]
public partial class RimeBoreadHerald : BaseCreature
{
    // Rallied riders; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public RimeBoreadHerald() : base(AIType.AI_Melee)
    {
        Body = 135;
        Hue = 0x047E;
        BaseSoundID = 427;

        SetStr(440, 480);
        SetDex(105, 130);
        SetInt(90, 115);

        SetHits(640, 700);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 26, 34);
        SetResistance(ResistanceType.Cold, 52, 62);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.MagicResist, 80.0, 90.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 9600;
        Karma = -9600;

        VirtualArmor = 60;
    }

    public override string CorpseName => "the Boread herald's corpse";
    public override string DefaultName => "the Boread herald";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 6;

    // Rally the Wind: 15% chance on being hit to call up to two dispellable Boread riders.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new RimeBoreadRider());
        }
    }

    public override void OnAfterDelete()
    {
        _adds.Clear();

        base.OnAfterDelete();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
