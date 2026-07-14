using ModernUO.Serialization;

namespace Server.Mobiles;

// The Drowned Tholos (dev-docs/dungeon-ladder-bestiary.md §1) - Pelagos crew (L5). Donor: Ghoul.
[SerializationGenerator(0, false)]
public partial class TidePelagosLookout : BaseCreature
{
    [Constructible]
    public TidePelagosLookout() : base(AIType.AI_Melee)
    {
        Body = 153;
        Hue = 0x0481;
        BaseSoundID = 0x482;

        SetStr(160, 190);
        SetDex(60, 80);
        SetInt(20, 35);

        SetHits(280, 330);

        SetDamage(9, 13);

        SetDamageType(ResistanceType.Physical, 100);

        SetResistance(ResistanceType.Physical, 30, 40);
        SetResistance(ResistanceType.Cold, 26, 36);
        SetResistance(ResistanceType.Poison, 15, 25);

        SetSkill(SkillName.MagicResist, 55.0, 65.0);
        SetSkill(SkillName.Tactics, 63.0, 73.0);
        SetSkill(SkillName.Wrestling, 63.0, 73.0);

        Fame = 1350;
        Karma = -1350;

        VirtualArmor = 38;
    }

    public override string CorpseName => "a lookout's corpse";
    public override string DefaultName => "a Pelagos lookout";

    public override SpeedLevel SpeedClass => SpeedLevel.Fast;

    public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

    public override int LootBagLevel => 4;

    public override void GenerateLoot()
    {
        AddLoot(LootPack.Meager);
    }
}
