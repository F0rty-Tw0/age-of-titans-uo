using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - L4 core family. Donor: Slime.
[SerializationGenerator(0, false)]
public partial class TideLeech : BaseCreature
{
    [Constructible]
    public TideLeech() : base(AIType.AI_Melee)
    {
        Body = 51;
        Hue = 0x0530;
        BaseSoundID = 456;

        SetStr(60, 80);
        SetDex(30, 45);
        SetInt(15, 25);

        SetHits(90, 130);

        SetDamage(5, 8);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 15, 22);
        SetResistance(ResistanceType.Cold, 20, 30);
        SetResistance(ResistanceType.Poison, 45, 55);

        SetSkill(SkillName.Poisoning, 45.0, 55.0);
        SetSkill(SkillName.MagicResist, 30.0, 40.0);
        SetSkill(SkillName.Tactics, 35.0, 45.0);
        SetSkill(SkillName.Wrestling, 35.0, 45.0);

        Fame = 750;
        Karma = -750;

        VirtualArmor = 18;
    }

    public override string CorpseName => "a leech's corpse";
    public override string DefaultName => "a brine leech";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lesser;
    public override Poison HitPoison => Poison.Lesser; // "Venom of the Deep"

    public override int LootBagLevel => 3;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
