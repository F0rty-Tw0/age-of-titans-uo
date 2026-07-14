using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L7 trash. Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class PyreKaminosEmberbrand : BaseCreature
{
    [Constructible]
    public PyreKaminosEmberbrand() : base(AIType.AI_Mage)
    {
        Body = 131;
        Hue = 0x0026;
        BaseSoundID = 768;

        SetStr(240, 280);
        SetDex(160, 190);
        SetInt(260, 300);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 60);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 62, 72);
        SetResistance(ResistanceType.Cold, 22, 30);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 38, 46);

        SetSkill(SkillName.EvalInt, 82.0, 92.0);
        SetSkill(SkillName.Magery, 82.0, 92.0);
        SetSkill(SkillName.MagicResist, 72.0, 82.0);
        SetSkill(SkillName.Tactics, 72.0, 82.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a Kaminoi emberbrand's corpse";
    public override string DefaultName => "a Kaminoi emberbrand";

    public override int LootBagLevel => 6;

    // Emberbrand: 20% chance on a landed hit to sear the defender's focus.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 14;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The emberbrand sears your focus!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
