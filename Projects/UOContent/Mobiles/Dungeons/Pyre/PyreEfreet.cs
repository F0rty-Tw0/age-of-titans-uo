using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre (dev-docs/gap-families-bestiary.md §6.1) - Fire dungeon. L6 trash. Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class PyreEfreet : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public PyreEfreet() : base(AIType.AI_Mage)
    {
        Body = 131;
        Hue = 0x0655;
        BaseSoundID = 768;

        SetStr(220, 250);
        SetDex(150, 175);
        SetInt(200, 230);

        SetHits(480, 540);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 60);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 60, 70);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 35, 42);

        SetSkill(SkillName.EvalInt, 78.0, 88.0);
        SetSkill(SkillName.Magery, 78.0, 88.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 6800;
        Karma = -6800;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a flamewind efreet's corpse";
    public override string DefaultName => "a flamewind efreet";

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
