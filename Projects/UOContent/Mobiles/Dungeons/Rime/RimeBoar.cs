using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L4 trash. Donor: Boar.
[SerializationGenerator(0, false)]
public partial class RimeBoar : BaseCreature
{
    [Constructible]
    public RimeBoar() : base(AIType.AI_Melee)
    {
        Body = 0x122;
        Hue = 0x0485;
        BaseSoundID = 0xC4;

        SetStr(160, 185);
        SetDex(120, 145);
        SetInt(40, 55);

        SetHits(200, 240);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 15, 22);
        SetResistance(ResistanceType.Cold, 28, 35);
        SetResistance(ResistanceType.Poison, 20, 28);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 40;
    }

    public override string CorpseName => "a tundra boar's corpse";
    public override string DefaultName => "a tundra boar";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
