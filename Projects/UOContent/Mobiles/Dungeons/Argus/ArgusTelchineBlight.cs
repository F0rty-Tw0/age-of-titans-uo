using System;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L7 trash. Donor: Elder Gazer.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineBlight : BaseCreature
{
    // Evil Eye pulse cadence; non-serialized, rebuilds naturally on restart.
    private DateTime _nextPulse;

    [Constructible]
    public ArgusTelchineBlight() : base(AIType.AI_Mage)
    {
        Body = 22;
        Hue = 0x0479;
        BaseSoundID = 377;

        SetStr(260, 300);
        SetDex(170, 200);
        SetInt(300, 340);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 38, 46);
        SetResistance(ResistanceType.Poison, 32, 40);
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

    public override string CorpseName => "a Telchine blight-caster's remains";
    public override string DefaultName => "a Telchine blight-caster";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 6;

    // Evil Eye: throttled energy damage to adjacent players and pets.
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
