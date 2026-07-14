using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L7 core. Donor: Dragon.
[SerializationGenerator(0, false)]
public partial class DrakonFirewyrm : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public DrakonFirewyrm() : base(AIType.AI_Mage)
    {
        Body = Utility.RandomList(12, 59);
        Hue = 0x0489;
        BaseSoundID = 362;

        SetStr(560, 600);
        SetDex(130, 155);
        SetInt(520, 570);

        SetHits(700, 720);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Fire, 40);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 70, 85);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 40, 50);

        SetSkill(SkillName.EvalInt, 95.0, 110.0);
        SetSkill(SkillName.Magery, 100.0, 115.0);
        SetSkill(SkillName.Meditation, 80.0, 90.0);
        SetSkill(SkillName.MagicResist, 90.0, 105.0);
        SetSkill(SkillName.Tactics, 70.0, 90.0);
        SetSkill(SkillName.Wrestling, 55.0, 80.0);

        Fame = 10000;
        Karma = -10000;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a drakon fire-wyrm's corpse";
    public override string DefaultName => "a drakon fire-wyrm";

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
