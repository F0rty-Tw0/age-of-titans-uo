using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder.md §5). Donor: HellHound (body 98).
[SerializationGenerator(0, false)]
public partial class StygianHound : BaseCreature
{
    [Constructible]
    public StygianHound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0453;
        BaseSoundID = 229;

        SetStr(500, 550);
        SetDex(150, 170);
        SetInt(60, 90);

        SetHits(2000, 2300);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 30, 40);

        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 100.0, 110.0);
        SetSkill(SkillName.Wrestling, 100.0, 110.0);

        Fame = 20000;
        Karma = -20000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a hound's charred corpse";
    public override string DefaultName => "a hound of Dis";

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override int LootBagLevel => 8;

    private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
