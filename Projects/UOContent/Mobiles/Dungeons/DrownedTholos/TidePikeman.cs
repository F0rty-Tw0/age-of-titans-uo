using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: "Bone
// Knight" per the design doc is body 147 (SkeletalKnight.cs) - the naming quirk established by
// TideHoplite.cs/TideMarine.cs applies here too.
[SerializationGenerator(0, false)]
public partial class TidePikeman : BaseCreature
{
    [Constructible]
    public TidePikeman() : base(AIType.AI_Melee)
    {
        Body = 147;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(180, 210);
        SetDex(55, 70);
        SetInt(25, 40);

        SetHits(320, 370);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 34, 44);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a barnacled pikeman's corpse";
    public override string DefaultName => "a barnacled pikeman";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    // Phalanx: no native combat hook keys off a pikeman-line pack instinct (BaseCreature's
    // PackInstinct enum is animal-species-only), so the adjacency check is done by hand -
    // same idiom as TideHoplite.IsPhalanxed.
    public override void AlterMeleeDamageTo(Mobile to, ref int damage)
    {
        base.AlterMeleeDamageTo(to, ref damage);

        if (IsPhalanxed())
        {
            damage = damage * 6 / 5; // +20% while another pikeman stands adjacent
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
            if (m != this && m is TidePikeman { Alive: true })
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
