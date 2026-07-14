using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: LichLord (body 79).
[SerializationGenerator(0, false)]
public partial class StygianArbiter : BaseCreature
{
    // Non-serialized: add tracking rebuilds naturally after a restart (DungeonAbilities idiom).
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public StygianArbiter() : base(AIType.AI_Mage)
    {
        Body = 79;
        Hue = 0x0454;
        BaseSoundID = 412;

        SetStr(320, 370);
        SetDex(100, 130);
        SetInt(520, 580);

        SetHits(2700, 3000);

        SetDamage(23, 29);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 105.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "the arbiter of asphodel's corpse";
    public override string DefaultName => "the arbiter of asphodel";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }

    // Summon the Judged: 12% on being hit, raise a shade from the ranks (capped).
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
