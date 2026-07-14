using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L8 core trash. Donor: Wisp.
[SerializationGenerator(0, false)]
public partial class StormSprite : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Levin Breath"

    [Constructible]
    public StormSprite() : base(AIType.AI_Mage)
    {
        Body = 58;
        Hue = 0x0480;
        BaseSoundID = 466;

        SetStr(260, 300);
        SetDex(200, 230);
        SetInt(220, 250);

        SetHits(780, 860);

        SetDamage(15, 19);

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

        VirtualArmor = 48;
    }

    public override string CorpseName => "a levin wisp's corpse";
    public override string DefaultName => "a levin wisp";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
