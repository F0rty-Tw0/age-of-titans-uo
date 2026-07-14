using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash.
// Donor: Hell Hound.
[SerializationGenerator(0, false)]
public partial class TartarusPyrehound : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public TartarusPyrehound() : base(AIType.AI_Melee)
    {
        Body = 98;
        Hue = 0x0022;
        BaseSoundID = 229;

        SetStr(300, 340);
        SetDex(220, 250);
        SetInt(90, 120);

        SetHits(640, 660);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 80.0, 95.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 9200;
        Karma = -9200;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a pyre-hound's corpse";
    public override string DefaultName => "a pyre-hound";

    public override PackInstinct PackInstinct => PackInstinct.Canine;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
