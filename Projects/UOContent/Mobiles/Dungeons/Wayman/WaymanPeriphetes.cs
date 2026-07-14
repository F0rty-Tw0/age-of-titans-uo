using ModernUO.Serialization;

namespace Server.Mobiles;

// The Wayman's Toll (dev-docs/gap-families-bestiary.md §6.8) - Wrong elite. Donor: Golem.
[SerializationGenerator(0, false)]
public partial class WaymanPeriphetes : DungeonElite
{
    [Constructible]
    public WaymanPeriphetes() : base(AIType.AI_Melee)
    {
        Name = "Periphetes";
        Title = "the Bronze-Cudgel";

        Body = 752;
        Hue = 0x0798;

        SetStr(880, 930);
        SetDex(140, 170);
        SetInt(120, 150);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 68, 78);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 100, 100);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 110.0, 125.0);
        SetSkill(SkillName.Tactics, 95.0, 108.0);
        SetSkill(SkillName.Wrestling, 95.0, 108.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 73;
    }

    public override string CorpseName => "Periphetes's shattered husk";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 8;

    // Cudgel Fall: 25% chance to sap stamina and knock the defender off balance.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Periphetes's bronze cudgel sends you sprawling!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
        AddLoot(LootPack.Gems, 4);
    }
}
