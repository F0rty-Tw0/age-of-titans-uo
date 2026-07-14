using ModernUO.Serialization;

namespace Server.Mobiles;

// Classic Five bestiary (dev-docs/classic-five-bestiary.md) - Deceit, Drowned family. L4 trash. Donor: Zombie.
[SerializationGenerator(0, false)]
public partial class DrownedDead : BaseCreature
{
    [Constructible]
    public DrownedDead() : base(AIType.AI_Melee)
    {
        Body = 3;
        Hue = 0x0835;
        BaseSoundID = 471;

        SetStr(150, 180);
        SetDex(55, 70);
        SetInt(25, 35);

        SetHits(195, 205);

        SetDamage(11, 16);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 35);
        SetResistance(ResistanceType.Cold, 30, 38);
        SetResistance(ResistanceType.Poison, 20, 28);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 58.0, 68.0);
        SetSkill(SkillName.Wrestling, 58.0, 68.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a drowned corpse";
    public override string DefaultName => "a drowned dead";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override bool BleedImmune => true;
    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
