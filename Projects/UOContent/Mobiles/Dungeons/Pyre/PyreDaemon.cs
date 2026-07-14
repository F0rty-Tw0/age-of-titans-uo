using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L7 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class PyreDaemon : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyreDaemon() : base(AIType.AI_Mage)
    {
        Body = 9;
        Hue = 0x0669;
        BaseSoundID = 357;

        SetStr(300, 340);
        SetDex(140, 170);
        SetInt(280, 310);

        SetHits(640, 700);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 55, 65);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 32, 42);

        SetSkill(SkillName.EvalInt, 78.0, 88.0);
        SetSkill(SkillName.Magery, 78.0, 88.0);
        SetSkill(SkillName.MagicResist, 88.0, 98.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 68.0, 88.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a cinder daemon's corpse";
    public override string DefaultName => "a cinder daemon";

    public override bool CanFly => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
