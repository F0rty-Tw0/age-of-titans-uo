using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum (dev-docs/gap-families-bestiary.md §6.5) - the first elite draw for the
// Terathan Keep serpents. Donor: Ophidian Matriarch.
[SerializationGenerator(0, false)]
public partial class OphianKeto : DungeonElite
{
    [Constructible]
    public OphianKeto() : base(AIType.AI_Mage)
    {
        Name = "Keto";
        Title = "the Scaled Matron";

        Body = 87;
        Hue = 0x0851;
        BaseSoundID = 644;

        SetStr(400, 430);
        SetDex(130, 150);
        SetInt(400, 440);

        SetHits(700, 720);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 58);
        SetResistance(ResistanceType.Fire, 35, 42);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 90, 100);
        SetResistance(ResistanceType.Energy, 35, 42);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.Poisoning, 80.0, 100.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 60.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 80.0);

        Fame = 15000;
        Karma = -15000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "the Scaled Matron's corpse";

    public override Poison HitPoison => Poison.Deadly;
    public override Poison PoisonImmune => Poison.Lethal;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 7;

    // Coil and Constrict: 25% chance to wrench a target off balance for stamina.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Stam -= 15;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Keto's coils wrench you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
