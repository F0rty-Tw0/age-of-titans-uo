using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - coalwalk host. L5 trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class PyreEmberwing : BaseCreature
{
    [Constructible]
    public PyreEmberwing() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0655;
        BaseSoundID = 402;

        SetStr(190, 220);
        SetDex(130, 155);
        SetInt(55, 75);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 28, 36);
        SetResistance(ResistanceType.Cold, 20, 28);
        SetResistance(ResistanceType.Poison, 22, 30);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 68.0, 80.0);
        SetSkill(SkillName.Wrestling, 65.0, 78.0);

        Fame = 4200;
        Karma = -4200;

        VirtualArmor = 44;
    }

    public override string CorpseName => "an emberwing harpy's corpse";
    public override string DefaultName => "an emberwing harpy";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
