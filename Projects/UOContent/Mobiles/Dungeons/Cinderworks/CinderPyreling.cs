using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L6 trash. Donor: Fire Gargoyle.
[SerializationGenerator(0, false)]
public partial class CinderPyreling : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public CinderPyreling() : base(AIType.AI_Mage)
    {
        Body = 130;
        Hue = 0x0655;
        BaseSoundID = 0x174;

        SetStr(200, 240);
        SetDex(90, 110);
        SetInt(110, 150);

        SetHits(460, 520);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.MagicResist, 65.0, 75.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2200;
        Karma = -2200;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a pyre gargoyle's corpse";
    public override string DefaultName => "a pyre gargoyle";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
