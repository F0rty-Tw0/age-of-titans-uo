using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Destard, Drakon expansion. L8 core. Donor: WhiteWyrm.
[SerializationGenerator(0, false)]
public partial class DrakonHoardWyrm : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { MonsterAbilities.ColdBreath };

    [Constructible]
    public DrakonHoardWyrm() : base(AIType.AI_Melee)
    {
        Body = Utility.RandomBool() ? 180 : 49;
        Hue = 0x0501;
        BaseSoundID = 362;

        SetStr(740, 780);
        SetDex(95, 120);
        SetInt(90, 120);

        SetHits(820, 860);

        SetDamage(19, 24);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 55, 65);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 70, 85);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 35, 45);

        SetSkill(SkillName.MagicResist, 90.0, 105.0);
        SetSkill(SkillName.Tactics, 95.0, 110.0);
        SetSkill(SkillName.Wrestling, 90.0, 105.0);

        Fame = 13500;
        Karma = -13500;

        VirtualArmor = 74;
    }

    public override string CorpseName => "a hoard wyrm's corpse";
    public override string DefaultName => "a hoard wyrm";

    public override int LootBagLevel => 7;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
