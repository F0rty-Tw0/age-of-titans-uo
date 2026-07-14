using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L8 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class TartarusBrimstone : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public TartarusBrimstone() : base(AIType.AI_Mage)
    {
        Body = 9;
        Hue = 0x0022;
        BaseSoundID = 357;

        SetStr(565, 615);
        SetDex(145, 175);
        SetInt(360, 390);

        SetHits(850, 870);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Fire, 30);

        SetResistance(ResistanceType.Physical, 56, 66);
        SetResistance(ResistanceType.Fire, 66, 76);
        SetResistance(ResistanceType.Cold, 36, 46);
        SetResistance(ResistanceType.Poison, 31, 41);
        SetResistance(ResistanceType.Energy, 36, 46);

        SetSkill(SkillName.EvalInt, 86.0, 96.0);
        SetSkill(SkillName.Magery, 86.0, 96.0);
        SetSkill(SkillName.MagicResist, 96.0, 111.0);
        SetSkill(SkillName.Tactics, 86.0, 96.0);
        SetSkill(SkillName.Wrestling, 81.0, 96.0);

        Fame = 13300;
        Karma = -13300;

        VirtualArmor = 61;
    }

    public override string CorpseName => "a brimstone daemon's corpse";
    public override string DefaultName => "a brimstone daemon";

    public override bool CanFly => true;

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
