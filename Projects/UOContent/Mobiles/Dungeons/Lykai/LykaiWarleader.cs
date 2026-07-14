using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - Orc Caves, L5 trash. Donor: Orcish Lord.
[SerializationGenerator(0, false)]
public partial class LykaiWarleader : BaseCreature
{
    // Howl of the Pack adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public LykaiWarleader() : base(AIType.AI_Melee)
    {
        Body = 138;
        Hue = 0x0021;
        BaseSoundID = 0x45A;

        SetStr(220, 250);
        SetDex(100, 120);
        SetInt(45, 60);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 22, 30);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 68.0, 80.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 48;
    }

    public override string CorpseName => "an Arcadian warleader's corpse";
    public override string DefaultName => "an Arcadian warleader";

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 4;

    // Howl of the Pack: 15% chance to call up to two Hound adds.
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
