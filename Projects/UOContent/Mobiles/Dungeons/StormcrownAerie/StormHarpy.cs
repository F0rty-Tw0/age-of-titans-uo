using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder.md §4) - L8 trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class StormHarpy : BaseCreature
{
    [Constructible]
    public StormHarpy() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0491;
        BaseSoundID = 402;

        SetStr(300, 340);
        SetDex(150, 180);
        SetInt(80, 110);

        SetHits(800, 900);

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

    public override string CorpseName => "a tempest harpy's corpse";
    public override string DefaultName => "a tempest harpy";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 7;

    // Gale Buffet: 20% chance on a landed hit to buffet the defender and sap stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The gale buffets you off balance!");
        }
    }

    // Pack instinct flavor: BaseCreature's PackInstinct enum is animal-species-only (no avian
    // entry - see TideHoplite's Phalanx in the Drowned Tholos), so the flock bonus is a
    // hand-rolled adjacency check instead of misusing an unrelated flag.
    public override void AlterMeleeDamageTo(Mobile to, ref int damage)
    {
        base.AlterMeleeDamageTo(to, ref damage);

        if (IsFlocked())
        {
            damage = damage * 6 / 5; // +20% while another tempest harpy stands adjacent
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
            if (m != this && m is StormHarpy { Alive: true })
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
