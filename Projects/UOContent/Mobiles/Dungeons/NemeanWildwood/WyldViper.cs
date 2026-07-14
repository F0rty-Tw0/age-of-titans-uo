using ModernUO.Serialization;

namespace Server.Mobiles;

// The Nemean Wildwood (dev-docs/dungeon-ladder.md §3) - L6 trash. Donor: Snake.
[SerializationGenerator(0, false)]
public partial class WyldViper : BaseCreature
{
    [Constructible]
    public WyldViper() : base(AIType.AI_Melee)
    {
        Body = 52;
        Hue = 0x0851;
        BaseSoundID = 0xDB;

        SetStr(175, 215);
        SetDex(110, 140);
        SetInt(35, 50);

        SetHits(440, 490);

        SetDamage(11, 15);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 40, 48);
        SetResistance(ResistanceType.Fire, 15, 25);
        SetResistance(ResistanceType.Cold, 15, 25);
        SetResistance(ResistanceType.Poison, 30, 40);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.Poisoning, 70.0, 90.0);
        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 65.0, 75.0);
        SetSkill(SkillName.Wrestling, 65.0, 75.0);

        Fame = 3000;
        Karma = -3000;

        VirtualArmor = 52;
    }

    public override string CorpseName => "a grove viper's corpse";
    public override string DefaultName => "a grove viper";

    public override Poison HitPoison => Poison.Greater;

    public override SpeedLevel SpeedClass => SpeedLevel.VeryFast;

    public override int LootBagLevel => 5;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
