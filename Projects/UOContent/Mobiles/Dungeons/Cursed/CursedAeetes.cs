using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun elite. Donor: Ancient Lich.
[SerializationGenerator(0, false)]
public partial class CursedAeetes : DungeonElite
{
    [Constructible]
    public CursedAeetes() : base(AIType.AI_Mage)
    {
        Name = "Aeetes";
        Title = "the Buried Oracle";

        Body = 78;
        Hue = 0x0486;
        BaseSoundID = 412;

        SetStr(420, 460);
        SetDex(150, 180);
        SetInt(560, 620);

        SetHits(2200, 2400);

        SetDamage(20, 26);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 35);
        SetDamageType(ResistanceType.Energy, 35);

        SetResistance(ResistanceType.Physical, 60, 70);
        SetResistance(ResistanceType.Fire, 35, 42);
        SetResistance(ResistanceType.Cold, 50, 60);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 42, 50);

        SetSkill(SkillName.EvalInt, 118.0, 128.0);
        SetSkill(SkillName.Magery, 118.0, 128.0);
        SetSkill(SkillName.MagicResist, 135.0, 150.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 80.0, 95.0);

        Fame = 26000;
        Karma = -26000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "the Buried Oracle's corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 9;

    // Hecate's Sentence: 25% chance on a landed hit to drain mana and cast the defender back.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 20;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0486, true, "Aeetes passes Hecate's sentence!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
