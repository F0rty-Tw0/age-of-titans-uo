using ModernUO.Serialization;

namespace Server.Mobiles;

// The Rimehold (dev-docs/gap-families-bestiary.md §6.2e) - Ice dungeon. L7 trash. Donor: Arctic Ogre Lord.
[SerializationGenerator(0, false)]
public partial class RimeColossus : BaseCreature
{
    [Constructible]
    public RimeColossus() : base(AIType.AI_Melee)
    {
        Body = 135;
        Hue = 0x047E;
        BaseSoundID = 427;

        SetStr(430, 470);
        SetDex(95, 120);
        SetInt(85, 110);

        SetHits(620, 680);

        SetDamage(15, 19);

        SetDamageType(ResistanceType.Physical, 60);
        SetDamageType(ResistanceType.Cold, 40);

        SetResistance(ResistanceType.Physical, 54, 64);
        SetResistance(ResistanceType.Fire, 25, 32);
        SetResistance(ResistanceType.Cold, 50, 60);
        SetResistance(ResistanceType.Poison, 32, 40);
        SetResistance(ResistanceType.Energy, 32, 40);

        SetSkill(SkillName.MagicResist, 78.0, 88.0);
        SetSkill(SkillName.Tactics, 90.0, 100.0);
        SetSkill(SkillName.Wrestling, 90.0, 100.0);

        Fame = 9500;
        Karma = -9500;

        VirtualArmor = 60;
    }

    public override string CorpseName => "a frostbound colossus's remains";
    public override string DefaultName => "a frostbound colossus";

    public override SpeedLevel SpeedClass => SpeedLevel.Slow;
    public override bool BleedImmune => true;

    public override int LootBagLevel => 6;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Rich);
    }
}
