using ModernUO.Serialization;

namespace Server.Mobiles;

// The Arcadian Warband — Expansion (dev-docs/gap-families-bestiary.md §6.6e) - feral warband, L6 trash. Donor: Orcish Lord.
[SerializationGenerator(0, false)]
public partial class LykaiChieftain : BaseCreature
{
    [Constructible]
    public LykaiChieftain() : base(AIType.AI_Melee)
    {
        Body = 138;
        Hue = 0x0844;
        BaseSoundID = 0x45A;

        SetStr(300, 330);
        SetDex(120, 140);
        SetInt(50, 65);

        SetHits(480, 530);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 28, 35);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 65.0, 78.0);
        SetSkill(SkillName.Tactics, 75.0, 88.0);
        SetSkill(SkillName.Wrestling, 75.0, 88.0);

        Fame = 7000;
        Karma = -7000;

        VirtualArmor = 54;
    }

    public override string CorpseName => "an Arcadian chieftain's corpse";
    public override string DefaultName => "an Arcadian chieftain";

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
