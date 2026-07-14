using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L5 core family. Donor: Spectre.
[SerializationGenerator(0, false)]
public partial class TideVotary : BaseCreature
{
    [Constructible]
    public TideVotary() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0532;
        BaseSoundID = 0x482;

        SetStr(130, 160);
        SetDex(55, 70);
        SetInt(80, 100);

        SetHits(280, 330);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Cold, 28, 38);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 55.0, 65.0);
        SetSkill(SkillName.Magery, 55.0, 65.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1450;
        Karma = -1450;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a votary's corpse";
    public override string DefaultName => "a drowned votary";

    public override Poison PoisonImmune => Poison.Regular;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
