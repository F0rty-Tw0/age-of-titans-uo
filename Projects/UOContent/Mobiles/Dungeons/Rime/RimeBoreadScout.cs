using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 trash. Donor: Harpy.
[SerializationGenerator(0, false)]
public partial class RimeBoreadScout : BaseCreature
{
    [Constructible]
    public RimeBoreadScout() : base(AIType.AI_Melee)
    {
        Body = 30;
        Hue = 0x0AF3;
        BaseSoundID = 402;

        SetStr(150, 175);
        SetDex(140, 165);
        SetInt(50, 70);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 25, 32);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a Boread scout's corpse";
    public override string DefaultName => "a Boread scout";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
