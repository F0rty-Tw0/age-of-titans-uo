using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L5 trash. Donor: Imp.
[SerializationGenerator(0, false)]
public partial class PyreEmberling : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyreEmberling() : base(AIType.AI_Mage)
    {
        Body = 74;
        Hue = 0x0655;
        BaseSoundID = 422;

        SetStr(180, 210);
        SetDex(110, 135);
        SetInt(150, 175);

        SetHits(280, 330);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 20, 25);
        SetResistance(ResistanceType.Poison, 25, 30);
        SetResistance(ResistanceType.Energy, 25, 30);

        SetSkill(SkillName.EvalInt, 60.0, 70.0);
        SetSkill(SkillName.Magery, 60.0, 70.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a living ember's ashes";
    public override string DefaultName => "a living ember";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
