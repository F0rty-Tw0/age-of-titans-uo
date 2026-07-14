using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Ismenian brood. L7. Donor: Drake.
[SerializationGenerator(0, false)]
public partial class DrakonIsmenianDrake : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };

    [Constructible]
    public DrakonIsmenianDrake() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomList(60, 61);
        Hue = 0x0489;
        BaseSoundID = 362;

        SetStr(595, 635);
        SetDex(120, 140);
        SetInt(95, 120);

        SetHits(665, 685);

        SetDamage(18, 23);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Fire, 40);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 65, 75);
        SetResistance(ResistanceType.Cold, 35, 45);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 85.0, 100.0);
        SetSkill(SkillName.Tactics, 90.0, 105.0);
        SetSkill(SkillName.Wrestling, 85.0, 95.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 59;
    }

    public override string CorpseName => "an Ismenian drake's corpse";
    public override string DefaultName => "an Ismenian drake";

    public override int LootBagLevel => 6;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
