using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - mini-boss, the L8 deep
// capstone above Keto (elite L7), using the L4-9 headroom. Donor: Ophidian Matriarch.
[SerializationGenerator(0, false)]
public partial class OphianPoine : DungeonElite
{
    [Constructible]
    public OphianPoine() : base(AIType.AI_Mage)
    {
        Name = "Poine";
        Title = "the Argive Coil";

        Body = 87;
        Hue = 0x0453;
        BaseSoundID = 644;

        SetStr(430, 470);
        SetDex(140, 160);
        SetInt(420, 460);

        SetHits(900, 950);

        SetDamage(18, 24);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 52, 60);
        SetResistance(ResistanceType.Fire, 37, 44);
        SetResistance(ResistanceType.Cold, 37, 44);
        SetResistance(ResistanceType.Poison, 92, 100);
        SetResistance(ResistanceType.Energy, 37, 44);

        SetSkill(SkillName.EvalInt, 92.0, 104.0);
        SetSkill(SkillName.Magery, 92.0, 104.0);
        SetSkill(SkillName.Poisoning, 82.0, 100.0);
        SetSkill(SkillName.MagicResist, 92.0, 104.0);
        SetSkill(SkillName.Tactics, 62.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 82.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "the Argive Coil's corpse";

    public override Poison HitPoison => Poison.Deadly;
    public override Poison PoisonImmune => Poison.Lethal;
    public override bool BleedImmune => true;

    public override int EliteBagLevel => 8;

    // Serpent's Toll: 25% chance to sap 18 mana and knock the target off balance.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Poine's coils wrench you off balance!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
