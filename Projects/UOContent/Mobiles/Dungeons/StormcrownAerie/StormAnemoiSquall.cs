using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L9. Donor: Wisp.
[SerializationGenerator(0, false)]
public partial class StormAnemoiSquall : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Squall Breath"

    [Constructible]
    public StormAnemoiSquall() : base(AIType.AI_Mage)
    {
        Body = 58;
        Hue = 0x0491;
        BaseSoundID = 466;

        SetStr(600, 650);
        SetDex(260, 300);
        SetInt(280, 320);

        SetHits(2100, 2350);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 30, 40);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 20, 30);
        SetResistance(ResistanceType.Energy, 60, 70);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 16000;
        Karma = -16000;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a squall-spirit's corpse";
    public override string DefaultName => "a squall-spirit";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
