using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 ambient/fodder. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class TideJelly : BaseCreature
{
    [Constructible]
    public TideJelly() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0481;
        BaseSoundID = 456;

        SetStr(50, 65);
        SetDex(25, 35);
        SetInt(10, 20);

        SetHits(80, 110);

        SetDamage(4, 7);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 12, 18);
        SetResistance(ResistanceType.Cold, 18, 26);
        SetResistance(ResistanceType.Poison, 40, 50);

        SetSkill(SkillName.Poisoning, 35.0, 45.0);
        SetSkill(SkillName.MagicResist, 25.0, 35.0);
        SetSkill(SkillName.Tactics, 30.0, 40.0);
        SetSkill(SkillName.Wrestling, 30.0, 40.0);

        Fame = 500;
        Karma = -500;

        VirtualArmor = 14;
    }

    public override string CorpseName => "a jelly's remains";
    public override string DefaultName => "a drifting jelly";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lesser;
    public override Poison HitPoison => Poison.Lesser; // "Venom of the Deep"

    public override int LootBagLevel => 1;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
