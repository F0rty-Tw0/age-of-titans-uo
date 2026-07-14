using ModernUO.Serialization;

namespace Server.Mobiles;

// The Stormcrown Aerie (dev-docs/dungeon-ladder-bestiary.md §4) - L9 core trash. Donor: Titan.
[SerializationGenerator(0, false)]
public partial class StormTitanling : BaseCreature
{
    // Reuses EnergyBreath (local reskin defined in StormWisp.cs, same namespace).
    private static readonly MonsterAbility[] _abilities = { new EnergyBreath() }; // "Levin Bolt"

    [Constructible]
    public StormTitanling() : base(AIType.AI_Mage)
    {
        Body = 76;
        Hue = 0x0492;
        BaseSoundID = 609;

        SetStr(700, 760);
        SetDex(150, 180);
        SetInt(300, 340);

        SetHits(2200, 2400);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 55, 65);

        SetSkill(SkillName.EvalInt, 95.0, 105.0);
        SetSkill(SkillName.Magery, 95.0, 105.0);
        SetSkill(SkillName.MagicResist, 95.0, 105.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 17000;
        Karma = -17000;

        VirtualArmor = 65;
    }

    public override string CorpseName => "a lesser titan's corpse";
    public override string DefaultName => "a lesser titan";

    public override int LootBagLevel => 8;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
