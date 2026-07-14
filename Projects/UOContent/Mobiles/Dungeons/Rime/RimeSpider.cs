using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2) - Ice dungeon. L4 trash. Donor: Frost Spider.
[SerializationGenerator(0, false)]
public partial class RimeSpider : BaseCreature
{
    [Constructible]
    public RimeSpider() : base(AIType.AI_Melee)
    {
        Body = 20;
        Hue = 0x047E;
        BaseSoundID = 0x388;

        SetStr(150, 175);
        SetDex(140, 165);
        SetInt(50, 70);

        SetHits(200, 235);

        SetDamage(8, 11);

        SetDamageType(ResistanceType.Physical, 30);
        SetDamageType(ResistanceType.Cold, 70);

        SetResistance(ResistanceType.Physical, 36, 44);
        SetResistance(ResistanceType.Fire, 12, 18);
        SetResistance(ResistanceType.Cold, 40, 48);
        SetResistance(ResistanceType.Poison, 25, 32);
        SetResistance(ResistanceType.Energy, 18, 25);

        SetSkill(SkillName.MagicResist, 48.0, 58.0);
        SetSkill(SkillName.Tactics, 62.0, 72.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3400;
        Karma = -3400;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a hoarfrost spider's corpse";
    public override string DefaultName => "a hoarfrost spider";

    public override PackInstinct PackInstinct => PackInstinct.Arachnid;
    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
