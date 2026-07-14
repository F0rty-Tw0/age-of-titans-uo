using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L6 trash. Donor: Ice Fiend.
[SerializationGenerator(0, false)]
public partial class RimeFiend : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() };

    [Constructible]
    public RimeFiend() : base(AIType.AI_Mage)
    {
        Body = 43;
        Hue = 0x0B0F;
        BaseSoundID = 357;

        SetStr(300, 340);
        SetDex(150, 175);
        SetInt(260, 290);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 70);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 55, 65);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 30, 38);

        SetSkill(SkillName.EvalInt, 78.0, 88.0);
        SetSkill(SkillName.Magery, 78.0, 88.0);
        SetSkill(SkillName.MagicResist, 68.0, 78.0);
        SetSkill(SkillName.Tactics, 78.0, 88.0);
        SetSkill(SkillName.Wrestling, 72.0, 82.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a boreal fiend's corpse";
    public override string DefaultName => "a boreal fiend";

    public override bool CanFly => true;

    public override int LootBagLevel => 5;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
