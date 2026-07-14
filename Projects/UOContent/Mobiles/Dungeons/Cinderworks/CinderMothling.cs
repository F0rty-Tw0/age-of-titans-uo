using ModernUO.Serialization;

namespace Server.Mobiles;

// The Cinderworks (dev-docs/dungeon-ladder-bestiary.md §2) - ambient fodder. Donor: Mongbat.
[SerializationGenerator(0, false)]
public partial class CinderMothling : BaseCreature
{
    [Constructible]
    public CinderMothling() : base(AIType.AI_Melee)
    {
        Body = 39;
        Hue = 0x0655;
        BaseSoundID = 422;

        SetStr(130, 160);
        SetDex(85, 105);
        SetInt(20, 35);

        SetHits(260, 290);

        SetDamage(7, 10);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 25, 35);
        SetResistance(ResistanceType.Fire, 35, 45);
        SetResistance(ResistanceType.Cold, 10, 20);
        SetResistance(ResistanceType.Energy, 15, 25);

        SetSkill(SkillName.MagicResist, 40.0, 50.0);
        SetSkill(SkillName.Tactics, 50.0, 60.0);
        SetSkill(SkillName.Wrestling, 50.0, 60.0);

        Fame = 1400;
        Karma = -1400;

        VirtualArmor = 32;
    }

    public override string CorpseName => "a cinder moth's corpse";
    public override string DefaultName => "a cinder moth";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;
    public override bool CanFly => true;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
