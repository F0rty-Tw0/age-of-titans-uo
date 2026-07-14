using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L6 trash. Donor: Ice Fiend.
[SerializationGenerator(0, false)]
public partial class RimeBoreadShaman : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() };

    [Constructible]
    public RimeBoreadShaman() : base(AIType.AI_Mage)
    {
        Body = 43;
        Hue = 0x0AF3;
        BaseSoundID = 357;

        SetStr(290, 330);
        SetDex(140, 170);
        SetInt(250, 280);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 70);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 76.0, 86.0);
        SetSkill(SkillName.Magery, 76.0, 86.0);
        SetSkill(SkillName.MagicResist, 66.0, 76.0);
        SetSkill(SkillName.Tactics, 76.0, 86.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a Boread storm-shaman's corpse";
    public override string DefaultName => "a Boread storm-shaman";

    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
