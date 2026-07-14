using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-enhancements.md) - Deceit elite. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class Aiakos : DungeonElite
{
    // Drowned Judge's summons; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public Aiakos() : base(AIType.AI_Mage)
    {
        Name = "Aiakos";
        Title = "the Drowned Judge";

        Body = 24;
        Hue = 0x0835;
        BaseSoundID = 0x3E9;

        SetStr(220, 250);
        SetDex(140, 160);
        SetInt(320, 350);

        SetHits(380, 450);

        SetDamage(20, 26);

        SetDamageType(ResistanceType.Physical, 10);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.Meditation, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 80.0, 95.0);

        Fame = 9000;
        Karma = -9000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a drowned judge's corpse";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool CanRummageCorpses => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 6;

    // Attrition lich: drains mana and heals off it, forcing a burst fight.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 15;
            Hits += 15;
            defender.PublicOverheadMessage(MessageType.Regular, 0x480, true, "Aiakos drains your magic away!");
        }
    }

    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        // Separate roll: calls up to two Spectre adds.
        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 2, () => new Spectre());
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
        AddLoot(LootPack.MedScrolls, 2);
    }
}
