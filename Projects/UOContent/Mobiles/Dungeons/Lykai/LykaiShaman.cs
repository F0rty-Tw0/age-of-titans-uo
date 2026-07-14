using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband (dev-docs/gap-families-bestiary.md §6.6) - Orc Caves, L4 trash. Donor: Orcish Mage.
[SerializationGenerator(0, false)]
public partial class LykaiShaman : BaseCreature
{
    [Constructible]
    public LykaiShaman() : base(AIType.AI_Mage)
    {
        Body = 140;
        Hue = 0x0021;
        BaseSoundID = 0x45A;

        SetStr(160, 185);
        SetDex(95, 115);
        SetInt(120, 145);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 32, 40);
        SetResistance(ResistanceType.Fire, 20, 28);
        SetResistance(ResistanceType.Cold, 18, 25);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.EvalInt, 60.0, 72.0);
        SetSkill(SkillName.Magery, 60.0, 72.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 50.0, 62.0);
        SetSkill(SkillName.Wrestling, 40.0, 52.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a wolf-cult shaman's corpse";
    public override string DefaultName => "a wolf-cult shaman";

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
