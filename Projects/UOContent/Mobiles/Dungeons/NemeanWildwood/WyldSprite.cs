using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 ambient. Donor: Pixie.
[SerializationGenerator(0, false)]
public partial class WyldSprite : BaseCreature
{
    [Constructible]
    public WyldSprite() : base(AIType.AI_Mage)
    {
        Body = 128;
        Hue = 0x0851;
        BaseSoundID = 0x467;

        SetStr(150, 175);
        SetDex(180, 220);
        SetInt(140, 170);

        SetHits(440, 470);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 25, 35);
        SetResistance(ResistanceType.Cold, 25, 35);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 25, 35);

        SetSkill(SkillName.EvalInt, 75.0, 90.0);
        SetSkill(SkillName.Magery, 75.0, 90.0);
        SetSkill(SkillName.Meditation, 60.0, 75.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 35.0, 45.0);
        SetSkill(SkillName.Wrestling, 30.0, 40.0);

        Fame = 2600;
        Karma = -2600;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a thorn sprite's corpse";
    public override string DefaultName => "a thorn sprite";

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
