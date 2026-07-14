using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L8 core. Donor: Dragon.
[SerializationGenerator(0, false)]
public partial class DrakonGreatDrake : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public DrakonGreatDrake() : base(AIType.AI_Mage)
    {
        Body = Utility.RandomList(12, 59);
        Hue = 0x066D;
        BaseSoundID = 362;

        SetStr(680, 720);
        SetDex(140, 165);
        SetInt(560, 610);

        SetHits(850, 890);

        SetDamage(20, 25);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Fire, 40);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 75, 90);
        SetResistance(ResistanceType.Cold, 40, 50);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 45, 55);

        SetSkill(SkillName.EvalInt, 100.0, 115.0);
        SetSkill(SkillName.Magery, 105.0, 120.0);
        SetSkill(SkillName.Meditation, 85.0, 95.0);
        SetSkill(SkillName.MagicResist, 95.0, 110.0);
        SetSkill(SkillName.Tactics, 75.0, 95.0);
        SetSkill(SkillName.Wrestling, 60.0, 85.0);

        Fame = 14000;
        Karma = -14000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a drakon great-drake's corpse";
    public override string DefaultName => "a drakon great-drake";

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
