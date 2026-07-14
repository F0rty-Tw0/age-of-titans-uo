using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - Anemoi wind-spirits, L9. Donor: Titan.
[SerializationGenerator(0, false)]
public partial class StormAnemoiTempest : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Tempest-Lord's Breath"

    [Constructible]
    public StormAnemoiTempest() : base(AIType.AI_Mage)
    {
        Body = 76;
        Hue = 0x0492;
        BaseSoundID = 609;

        SetStr(720, 780);
        SetDex(160, 190);
        SetInt(320, 360);

        SetHits(2300, 2400);

        SetDamage(21, 26);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 52, 62);
        SetResistance(ResistanceType.Fire, 37, 47);
        SetResistance(ResistanceType.Cold, 37, 47);
        SetResistance(ResistanceType.Energy, 58, 68);

        SetSkill(SkillName.EvalInt, 97.0, 107.0);
        SetSkill(SkillName.Magery, 97.0, 107.0);
        SetSkill(SkillName.MagicResist, 97.0, 107.0);
        SetSkill(SkillName.Tactics, 92.0, 102.0);
        SetSkill(SkillName.Wrestling, 87.0, 97.0);

        Fame = 17500;
        Karma = -17500;

        VirtualArmor = 66;
    }

    public override string CorpseName => "the anemoi tempest-lord's corpse";
    public override string DefaultName => "the anemoi tempest-lord";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
