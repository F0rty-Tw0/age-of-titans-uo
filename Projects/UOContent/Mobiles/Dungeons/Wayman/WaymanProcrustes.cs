using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll — Expansion (dev-docs/gap-families-bestiary.md §6.8e) - Wrong mini-boss, between the
// core and Periphetes (elite L8). L7 elite. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class WaymanProcrustes : DungeonElite
{
    [Constructible]
    public WaymanProcrustes() : base(AIType.AI_Melee)
    {
        Name = "Procrustes";
        Title = "the Stretcher";

        Body = 752;
        Hue = 0x0964;

        SetStr(620, 660);
        SetDex(110, 135);
        SetInt(110, 135);

        SetHits(700, 720);

        SetDamage(16, 22);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 66, 76);
        SetResistance(ResistanceType.Fire, 52, 62);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 38, 48);

        SetSkill(SkillName.MagicResist, 106.0, 120.0);
        SetSkill(SkillName.Tactics, 92.0, 105.0);
        SetSkill(SkillName.Wrestling, 92.0, 105.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 68;
    }

    public override string CorpseName => "Procrustes's stretched-out husk";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 7;

    // Fit to the Bed: 25% chance to rack the defender onto the iron bed, sapping stamina and knocking them down.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Procrustes racks you onto his iron bed!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
        AddLoot(LootPack.Gems, 3);
    }
}
