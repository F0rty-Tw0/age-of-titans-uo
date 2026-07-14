using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3e) - Khaldun mini-boss. Donor: Ancient Lich.
[SerializationGenerator(0, false)]
public partial class CursedPerses : DungeonElite
{
    [Constructible]
    public CursedPerses() : base(AIType.AI_Mage)
    {
        Name = "Perses";
        Title = "the Grave-Titan";

        Body = 78;
        Hue = 0x0486;
        BaseSoundID = 412;

        SetStr(320, 350);
        SetDex(190, 220);
        SetInt(360, 400);

        SetHits(900, 950);

        SetDamage(19, 25);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 35);
        SetDamageType(ResistanceType.Energy, 35);

        SetResistance(ResistanceType.Physical, 55, 63);
        SetResistance(ResistanceType.Fire, 32, 40);
        SetResistance(ResistanceType.Cold, 45, 53);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 38, 46);

        SetSkill(SkillName.EvalInt, 100.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 110.0);
        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 13000;
        Karma = -13000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "the Grave-Titan's corpse";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 8;

    // Titan's Sentence: 25% chance on a landed hit to drain mana and cast the defender back.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 18;
            defender.PublicOverheadMessage(MessageType.Regular, 0x0486, true, "Perses passes the titan's sentence!");
        }
    }

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }
}
