using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor:
// Wraith.
[SerializationGenerator(0, false)]
public partial class TidePelagosBrineshade : BaseCreature
{
    [Constructible]
    public TidePelagosBrineshade() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0530;
        BaseSoundID = 0x482;

        SetStr(130, 160);
        SetDex(70, 90);
        SetInt(80, 100);

        SetHits(280, 330);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 55.0, 65.0);
        SetSkill(SkillName.Magery, 55.0, 65.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1450;
        Karma = -1450;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a brineshade's remains";
    public override string DefaultName => "a Pelagos brineshade";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        // Salt Rot: 20% chance to corrode mana.
        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 12;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Salt rot gnaws at your focus!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
