using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L6 trash. Donor: Frost Troll.
[SerializationGenerator(0, false)]
public partial class RimeGiant : BaseCreature
{
    [Constructible]
    public RimeGiant() : base(AIType.AI_Melee)
    {
        Body = 55;
        Hue = 0x0485;
        BaseSoundID = 461;

        SetStr(340, 380);
        SetDex(90, 115);
        SetInt(70, 90);

        SetHits(480, 540);

        SetDamage(13, 17);

        SetDamageType(ResistanceType.Physical, 75);
        SetDamageType(ResistanceType.Cold, 25);

        SetResistance(ResistanceType.Physical, 46, 54);
        SetResistance(ResistanceType.Fire, 18, 25);
        SetResistance(ResistanceType.Cold, 48, 56);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 20, 28);

        SetSkill(SkillName.MagicResist, 58.0, 68.0);
        SetSkill(SkillName.Tactics, 82.0, 92.0);
        SetSkill(SkillName.Wrestling, 82.0, 92.0);

        Fame = 6400;
        Karma = -6400;

        VirtualArmor = 54;
    }

    public override string CorpseName => "a rime frost-giant's corpse";
    public override string DefaultName => "a rime frost-giant";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Average);
    }
}
