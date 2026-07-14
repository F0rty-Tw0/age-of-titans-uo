using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - mini-boss beside Myrmex, the swarm's field-marshal. Donor: Terathan Matriarch.
[SerializationGenerator(0, false)]
public partial class MyrmiMenoitios : DungeonElite
{
    // Marshal the Swarm adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public MyrmiMenoitios() : base(AIType.AI_Mage)
    {
        Name = "Menoitios";
        Title = "the Ant-Marshal";

        Body = 72;
        Hue = 0x0966;
        BaseSoundID = 599;

        SetStr(400, 430);
        SetDex(130, 150);
        SetInt(400, 440);

        SetHits(700, 720);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Poison, 30);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 35, 42);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 90, 100);
        SetResistance(ResistanceType.Energy, 35, 42);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.Poisoning, 70.0, 90.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 60.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 15000;
        Karma = -15000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "Menoitios's corpse";

    public override Poison HitPoison => Poison.Deadly;
    public override Poison PoisonImmune => Poison.Lethal;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 7;

    // Marshal the Swarm: 12% chance to call up to three Aiakid Runner adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.12)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 3, () => new MyrmiAiakidRunner());
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
