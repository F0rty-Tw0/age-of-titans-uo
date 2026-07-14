using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L5 trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class RimeBoreadRider : BaseCreature
{
    [Constructible]
    public RimeBoreadRider() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0B0F;
        BaseSoundID = 402;

        SetStr(200, 230);
        SetDex(160, 185);
        SetInt(60, 80);

        SetHits(320, 360);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 70);
        SetDamageType(ResistanceType.Cold, 30);

        SetResistance(ResistanceType.Physical, 44, 52);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 42, 50);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 25, 32);

        SetSkill(SkillName.MagicResist, 52.0, 62.0);
        SetSkill(SkillName.Tactics, 68.0, 78.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 4600;
        Karma = -4600;

        VirtualArmor = 46;
    }

    public override string CorpseName => "a Boread wind-rider's corpse";
    public override string DefaultName => "a Boread wind-rider";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
