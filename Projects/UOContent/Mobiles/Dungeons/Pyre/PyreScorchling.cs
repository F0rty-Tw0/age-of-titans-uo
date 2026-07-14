using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - coalwalk host. L5 trash. Donor: Lava Lizard.
[SerializationGenerator(0, false)]
public partial class PyreScorchling : BaseCreature
{
    [Constructible]
    public PyreScorchling() : base(AIType.AI_Melee)
    {
        Body = 0xCE;
        Hue = 0x0655;
        BaseSoundID = 0x5A;

        SetStr(200, 230);
        SetDex(100, 125);
        SetInt(40, 60);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 50);
        SetResistance(ResistanceType.Fire, 32, 42);
        SetResistance(ResistanceType.Cold, 20, 25);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 20, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 68.0, 78.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a scorchling's corpse";
    public override string DefaultName => "a scorchling";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
