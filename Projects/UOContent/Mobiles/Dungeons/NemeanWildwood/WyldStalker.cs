using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Ratman.
[SerializationGenerator(0, false)]
public partial class WyldStalker : BaseCreature
{
    [Constructible]
    public WyldStalker() : base(AIType.AI_Mage)
    {
        Body = 42;
        Hue = 0x0844;
        BaseSoundID = 437;

        SetStr(170, 210);
        SetDex(90, 110);
        SetInt(110, 140);

        SetHits(440, 500);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 38, 46);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 25, 35);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.EvalInt, 70.0, 85.0);
        SetSkill(SkillName.Magery, 70.0, 85.0);
        SetSkill(SkillName.MagicResist, 60.0, 70.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 3200;
        Karma = -3200;

        VirtualArmor = 50;
    }

    public override string CorpseName => "a thornclad corpse";
    public override string DefaultName => "a thornclad stalker";

    public override Poison HitPoison => Poison.Greater; // "Nightshade Arrow"

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
