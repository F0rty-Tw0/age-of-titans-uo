using ModernUO.Serialization;

namespace Server.Mobiles;

// The Accursed Dig (dev-docs/gap-families-bestiary.md §6.3) - Khaldun. L6 trash. Donor: Cursed.
[SerializationGenerator(0, false)]
public partial class CursedAccursed : BaseCreature
{
    [Constructible]
    public CursedAccursed() : base(AIType.AI_Mage)
    {
        Body = 0x190;
        Hue = 0x0851;
        BaseSoundID = 471;

        SetStr(230, 260);
        SetDex(150, 175);
        SetInt(230, 260);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.EvalInt, 70.0, 80.0);
        SetSkill(SkillName.Magery, 70.0, 80.0);
        SetSkill(SkillName.MagicResist, 62.0, 72.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 44;
    }

    public override string CorpseName => "an inhuman corpse";
    public override string DefaultName => "one of the accursed";

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
