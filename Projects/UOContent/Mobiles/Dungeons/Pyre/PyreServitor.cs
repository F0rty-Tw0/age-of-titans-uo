using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L6 trash. Donor: Fire Elemental.
[SerializationGenerator(0, false)]
public partial class PyreServitor : BaseCreature
{
    // River-Glare pulse cadence; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public PyreServitor() : base(AIType.AI_Melee)
    {
        Body = 15;
        Hue = 0x0026;
        BaseSoundID = 838;

        SetStr(230, 260);
        SetDex(140, 160);
        SetInt(90, 115);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Fire, 75);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 75.0, 85.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a river-fire elemental's embers";
    public override string DefaultName => "a river-fire elemental";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    // River-Glare: throttled fire damage to adjacent players and pets.
    public override void OnThink()
    {
        base.OnThink();

        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 3);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
