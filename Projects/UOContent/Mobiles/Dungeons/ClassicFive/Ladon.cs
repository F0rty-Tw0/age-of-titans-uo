using System;
using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-enhancements.md) - Destard elite. Donor: Dragon.
[SerializationGenerator(0, false)]
public partial class Ladon : DungeonElite
{
    // Sleepless Wyrm's summons; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    private static readonly MonsterAbility[] _abilities = { new LadonBreath() };

    [Constructible]
    public Ladon() : base(AIType.AI_Mage)
    {
        Name = "Ladon";
        Title = "the Sleepless Wyrm";

        Body = Utility.RandomList(12, 59);
        Hue = 0x0501;
        BaseSoundID = 362;

        SetStr(900, 960);
        SetDex(110, 130);
        SetInt(480, 520);

        SetHits(620, 700);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 65, 75);
        SetResistance(ResistanceType.Fire, 70, 80);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 40.0, 50.0);
        SetSkill(SkillName.Magery, 40.0, 50.0);
        SetSkill(SkillName.MagicResist, 110.0, 120.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "the sleepless wyrm's corpse";

    public override int Meat => 19;
    public override int Hides => 20;
    public override HideType HideType => HideType.Barbed;
    public override int Scales => 7;
    public override ScaleType ScaleType => Body == 12 ? ScaleType.Yellow : ScaleType.Red;
    public override bool CanFly => true;

    public override int EliteBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Separate roll: calls up to two Drake adds.
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Drake());
        }
    }

    public override void OnAfterDelete()
    {
        base.OnAfterDelete();

        _adds.Clear();
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich, 2);
        AddLoot(LootPack.Gems, 10);
    }
}

// Ladon's enhanced fire breath: higher damage scalar and shorter cooldown than the stock breath.
public class LadonBreath : FireBreath
{
    public override double BreathDamageScalar => Core.AOS ? 0.22 : 0.09;

    public override TimeSpan MinTriggerCooldown => TimeSpan.FromSeconds(20.0);
    public override TimeSpan MaxTriggerCooldown => TimeSpan.FromSeconds(30.0);
}
