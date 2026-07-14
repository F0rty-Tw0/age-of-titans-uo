using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Hythloth, Tartarus family. L7 trash.
// Donor: Fire Gargoyle.
[SerializationGenerator(0, false)]
public partial class TartarusEmberwing : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public TartarusEmberwing() : base(AIType.AI_Mage)
    {
        Body = 130;
        Hue = 0x0022;
        BaseSoundID = 0x174;

        SetStr(340, 380);
        SetDex(200, 230);
        SetInt(240, 270);

        SetHits(645, 665);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 80);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 58, 68);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 22, 30);

        SetSkill(SkillName.EvalInt, 85.0, 100.0);
        SetSkill(SkillName.Magery, 85.0, 100.0);
        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 78.0, 92.0);
        SetSkill(SkillName.Wrestling, 55.0, 80.0);

        Fame = 9300;
        Karma = -9300;

        VirtualArmor = 46;
    }

    public override string CorpseName => "an ember-wing gargoyle's corpse";
    public override string DefaultName => "an ember-wing gargoyle";

    public override bool CanFly => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
