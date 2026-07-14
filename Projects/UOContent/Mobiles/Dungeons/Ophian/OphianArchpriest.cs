using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum (dev-docs/gap-families-bestiary.md §6.5) - Terathan Keep serpents, L6 trash. Donor: Ophidian Archmage.
[SerializationGenerator(0, false)]
public partial class OphianArchpriest : BaseCreature
{
    [Constructible]
    public OphianArchpriest() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0851;
        BaseSoundID = 639;

        SetStr(320, 350);
        SetDex(120, 140);
        SetInt(180, 210);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 45, 52);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 85.0, 98.0);
        SetSkill(SkillName.Magery, 85.0, 98.0);
        SetSkill(SkillName.Poisoning, 65.0, 85.0);
        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 40.0, 55.0);

        Fame = 7200;
        Karma = -7200;

        VirtualArmor = 50;
    }

    public override string CorpseName => "an ophian archpriest's corpse";
    public override string DefaultName => "an ophian archpriest";

    public override Poison PoisonImmune => Poison.Greater;

    public override int LootBagLevel => 5;

    // Ophion's Venom: 20% chance to sap 12 mana on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Ophion's venom saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
