using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Evil Mage (human
// body; T2A has no distinct huntress body).
[SerializationGenerator(0, false)]
public partial class WyldThiasosMatron : BaseCreature
{
    [Constructible]
    public WyldThiasosMatron() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0798;

        SetStr(215, 245);
        SetDex(90, 110);
        SetInt(155, 185);

        SetHits(640, 700);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 47, 57);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 82.0, 97.0);
        SetSkill(SkillName.Magery, 82.0, 97.0);
        SetSkill(SkillName.MagicResist, 67.0, 77.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 52.0, 62.0);

        Fame = 4900;
        Karma = -4900;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a fallen matron's corpse";
    public override string DefaultName => "the Thiasos matron";

    public override int LootBagLevel => 6;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Marked Quarry: 20% chance to mark the defender as quarry and sap stamina.
        if (Utility.RandomDouble() < 0.20)
        {
            defender.Stam -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "You have been marked as Quarry!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
