using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 ambient. Donor: Giant Spider.
[SerializationGenerator(0, false)]
public partial class WyldSpiderling : BaseCreature
{
    [Constructible]
    public WyldSpiderling() : base(AIType.AI_Melee)
    {
        Body = 28;
        Hue = 0x0483;
        BaseSoundID = 0x388;

        SetStr(150, 180);
        SetDex(100, 125);
        SetInt(40, 55);

        SetHits(440, 470);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 43);
        SetResistance(ResistanceType.Fire, 10, 20);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 10, 20);

        SetSkill(SkillName.Poisoning, 60.0, 80.0);
        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 2500;
        Karma = -2500;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a silverweb spiderling's corpse";
    public override string DefaultName => "a silverweb spiderling";

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
