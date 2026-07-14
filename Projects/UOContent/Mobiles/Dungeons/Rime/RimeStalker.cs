using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L4 trash. Donor: Ice Snake.
[SerializationGenerator(0, false)]
public partial class RimeStalker : BaseCreature
{
    [Constructible]
    public RimeStalker() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0B0F;
        BaseSoundID = 0xDB;

        SetStr(140, 165);
        SetDex(140, 165);
        SetInt(40, 55);

        SetHits(190, 230);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 34, 42);
        SetResistance(ResistanceType.Fire, 12, 18);
        SetResistance(ResistanceType.Cold, 35, 42);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 62.0, 72.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a frostfang stalker's corpse";
    public override string DefaultName => "a frostfang stalker";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
