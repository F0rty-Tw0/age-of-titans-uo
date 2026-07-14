using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor: "Bone
// Knight" per the design doc is body 147 (SkeletalKnight.cs) - same naming quirk established by
// TideHoplite.cs/TidePikeman.cs.
[SerializationGenerator(0, false)]
public partial class TidePelagosOarmaster : BaseCreature
{
    [Constructible]
    public TidePelagosOarmaster() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(185, 215);
        SetDex(55, 70);
        SetInt(25, 40);

        SetHits(330, 370);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 1550;
        Karma = -1550;

        VirtualArmor = 43;
    }

    public override string CorpseName => "the oarmaster's corpse";
    public override string DefaultName => "the Pelagos oarmaster";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    // Phalanx: same manual-adjacency idiom as TideHoplite.IsPhalanxed/TidePikeman.IsPhalanxed
    // (BaseCreature's PackInstinct enum is animal-species-only).
    public override void AlterMeleeDamageTo(Mobile to, ref int damage)
    {
        base.AlterMeleeDamageTo(to, ref damage);

        if (IsPhalanxed())
        {
            damage = damage * 6 / 5; // +20% while another oarmaster stands adjacent
        }
    }

    private bool IsPhalanxed()
    {
        if (Map == null)
        {
            return false;
        }

        foreach (var m in Map.GetMobilesInRange(Location, 1))
        {
            if (m != this && m is TidePelagosOarmaster { Alive: true })
            {
                return true;
            }
        }

        return false;
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
