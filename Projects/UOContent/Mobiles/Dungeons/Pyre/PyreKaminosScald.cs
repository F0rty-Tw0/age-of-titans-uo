using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - Kaminoi kiln-priests. L6 trash. Donor: Lich.
[SerializationGenerator(0, false)]
public partial class PyreKaminosScald : BaseCreature
{
    [Constructible]
    public PyreKaminosScald() : base(AIType.AI_Mage)
    {
        Body = 24;
        Hue = 0x0026;
        BaseSoundID = 0x3E9;

        SetStr(210, 240);
        SetDex(140, 165);
        SetInt(260, 290);

        SetHits(460, 510);

        SetDamage(12, 16);

        SetDamageType(ResistanceType.Physical, 20);
        SetDamageType(ResistanceType.Fire, 60);
        SetDamageType(ResistanceType.Energy, 20);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 50, 60);
        SetResistance(ResistanceType.Cold, 25, 33);
        SetResistance(ResistanceType.Poison, 30, 38);
        SetResistance(ResistanceType.Energy, 28, 36);

        SetSkill(SkillName.EvalInt, 78.0, 88.0);
        SetSkill(SkillName.Magery, 78.0, 88.0);
        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 66.0, 76.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 6200;
        Karma = -6200;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a Kaminoi scald-priest's corpse";
    public override string DefaultName => "a Kaminoi scald-priest";

    public override bool BleedImmune => true;
    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
