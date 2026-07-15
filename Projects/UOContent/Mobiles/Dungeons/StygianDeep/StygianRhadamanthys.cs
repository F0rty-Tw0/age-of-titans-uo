using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles;

// Stygian Deep mini-boss (dev-docs/dungeon-ladder-bestiary.md §5). Donor: LichLord (body 79).
[SerializationGenerator(0, false)]
public partial class StygianRhadamanthys : DungeonElite
{
    [Constructible]
    public StygianRhadamanthys() : base(AIType.AI_Mage)
    {
        Name = "Rhadamanthys";
        Title = "the Judge-King";

        Body = 79;
        Hue = 0x0454;
        BaseSoundID = 412;

        SetStr(320, 370);
        SetDex(100, 130);
        SetInt(520, 580);

        SetHits(3050, 3150);

        SetDamage(25, 31);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 30);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 105.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 40000;
        Karma = -40000;

        VirtualArmor = 78;
    }

    public override string CorpseName => "the corpse of the Judge-King";

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int EliteBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.FilthyRich);
    }

    // Sentence of the Dead: 25% on a landed hit, drain mana and cast the defender back.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.25)
        {
            defender.Mana -= 20;
            defender.PublicOverheadMessage(MessageType.Regular, 0x480, true, "Rhadamanthys passes sentence!");
        }
    }

    protected override Item CreateDecoDrop() => new ScalesOfTheJudgeKing();
}
