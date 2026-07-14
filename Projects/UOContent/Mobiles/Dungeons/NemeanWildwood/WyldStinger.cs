using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class WyldStinger : BaseCreature
{
    [Constructible]
    public WyldStinger() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0844;
        BaseSoundID = 397;

        SetStr(180, 220);
        SetDex(95, 120);
        SetInt(30, 45);

        SetHits(440, 490);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Poison, 40);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 45, 55);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.Poisoning, 85.0, 100.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 55;
    }

    public override string CorpseName => "a thornscorpion's corpse";
    public override string DefaultName => "a thornscorpion";

    public override Poison HitPoison => Poison.Deadly;

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
