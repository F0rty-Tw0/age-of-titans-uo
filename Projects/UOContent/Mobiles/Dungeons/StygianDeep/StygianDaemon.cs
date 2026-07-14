using ModernUO.Serialization;

namespace Server.Mobiles;

// Stygian Deep trash (dev-docs/dungeon-ladder-bestiary.md §5). Donor: Daemon (body 9).
[SerializationGenerator(0, false)]
public partial class StygianDaemon : BaseCreature
{
    [Constructible]
    public StygianDaemon() : base(AIType.AI_Mage)
    {
        Body = 9;
        Hue = 0x0454;
        BaseSoundID = 357;

        SetStr(320, 370);
        SetDex(100, 130);
        SetInt(520, 580);

        SetHits(2700, 3000);

        SetDamage(23, 29);

        SetDamageType(ResistanceType.Physical, 25);
        SetDamageType(ResistanceType.Cold, 75);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 65, 75);
        SetResistance(ResistanceType.Poison, 50, 60);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 105.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 115.0);
        SetSkill(SkillName.MagicResist, 105.0, 120.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 25000;
        Karma = -25000;

        VirtualArmor = 68;
    }

    public override string CorpseName => "a stygian daemon's corpse";
    public override string DefaultName => "a stygian daemon";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 9;

    private static MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
