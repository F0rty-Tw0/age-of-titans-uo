using ModernUO.Serialization;

namespace Server.Mobiles;

// The Myrmex Nest — Expansion (dev-docs/gap-families-bestiary.md §6.4e) - ambient fodder, L3 trash. Donor: Giant Rat.
[SerializationGenerator(0, false)]
public partial class MyrmiMite : BaseCreature
{
    [Constructible]
    public MyrmiMite() : base(AIType.AI_Melee)
    {
        Body = 0xD7;
        Hue = 0x0964;
        BaseSoundID = 0x188;

        SetStr(90, 110);
        SetDex(70, 90);
        SetInt(25, 40);

        SetHits(110, 140);

        SetDamage(6, 9);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 26, 33);
        SetResistance(ResistanceType.Fire, 10, 17);
        SetResistance(ResistanceType.Cold, 10, 17);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 8, 15);

        SetSkill(SkillName.MagicResist, 38.0, 48.0);
        SetSkill(SkillName.Tactics, 48.0, 58.0);
        SetSkill(SkillName.Wrestling, 48.0, 58.0);

        Fame = 1300;
        Karma = -1300;

        VirtualArmor = 28;
    }

    public override string CorpseName => "a nest mite's corpse";
    public override string DefaultName => "a nest mite";

    public override int LootBagLevel => 2;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
