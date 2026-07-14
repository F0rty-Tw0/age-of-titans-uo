using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-enhancements.md) - Hythloth elite. Donor: Balron.
[SerializationGenerator(0, false)]
public partial class Alastor : DungeonElite
{
    // Tormentor's summons; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Alastor() : base(AIType.AI_Mage)
    {
        Name = "Alastor";
        Title = "the Tormentor";

        Body = 40;
        Hue = 0x0021;
        BaseSoundID = 357;

        SetStr(1100, 1300);
        SetDex(200, 260);
        SetInt(200, 260);

        SetHits(780, 900);

        SetDamage(24, 30);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Fire, 25);
        SetDamageType(ResistanceType.Energy, 25);

        SetResistance(ResistanceType.Physical, 70, 85);
        SetResistance(ResistanceType.Fire, 65, 85);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 100);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.Anatomy, 25.0, 50.0);
        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.Meditation, 25.0, 50.0);
        SetSkill(SkillName.MagicResist, 115.0, 130.0);
        SetSkill(SkillName.Tactics, 100.0, 110.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 26000;
        Karma = -26000;

        VirtualArmor = 95;
    }

    public override string CorpseName => "the tormentor's corpse";

    public override bool CanRummageCorpses => true;
    public override Poison PoisonImmune => Poison.Deadly;
    public override int Meat => 1;

    public override int EliteBagLevel => 9;

    // Fire-curse burst: a flamestrike-style pop on the defender.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            DoHarmful(defender);
            defender.Damage(Utility.RandomMinMax(15, 25), this);
            defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            defender.PublicOverheadMessage(MessageType.Regular, 0x21, true, "Alastor's curse sears your flesh!");
        }
    }

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Separate roll: calls up to two Imp adds.
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Imp());
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
        AddLoot(LootPack.Rich);
        AddLoot(LootPack.MedScrolls, 3);
    }
}
