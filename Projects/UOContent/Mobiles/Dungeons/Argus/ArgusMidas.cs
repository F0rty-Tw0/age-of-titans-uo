using ModernUO.Serialization;

namespace Server.Mobiles;

// The Hundred-Eyed Vault — Expansion (dev-docs/gap-families-bestiary.md §6.7e) - mini-boss beside Erysichthon. Donor: Lich Lord.
[SerializationGenerator(0, false)]
public partial class ArgusMidas : DungeonElite
{
    [Constructible]
    public ArgusMidas() : base(AIType.AI_Mage)
    {
        Name = "Midas";
        Title = "the Gilded";

        Body = 79;
        Hue = 0x0479;
        BaseSoundID = 412;

        SetStr(380, 410);
        SetDex(150, 175);
        SetInt(380, 420);

        SetHits(700, 720);

        SetDamage(16, 22);

        SetDamageType(ResistanceType.Physical, 15);
        SetDamageType(ResistanceType.Cold, 35);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 55, 63);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 55, 63);
        SetResistance(ResistanceType.Poison, 90, 100);
        SetResistance(ResistanceType.Energy, 42, 50);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 100.0, 115.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 70.0, 85.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "Midas's corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 7;

    // Golden Touch: 25% chance on a landed hit to drain mana and knock the defender back.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 18;
            defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "Midas's golden touch knocks you back and drains your will!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
