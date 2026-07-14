using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood mini-boss. Donor: SerpentineDragon.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenos : DungeonElite
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    // Coil and crush adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public DrakonIsmenos() : base(AIType.AI_Mage)
    {
        Name = "Ismenos";
        Title = "the Hoard-Coiled";

        Body = 103;
        Hue = 0x0489;
        BaseSoundID = 362;

        SetStr(680, 720);
        SetDex(150, 170);
        SetInt(510, 560);

        SetHits(705, 720);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Fire, 30);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 100.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 120.0);
        SetSkill(SkillName.Meditation, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 70.0, 85.0);
        SetSkill(SkillName.Wrestling, 60.0, 85.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "the Hoard-Coiled's corpse";

    public override int EliteBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override int GetIdleSound() => 0x2C4;

    public override int GetAttackSound() => 0x2C0;

    public override int GetDeathSound() => 0x2C1;

    public override int GetAngerSound() => 0x2C4;

    public override int GetHurtSound() => 0x2C3;

    // Coil and crush: 15% chance to call up to two GiantSerpent adds - kept distinct from
    // Pythios's Drake adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new GiantSerpent());
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
