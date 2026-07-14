using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun. L7 trash. Donor: Cursed.
[SerializationGenerator(0, false)]
public partial class CursedLampadCrossroads : BaseCreature
{
    // Crossroads Fire pulse cadence; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public CursedLampadCrossroads() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0486;
        BaseSoundID = 471;

        SetStr(260, 300);
        SetDex(170, 200);
        SetInt(300, 340);

        SetHits(610, 670);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 38, 46);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 72.0, 82.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 9600;
        Karma = -9600;

        VirtualArmor = 52;
    }

    public override string CorpseName => "an inhuman corpse";
    public override string DefaultName => "a crossroads lampad";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 6;

    // Crossroads Fire: throttled fire damage to adjacent players and pets.
    public override void OnThink()
    {
        base.OnThink();

        DungeonAbilities.AuraPulse(this, ref _nextPulse, TimeSpan.FromSeconds(2), 3);
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
