using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L5 trash. Donor: Ratman Mage.
[SerializationGenerator(0, false)]
public partial class RimeBoreadHoundmaster : BaseCreature
{
    [Constructible]
    public RimeBoreadHoundmaster() : base(AIType.AI_Mage)
    {
        Body = 0x8F;
        Hue = 0x0485;
        BaseSoundID = 437;

        SetStr(200, 230);
        SetDex(120, 145);
        SetInt(210, 240);

        SetHits(320, 360);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 42, 50);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.EvalInt, 70.0, 80.0);
        SetSkill(SkillName.Magery, 70.0, 80.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a Boread hound-master's corpse";
    public override string DefaultName => "a Boread hound-master";

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
