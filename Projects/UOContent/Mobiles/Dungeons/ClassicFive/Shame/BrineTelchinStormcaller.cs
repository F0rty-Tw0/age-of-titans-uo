using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five (dev-docs/classic-five-bestiary.md) - Shame family (Telchines), L7. Donor: Air Elemental.
[SerializationGenerator(0, false)]
public partial class BrineTelchinStormcaller : BaseCreature
{
    [Constructible]
    public BrineTelchinStormcaller() : base(AIType.AI_Mage)
    {
        Name = "a telchine stormcaller";

        Body = 13;
        Hue = 0x04F8;
        BaseSoundID = 655;

        SetStr(230, 260);
        SetDex(190, 210);
        SetInt(230, 260);

        SetHits(620, 650);

        SetDamage(17, 22);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Cold, 40);
        SetDamageType(ResistanceType.Energy, 40);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 85.0, 100.0);
        SetSkill(SkillName.Magery, 85.0, 100.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 85.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 100.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 64;
    }

    public override string CorpseName => "a telchine stormcaller's remains";

    public override bool BleedImmune => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    // Sleet: a cold breath.
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };
    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
