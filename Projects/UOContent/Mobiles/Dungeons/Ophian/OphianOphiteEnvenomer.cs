using ModernUO.Serialization;

namespace Server.Mobiles;

// The Coiled Sanctum — Expansion (dev-docs/gap-families-bestiary.md §6.5e) - Ophite hierophants, L7 trash. Donor: Ophidian Archmage.
[SerializationGenerator(0, false)]
public partial class OphianOphiteEnvenomer : BaseCreature
{
    [Constructible]
    public OphianOphiteEnvenomer() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x0851;
        BaseSoundID = 639;

        SetStr(420, 460);
        SetDex(140, 160);
        SetInt(240, 270);

        SetHits(600, 660);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 34, 42);
        SetResistance(ResistanceType.Cold, 34, 42);
        SetResistance(ResistanceType.Poison, 52, 60);
        SetResistance(ResistanceType.Energy, 34, 42);

        SetSkill(SkillName.EvalInt, 88.0, 100.0);
        SetSkill(SkillName.Magery, 88.0, 100.0);
        SetSkill(SkillName.Poisoning, 70.0, 88.0);
        SetSkill(SkillName.MagicResist, 70.0, 82.0);
        SetSkill(SkillName.Tactics, 58.0, 70.0);
        SetSkill(SkillName.Wrestling, 45.0, 58.0);

        Fame = 9700;
        Karma = -9700;

        VirtualArmor = 56;
    }

    public override string CorpseName => "an Ophite envenomer's corpse";
    public override string DefaultName => "an Ophite envenomer";

    public override Poison PoisonImmune => Poison.Greater;
    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 6;

    // Ophion's Gift: 20% chance to sap 14 mana on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 14;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Ophion's gift saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
