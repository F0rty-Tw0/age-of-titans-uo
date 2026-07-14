using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood. L7. Donor: OphidianMage.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenianAugur : BaseCreature
{
    [Constructible]
    public DrakonIsmenianAugur() : base(AIType.AI_Mage)
    {
        Body = 85;
        Hue = 0x066D;
        BaseSoundID = 639;

        SetStr(210, 240);
        SetDex(195, 215);
        SetInt(545, 595);

        SetHits(640, 660);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 95.0, 110.0);
        SetSkill(SkillName.Magery, 95.0, 110.0);
        SetSkill(SkillName.Meditation, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 65.0, 85.0);
        SetSkill(SkillName.Wrestling, 30.0, 55.0);

        Fame = 9100;
        Karma = -9100;

        VirtualArmor = 40;
    }

    public override string CorpseName => "an Ismenian augur's corpse";
    public override string DefaultName => "an Ismenian augur";

    public override int LootBagLevel => 6;

    // Venom-word: 20% chance on a landed hit to corrode the defender's mana.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "The augur's venom-word gnaws at your focus!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
