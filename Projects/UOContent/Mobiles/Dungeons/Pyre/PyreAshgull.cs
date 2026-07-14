using ModernUO.Serialization;

namespace Server.Mobiles;

// The Pyre expansion (dev-docs/gap-families-bestiary.md §6.1e) - ambient/fodder. L5 trash. Donor: Eagle.
[SerializationGenerator(0, false)]
public partial class PyreAshgull : BaseCreature
{
    [Constructible]
    public PyreAshgull() : base(AIType.AI_Melee)
    {
        Body = 5;
        Hue = 0x0021;
        BaseSoundID = 0x2EE;

        SetStr(150, 180);
        SetDex(140, 165);
        SetInt(25, 40);

        SetHits(280, 310);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 28, 38);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 15, 23);
        SetResistance(ResistanceType.Poison, 15, 22);
        SetResistance(ResistanceType.Energy, 15, 23);

        SetSkill(SkillName.MagicResist, 42.0, 52.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1500;
        Karma = -1500;

        VirtualArmor = 34;
    }

    public override string CorpseName => "an ash gull's corpse";
    public override string DefaultName => "an ash gull";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
