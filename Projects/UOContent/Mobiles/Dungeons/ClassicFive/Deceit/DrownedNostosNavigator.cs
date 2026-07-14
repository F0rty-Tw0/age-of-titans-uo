using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Themed sub-faction - the Nostoi. L5 trash. Donor: Spectre.
[SerializationGenerator(0, false)]
public partial class DrownedNostosNavigator : BaseCreature
{
    [Constructible]
    public DrownedNostosNavigator() : base(AIType.AI_Mage)
    {
        Body = 26;
        Hue = 0x0835;
        BaseSoundID = 0x482;

        SetStr(108, 130);
        SetDex(90, 110);
        SetInt(195, 220);

        SetHits(300, 330);

        SetDamage(12, 17);

        SetDamageType(ResistanceType.Physical, 50);
        SetDamageType(ResistanceType.Cold, 50);

        SetResistance(ResistanceType.Physical, 34, 42);
        SetResistance(ResistanceType.Cold, 32, 42);
        SetResistance(ResistanceType.Poison, 26, 36);

        SetSkill(SkillName.EvalInt, 75.0, 87.0);
        SetSkill(SkillName.Magery, 75.0, 87.0);
        SetSkill(SkillName.MagicResist, 67.0, 79.0);
        SetSkill(SkillName.Tactics, 58.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1900;
        Karma = -1900;

        VirtualArmor = 42;
    }

    public override string CorpseName => "a drowned haunt's remnant";
    public override string DefaultName => "the Nostoi navigator";

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
