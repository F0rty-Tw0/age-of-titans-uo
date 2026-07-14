using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder.md §5). Donor: Spectre (body 26).
[SerializationGenerator(0, false)]
public partial class StygianShade : BaseCreature
{
    [Constructible]
    public StygianShade() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0455;
        BaseSoundID = 0x482;

        SetStr(280, 320);
        SetDex(100, 120);
        SetInt(500, 560);

        SetHits(2100, 2350);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Cold, 50);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 40, 50);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.EvalInt, 100.0, 115.0);
        SetSkill(SkillName.Magery, 100.0, 115.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 80.0, 90.0);

        Fame = 20000;
        Karma = -20000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a shadowy corpse";
    public override string DefaultName => "an unransomed shade";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 8;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }

    // Grave Chill: 20% on a landed hit, drain the defender's mana.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 15;
            defender.PublicOverheadMessage(MessageType.Regular, 0x480, true, "Grave Chill saps your mana!");
        }
    }
}
