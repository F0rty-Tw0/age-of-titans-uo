using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family expansion.
// Core expansion - the sunken garrison. L5 trash. Donor: Mummy.
[SerializationGenerator(0, false)]
public partial class DrownedSaltMummy : BaseCreature
{
    [Constructible]
    public DrownedSaltMummy() : base(AIType.AI_Melee)
    {
        Body = 154;
        Hue = 0x0830;
        BaseSoundID = 471;

        SetStr(235, 265);
        SetDex(55, 70);
        SetInt(33, 48);

        SetHits(340, 360);

        SetDamage(15, 20);

        SetDamageType(ResistanceType.Physical, 40);
        SetDamageType(ResistanceType.Cold, 60);

        SetResistance(ResistanceType.Physical, 40, 50);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 48, 58);
        SetResistance(ResistanceType.Poison, 26, 36);
        SetResistance(ResistanceType.Energy, 22, 32);

        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 2000;
        Karma = -2000;

        VirtualArmor = 48;
    }

    public override string CorpseName => "a salt-crusted mummy's corpse";
    public override string DefaultName => "a salt-crusted mummy";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
