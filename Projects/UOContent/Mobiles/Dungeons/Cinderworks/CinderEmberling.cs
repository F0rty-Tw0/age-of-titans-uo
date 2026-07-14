using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L5 trash. Donor: Imp.
[SerializationGenerator(0, false)]
public partial class CinderEmberling : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath }; // "Cinder Breath"

    [Constructible]
    public CinderEmberling() : base(AIType.AI_Mage)
    {
        Body = 74;
        Hue = 0x0655;
        BaseSoundID = 422;

        SetStr(140, 170);
        SetDex(80, 100);
        SetInt(100, 130);

        SetHits(260, 310);

        SetDamage(8, 12);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 25, 35);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 70.0, 85.0);
        SetSkill(SkillName.Magery, 70.0, 85.0);
        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 45.0, 55.0);

        Fame = 1600;
        Karma = -1600;

        VirtualArmor = 32;
    }

    public override string CorpseName => "an emberling's corpse";
    public override string DefaultName => "an emberling";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 4;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
