using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - ambient/fodder. L5 trash. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class PyreCindermoth : BaseCreature
{
    [Constructible]
    public PyreCindermoth() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0655;
        BaseSoundID = 422;

        SetStr(150, 180);
        SetDex(90, 110);
        SetInt(25, 40);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 38, 48);
        SetResistance(ResistanceType.Cold, 12, 20);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 36;
    }

    public override string CorpseName => "a cinder moth's corpse";
    public override string DefaultName => "a pyre moth";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
