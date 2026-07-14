using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - boss. Donor: Great Hart.
[SerializationGenerator(0, false)]
public partial class WyldStag : DungeonElite
{
    [Constructible]
    public WyldStag() : base(AIType.AI_Melee)
    {
        Name = "Elaphos";
        Title = "the Golden-Horned";

        Body = 0xEA;
        Hue = 0x0501;

        SetStr(320, 340);
        SetDex(160, 190);
        SetInt(60, 80);

        SetHits(710, 720);

        SetDamage(17, 21);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 30, 40);
        SetResistance(ResistanceType.Poison, 60, 70);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 80.0, 90.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 75;
    }

    public override string CorpseName => "a golden-horned corpse";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 7;
    public override int EliteBagCount => 2;

    public override int GetAttackSound() => 0x82;
    public override int GetHurtSound() => 0x83;
    public override int GetDeathSound() => 0x84;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Endless Hunt: 25% chance to mark the defender as quarry and sap stamina.
        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "You have been marked as Quarry!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
