using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - Aiakid war-brood, L6 trash. Donor: Terathan Matriarch.
[SerializationGenerator(0, false)]
public partial class MyrmiAiakidSpearcaller : BaseCreature
{
    [Constructible]
    public MyrmiAiakidSpearcaller() : base(AIType.AI_Mage)
    {
        Body = 72;
        Hue = 0x0966;
        BaseSoundID = 599;

        SetStr(290, 320);
        SetDex(115, 135);
        SetInt(175, 205);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 28, 35);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 35, 42);
        SetResistance(ResistanceType.Energy, 28, 35);

        SetSkill(SkillName.EvalInt, 85.0, 98.0);
        SetSkill(SkillName.Magery, 85.0, 98.0);
        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 55.0, 68.0);
        SetSkill(SkillName.Wrestling, 40.0, 55.0);

        Fame = 7300;
        Karma = -7300;

        VirtualArmor = 50;
    }

    public override string CorpseName => "an Aiakid spear-caller's corpse";
    public override string DefaultName => "an Aiakid spear-caller";

    public override Poison PoisonImmune => Poison.Deadly;

    public override int LootBagLevel => 5;

    // Venom Spray: 20% chance to sap 12 mana on a successful hit.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The spear-caller's venom saps your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
