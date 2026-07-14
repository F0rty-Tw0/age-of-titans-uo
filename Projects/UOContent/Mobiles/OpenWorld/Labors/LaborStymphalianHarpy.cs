using System.Collections.Generic;
using ModernUO.Serialization;

namespace Server.Mobiles;

// The Labors (dev-docs/open-world-bestiary.md §6) - roaming world-hunt, L6. Donor: Harpy.
// Displayed "the Stymphalian Matriarch" to distinguish from the Stymphal* archery legendary.
[SerializationGenerator(0, false)]
public partial class LaborStymphalianHarpy : DungeonElite
{
    // Roused Flock adds; non-serialized, rebuilds naturally on restart.
    private readonly List<BaseCreature> _adds = [];

    [Constructible]
    public LaborStymphalianHarpy() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x08A5;
        BaseSoundID = 402;

        SetStr(260, 300);
        SetDex(190, 220);
        SetInt(60, 80);

        SetHits(530, 550);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 25, 33);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 25, 33);

        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 85.0, 97.0);
        SetSkill(SkillName.Wrestling, 80.0, 92.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 56;
    }

    public override string CorpseName => "the Stymphalian Matriarch's corpse";
    public override string DefaultName => "the Stymphalian Matriarch";

    public override bool ClickTitle => false;

    public override bool CanFly => true;

    public override int EliteBagLevel => 6;

    // Roused Flock: 15% chance to call up to three Harpy adds.
    public override void OnGotMeleeAttack(Mobile attacker, int damage)
    {
        base.OnGotMeleeAttack(attacker, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            DungeonAbilities.TrySpawnAdd(this, _adds, 3, () => new Harpy());
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
