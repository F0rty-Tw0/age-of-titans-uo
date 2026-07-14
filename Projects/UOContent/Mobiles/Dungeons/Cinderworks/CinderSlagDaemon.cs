using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L6 trash. Donor: Daemon.
[SerializationGenerator(0, false)]
public partial class CinderSlagDaemon : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public CinderSlagDaemon() : base(AIType.AI_Mage)
    {
        Body = 9;
        Hue = 0x0967;
        BaseSoundID = 357;

        SetStr(240, 280);
        SetDex(70, 90);
        SetInt(120, 160);

        SetHits(500, 550);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Fire, 60);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.MagicResist, 75.0, 85.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a slag daemon's corpse";
    public override string DefaultName => "a slag daemon";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
