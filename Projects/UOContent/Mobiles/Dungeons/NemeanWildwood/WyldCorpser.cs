using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L7 trash. Donor: Corpser.
[SerializationGenerator(0, false)]
public partial class WyldCorpser : BaseCreature
{
    [Constructible]
    public WyldCorpser() : base(AIType.AI_Melee)
    {
        Body = 8;
        Hue = 0x0844;
        BaseSoundID = 684;

        SetStr(215, 250);
        SetDex(60, 85);
        SetInt(60, 80);

        SetHits(620, 690);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Poison, 40);

        SetResistance(ResistanceType.Physical, 45, 55);
        SetResistance(ResistanceType.Fire, 20, 30);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 35, 45);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 70.0, 80.0);
        SetSkill(SkillName.Wrestling, 70.0, 80.0);

        Fame = 4700;
        Karma = -4700;

        VirtualArmor = 58;
    }

    public override string CorpseName => "a thornwood corpser's corpse";
    public override string DefaultName => "a thornwood corpser";

    public override Poison HitPoison => Poison.Greater;
    public override bool DisallowAllMoves => true;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
