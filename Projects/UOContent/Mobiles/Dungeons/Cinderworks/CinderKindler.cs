using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L6 trash. Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class CinderKindler : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public CinderKindler() : base(AIType.AI_Mage)
    {
        Body = 131;
        Hue = 0x0655;
        BaseSoundID = 768;

        SetStr(220, 260);
        SetDex(70, 90);
        SetInt(130, 170);

        SetHits(480, 540);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 80.0, 90.0);
        SetSkill(SkillName.Magery, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 2300;
        Karma = -2300;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a forge kindler's corpse";
    public override string DefaultName => "a forge kindler";

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
