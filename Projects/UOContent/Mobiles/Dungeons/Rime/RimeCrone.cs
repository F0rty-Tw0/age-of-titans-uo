using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 trash. Donor: Skeletal Mage.
[SerializationGenerator(0, false)]
public partial class RimeCrone : BaseCreature
{
    private static readonly MonsterAbility[] _abilities = { new ColdBreath() }; // "Frost Breath"

    [Constructible]
    public RimeCrone() : base(AIType.AI_Mage)
    {
        Body = 148;
        Hue = 0x0AF3;
        BaseSoundID = 451;

        SetStr(150, 175);
        SetDex(110, 135);
        SetInt(160, 190);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 18, 25);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.EvalInt, 65.0, 75.0);
        SetSkill(SkillName.Magery, 65.0, 75.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a rime crone's corpse";
    public override string DefaultName => "a rime crone";

    public override int LootBagLevel => 3;

    public override MonsterAbility[] GetMonsterAbilities() => _abilities;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
