using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L5 trash. Donor: Scorpion.
[SerializationGenerator(0, false)]
public partial class CinderScorial : BaseCreature
{
    [Constructible]
    public CinderScorial() : base(AIType.AI_Melee)
    {
        Body = 48;
        Hue = 0x0967;
        BaseSoundID = 397;

        SetStr(160, 190);
        SetDex(50, 65);
        SetInt(25, 40);

        SetHits(260, 310);

        SetDamage(8, 12);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Fire, 40, 50);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 45.0, 55.0);
        SetSkill(SkillName.Tactics, 55.0, 65.0);
        SetSkill(SkillName.Wrestling, 55.0, 65.0);

        Fame = 1600;
        Karma = -1600;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a cinder scorpion's corpse";
    public override string DefaultName => "a cinder scorpion";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison HitPoison => Poison.Greater;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
