using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Giant Spider.
[SerializationGenerator(0, false)]
public partial class WyldSpider : BaseCreature
{
    [Constructible]
    public WyldSpider() : base(AIType.AI_Melee)
    {
        Body = 28;
        Hue = 0x0483;
        BaseSoundID = 0x388;

        SetStr(200, 230);
        SetDex(130, 160);
        SetInt(60, 80);

        SetHits(600, 670);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 42, 52);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.Poisoning, 75.0, 95.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 4500;
        Karma = -4500;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a silverweb spider's corpse";
    public override string DefaultName => "a silverweb spider";

    public override Poison HitPoison => Poison.Deadly;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
