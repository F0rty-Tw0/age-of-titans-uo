using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Chained Titans mini-boss. Donor: Titan.
[SerializationGenerator(0, false)]
public partial class TartarusMenoetius : DungeonElite
{
    // Rage of the pit's summons; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    // MonsterAbilities has no energy breath variant; reuses the EnergyBreath reskin defined
    // in StormcrownAerie/StormWisp.cs (same Server.Mobiles namespace) - kept distinct from
    // Eurynomos's fire aura.
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Levin Breath"

    [Constructible]
    public TartarusMenoetius() : base(AIType.AI_Mage)
    {
        Name = "Menoetius";
        Title = "the Chained";

        Body = 76;
        Hue = 0x0021;
        BaseSoundID = 609;

        SetStr(620, 680);
        SetDex(200, 230);
        SetInt(380, 410);

        SetHits(930, 950);

        SetDamage(21, 26);

        SetDamageType(ResistanceType.Physical, 65);
        SetDamageType(ResistanceType.Energy, 35);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 65, 75);

        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 110.0);
        SetSkill(SkillName.MagicResist, 110.0, 125.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 60.0, 80.0);

        Fame = 22000;
        Karma = -22000;

        VirtualArmor = 72;
    }

    public override string CorpseName => "the Chained's corpse";

    public override Poison PoisonImmune => Poison.Deadly;

    public override int EliteBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    // Rage of the pit: separate roll, calls up to two Imp adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Imp());
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
