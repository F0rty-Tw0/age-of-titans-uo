using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash. Donor: Efreet.
[SerializationGenerator(0, false)]
public partial class TartarusEfreet : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public TartarusEfreet() : base(AIType.AI_Mage)
    {
        Body = 131;
        Hue = 0x0022;
        BaseSoundID = 768;

        SetStr(330, 370);
        SetDex(240, 270);
        SetInt(230, 260);

        SetHits(645, 665);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Fire, 50);
        SetDamageType(ResistanceType.Energy, 50);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 58, 68);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 25, 33);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.MagicResist, 78.0, 92.0);
        SetSkill(SkillName.Tactics, 75.0, 90.0);
        SetSkill(SkillName.Wrestling, 60.0, 85.0);

        Fame = 9300;
        Karma = -9300;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a pit-efreet's corpse";
    public override string DefaultName => "a pit-efreet";

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
