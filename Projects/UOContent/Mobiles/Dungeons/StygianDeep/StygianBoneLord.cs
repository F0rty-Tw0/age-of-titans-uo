using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder.md §5). Donor: Bone Knight (body 57).
[SerializationGenerator(0, false)]
public partial class StygianBoneLord : BaseCreature
{
    // Non-serialized: add tracking rebuilds naturally after a restart (DungeonAbilities idiom).
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public StygianBoneLord() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(450, 500);
        SetDex(90, 110);
        SetInt(60, 90);

        SetHits(2600, 2900);

        SetDamage(23, 29);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 60, 70);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a bronze-clad skeletal corpse";
    public override string DefaultName => "a bronze bone lord";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override PackInstinct PackInstinct => PackInstinct.Daemon;

    public override int LootBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }

    // Deathless Ranks: 12% on being hit, raise one more shade from the ranks (capped).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.12)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new StygianShade());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }
}
