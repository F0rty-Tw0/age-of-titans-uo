using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned mini-boss.
// Themed sub-faction - the Nostoi. Donor: BoneKnight. (Spawns Spectre, not Skeleton -
// kept distinct from Minos.)
[SerializationGenerator(0, false)]
public partial class DrownedPhrontis : DungeonElite
{
    // The tide reclaims; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public DrownedPhrontis() : base(AIType.AI_Melee)
    {
        Name = "Phrontis";
        Title = "the Unreturned";

        Body = 57;
        Hue = 0x0835;
        BaseSoundID = 451;

        SetStr(290, 320);
        SetDex(120, 140);
        SetInt(60, 80);

        SetHits(490, 510);

        SetDamage(16, 21);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 60, 70);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 42, 52);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 82.0, 92.0);

        Fame = 8200;
        Karma = -8200;

        VirtualArmor = 58;
    }

    public override string CorpseName => "the Unreturned's corpse";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool CanRummageCorpses => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 5;

    // The tide reclaims: 15% chance to call up to two Spectre adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

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
    }
}
