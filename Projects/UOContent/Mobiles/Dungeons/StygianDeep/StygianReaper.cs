using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder.md §5). Donor: Lich (body 24).
[SerializationGenerator(0, false)]
public partial class StygianReaper : BaseCreature
{
    [Constructible]
    public StygianReaper() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0454;
        BaseSoundID = 0x3E9;

        SetStr(350, 400);
        SetDex(120, 140);
        SetInt(560, 620);

        SetHits(2500, 2800);

        SetDamage(22, 28);

        SetDamageType(ResistanceType.Physical, 10);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 55, 65);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 110.0, 120.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.MagicResist, 105.0, 115.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a reaper's corpse";
    public override string DefaultName => "a soul-reaper";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison HitPoison => Poison.Lethal;

    public override int LootBagLevel => 9;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }

    // Stygian Rot lands via HitPoison automatically; the self-heal is a separate proc.
    public override void OnGaveMeleeAttack(Mobile defender, int damage)
    {
        base.OnGaveMeleeAttack(defender, damage);

        if (Utility.RandomDouble() < 0.15)
        {
            Hits += HitsMax * 8 / 100;
        }
    }
}
