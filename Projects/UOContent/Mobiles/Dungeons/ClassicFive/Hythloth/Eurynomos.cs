using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus mini-boss. Donor: Arcane Daemon.
[SerializationGenerator(0, false)]
public partial class Eurynomos : DungeonElite
{
    // Pit-heat pulse cadence; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public Eurynomos() : base(AIType.AI_Mage)
    {
        Name = "Eurynomos";
        Title = "the Corpse-Eater";

        Body = 0x310;
        Hue = 0x0021;
        BaseSoundID = 0x47D;

        SetStr(900, 980);
        SetDex(220, 260);
        SetInt(480, 520);

        SetHits(930, 940);

        SetDamage(20, 26);

        SetDamageType(ResistanceType.Physical, 80);
        SetDamageType(ResistanceType.Fire, 20);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 70, 80);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 95.0, 105.0);
        SetSkill(SkillName.Wrestling, 90.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Meditation, 30.0, 50.0);

        Fame = 24000;
        Karma = -24000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "the Corpse-Eater's corpse";

    public override Poison PoisonImmune => Poison.Deadly;

    public override int EliteBagLevel => 8;

    // Pit-heat: throttled fire damage to adjacent players and pets.
    public override void OnThink()
    {
        base.OnThink();

        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 5);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
