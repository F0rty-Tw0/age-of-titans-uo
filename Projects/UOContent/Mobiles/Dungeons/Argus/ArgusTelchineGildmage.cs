using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - Telchine hoard-sorcerers, L7 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class ArgusTelchineGildmage : BaseCreature
{
    [Constructible]
    public ArgusTelchineGildmage() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0479;
        BaseSoundID = 0x3E9;

        SetStr(270, 300);
        SetDex(170, 195);
        SetInt(300, 330);

        SetHits(610, 670);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 15);
        SetDamageType(ResistanceType.Cold, 35);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 48, 56);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 55, 63);
        SetResistance(ResistanceType.Poison, 55, 63);
        SetResistance(ResistanceType.Energy, 42, 50);

        SetSkill(SkillName.EvalInt, 85.0, 95.0);
        SetSkill(SkillName.Magery, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 9700;
        Karma = -9700;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a Telchine gild-mage's corpse";
    public override string DefaultName => "a Telchine gild-mage";

    public override SpeedLevel SpeedClass => SpeedLevel.Medium;

    public override bool CanRummageCorpses => true;
    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 6;

    // Covet: 20% chance on a landed hit to drain mana from the defender.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.20)
        {
            defender.Mana -= 14;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0479, true, "The Telchine gild-mage covets your strength!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
