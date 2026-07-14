using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Despise, Gegenes mini-boss. Donor: Titan.
[SerializationGenerator(0, false)]
public partial class GaianPeloreus : DungeonElite
{
    // Heaved Ettin; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public GaianPeloreus() : base(AIType.AI_Melee)
    {
        Name = "Peloreus";
        Title = "the Unearthed";

        Body = 76;
        Hue = 0x0972;
        BaseSoundID = 609;

        SetStr(290, 320);
        SetDex(75, 95);
        SetInt(45, 65);

        SetHits(365, 380);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 60);
        SetResistance(ResistanceType.Fire, 30, 38);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.MagicResist, 68.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 92.0);
        SetSkill(SkillName.Wrestling, 78.0, 90.0);

        Fame = 5300;
        Karma = -5300;

        VirtualArmor = 54;
    }

    public override string CorpseName => "Peloreus's corpse";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 4;

    // Heave the earth: 15% chance to call up to two Ettin adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Ettin());
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
