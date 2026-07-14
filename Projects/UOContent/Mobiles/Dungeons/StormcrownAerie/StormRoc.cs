using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L8 core trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class StormRoc : BaseCreature
{
    [Constructible]
    public StormRoc() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0491;
        BaseSoundID = 402;

        SetStr(300, 340);
        SetDex(150, 180);
        SetInt(80, 110);

        SetHits(800, 880);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 85.0, 95.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a squall harpy's corpse";
    public override string DefaultName => "a squall harpy";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    // Pack instinct flavor (free stat-block feature, not a counted custom ability): BaseCreature's
    // PackInstinct enum is animal-species-only, so the flock bonus is a hand-rolled adjacency
    // check - mirrors the idiom on the existing StormHarpy.
    public override void AlterMeleeDamageTo(Mobile to, ref int damage)
    {
        base.AlterMeleeDamageTo(to, ref damage);

        if (IsFlocked())
        {
            damage = damage * 6 / 5; // +20% while another squall harpy stands adjacent
        }
    }

    private bool IsFlocked()
    {
        if (Map == null)
        {
            return false;
        }

        foreach (var m in Map.GetMobilesInRange(Location, 1))
        {
            if (m != this && m is StormRoc { Alive: true })
            {
                return true;
            }
        }

        return false;
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
