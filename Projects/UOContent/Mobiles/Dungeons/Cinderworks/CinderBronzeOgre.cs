using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L6 trash. Donor: Ogre Lord.
[SerializationGenerator(0, false)]
public partial class CinderBronzeOgre : BaseCreature
{
    [Constructible]
    public CinderBronzeOgre() : base(AIType.AI_Melee)
    {
        Body = 83;
        Hue = 0x0798;
        BaseSoundID = 427;

        SetStr(280, 320);
        SetDex(50, 65);
        SetInt(50, 70);

        SetHits(500, 550);

        SetDamage(14, 18);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 50, 60);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 20, 30);

        SetSkill(SkillName.MagicResist, 70.0, 80.0);
        SetSkill(SkillName.Tactics, 80.0, 90.0);
        SetSkill(SkillName.Wrestling, 75.0, 85.0);

        Fame = 2400;
        Karma = -2400;

        VirtualArmor = 56;
    }

    public override string CorpseName => "a bronze ogre lord's corpse";
    public override string DefaultName => "a bronze ogre lord";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
