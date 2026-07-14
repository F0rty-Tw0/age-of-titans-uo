using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - the Lykaonid wolf-sons, L6 trash. Donor: Orcish Lord.
[SerializationGenerator(0, false)]
public partial class LykaiLykaonidPriest : BaseCreature
{
    // Call the Pack adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public LykaiLykaonidPriest() : base(AIType.AI_Mage)
    {
        Body = 138;
        Hue = 0x0021;
        BaseSoundID = 0x45A;

        SetStr(280, 310);
        SetDex(110, 130);
        SetInt(170, 200);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 85.0, 98.0);
        SetSkill(SkillName.Magery, 85.0, 98.0);
        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 40.0, 55.0);

        Fame = 7200;
        Karma = -7200;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a Lykaonid moon-priest's corpse";
    public override string DefaultName => "a Lykaonid moon-priest";

    public override int LootBagLevel => 5;

    // Call the Pack: 15% chance to summon a Lykai Hound add, capped at 2 (dispel-vulnerable).
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new LykaiHound());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
