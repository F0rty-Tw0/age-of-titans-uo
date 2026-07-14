using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - the Ophite hierophants' L8 trash capstone. Donor: Ophidian Matriarch.
[SerializationGenerator(0, false)]
public partial class OphianOphiteHierophant : BaseCreature
{
    // Wake the Coil adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public OphianOphiteHierophant() : base(AIType.AI_Mage)
    {
        Body = 87;
        Hue = 0x0851;
        BaseSoundID = 644;

        SetStr(520, 560);
        SetDex(150, 170);
        SetInt(320, 360);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 56, 64);
        SetResistance(ResistanceType.Fire, 38, 46);
        SetResistance(ResistanceType.Cold, 38, 46);
        SetResistance(ResistanceType.Poison, 92, 100);
        SetResistance(ResistanceType.Energy, 38, 46);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.Poisoning, 78.0, 95.0);
        SetSkill(SkillName.MagicResist, 85.0, 98.0);
        SetSkill(SkillName.Tactics, 62.0, 75.0);
        SetSkill(SkillName.Wrestling, 48.0, 62.0);

        Fame = 13000;
        Karma = -13000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "the Ophite high-hierophant's corpse";
    public override string DefaultName => "the Ophite high-hierophant";

    public override Poison PoisonImmune => Poison.Lethal;
    public override Poison HitPoison => Poison.Deadly;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 7;

    // Wake the Coil: 15% chance to call up to two temple-constrictor adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new OphianConstrictor());
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
