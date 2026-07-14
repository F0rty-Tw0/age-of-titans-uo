using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - L5 trash. Donor: Ogre.
[SerializationGenerator(0, false)]
public partial class CinderStoker : BaseCreature
{
    [Constructible]
    public CinderStoker() : base(AIType.AI_Melee)
    {
        Body = 1;
        Hue = 0x0964;
        BaseSoundID = 427;

        SetStr(200, 230);
        SetDex(50, 65);
        SetInt(30, 45);

        SetHits(300, 350);

        SetDamage(10, 14);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 35, 45);
        SetResistance(ResistanceType.Fire, 45, 55);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 50.0, 60.0);
        SetSkill(SkillName.Tactics, 60.0, 70.0);
        SetSkill(SkillName.Wrestling, 60.0, 70.0);

        Fame = 1800;
        Karma = -1800;

        VirtualArmor = 44;
    }

    public override string CorpseName => "a slag stoker's corpse";
    public override string DefaultName => "a slag stoker";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;

    public override Poison PoisonImmune => Poison.Lethal;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
