using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: LichLord (body 79).
[SerializationGenerator(0, false)]
public partial class StygianLichLord : BaseCreature
{
    [Constructible]
    public StygianLichLord() : base(AIType.AI_Mage)
    {
        Body = 79;
        Hue = 0x0454;
        BaseSoundID = 412;

        SetStr(320, 370);
        SetDex(100, 130);
        SetInt(520, 580);

        SetHits(2600, 2900);

        SetDamage(23, 29);

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

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "a lich of Dis's corpse";
    public override string DefaultName => "a lich of Dis";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }

    // Grave Leech: 15% on a landed hit, self-heal 8% max HP.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            Hits += HitsMax * 8 / 100;
        }
    }
}
