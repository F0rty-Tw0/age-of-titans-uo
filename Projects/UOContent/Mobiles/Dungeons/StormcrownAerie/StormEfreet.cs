using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L8 core trash. Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class StormEfreet : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Sky Breath"

    [Constructible]
    public StormEfreet() : base(AIType.AI_Mage)
    {
        Body = 131;
        Hue = 0x0480;
        BaseSoundID = 768;

        SetStr(300, 340);
        SetDex(220, 250);
        SetInt(180, 210);

        SetHits(800, 880);

        SetDamage(16, 20);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 15, 25);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 90.0, 100.0);
        SetSkill(SkillName.Magery, 90.0, 100.0);
        SetSkill(SkillName.MagicResist, 90.0, 100.0);
        SetSkill(SkillName.Tactics, 85.0, 95.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 8000;
        Karma = -8000;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a sky efreet's corpse";
    public override string DefaultName => "a sky efreet";

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
