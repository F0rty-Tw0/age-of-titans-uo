using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: HellHound (body 98).
[SerializationGenerator(0, false)]
public partial class StygianDishound : BaseCreature
{
    [Constructible]
    public StygianDishound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0453;
        BaseSoundID = 229;

        SetStr(480, 530);
        SetDex(150, 180);
        SetInt(50, 80);

        SetHits(2500, 2800);

        SetDamage(22, 28);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 45, 55);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 105.0, 115.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 70;
    }

    public override string CorpseName => "a hound's charred corpse";
    public override string DefaultName => "a great hound of Dis";

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 9;

    private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
