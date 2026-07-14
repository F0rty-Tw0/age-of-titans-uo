using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder.md §1) - L5 trash. Donor: Bone Knight (body 57);
// the design doc's "Skeletal Knight (57)" refers to this body - the SkeletalKnight.cs class
// is body 147, a different donor.
[SerializationGenerator(0, false)]
public partial class TideHoplite : BaseCreature
{
    [Constructible]
    public TideHoplite() : base(AIType.AI_Melee)
    {
        Body = 57;
        Hue = 0x08A5;
        BaseSoundID = 451;

        SetStr(180, 220);
        SetDex(60, 80);
        SetInt(30, 45);

        SetHits(300, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 20, 30);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a barnacled skeletal corpse";
    public override string DefaultName => "a barnacled hoplite";

    public override bool BleedImmune => true;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    // Phalanx: no native combat hook keys off a "hoplite" pack instinct (BaseCreature's
    // PackInstinct enum is animal-species-only), so the adjacency check is done by hand.
    public override void AlterMeleeDamageTo(Mobile to, ref int damage)
    {
        base.AlterMeleeDamageTo(to, ref damage);

        if (IsPhalanxed())
        {
            damage = damage * 6 / 5; // +20% while another hoplite stands adjacent
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
            if (m != this && m is TideHoplite { Alive: true })
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
