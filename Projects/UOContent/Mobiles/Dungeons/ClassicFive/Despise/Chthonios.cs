using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gaian mini-boss. Donor: Cyclops.
[SerializationGenerator(0, false)]
public partial class Chthonios : DungeonElite
{
    // Sown teeth; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Chthonios() : base(AIType.AI_Melee)
    {
        Name = "Chthonios";
        Title = "the First-Sown";

        Body = 75;
        Hue = 0x0972;
        BaseSoundID = 604;

        SetStr(340, 380);
        SetDex(90, 110);
        SetInt(40, 60);

        SetHits(230, 240);

        SetDamage(14, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 55);
        SetResistance(ResistanceType.Fire, 28, 38);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 28, 38);

        SetSkill(SkillName.MagicResist, 62.0, 75.0);
        SetSkill(SkillName.Tactics, 78.0, 92.0);
        SetSkill(SkillName.Wrestling, 78.0, 90.0);

        Fame = 6500;
        Karma = -6500;

        VirtualArmor = 50;
    }

    public override string CorpseName => "the First-Sown's corpse";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 4;

    // Sow the teeth: 15% chance to call up to two Lizardman adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Lizardman());
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
