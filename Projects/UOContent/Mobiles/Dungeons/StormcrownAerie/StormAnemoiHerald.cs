using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L9. Donor: Dark Wisp.
[SerializationGenerator(0, false)]
public partial class StormAnemoiHerald : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Herald's Breath"

    [Constructible]
    public StormAnemoiHerald() : base(AIType.AI_Mage)
    {
        Body = 165;
        Hue = 0x0491;
        BaseSoundID = 466;

        SetStr(620, 670);
        SetDex(270, 310);
        SetInt(300, 340);

        SetHits(2200, 2400);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 47, 57);
        SetResistance(ResistanceType.Fire, 32, 42);
        SetResistance(ResistanceType.Cold, 27, 37);
        SetResistance(ResistanceType.Poison, 22, 32);
        SetResistance(ResistanceType.Energy, 62, 72);

        SetSkill(SkillName.EvalInt, 97.0, 107.0);
        SetSkill(SkillName.Magery, 97.0, 107.0);
        SetSkill(SkillName.MagicResist, 97.0, 107.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 87.0, 97.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 62;
    }

    public override string CorpseName => "the anemoi herald's corpse";
    public override string DefaultName => "the anemoi herald";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
