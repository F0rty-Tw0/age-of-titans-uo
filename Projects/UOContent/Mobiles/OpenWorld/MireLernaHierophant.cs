using ModernUO.Serialization;

namespace Server.Mobiles;

// Open-World Bestiary (dev-docs/open-world-bestiary.md) - Mire Expansion, Cult of Lerna, L6. Donor: Evil Mage Lord.
[SerializationGenerator(0, false)]
public partial class MireLernaHierophant : BaseCreature
{
    [Constructible]
    public MireLernaHierophant() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0491;

        SetStr(305, 350);
        SetDex(96, 120);
        SetInt(120, 145);
        SetMana(120, 145);

        SetHits(490, 540);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 34, 42);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 42, 52);
        SetResistance(ResistanceType.Energy, 34, 42);

        SetSkill(SkillName.EvalInt, 90.0, 105.0);
        SetSkill(SkillName.Magery, 90.0, 105.0);
        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 62.0, 75.0);

        Fame = 5500;
        Karma = -5500;

        VirtualArmor = 46;
    }

    public override string CorpseName => "an evil mage lord corpse";
    public override string DefaultName => "the Lerna hierophant";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;
    public override Poison PoisonImmune => Poison.Deadly;

    // Venom Rite: 20% chance to drain mana on a landed swing.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The hierophant's venom rite saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
