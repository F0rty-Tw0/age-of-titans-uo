using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned mini-boss. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class Minos : DungeonElite
{
    // The dead rise; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Minos() : base(AIType.AI_Mage)
    {
        Name = "Minos";
        Title = "the Pale Arbiter";

        Body = 24;
        Hue = 0x0835;
        BaseSoundID = 0x3E9;

        SetStr(190, 210);
        SetDex(130, 150);
        SetInt(300, 330);

        SetHits(370, 380);

        SetDamage(14, 19);

        SetDamageType(ResistanceType.Physical, 10);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 22, 32);
        SetResistance(ResistanceType.Cold, 52, 62);
        SetResistance(ResistanceType.Poison, 58, 68);
        SetResistance(ResistanceType.Energy, 42, 52);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.Meditation, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);

        Fame = 8500;
        Karma = -8500;

        VirtualArmor = 52;
    }

    public override string CorpseName => "the Pale Arbiter's corpse";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool CanRummageCorpses => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 5;

    // The dead rise: 15% chance to call up to two Skeleton adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Skeleton());
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
